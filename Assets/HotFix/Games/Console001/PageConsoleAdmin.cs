using Game;
using GameMaker;
using IOT;
using SBoxApi;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using System.Reflection;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class PageConsoleAdmin : PageCorBase
{
    public Button btnClose1,
        btnAgentID, btnMachineID ,btnSaveSettings,
        btnErrorCode,
        btnSasConnection, btnSasAccount, btnSasInOutScale;

    public TextMeshProUGUI tmpAgentID, tmpMachineID,
        tmpAppKey, tmpSeverAddr, tmpReportAddr, tmpSofwareVer, tmpHotfixKey, tmpAutoHotfixAddr, tmpInstallVer,

        tmpIOTAddress;

    public Toggle tgIsDebug,tgEnableDebugTool, tgEnableReporterPage, tgIsUpdateInfo,
        tgIsConnectSas, tgIsConnectMoneyBox,
        tgIsTestOfficalIOT,
        tgPauseAtPopupFreeSpinTrigger, tgPauseAtPopupJackpotGame, tgPauseAtPopupJackpotOnline;

    public PageController ctrlPage;

    string _agentID = "";
    string agentID
    {
        get => _agentID;
        set
        {
            _agentID = value;
            tmpAgentID.text = _agentID;
        }
    }

    string _machineID = "";
    string machineID
    {
        get => _machineID;
        set
        {
            _machineID = value;
            tmpMachineID.text = _machineID;
        }
    }


    MessageDelegates onPropertyChangedEventDelegates;

    void Awake()
    {

        onPropertyChangedEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                { "@console/isSasConnect", OnPropertyChangeSasConnection}
             }
         );


        btnClose1.onClick.AddListener(OnClickClose);
        btnAgentID.onClick.AddListener(OnClickAgentID);
        btnMachineID.onClick.AddListener(OnClickMachineID);
        btnSaveSettings.onClick.AddListener(OnClickSaveSettings);


        btnErrorCode.onClick.AddListener(OnClickErrorCode);
        //goReporter = GOFind.FindObjectIncludeInactive("Reporter");
        //goTestMgrBaseBtn = GameObject.Find("INSTANCE/Test/Test Manager/Button");


        btnSasConnection.onClick.AddListener(OnClickSasConnection); 
        btnSasAccount.onClick.AddListener(OnClickSasAccount); 
        btnSasInOutScale.onClick.AddListener(OnClickSasInOutScale);




        tgIsDebug.isOn = _consoleBB.Instance.isDebug;
        tgIsDebug.onValueChanged.AddListener(OnChangeIsDebug);

        tgIsUpdateInfo.isOn = _consoleBB.Instance.isUpdateInfo;
        tgIsUpdateInfo.onValueChanged.AddListener(OnChangeIsUpdateInfo);


        tgEnableDebugTool.isOn =  _consoleBB.Instance.enableTestTool;
        tgEnableDebugTool.onValueChanged.AddListener(OnChangeEnableTestTool);

        tgEnableReporterPage.isOn = _consoleBB.Instance.enableReporterPage;
        tgEnableReporterPage.onValueChanged.AddListener(OnChangeEnableReporterPage);


        tgIsConnectSas.isOn = _consoleBB.Instance.enableTestTool;

        tgIsConnectMoneyBox.isOn = _consoleBB.Instance.enableTestTool;



        tgIsTestOfficalIOT.isOn = PlayerPrefsUtils.isUseReleaseIot;
        tgIsTestOfficalIOT.onValueChanged.AddListener(OnChangeIsTestOfficalIOT);

        if (ApplicationSettings.Instance.isRelease)
        {
            tgIsTestOfficalIOT.interactable = false;
        }


        if (ApplicationSettings.Instance.isRelease) //先不放出去
        {
            tgIsConnectSas.interactable = false;
            tgIsConnectMoneyBox.interactable = false;
        }
        else
        {
            tgIsConnectSas.interactable = true;
            tgIsConnectMoneyBox.interactable = true;

            tgIsConnectSas.isOn = PlayerPrefsUtils.isUseSas;
            tgIsConnectSas.onValueChanged.AddListener(OnChangeIsConnectSas);

            tgIsConnectMoneyBox.isOn = _consoleBB.Instance.isConnectMoneyBox;
            tgIsConnectMoneyBox.onValueChanged.AddListener(OnChangeIsConnectMoneyBox);

        }

        if (ApplicationSettings.Instance.isRelease)//先不放出去
        {
            tgEnableReporterPage.interactable = false;
            tgEnableDebugTool.interactable = false;
        }
        else
        {
            tgEnableReporterPage.interactable = true;
            tgEnableDebugTool.interactable = true;
            tgEnableReporterPage.isOn = _consoleBB.Instance.enableReporterPage;
            tgEnableDebugTool.isOn = _consoleBB.Instance.enableTestTool;
        }


        if (ApplicationSettings.Instance.isRelease)
        {
            tgPauseAtPopupFreeSpinTrigger.interactable = false;
            tgPauseAtPopupJackpotGame.interactable = false;
            tgPauseAtPopupJackpotOnline.interactable = false;
        }
        else
        {
            tgPauseAtPopupFreeSpinTrigger.isOn = PlayerPrefsUtils.isPauseAtPopupFreeSpinTrigger;
            tgPauseAtPopupJackpotGame.isOn = PlayerPrefsUtils.isPauseAtPopupJackpotGame;
            tgPauseAtPopupJackpotOnline.isOn = PlayerPrefsUtils.isPauseAtPopupJackpotOnline;

            tgPauseAtPopupFreeSpinTrigger.onValueChanged.AddListener(OnChangePauseAtPopupFreeSpinTrigger);
            tgPauseAtPopupJackpotGame.onValueChanged.AddListener(OnChangePauseAtPopupJackpotGame);
            tgPauseAtPopupJackpotOnline.onValueChanged.AddListener(OnChangePauseAtPopupJackpotOnline);
        }





        tmpAppKey.text =ApplicationSettings.Instance.appKey;
        tmpSeverAddr.text = ApplicationSettings.Instance.resourceServer;
        tmpReportAddr.text = ApplicationSettings.Instance.reportUrl;

        tmpIOTAddress.text = IoTConst.GetDevParamURL;
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
    }


    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);
        InitParam();
    }

    //GameObject goTestMgrBaseBtn;
    //GameObject goReporter;
    void InitParam()
    {
        agentID = _consoleBB.Instance.agentID;
        machineID = _consoleBB.Instance.machineID;
        isChangeAgentID = false;
        isChangeMachineID = false;

        ctrlPage.PageSet(0, 10);




        tmpSofwareVer.text = $"{ApplicationSettings.Instance.appVersion}/{GlobalData.hotfixVersion}";
        tmpHotfixKey.text = GlobalData.hotfixKey;
        tmpAutoHotfixAddr.text = GlobalData.autoHotfixUrl;

        if (GlobalData.streamingAssetsVersion != null)
        {
            //tmpInstallVer.text = $"{ApplicationSettings.Instance.appVersion}/{GlobalData.streamingAssetsVersion["hotfix_version"].ToObject<string>()}";
            tmpInstallVer.text = $"{ApplicationSettings.Instance.appVersion}/{GlobalData.installHofixVersion}";
            
        }
        else
        {
            tmpInstallVer.text = "null";
        }


        OnPropertyChangeSasConnection();
        OnPropertyChangeSasPassword();
        OnPropertyChangeSasInOutScale();
    }


  


    void OnClickClose() => PageManager.Instance.ClosePage(this);

    bool isChangeAgentID = false;
    bool isChangeMachineID = false;
    async void  OnClickAgentID()
    {
        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Set Agent ID"),
                    ["isPlaintext"] = true,
                    ["contentLength"] = 4,
                }));

        if (res.value != null)
        {
            string idStr = (string)res.value;
            DebugUtils.Log($"键盘输入结果 ：{idStr}");

            bool isOk = false;
            try
            {
                //int i = id;

                if(idStr.Length == 4 && idStr[0] != '0')
                {
                    isOk = true;
                    agentID = idStr;
                    isChangeAgentID = _consoleBB.Instance.agentID != idStr;
                }
            }
            catch
            {

            }
            finally
            {
                if (!isOk)
                {
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The agent ID must be a 4-digit number"));
                }
            }

        }
    }


    async void OnClickMachineID()
    {
        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Set Machine ID"),
                    ["isPlaintext"] = true,
                    ["contentLength"] = 8,
                }));

        if (res.value != null)
        {
            string idStr = (string)res.value;
            DebugUtils.Log($"键盘输入结果 ：{idStr}");

            bool isOk = false;
            try
            {
                if (idStr.Length == 8 && idStr[0] != '0')
                {
                    isOk = true;
                    machineID = idStr;
                    isChangeMachineID = _consoleBB.Instance.machineID != idStr;
                }
            }
            catch
            {

            }
            finally
            {
                if (!isOk)
                {
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The machine ID must be an 8-digit number"));
                }
            }

        }
    }

    void OnClickSaveSettings()
    {
        if (isChangeMachineID || isChangeAgentID)
        {
            isChangeMachineID = false;
            isChangeAgentID = false;

            //string oldAgentID = _consoleBB.Instance.agentID;
            //string oldMachineID = _consoleBB.Instance.machineID;
            //_consoleBB.Instance.agentID = agentID;
            //_consoleBB.Instance.machineID = machineID;

            MachineDataUtils.RequestSetLineIDMachineID(int.Parse(agentID), int.Parse(machineID),
            (res)=>{
                SBoxPermissionsData data = res as SBoxPermissionsData;
                if (data.result == 0)
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Successfully saved"));
                else
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Save failed"));
            },
            (err) =>
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T(err.msg));
            });

            /*
            MachineDataManager.Instance.RequestWriteConf(_consoleBB.Instance.sboxConfData, (res) =>
            {
                SBoxPermissionsData data = res as SBoxPermissionsData;

                if(data.result == 0)
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Successfully saved"));
                else
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Save failed"));

                MachineDataManager.Instance.RequestReadConf((data) =>
                {
                    SBoxConfData res = (SBoxConfData)data;
                    _consoleBB.Instance.sboxConfData = res;
                }, (BagelCodeError err) =>
                {
                    DebugUtil.LogError(err.msg);
                });

            }, (BagelCodeError err) =>
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T(err.msg));

                MachineDataManager.Instance.RequestReadConf((data) =>
                {
                    SBoxConfData res = (SBoxConfData)data;
                    _consoleBB.Instance.sboxConfData = res;
                }, (BagelCodeError err) =>
                {
                    DebugUtil.LogError(err.msg);
                });
            });*/
        }
        else
        {
            TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The settings have not changed and do not need to be saved"));
        }
    }
   void OnClickErrorCode()
    {
        List<string> codeInfo = new List<string>();

        codeInfo.Add($"【Software Code】");
        Type staticClassType = typeof(Code);
        // 获取静态类的所有字段
        FieldInfo[] fields = staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static);
        // 遍历字段并打印字段名和值
        foreach (FieldInfo field in fields)
        {
            // 获取字段的值
            object value = field.GetValue(null); // 对于静态字段，传递null作为实例参数
            // 打印字段名和值
            //DebugUtil.Log($"【Code】{value}: {field.Name}");
            codeInfo.Add($"{value}: {field.Name}");
        }

        PageManager.Instance.OpenPage(PageName.Console001PopupConsoleNotice,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Error Code Query"),
                    ["content"] = codeInfo,
                    //["AutoCloseMS"] = 10000,
                }));
    }



    #region Sas
    async void OnClickSasConnection()
    {

        Func<string, string> chekParam1Func = (res) =>
        {
            if (string.IsNullOrEmpty(res))
                return string.Format(I18nMgr.T("The {0} cannot be empty"), I18nMgr.T("Account"));
            return null;
        };

        Func<string, string> chekParam2Func = (res) =>
        {
            if (string.IsNullOrEmpty(res))
                return string.Format(I18nMgr.T("The {0} cannot be empty"), I18nMgr.T("Password"));
            return null;
        };

        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetParameter002,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Configure Sas Server Connection"),
                    ["paramName1"] = I18nMgr.T("Server IP Address:"),
                    ["paramName2"] = I18nMgr.T("Server Port Number:"),
                    ["checkParam1Func"] = chekParam1Func,
                    ["checkParam2Func"] = chekParam2Func,
                }
            ));

        if (res.value != null)
        {
            List<string> lst = (List<string>)res.value;
            try
            {
                _consoleBB.Instance.sasConnection = $"{lst[0]}:{lst[1]}";
                //_consoleBB.Instance.remoteControlAccount = lst[0];
                // _consoleBB.Instance.remoteControlPassword = lst[1];
                OnPropertyChangeSasConnection();
                MachineDeviceCommonBiz.Instance.CheckSas();
            }
            catch
            {

            }
        }

    }
    void OnPropertyChangeSasConnection(EventData res = null)
    {
        btnSasConnection.transform.Find("Text").GetComponent<TextMeshProUGUI>().text
            = _consoleBB.Instance.sasConnection  +" " +
                (_consoleBB.Instance.isSasConnect ? "<sprite name=\"icon_link\" color=\"#81F300\" >" : "<sprite name=\"icon_link\" color=\"#666666\" >");
    }




    async void OnClickSasAccount()
    {
        Func<string, string> chekParam1Func = (res) =>
        {
            if (string.IsNullOrEmpty(res))
                return string.Format(I18nMgr.T("The {0} cannot be empty"), I18nMgr.T("Account"));
            return null;
        };

        Func<string, string> chekParam2Func = (res) =>
        {
            if (string.IsNullOrEmpty(res))
                return string.Format(I18nMgr.T("The {0} cannot be empty"), I18nMgr.T("Password"));
            return null;
        };

        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetParameter002,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Set Sas Account"),
                    ["paramName1"] = I18nMgr.T("Account:"),
                    ["paramName2"] = I18nMgr.T("Password:"),
                    ["checkParam1Func"] = chekParam1Func,
                    ["checkParam2Func"] = chekParam2Func,
                }
            ));

        if (res.value != null)
        {
            List<string> lst = (List<string>)res.value;
            try
            {

                _consoleBB.Instance.sasAccount = lst[0];
                _consoleBB.Instance.sasPassword = lst[1];

                OnPropertyChangeSasPassword();
                MachineDeviceCommonBiz.Instance.CheckSas();
            }
            catch
            {

            }
        }
    }
    void OnPropertyChangeSasPassword(EventData res = null)
    {
        btnSasAccount.transform.Find("Text").GetComponent<TextMeshProUGUI>().text
            = $"{_consoleBB.Instance.sasAccount}\n{_consoleBB.Instance.sasPassword}";
    }




    async void OnClickSasInOutScale()
    {

        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting001,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Sas In Out Scale"),
                    ["unitLeft"] = I18nMgr.T("Bill"),
                    ["unitRight"] = I18nMgr.T("_CON1f"),//I18nMgr.T("Credit"),
                    ["valueMax"] = 100000,
                    ["valueMin"] = 1,
                    ["valueCur"] = _consoleBB.Instance.sasInOutScale, // 1次多少分
                })
        );


        if (res.value != null)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)res.value;
            int scoreUpDownScale = (int)data["valueCur"];

            _consoleBB.Instance.sasInOutScale = scoreUpDownScale;
            OnPropertyChangeSasInOutScale();
            /*
            MachineDataUtils.RequestSetCoinInCoinOutScale(null, null, null, scoreUpDownScale,
            (res) => {
                SBoxPermissionsData data = res as SBoxPermissionsData;
                if (data.result == 0)
                {
                    btnScoreScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.scoreUpDownScale}";
                }
                else
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Save failed, clear first with a reset code."));
            },
            (err) =>
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T(err.msg));
            });*/
        }

    }

    void OnPropertyChangeSasInOutScale(EventData res = null)
    {
        btnSasInOutScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text
            = $"1:{_consoleBB.Instance.sasInOutScale}";
    }
    #endregion






    void OnChangeIsDebug(bool isOn)
    {
        _consoleBB.Instance.isDebug = isOn;
    }

    void OnChangeIsUpdateInfo(bool isOn)
    {
        _consoleBB.Instance.isUpdateInfo = isOn;
    }

    void OnChangeEnableTestTool(bool isOn)
    {
        _consoleBB.Instance.enableTestTool = isOn;
        //TestManager.Instance.SetToolActive(isOn);
        TestUtils.CheckTestManager();
    }

    void OnChangeEnableReporterPage(bool isOn)
    {
        _consoleBB.Instance.enableReporterPage = isOn;
        //if (goReporter != null) goReporter.SetActive(isOn);
        TestUtils.CheckReporter();
    }

    void OnChangeIsTestOfficalIOT(bool isOn)
    {
        PlayerPrefsUtils.isUseReleaseIot = isOn;
        tmpIOTAddress.text = IoTConst.GetDevParamURL;
        MachineDeviceCommonBiz.Instance.CheckIOT();
    }

    void OnChangeIsConnectSas(bool isOn)
    {
        PlayerPrefsUtils.isUseSas = isOn;

        MachineDeviceCommonBiz.Instance.CheckSas();

    }
    void OnChangeIsConnectMoneyBox(bool isOn)
    {
        _consoleBB.Instance.isConnectMoneyBox = isOn;

        MachineDeviceCommonBiz.Instance.CheckMoneyBox();
    }

    void OnChangePauseAtPopupFreeSpinTrigger(bool isOn)
    {
        PlayerPrefsUtils.isPauseAtPopupFreeSpinTrigger = isOn;
    }
    void OnChangePauseAtPopupJackpotGame(bool isOn)
    {
        PlayerPrefsUtils.isPauseAtPopupJackpotGame = isOn;
    }
    void OnChangePauseAtPopupJackpotOnline(bool isOn)
    {
        PlayerPrefsUtils.isPauseAtPopupJackpotOnline = isOn;
    }

}
