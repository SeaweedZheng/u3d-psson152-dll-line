using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMaker;
using System;
using SBoxApi;
using _consoleBB = PssOn00152.ConsoleBlackboard02;


namespace Console001
{
    public class PageConsoleMain : PageCorBase //  BasePageMachineButton
    {

        public Button btnGameInfo, btnBusinessRecord, btnGameHistory, btnEventRecord,
            btnSettings, btnVolumeSetting, btnInputTest, btnTouchCallbrate,
            btnTimeAndDate, btnRobootMachine,btnLanguage,btnAdmin, btnExit;


        public GameObject goMaskDontClick;

        void Awake()
        {
            btnGameInfo.onClick.AddListener(OnClickGameInfo);
            btnBusinessRecord.onClick.AddListener(OnClickBusinessRecord);
            btnGameHistory.onClick.AddListener(OnClickGameHistory);
            btnEventRecord.onClick.AddListener(OnClickEventRecord);
            btnSettings.onClick.AddListener(OnClickSettings);
            btnVolumeSetting.onClick.AddListener(OnClickVolumeSetting);
            btnInputTest.onClick.AddListener(OnClickInputTest);
            btnTouchCallbrate.onClick.AddListener(OnClickTouchCallbrate);
            btnTimeAndDate.onClick.AddListener(OnClickTimeAndDate);
            btnRobootMachine.onClick.AddListener(OnClickRobootMachine);
            btnLanguage.onClick.AddListener(OnClickLanguage);
            btnAdmin.onClick.AddListener(OnClickAdmin);
            btnExit.onClick.AddListener(OnClickExit);

        }


        void OnEnable()
        {
            EventCenter.Instance.AddEventListener<I18nLang>(I18nManager.I18N, OnChangeLanguage);

        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<I18nLang>(I18nManager.I18N, OnChangeLanguage);
        }

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            //清楚所有弹窗
            CommonPopupHandler.Instance.CloseAllPopup();

            if(PageManager.Instance.IndexOf(PageName.PageSysMessage) != -1)
                PageManager.Instance.ClosePage(PageName.PageSysMessage);

            //btnSettings.interactable = false;
            //btnAdmin.interactable = false;

            btnSettings.gameObject.SetActive(false);
            btnAdmin.gameObject.SetActive(false);

            OnChenkUser();
        }


        void OnClickGameInfo() => PageManager.Instance.OpenPage(PageName.Console001PageConsoleGameInformation);
        
        void OnClickBusinessRecord() => PageManager.Instance.OpenPage(PageName.Console001PageConsoleBusinessRecord);
        void OnClickGameHistory() => PageManager.Instance.OpenPage(PageName.Console001PageConsoleGameHistory);
        
        void OnClickEventRecord()=>PageManager.Instance.OpenPage(PageName.Console001PageConsoleLogRecord);




        int permissions = -1; //1：普通密码权限，2：管理员密码权限，3：超级管理员密码权限

        void OnCheckUserError()
        {
            OnChenkUser();
            CommonPopupHandler.Instance.OpenPopupSingle(
            new CommonPopupInfo()
            {
                isUseXButton = false,
                buttonAutoClose1 = true,
                buttonAutoClose2 = true,
                type = CommonPopupType.YesNo,
                text = I18nMgr.T("Error Password"),
                buttonText1 = I18nMgr.T("Cancel"),
                buttonText2 = I18nMgr.T("Confirm"),
            });

        }
        async void OnChenkUser()
        {
            goMaskDontClick.SetActive(true);

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));


            permissions = -1;

            if (res.value != null)
            {
                string pwdStr = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwdStr}");

                try
                {
                    int pwd = int.Parse(pwdStr); //这里有可能失败


                    // 加个延时 调用 OnCheckUserError();？？

                    MachineDataManager.Instance.RequestCheckPassword(pwd,
                    (res) =>
                    {
                        SBoxPermissionsData data = res as SBoxPermissionsData;
                        if (data.result == 0 && data.permissions > 0)
                        {
                            goMaskDontClick.SetActive(false);


                            permissions = data.permissions;//1：普通密码权限，2：管理员密码权限，3：超级管理员密码权限

                            btnSettings.gameObject.SetActive(permissions >= 2);
                            btnAdmin.gameObject.SetActive(permissions == 3);
                            //btnSettings.interactable = permissions >= 2;
                            //btnAdmin.interactable = permissions == 3;

                            _consoleBB.Instance.curPermissions = permissions;


                            DebugUtils.Log($"当前用户权限{_consoleBB.Instance.curPermissions}; 密码: {pwd}");
                        }
                        else
                        {
                            OnCheckUserError();
                        }
                    }, (error) =>
                    {
                        OnCheckUserError();
                    });
                }
                catch
                {
                    OnCheckUserError();
                }
            }
            else
            {
                OnClickExit();
            }
        }


        void OnClickSettings() => PageManager.Instance.OpenPage(PageName.Console001PageConsoleMachineSettings);

        /*async void OnClickSettings() {

            EventData res =await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Manager/Admin"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));

            if(res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtil.Log($"键盘输入结果 ：{pwd}");
                try
                {
                    MachineDataManager.Instance.RequestCheckPassword(int.Parse(pwd),
                    (res) =>
                    {
                        SBoxPermissionsData data = res as SBoxPermissionsData;
                        if (data.result == 0 &&  data.permissions > 0)
                        {
                            PageManager.Instance.OpenPage(PageName.Console001PageConsoleMachineSettings);
                            //1：普通密码权限，2：管理员密码权限，3：超级管理员密码权限
                        }
                        else
                        {
                            ErrorPopupHandler.Instance.OpenPopup(
                            new ErrorPopupInfo()
                            {
                                isUseXButton = false,
                                buttonAutoClose1 = true,
                                buttonAutoClose2 = true,
                                type = ErrorPopupType.YesNo,
                                text = I18nMgr.T("Error Password"),
                                buttonText1 = I18nMgr.T("Cancel"),
                                buttonText2 = I18nMgr.T("Confirm"),
                            });
                        }
                    });
                }
                catch
                {
                    ErrorPopupHandler.Instance.OpenPopup(
                    new ErrorPopupInfo()
                    {
                        isUseXButton = false,
                        buttonAutoClose1 = true,
                        buttonAutoClose2 = true,
                        type = ErrorPopupType.YesNo,
                        text = I18nMgr.T("Error Password"),
                        buttonText1 = I18nMgr.T("Cancel"),
                        buttonText2 = I18nMgr.T("Confirm"),
                    });
                }
            }
        }*/



        async void OnClickVolumeSetting() {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSound,
                new EventData<Dictionary<string, object>>("Resault",
                new Dictionary<string, object>()
                {
                    //["music"] = ConsoleDataUtil.GetMuise(),
                    //["sound"] = ConsoleDataUtil.GetSound(),
                    //["isDemoVoice"] = ConsoleDataUtil.GetIsDemoVoice(),

                    ["music"] = _consoleBB.Instance.music,
                    //BlackboardUtils.GetValue<float>("@console/music"),
                    ["sound"] = _consoleBB.Instance.sound,
                    //BlackboardUtils.GetValue<float>("@console/sound"),
                    ["isDemoVoice"] = _consoleBB.Instance.isDemoVoice,
                    //BlackboardUtils.GetValue<bool>("@console/isDemoVoice"),

                }));

            if (res != null)
            {
                Dictionary<string, object>  argDic = (Dictionary<string, object>)res.value;

                //ConsoleDataUtil.SetMuise((float)argDic["music"]);
                //ConsoleDataUtil.SetSound((float)argDic["sound"]);
                //ConsoleDataUtil.SetIsDemoVoice((bool)argDic["isDemoVoice"]);

                _consoleBB.Instance.music = (float)argDic["music"];
                _consoleBB.Instance.sound = (float)argDic["sound"];
                _consoleBB.Instance.isDemoVoice = (bool)argDic["isDemoVoice"];
            }
            
        }
        void OnClickInputTest()=>PageManager.Instance.OpenPage(PageName.Console001PageConsoleMachineTest);

        // PageManager.Instance.OpenPage(PageName.Console);

        void OnClickAdmin() => PageManager.Instance.OpenPage(PageName.Console001PageConsoleAdmin);


        void OnClickTouchCallbrate() => PageManager.Instance.OpenPage(PageName.Console001PageConsoleDrawLine);
        async void OnClickTimeAndDate()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleCalendar);
            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                DebugUtils.Log($"选择的时间 ： {data["date"]}");

                // 假设你有一个时间戳（UTC时间）
                long unixTimestamp = (long)data["timestamp"];
                // 将时间戳转换为DateTimeOffset（UTC时间）
                //DateTimeOffset dateTimeOffsetUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
                DateTimeOffset dateTimeOffsetUtc = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp);
                // 将UTC时间转换为本地时间
                DateTimeOffset dateTimeOffsetLocal = dateTimeOffsetUtc.ToLocalTime();
                // 输出结果
                DebugUtils.Log("UTC时间: " + dateTimeOffsetUtc.DateTime);
                DebugUtils.Log("本地时间: " + dateTimeOffsetLocal.DateTime);

                string[] dateLst = ((string)data["date"]).Split(' ')[0].Split('-');
                string[] timeLst = ((string)data["date"]).Split(' ')[1].Split(':');
                int year = int.Parse(dateLst[0]);
                int month = int.Parse(dateLst[1]);
                int day = int.Parse(dateLst[2]);
                int hour = int.Parse(timeLst[0]);
                int min = int.Parse(timeLst[1]);

                DebugUtils.Log($"year:{year} month:{month} day:{day} hour:{hour} min:{min}");
                AndroidSystemHelper.Instance.SetSystemTime(year, month, day, hour, min, 0);
            }
        }
      
        void OnClickRobootMachine() { }
        void OnClickExit(){
            _consoleBB.Instance.curPermissions = -1;
            PageManager.Instance.ClosePage(this);
        }


        async void OnClickLanguage() {


            Dictionary<string, string> languageLst = new Dictionary<string, string>();
            TableSupportLanguageItem[] tableSupportLanguage = _consoleBB.Instance.tableSupportLanguage;
            // BlackboardUtils.GetValue<List<TableSupportLanguageItem>>("@console/tableSupportLanguage");

            foreach (TableSupportLanguageItem item in tableSupportLanguage)
            {
                languageLst.Add(item.number,item.name);
            }

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleChooseLanguage,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["selectLst"] = languageLst,
                        ["selectNumber"] = _consoleBB.Instance.language,
                        ["selectedDes"] = "Selected language: {0}",
                        ["title"] = "Choose Language"
                        //BlackboardUtils.GetValue<string>("@console/language")
                    }
                ));

            if (res.value != null)
            {
                string languageNumber = (string)res.value;
                if (_consoleBB.Instance.language != languageNumber)
                {
                    MaskPopupHandler.Instance.OpenPopup(5000);
                    DoCor(COR_SET_LANGUAGE, DoTask(() =>
                    {
                        string languageNumber = (string)res.value;
                        _consoleBB.Instance.language = languageNumber;
                        //BlackboardUtils.SetValue<string>("@console/language", languageNumber);
                    }, 800));
                }
            }
        }
        const string COR_SET_LANGUAGE = "COR_SET_LANGUAGE";
        void OnChangeLanguage(I18nLang lang)
        {
            MaskPopupHandler.Instance.ClosePopup();
        }
    }


}
