using Game;
using GameMaker;
using IOT;
using PssOn00152;
using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



//using _DeviceInfo = GameMaker.DeviceInfo;

using _consoleBB = PssOn00152.ConsoleBlackboard02;

namespace Console001
{

    public class PageConsoleMachineSettings : PageCorBase
    {


        public Button btnClose, btnCoinInScale, btnCoinOutScale, btnScoreScale, btnPrintOut, btnBillIn, btnScoreUpLongClickScale,
            btnChangePwdShift, btnChangePwdManager, btnChangePwdAdmin,
            btnMaxCoinInOutRecord, btnMaxGameRecord, btnMaxEventRecord, btnMaxErrorRecord, btnMaxBusinessDayRecord,
            btnFlipScreen,
            btnBillValidatorModel, btnPrinterModel,
            btnJpPercent,
            btnIOTAccessMethods,
            btnAgentID, btnMachineID,
            btnRemoteControlSetting, btnRemoteControlAccount,
            btnBonusReportSetting,
            btnJackpotGamePercent,
            btnLevel, btnWaveScore;

        public GameObject goMaxJPGrand, goMaxJPMajor, goMaxJPMinor, goMaxJPMini, goMinJPGrand, goMinJPMajor, goMinJPMinor, goMinJPMini,
            goBetAllowed, goBetAllowed02,
            goPrinterConnect, goBillValidatorConnect,
            goCoding,
            goDifficulty,
            goLanguage;

        public Toggle tgJackpotOnline, tgIot, tgCoinOutImmediately, tgCheckHotfixMultipleTimes, tgBillValidator, tgPrinter,
            tgRemoteControl,
            tgBonusReport;


        public TextMeshProUGUI tmpChannelID,// tmpAgentID, tmpMachineID ,
            tmpMachineName, tmpDifficulty, tmpJpScoreRate,
            tmpIOTAddr, tmpBonusReportAddr;

        public PageController ctrlPage;


        //设置游戏难度-【没有次功能】
        //TMP_Dropdown drpDifficulty;

        TMP_Dropdown drpLanguage;

        TextMeshProUGUI tmpMaxJPGrand, tmpMaxJPMajor, tmpMaxJPMinor, tmpMaxJPMini, tmpMinJPGrand, tmpMinJPMajor, tmpMinJPMinor, tmpMinJPMini;



        TextMeshProUGUI tmpPrinterConnect, tmpBillValidatorConnect;

        const string COR_IS_DEVICE_CONNECT = "COR_IS_DEVICE_CONNECT";

        MessageDelegates onPropertyChangedEventDelegates;

        private void Awake()
        {

            onPropertyChangedEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     //{ "@console/betAllowList", OnPropertyChangeBetAllowList},
                     { "@console/isConnectBiller", OnPropertyChangeIsConnectBiller},
                     { "@console/isConnectPrinter", OnPropertyChangeIsConnectPrinter},
                     { "@console/isConnectIot", OnPropertyChangeIsConnectIot},
                     { "@console/isConnectRemoteControl", OnPropertyChangeIsConnectRemoteControl}
                 }
             );

            tgJackpotOnline.isOn = _consoleBB.Instance.isJackpotOnLine;
            tgJackpotOnline.onValueChanged.AddListener(OnChangeJackpotOnline);

            tgIot.isOn = _consoleBB.Instance.isUseIot;
            tgIot.onValueChanged.AddListener(OnChangeIot);

            tgCoinOutImmediately.isOn = _consoleBB.Instance.isCoinOutImmediately;
            tgCoinOutImmediately.onValueChanged.AddListener(OnCoinOutImmediately);


            tgCheckHotfixMultipleTimes.isOn = PlayerPrefsUtils.isCheckHotfixMultipleTimes;
            tgCheckHotfixMultipleTimes.onValueChanged.AddListener(OnChangeHotfixMultipleTimes);


            tgBillValidator.isOn = _consoleBB.Instance.isUseBiller;
            tgBillValidator.onValueChanged.AddListener(OnChangeIsUseBiller);

            tgPrinter.isOn = _consoleBB.Instance.isUsePrinter;
            tgPrinter.onValueChanged.AddListener(OnChangeIsUsePrinter);

            tgRemoteControl.isOn = _consoleBB.Instance.isUseRemoteControl;
            tgRemoteControl.onValueChanged.AddListener(OnChangeIsUseRemoteControl);


            tgBonusReport.isOn = _consoleBB.Instance.isUseBonusReport;
            tgBonusReport.onValueChanged.AddListener(OnChangeIsUseBonusReportControl);

            btnClose.onClick.AddListener(OnClickClose);



            goCoding.GetComponent<Button>().onClick.AddListener(OnClickLicense);

            btnFlipScreen.onClick.AddListener(OnClickFlipScreen);

            btnChangePwdShift.GetComponent<Button>().onClick.AddListener(OnClickSetPasswordShift);
            btnChangePwdManager.GetComponent<Button>().onClick.AddListener(OnClickSetPasswordManager);
            btnChangePwdAdmin.GetComponent<Button>().onClick.AddListener(OnClickSetPasswordAdmin);



            btnBillValidatorModel.GetComponent<Button>().onClick.AddListener(OnClickBillValidatorModel);

            btnPrinterModel.GetComponent<Button>().onClick.AddListener(OnClickPrinterModel);


            tmpMaxJPGrand = goMaxJPGrand.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmpMinJPGrand = goMinJPGrand.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            tmpMaxJPMajor = goMaxJPMajor.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmpMinJPMajor = goMinJPMajor.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            tmpMaxJPMinor = goMaxJPMinor.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmpMinJPMinor = goMinJPMinor.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            tmpMaxJPMini = goMaxJPMini.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            tmpMinJPMini = goMinJPMini.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            //tmpPrinterConnect = goPrinterConnect.transform.Find("Text (2)").GetComponent<TextMeshProUGUI>();
            //tmpBillValidatorConnect = goBillValidatorConnect.transform.Find("Text (2)").GetComponent<TextMeshProUGUI>();

            //btnScoreUpLongClickScale.onClick.AddListener(OnClickScoreUpLongClickScale);
            btnScoreScale.onClick.AddListener(OnClickScoreScale);
            btnCoinInScale.onClick.AddListener(OnClickCoinInScale);
            btnCoinOutScale.onClick.AddListener(OnClickCoinOutScale);
            btnPrintOut.onClick.AddListener(OnClickPrintOutScale);
            btnBillIn.onClick.AddListener(OnClickBillInScale);
            btnJpPercent.onClick.AddListener(OnClickJackpotPercent);

            btnIOTAccessMethods.onClick.AddListener(OnClickIOTAccessMethods);

            btnRemoteControlSetting.onClick.AddListener(OnClickRemoteControlSetting);

            btnRemoteControlAccount.onClick.AddListener(OnClickRemoteControlAccount);


            btnBonusReportSetting.onClick.AddListener(OnClickBonusReportSetting);


            btnJackpotGamePercent.onClick.AddListener(OnClickJackpotGamePercent);

            long maxRecord = _consoleBB.Instance.coinInOutRecordMax;
            btnMaxCoinInOutRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = maxRecord.ToString();
            btnMaxCoinInOutRecord.onClick.AddListener(OnClickMaxCoinInOutRecord);

            maxRecord = _consoleBB.Instance.gameRecordMax; //BlackboardUtils.GetValue<long>("@console/maxGameRecord");
            btnMaxGameRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = maxRecord.ToString();
            btnMaxGameRecord.onClick.AddListener(OnClickMaxGameRecord);

            maxRecord = _consoleBB.Instance.eventRecordMax;// BlackboardUtils.GetValue<long>("@console/maxEventRecord");
            btnMaxEventRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = maxRecord.ToString();
            btnMaxEventRecord.onClick.AddListener(OnClickMaxEventRecord);

            maxRecord = _consoleBB.Instance.errorRecordMax;// BlackboardUtils.GetValue<long>("@console/maxErrorRecord");
            btnMaxErrorRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = maxRecord.ToString();
            btnMaxErrorRecord.onClick.AddListener(OnClickMaxErrorRecord);

            maxRecord = _consoleBB.Instance.businiessDayRecordMax;
            btnMaxBusinessDayRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = maxRecord.ToString();
            btnMaxBusinessDayRecord.onClick.AddListener(OnClickMaxBusinessDayRecord);


            long perCredit2Ticket = _consoleBB.Instance.coinOutScaleTicketPerCredit;//BlackboardUtils.GetValue<long>("@console/coinOutScalePerCredit2Ticket");
            long perTicket2Credit = _consoleBB.Instance.coinOutScaleCreditPerTicket;//BlackboardUtils.GetValue<long>("@console/coinOutScalePerTicket2Credit");
            string str = perCredit2Ticket > 1 ? $"{perCredit2Ticket}:1" : $"1:{perTicket2Credit}";
            btnCoinOutScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = str;


            btnCoinInScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.coinInScale}";

            //btnScoreUpLongClickScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.scoreUpScaleLongClick}";

            btnScoreScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.scoreUpDownScale}";

            btnBillIn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.billInScale}";

            btnPrintOut.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.printOutScale}";


            btnJpPercent.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{(float)_consoleBB.Instance.jackpotPercent / 1000f}";




            btnAgentID.onClick.AddListener(OnClickAgentIDMachineID);
            btnMachineID.onClick.AddListener(OnClickAgentIDMachineID);




            btnLevel.onClick.AddListener(OnClickLevel);
            //btnLevel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{MachineDeviceCommonBiz.Instance.GetLevel()}";
            btnLevel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{MachineDeviceCommonBiz.Instance.GetLevel()}";
            


            btnWaveScore.onClick.AddListener(OnClickWaveScore);
            btnWaveScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{MachineDeviceCommonBiz.Instance.GetWaveScore()}";




            // 游戏难度:
            /*
            drpDifficulty = goDifficulty.GetComponent<Dropdown>();
            drpDifficulty.options = new List<Dropdown.OptionData>();
            int maxGameDifficulty = BlackboardUtils.GetValue<int>("@console/maxDifficulty");
            int minGameDifficulty = BlackboardUtils.GetValue<int>("@console/minDifficulty");
            for (int i= minGameDifficulty; i<= maxGameDifficulty; i++)
            {
                drpDifficulty.options.Add(new Dropdown.OptionData($"{i}"));
                //drpDifficulty.options.Add(new Dropdown.OptionData("Option 1"));
            }
            // 更新下拉列表显示
            drpDifficulty.RefreshShownValue();
            drpDifficulty.value = BlackboardUtils.GetValue<int>("@console/difficulty");
            drpDifficulty.onValueChanged.AddListener(OnGameDifficultyChange);
            */

            /*
            drpDifficulty = goDifficulty.GetComponent<TMP_Dropdown>();
            drpDifficulty.options = new List<TMP_Dropdown.OptionData>();
            int maxGameDifficulty = BlackboardUtils.GetValue<int>("@console/maxDifficulty");
            int minGameDifficulty = BlackboardUtils.GetValue<int>("@console/minDifficulty");
            for (int i = minGameDifficulty; i <= maxGameDifficulty; i++)
            {
                //drpDifficulty.options.Insert(j++, new TMP_Dropdown.OptionData($"{i}"));
                drpDifficulty.options.Add(new TMP_Dropdown.OptionData ($"{i}"));
            }
            // 更新下拉列表显示
            drpDifficulty.RefreshShownValue();
            drpDifficulty.value = BlackboardUtils.GetValue<int>("@console/difficulty");
            drpDifficulty.onValueChanged.AddListener(OnGameDifficultyChange);
            */


            drpLanguage = goLanguage.GetComponent<TMP_Dropdown>();
            drpLanguage.options = new List<TMP_Dropdown.OptionData>();

            TableSupportLanguageItem[] tableSupportLanguage =
                 _consoleBB.Instance.tableSupportLanguage;
            //BlackboardUtils.GetValue<List<TableSupportLanguageItem>>("@console/tableSupportLanguage");

            //TableSysSettingItem tableSysSetting = BlackboardUtils.GetValue<TableSysSettingItem>("@console/tableSysSetting");

            string language = _consoleBB.Instance.language;
            //BlackboardUtils.GetValue<string>("@console/language");
            int curIndexLanguage = -1;
            for (int i = 0; i < tableSupportLanguage.Length; i++)
            {
                //drpDifficulty.options.Insert(j++, new TMP_Dropdown.OptionData($"{i}"));
                drpLanguage.options.Add(new TMP_Dropdown.OptionData($"{tableSupportLanguage[i].name}"));
                if (language == tableSupportLanguage[i].number)
                    curIndexLanguage = i;
            }
            // 更新下拉列表显示
            drpLanguage.RefreshShownValue();
            drpLanguage.onValueChanged.AddListener(OnLanguageChange);
            drpLanguage.value = curIndexLanguage;

        }

        /*
        private void OnGameDifficultyChange(int index)
        {
            _consoleBB.Instance.difficulty = index;
            //BlackboardUtils.SetValue<int>("@console/difficulty", index);
        }*/
        private void OnLanguageChange(int index)
        {
            TableSupportLanguageItem[] tableSupportLanguage =
                _consoleBB.Instance.tableSupportLanguage;
            //BlackboardUtils.GetValue<List<TableSupportLanguageItem>>("@console/tableSupportLanguage");

            _consoleBB.Instance.language = tableSupportLanguage[index].number;
            //BlackboardUtils.SetValue<string>("@console/language", tableSupportLanguage[index].number);
        }
        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_DEVICE_EVENT, OnCodeCompleted);

            InitParam();
        }

        private void OnDisable()
        {
            isNeedRefreshUIBetLst = true;

            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_DEVICE_EVENT, OnCodeCompleted);

            ClearCor(COR_IS_DEVICE_CONNECT);
        }




        void OnCodeCompleted(EventData res)
        {
            if (res.name == GlobalEvent.CodeCompleted)
            {
                tmpDifficulty.text = _consoleBB.Instance.difficultyName;
            }
        }

        void InitParam()
        {

            RefreshUIBetLst();

            tmpDifficulty.text = _consoleBB.Instance.difficultyName;

            tgBonusReport.isOn = _consoleBB.Instance.isUseBonusReport;

            /*
            btnChangePwdShift.interactable = true;
            btnChangePwdManager.interactable = _consoleBB.Instance.curPermissions >= 2;
            btnChangePwdAdmin.interactable = _consoleBB.Instance.curPermissions >= 3;
            */

            /*
             * 超级管理员可以改“自己”或“管理员”或“用户”的密码。
             * 管理员可以改“自己”或“用户”的密码。
             * 用户只能改“自己”的密码。
            btnChangePwdShift.gameObject.SetActive(true);
            btnChangePwdManager.gameObject.SetActive(_consoleBB.Instance.curPermissions >= 2);
            btnChangePwdAdmin.gameObject.SetActive(_consoleBB.Instance.curPermissions >= 3);
            */

            // 那个账号就只能改那个账号密码
            btnChangePwdShift.gameObject.SetActive(true);
            btnChangePwdManager.gameObject.SetActive(_consoleBB.Instance.curPermissions >= 2);
            btnChangePwdAdmin.gameObject.SetActive(_consoleBB.Instance.curPermissions >= 3);


            tgJackpotOnline.isOn = _consoleBB.Instance.isJackpotOnLine;

            //tmpMachineID.text = _consoleBB.Instance.machineID;

            //tmpAgentID.text = _consoleBB.Instance.agentID;

            tmpChannelID.text = _consoleBB.Instance.channelID;

            tmpMachineName.text = ApplicationSettings.Instance.gameTheme; // _consoleBB.Instance.machineName;




            tmpJpScoreRate.text = _consoleBB.Instance.jackpotScoreRate.ToString();


            tmpIOTAddr.text = IoTConst.GetDevParamURL;

            CheckAgentIDMachineID();




            for (int i = _consoleBB.Instance.betAllowList.Count; i < goBetAllowed.transform.childCount; i++)
            {
                Transform tfmBet = goBetAllowed.transform.GetChild(i);
                tfmBet.gameObject.SetActive(false);
            }

            List<BetAllow> betAllowList = _consoleBB.Instance.betAllowList;
            //for (int i= 0; i < goBetAllowed.transform.childCount; i++)
            for (int i = 0; i < betAllowList.Count; i++)
            {
                Transform tfmBet = goBetAllowed.transform.GetChild(i);
                Toggle tg = tfmBet.GetComponent<Toggle>();
                tg.isOn = betAllowList[i].allowed == 1;
                tg.onValueChanged.AddListener((isOn) => OnValueChangeBetAllowed(tfmBet.GetSiblingIndex(), isOn, tg));
                //tfmBet.Find("Button Bet Set/Label").GetComponent<Text>().text = betAllowList[i].value.ToString();
                tfmBet.Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text = betAllowList[i].value.ToString();

                int idx = i;

                tfmBet.Find("Button Bet Set").GetComponent<Button>().onClick.RemoveAllListeners();
                tfmBet.Find("Button Bet Set").GetComponent<Button>().onClick.AddListener(() => { OnClickBetSet(idx); });

                tfmBet.Find("Button Up").GetComponent<PIDButtonX>().onClickDown.RemoveAllListeners();
                tfmBet.Find("Button Up").GetComponent<PIDButtonX>().onClickUp.RemoveAllListeners();
                tfmBet.Find("Button Up").GetComponent<PIDButtonX>().onClickUp.AddListener(() => { OnClickBetUp(idx, true); });
                tfmBet.Find("Button Up").GetComponent<PIDButtonX>().onClickDown.AddListener(() => { OnClickBetUp(idx, false); });

                tfmBet.Find("Button Down").GetComponent<PIDButtonX>().onClickDown.RemoveAllListeners();
                tfmBet.Find("Button Down").GetComponent<PIDButtonX>().onClickUp.RemoveAllListeners();
                tfmBet.Find("Button Down").GetComponent<PIDButtonX>().onClickUp.AddListener(() => { OnClickBetDown(idx, true); });
                tfmBet.Find("Button Down").GetComponent<PIDButtonX>().onClickDown.AddListener(() => { OnClickBetDown(idx, false); });
            }

            for (int i = _consoleBB.Instance.betAllowList.Count; i < goBetAllowed02.transform.childCount; i++)
            {
                Transform tfmBet = goBetAllowed02.transform.GetChild(i);
                tfmBet.gameObject.SetActive(false);
            }
            for (int i = 0; i < _consoleBB.Instance.betAllowList.Count; i++)
            {
                Transform tfmBet = goBetAllowed02.transform.GetChild(i);
                Toggle tg = tfmBet.GetComponent<Toggle>();
                tg.isOn = betAllowList[i].allowed == 1;
                tg.onValueChanged.AddListener((isOn) => OnValueChangeBetAllowed(tfmBet.GetSiblingIndex(), isOn, tg));
                //tfmBet.Find("Button Bet Set/Label").GetComponent<Text>().text = betAllowList[i].value.ToString();
                tfmBet.Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text = betAllowList[i].value.ToString();

            }



            btnJackpotGamePercent.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{_consoleBB.Instance.jackpotGamePercent}";





            OnPropertyChangeIsConnectIot();
            OnPropertyChangeIsConnectBiller();
            OnPropertyChangeIsConnectPrinter();
            OnPropertyChangeIsConnectRemoteControl();
            OnPropertyChangeIsUseBonusReport();
            OnPropertyChangeRemoteControlAccount();

            //btnIOTAccessMethods.interactable = _consoleBB.Instance.isIot;
            //btnBillValidatorModel.interactable = _consoleBB.Instance.isUseBiller;
            //btnPrinterModel.interactable = _consoleBB.Instance.isUsePrinter;


            /* 检查设备状态：
            if (ApplicationSettings.Instance.isMachine)
            {
                DoCor(COR_IS_DEVICE_CONNECT, DoTaskRepeat(() => {

                    tmpPrinterConnect.text = SBoxSandbox.PrinterState() >= 0 ? "Yes" : "No";
                    tmpBillValidatorConnect.text = SBoxSandbox.BillState() >= 0 ?"Yes" : "No";
                },1000));
            }*/

            ctrlPage.PageSet(0, 10);
        }

        const string COR_DELAY_CHECK_ID = "COR_DELAY_CHECK_ID";
        void CheckAgentIDMachineID()
        {

            Action callback = () =>
            {
                btnAgentID.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _consoleBB.Instance.agentID;
                btnMachineID.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _consoleBB.Instance.machineID;

                if (_consoleBB.Instance.isCurAdministrator)
                {
                    btnAgentID.interactable = true;
                    btnMachineID.interactable = true;
                }
                else
                {
                    // SBoxIdea.IsMachineIdReady() 只正对666666、88888888 有效
                    btnAgentID.interactable = SBoxIdea.IsMachineIdReady() ? false : true;
                    btnMachineID.interactable = SBoxIdea.IsMachineIdReady() ? false : true;
                }
            };

            callback();

            DoCor(COR_DELAY_CHECK_ID, DoTask(callback, 500));
        }




        bool isNeedRefreshUIBetLst = false;
        void RefreshUIBetLst()
        {
            isNeedRefreshUIBetLst = true;

            List<BetAllow> betAllowList = _consoleBB.Instance.betAllowList;

            for (int i = 0; i < betAllowList.Count; i++)
            {
                Transform tfmBet = goBetAllowed02.transform.GetChild(i);
                Toggle tg = tfmBet.GetComponent<Toggle>();
                tg.isOn = betAllowList[i].allowed == 1;
            }
            isNeedRefreshUIBetLst = false;
        }


        #region 设置压注列表
        void OnValueChangeBetAllowed(int index, bool isOn, Toggle tg)
        {

            if (isNeedRefreshUIBetLst)
                return;

            List<BetAllow> betAllowList = _consoleBB.Instance.betAllowList;
            //BlackboardUtils.GetValue<List<BetAllow>>("@console/betAllowList");


            int _index = -1;
            int num = 0;
            for (int i = 0; i < betAllowList.Count; i++)
            {
                if (betAllowList[i].allowed == 1)
                {
                    _index = i;
                    num++;
                }
            }

            if (index == _index && isOn == false && num == 1)
            {
                CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                {
                    text = I18nMgr.T("The betting list option does not support closing all options."),
                    type = CommonPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = delegate
                    {

                    },
                    isUseXButton = false,
                });

                tg.isOn = true;
                //RefreshUIBetLst();
                return;
            }

            betAllowList[index].allowed = isOn ? 1 : 0;

        }






        async void OnClickBetSet(int idx)
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Set Bet"),
                        ["isPlaintext"] = true,
                    }));

            if (res.value != null)
            {
                if (res.value != null)
                {
                    bool isErr = true;

                    int minBet = 1;
                    int maxBet = 10000;
                    try
                    {
                        int val = int.Parse((string)res.value);  // (long)res.value;

                        if (val >= minBet && val <= maxBet)
                        {
                            isErr = false;
                            //Variable<List<BetAllow>> vBetAllowList = BlackboardUtils.FindVariable<List<BetAllow>>("@console/betAllowList");
                            //vBetAllowList.value[idx].value = val;

                            _consoleBB.Instance.betAllowList[idx].value = val;


                            goBetAllowed.transform.GetChild(idx).Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text
                                = _consoleBB.Instance.betAllowList[idx].value.ToString();
                        }
                    }
                    catch { }

                    if (isErr)
                        TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The bet value must be between {0} and {1}"), minBet, maxBet));
                    //TipPopupHandler.Instance.OpenPopup($"The bet value must be between {minBet} and {maxBet}");
                }
            }

        }


        const string COR_BET_UP_DOWN = "COR_BET_UP_DOWN";
        void OnClickBetUp(int idx, bool isUp)
        {
            if (!isUp)
                DoCor(COR_BET_UP_DOWN, DoBetUp(idx));
            else
                ClearCor(COR_BET_UP_DOWN);
        }

        void OnClickBetDown(int idx, bool isUp)
        {
            if (!isUp)
                DoCor(COR_BET_UP_DOWN, DoBetDown(idx));
            else
                ClearCor(COR_BET_UP_DOWN);
        }


        /*IEnumerator DoBetDown(int idx)
        {
            List<BetAllow> betAllowList = BlackboardUtils.GetValue<List<BetAllow>>("@console/betAllowList");

            if (--betAllowList[idx].value < 0)// 减1 (这里只会有一次事件！！！)
                betAllowList[idx].value = 0;
            yield return new WaitForSeconds(2f);
            while (true)
            {

                if (--betAllowList[idx].value < 0)// 减1  (这里不会有事件！！！)
                    betAllowList[idx].value = 0;

                DebugUtil.Log($"{idx}-- {betAllowList[idx].value}");

                yield return new WaitForSeconds(0.5f);
            }
        }*/

        IEnumerator DoBetDown(int idx)
        {

            if (--_consoleBB.Instance.betAllowList[idx].value < 0)// 减1 (这里只会有一次事件！！！)
                _consoleBB.Instance.betAllowList[idx].value = 0;

            goBetAllowed.transform.GetChild(idx).Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text
                = _consoleBB.Instance.betAllowList[idx].value.ToString();

            yield return new WaitForSeconds(2f);
            while (true)
            {

                if (--_consoleBB.Instance.betAllowList[idx].value < 0)// 减1  (这里不会有事件！！！)
                    _consoleBB.Instance.betAllowList[idx].value = 0;

                goBetAllowed.transform.GetChild(idx).Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text
                    = _consoleBB.Instance.betAllowList[idx].value.ToString();

                yield return new WaitForSeconds(0.05f);
            }

        }


        IEnumerator DoBetUp(int idx)
        {

            if (++_consoleBB.Instance.betAllowList[idx].value > 10000)
                _consoleBB.Instance.betAllowList[idx].value = 10000;

            goBetAllowed.transform.GetChild(idx).Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text
                = _consoleBB.Instance.betAllowList[idx].value.ToString();

            yield return new WaitForSeconds(2f);
            while (true)
            {

                if (++_consoleBB.Instance.betAllowList[idx].value > 10000)
                    _consoleBB.Instance.betAllowList[idx].value = 10000;

                goBetAllowed.transform.GetChild(idx).Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text
                    = _consoleBB.Instance.betAllowList[idx].value.ToString();

                //DebugUtil.Log($"{idx}++ {vBetAllowList.value[idx].value}");
                yield return new WaitForSeconds(0.05f);
            }
        }


        void OnPropertyChangeRemoteControlAccount(EventData data = null)
        {
            btnRemoteControlAccount.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                $"{_consoleBB.Instance.remoteControlAccount}\n{_consoleBB.Instance.remoteControlPassword}";
        }

        void OnPropertyChangeIsConnectRemoteControl(EventData data = null)
        {
            btnRemoteControlSetting.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                _consoleBB.Instance.remoteControlSetting + " " +
                (_consoleBB.Instance.isConnectRemoteControl ? "<sprite name=\"icon_link\" color=\"#81F300\" >" : "<sprite name=\"icon_link\" color=\"#666666\" >");
        }


        void OnPropertyChangeIsUseBonusReport(EventData data = null)
        {
            btnBonusReportSetting.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _consoleBB.Instance.bonusReportSetting;

            tmpBonusReportAddr.text = _consoleBB.Instance.bnousReportUrl;
        }


        void OnPropertyChangeIsConnectIot(EventData data = null)
        {
            btnIOTAccessMethods.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                I18nMgr.T(_consoleBB.Instance.selectIOTAccessMethod) + " " +
                (_consoleBB.Instance.isConnectIot ? "<sprite name=\"icon_link\" color=\"#81F300\" >" : "<sprite name=\"icon_link\" color=\"#666666\" >");
        }
        void OnPropertyChangeIsConnectBiller(EventData data = null)
        {
            //Debug.LogError("OnPropertyChangeIsConnectBiller " + _consoleBB.Instance.isConnectBiller);
            btnBillValidatorModel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                _consoleBB.Instance.selectBillerModel + " " +
                (_consoleBB.Instance.isConnectBiller ? "<sprite name=\"icon_link\" color=\"#81F300\" >" : "<sprite name=\"icon_link\" color=\"#666666\" >");
        }

        void OnPropertyChangeIsConnectPrinter(EventData data = null)
        {
            //Debug.LogError("OnPropertyChangeIsConnectPrinter " + _consoleBB.Instance.isConnectPrinter);
            btnPrinterModel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                 _consoleBB.Instance.selectPrinterModel + " " +
                (_consoleBB.Instance.isConnectPrinter ? "<sprite name=\"icon_link\" color=\"#81F300\" >" : "<sprite name=\"icon_link\" color=\"#666666\" >");
        }


        void OnPropertyChangeBetAllowList(EventData data)
        {
            return;
            // 等数据刷新有些忙！！！
            List<BetAllow> betAllowList = (List<BetAllow>)data.value;
            for (int i = 0; i < goBetAllowed.transform.childCount; i++)
            {
                Transform tfmBet = goBetAllowed.transform.GetChild(i);
                Toggle tg = tfmBet.GetComponent<Toggle>();
                tg.isOn = betAllowList[i].allowed == 1;
                tfmBet.Find("Button Bet Set/Label").GetComponent<TextMeshProUGUI>().text = betAllowList[i].value.ToString();
            }
        }
        #endregion


        void OnChangeJackpotOnline(bool isOn)
        {
            MachineDataUtils.RequestSetIsJackpotOnLine(isOn, (res) =>
            {
                _consoleBB.Instance.isJackpotOnLine = isOn;
            },
            (err) =>
            {
            });
        }


        void OnCoinOutImmediately(bool isOn) => _consoleBB.Instance.isCoinOutImmediately = isOn;


        void OnChangeHotfixMultipleTimes(bool isOn)
        {
            PlayerPrefsUtils.isCheckHotfixMultipleTimes = isOn;
        }


        void OnChangeIot(bool isOn)
        {
            _consoleBB.Instance.isUseIot = isOn;
            MachineDeviceCommonBiz.Instance.CheckIOT();
            //btnIOTAccessMethods.interactable = _consoleBB.Instance.isIot;
            OnPropertyChangeIsConnectIot();
        }

        void OnChangeIsUseBiller(bool isOn)
        {
            _consoleBB.Instance.isUseBiller = isOn;
            //btnBillValidatorModel.interactable = _consoleBB.Instance.isUseBiller;
            //OnPropertyChangeIsConnectBiller();

            MachineDeviceCommonBiz.Instance.InitBiller(() => { }, (err) => { });
        }

        void OnChangeIsUsePrinter(bool isOn)
        {
            _consoleBB.Instance.isUsePrinter = isOn;
            //btnPrinterModel.interactable = _consoleBB.Instance.isUsePrinter;
            //OnPropertyChangeIsConnectPrinter();

            MachineDeviceCommonBiz.Instance.InitPrinter(() => { }, (err) => { });
        }



        void OnChangeIsUseRemoteControl(bool isOn)
        {
            _consoleBB.Instance.isUseRemoteControl = isOn;
            MachineDeviceCommonBiz.Instance.CheckMqttRemoteButtonController();
            OnPropertyChangeIsConnectRemoteControl();
        }

        void OnChangeIsUseBonusReportControl(bool isOn)
        {
            _consoleBB.Instance.isUseBonusReport = isOn;
            MachineDeviceCommonBiz.Instance.CheckBonusReport();
            OnPropertyChangeIsUseBonusReport();
        }


        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this);
        }







        void OnClickLicense()
        {

            EventCenter.Instance.EventTrigger<EventData>(MachineUIEvent.ON_MACHINE_UI_EVENT, new EventData(MachineUIEvent.ShowCoding));
        }


        void OnClickFlipScreen()
        {
            AndroidSystemHelper.Instance.ScreenFlip();
        }
        async void OnClickCoinOutScale()
        {


            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Ticket Out Scale"),
                        ["unitLeft"] = I18nMgr.T("Ticket"),
                        ["unitRight"] = I18nMgr.T("_CON1f"),//I18nMgr.T("Credit"),
                        ["valueMaxLeft"] = DefaultSettingsUtils.Config().maxCoinOutPerCredit2Ticket, //50
                        ["valueMaxRight"] = DefaultSettingsUtils.Config().maxCoinOutPerTicket2Credit,//200
                        ["valueLeft"] = _consoleBB.Instance.coinOutScaleTicketPerCredit, // 1分多少票
                        ["valueRight"] = _consoleBB.Instance.coinOutScaleCreditPerTicket, // 1票多少分
                    })
            );

            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                //DebugUtil.Log($"@@ 1分几票   {data["valueLeft"]};  1票多少分 {data["valueRight"]}");

                /*
                int perCredit2Ticket = (int)data["valueLeft"];
                int perTicket2Credit = (int)data["valueRight"];
                _consoleBB.Instance.coinOutScalePerCredit2Ticket = perCredit2Ticket;
                _consoleBB.Instance.coinOutScalePerTicket2Credit = perTicket2Credit;

                string str = perCredit2Ticket > 0 ? $"{perCredit2Ticket}:1" : $"1:{perTicket2Credit}";
                btnCoinOutScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = str;
                */

                int perCredit2Ticket = (int)data["valueLeft"];
                int perTicket2Credit = (int)data["valueRight"];

                MachineDataUtils.RequestSetCoinInCoinOutScale(null, perTicket2Credit, perCredit2Ticket, null,
                (res01) => {
                    SBoxPermissionsData data = res01 as SBoxPermissionsData;
                    if (data.result == 0)
                    {

                        string str = _consoleBB.Instance.coinOutScaleTicketPerCredit > 1 ?
                        $"{_consoleBB.Instance.coinOutScaleTicketPerCredit}:1" : $"1:{_consoleBB.Instance.coinOutScaleCreditPerTicket}";

                        //DebugUtil.Log("@@@ = " + str);
                        btnCoinOutScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = str;
                    }
                    else
                        TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Save failed, clear first with a reset code."));
                },
                (err) =>
                {
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T(err.msg));
                });

            }

        }
        async void OnClickCoinInScale()
        {
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Coin In Scale"),
                        ["unitLeft"] = I18nMgr.T("Coin"),
                        ["unitRight"] = I18nMgr.T("_CON1f"), //I18nMgr.T("Credit"),
                        ["valueMax"] = DefaultSettingsUtils.Config().maxCoinInScale,//200;
                        ["valueMin"] = DefaultSettingsUtils.Config().minCoinInScale,//200;
                        ["valueCur"] = _consoleBB.Instance.coinInScale, // 1币多少分
                    })
            );

            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                DebugUtils.Log($"@@ 1币多少分 {data["valueCur"]}");

                int coinInScale = (int)data["valueCur"];

                //_consoleBB.Instance.coinInScale = coinInScale;
                //btnCoinInScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{coinInScale}";



                MachineDataUtils.RequestSetCoinInCoinOutScale(coinInScale, null, null, null,
                (res) => {
                    SBoxPermissionsData data = res as SBoxPermissionsData;
                    if (data.result == 0)
                    {
                        btnCoinInScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{_consoleBB.Instance.coinInScale}";
                    }
                    else
                        TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Save failed, clear first with a reset code."));
                },
                (err) =>
                {
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T(err.msg));
                });
            }

        }


        async void OnClickScoreScale()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Score Short Click Scale"),
                        ["unitLeft"] = I18nMgr.T("Time"),
                        ["unitRight"] = I18nMgr.T("_CON1f"),//I18nMgr.T("Credit"),
                        ["valueMax"] = DefaultSettingsUtils.Config().maxScoreUpDownScale,
                        ["valueMin"] = DefaultSettingsUtils.Config().minScoreUpDownScale,
                        ["valueCur"] = _consoleBB.Instance.scoreUpDownScale, // 1次多少分
                    })
            );


            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                DebugUtils.Log($"@@ 1币多少分 {data["valueCur"]}");

                int scoreUpDownScale = (int)data["valueCur"];

                //_consoleBB.Instance.scoreUpDownScale = scoreUpDownScale;
                //btnScoreScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{scoreUpDownScale}";

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
                });
            }

        }


        /// <summary>
        /// 这个暂时不用
        /// </summary>
        async void OnClickScoreUpLongClickScale()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Score Up Long Click Scale"),
                        ["unitLeft"] = I18nMgr.T("Time"),
                        ["unitRight"] = I18nMgr.T("_CON1f"),//I18nMgr.T("Credit"),
                        ["valueMax"] = DefaultSettingsUtils.Config().maxScoreUpLongClickScale,
                        ["valueMin"] = DefaultSettingsUtils.Config().minScoreUpLongClickScale,
                        ["valueCur"] = _consoleBB.Instance.scoreUpScaleLongClick, // 1次多少分
                    })
            );


            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                DebugUtils.Log($"@@ 1币多少分 {data["valueCur"]}");

                int valueCur = (int)data["valueCur"];

                _consoleBB.Instance.scoreUpScaleLongClick = valueCur;

                btnScoreUpLongClickScale.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{valueCur}";
            }
        }

        async void OnClickPrintOutScale()
        {

            Debug.LogError("打印使用“退票比例”。");
            return;
#if false
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Print Out Scale"),
                        ["unitLeft"] = I18nMgr.T("Bill"),
                        ["unitRight"] = I18nMgr.T("_CON1f"),//I18nMgr.T("Credit"),
                        ["valueMax"] = DefaultSettingsUtils.Config().maxPrintOutScale,
                        ["valueMin"] = DefaultSettingsUtils.Config().minPrintOutScale,
                        ["valueCur"] = _consoleBB.Instance.printOutScale, // 1次多少分
                    })
);

            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                DebugUtil.Log($"@@ 1币多少分 {data["valueCur"]}");

                int valueCur = (int)data["valueCur"];

                _consoleBB.Instance.printOutScale =  valueCur;
                btnPrintOut.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{valueCur}";
            }
#endif
        }


        async void OnClickBillInScale()
        {

            Debug.LogError("入钞使用“投币比例”");
            return;

#if false
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSlideSetting001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Bill In Scale"),
                        ["unitLeft"] = I18nMgr.T("Bill"),
                        ["unitRight"] = I18nMgr.T("_CON1f"),//I18nMgr.T("Credit"),
                        ["valueMax"] = DefaultSettingsUtils.Config().maxBillInScale,
                        ["valueMin"] = DefaultSettingsUtils.Config().minBillInScale,
                        ["valueCur"] = _consoleBB.Instance.billInScale, // 1次多少分
                    })
            );

            if (res.value != null)
            {
                Dictionary<string, object> data = (Dictionary<string, object>)res.value;
                DebugUtil.Log($"@@ 1币多少分 {data["valueCur"]}");

                int valueCur = (int)data["valueCur"];

                _consoleBB.Instance.billInScale =  valueCur;
                btnBillIn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"1:{valueCur}";
            }
#endif
        }




        async void OnClickJackpotPercent()
        {
            /*
                                    ["valueMax"] = DefaultSettingsUtils.Config().maxJackpotPercent,
                                    ["valueMin"] = DefaultSettingsUtils.Config().minJackpotPercent,
                                    ["valueCur"] = _consoleBB.Instance.jackpotPercent, // 1次多少分
             */

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Jackpot Percent(0.001)"),
                        ["isPlaintext"] = true,
                    }));

            if (res.value != null)
            {

                int jpPerMinDef = DefaultSettingsUtils.Config().minJackpotPercent;
                int jpPerMaxDef = DefaultSettingsUtils.Config().maxJackpotPercent;

                int val = int.Parse((string)res.value);

                if (val < jpPerMinDef || val > jpPerMaxDef)
                {
                    TipPopupHandler.Instance.OpenPopup(
                        string.Format(I18nMgr.T("The {0} must be between {1} and {2}"), I18nMgr.T("Jackpot Percent"), jpPerMinDef, jpPerMaxDef));
                }
                else
                {
                    if (val != _consoleBB.Instance.jackpotPercent)
                    {
                        _consoleBB.Instance.jackpotPercent = val;
                        btnJpPercent.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{(float)_consoleBB.Instance.jackpotPercent / 1000f}";
                        TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Successfully saved"));
                    }
                }
            }
        }


        #region 用户修改密码
        /// <summary>
        /// 【待完善】切户用户权限（等完善算方法卡密码接口-这个方法可以删除）
        /// </summary>
        IEnumerator ResetUserPermissions()
        {
            bool isNext = false;
            bool isFinish = false;
            while (true)
            {
                yield return new WaitForSecondsRealtime(3);


                if (PageManager.Instance.IndexOf(PageName.Console001PopupConsoleKeyboard001) == -1
                    && PageManager.Instance.IndexOf(PageName.Console001PopupConsoleSetPassword001) == -1)
                {
                    string pwd = _consoleBB.Instance.curPermissions == 3 ? _consoleBB.Instance.passwordAdmin :
                        _consoleBB.Instance.curPermissions == 2 ? _consoleBB.Instance.passwordManager :
                      _consoleBB.Instance.curPermissions == 1 ? _consoleBB.Instance.passwordShift : "";
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        MachineDataManager.Instance.RequestCheckPassword(int.Parse(pwd), (res) =>
                        {
                            isNext = true;
                            isFinish = true;
                            DebugUtils.Log($"已切回用户权限; 密码: {pwd}");
                        }, (err) =>
                        {
                            isNext = true;
                            isFinish = false;
                            DebugUtils.LogError(err.msg);
                        });

                        yield return new WaitUntil(() => isNext == true);
                        isNext = false;

                        if (isFinish)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }

        const string COR_RESET_USER_PRTMISSIONS = "COR_RESET_USER_PRTMISSIONS";

        async void OnClickSetPasswordAdmin()
        {
            DoCor(COR_RESET_USER_PRTMISSIONS, ResetUserPermissions());

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Admin"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));

            if (res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwd}");


                MachineDataManager.Instance.RequestCheckPassword(int.Parse(pwd),
                async (res) =>
                {
                    SBoxPermissionsData data = res as SBoxPermissionsData;
                    if (data.result == 0 && data.permissions == 3)
                    {

                        EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,//Console001PopupConsoleSetPassword,
                            new EventData<Dictionary<string, object>>("",
                                new Dictionary<string, object>()
                                {
                                    ["title"] = I18nMgr.T("Reset Password: Admin"),
                                    ["pwdMustCount"] = 9,
                                })
                            );
                        if (res1.value != null)
                        {
                            int pwd01 = int.Parse((string)res1.value);

                            MachineDataManager.Instance.RequestChangePassword(pwd01, (res) =>
                            {
                                _consoleBB.Instance.passwordAdmin = $"{pwd01}"; // 修改成功
                                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password successfully changed"));
                            }, (err) =>
                            {
                                // 修改失败
                                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password modification failed"));
                            });
                            //修改密码！
                        }
                    }
                    else
                    {
                        PopupErrorPassword();
                    }
                }, (error) =>
                {
                    PopupErrorPassword();
                });

                /*
                if (pwd != _consoleBB.Instance.passwordAdmin)
                {
                    PopupErrorPassword();
                }
                else
                {

                    EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = I18nMgr.T("Reset Password: Admin"),
                            })
                        );
                    if (res1.value != null)
                    {
                        string pwdNew = (string)res1.value;
                        int pwd01 = int.Parse(pwdNew);

                        MachineDataManager.Instance.RequestChangePassword(pwd01, (res) =>
                        {
                            _consoleBB.Instance.passwordAdmin = $"{pwd01}"; // 修改成功
                            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password successfully changed"));
                        }, (err) =>
                        {
                            // 修改失败
                            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password modification failed"));
                        });
                        //修改密码！
                    }
                }*/
            }

        }
        async void OnClickSetPasswordAdmin001()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Admin"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 9,
                    }));

            if (res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwd}");

                if (pwd != _consoleBB.Instance.passwordAdmin)
                {
                    PopupErrorPassword();
                }
                else
                {

                    EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = I18nMgr.T("Reset Password: Admin"),
                                ["pwdMustCount"] = 9,
                            })
                        );
                    if (res1.value != null)
                    {
                        string pwdNew = (string)res1.value;
                        _consoleBB.Instance.passwordAdmin = pwdNew;
                    }
                }
            }

        }

        async void OnClickSetPasswordManager()
        {
            DoCor(COR_RESET_USER_PRTMISSIONS, ResetUserPermissions());

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Manager"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));

            if (res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwd}");

                MachineDataManager.Instance.RequestCheckPassword(int.Parse(pwd),
                async (res) =>
                {
                    SBoxPermissionsData data = res as SBoxPermissionsData;
                    if (data.result == 0 && data.permissions == 2)
                    {

                        EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,//Console001PopupConsoleSetPassword,
                            new EventData<Dictionary<string, object>>("",
                                new Dictionary<string, object>()
                                {
                                    ["title"] = I18nMgr.T("Reset Password: Manager"),
                                    ["pwdMustCount"] = 8,
                                })
                            );
                        if (res1.value != null)
                        {
                            int pwd01 = int.Parse((string)res1.value);

                            MachineDataManager.Instance.RequestChangePassword(pwd01, (res) =>
                            {
                                _consoleBB.Instance.passwordManager = $"{pwd01}"; // 修改成功
                                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password successfully changed"));
                            }, (err) =>
                            {
                                // 修改失败
                                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password modification failed"));
                            });
                            //修改密码！
                        }

                    }
                    else
                    {
                        PopupErrorPassword();
                    }
                }, (error) =>
                {
                    PopupErrorPassword();
                });
                /*
                if (pwd != _consoleBB.Instance.passwordManager)
                {
                    PopupErrorPassword();
                }
                else
                {
                    EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = I18nMgr.T("Reset Password: Manager"),
                            })
                        );
                    if (res1.value != null)
                    {
                        string pwdNew = (string)res1.value;
                        int pwd01 = int.Parse(pwdNew);

                        MachineDataManager.Instance.RequestChangePassword(pwd01, (res) =>
                        {
                            _consoleBB.Instance.passwordManager = $"{pwd01}"; // 修改成功
                            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password successfully changed"));
                        }, (err) =>
                        {
                            // 修改失败
                            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password modification failed"));
                        });
                        //修改密码！
                    }
                }*/
            }

        }

        async void OnClickSetPasswordManager001()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Manager/Admin"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));

            if (res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwd}");

                if (pwd != _consoleBB.Instance.passwordManager && pwd != _consoleBB.Instance.passwordAdmin)
                {
                    PopupErrorPassword();
                }
                else
                {
                    EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = I18nMgr.T("Reset Password: Manager"),
                                ["pwdMustCount"] = 8,
                            })
                        );
                    if (res1.value != null)
                    {
                        string pwdNew = (string)res1.value;
                        _consoleBB.Instance.passwordManager = pwdNew;
                    }
                }
            }

        }


        async void OnClickSetPasswordShift()
        {
            DoCor(COR_RESET_USER_PRTMISSIONS, ResetUserPermissions());

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Shift"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));

            if (res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwd}");

                MachineDataManager.Instance.RequestCheckPassword(int.Parse(pwd),
                async (res) =>
                {
                    SBoxPermissionsData data = res as SBoxPermissionsData;
                    if (data.result == 0 && data.permissions == 1)
                    {

                        EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,//Console001PopupConsoleSetPassword,
                            new EventData<Dictionary<string, object>>("",
                                new Dictionary<string, object>()
                                {
                                    ["title"] = I18nMgr.T("Reset Password: Shift"),
                                    ["pwdMustCount"] = 6,
                                })
                            );
                        if (res1.value != null)
                        {
                            int pwd01 = int.Parse((string)res1.value);

                            MachineDataManager.Instance.RequestChangePassword(pwd01, (res) =>
                            {
                                _consoleBB.Instance.passwordShift = $"{pwd01}"; // 修改成功
                                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password successfully changed"));
                            }, (err) =>
                            {
                                // 修改失败
                                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password modification failed"));
                            });
                            //修改密码！
                        }
                    }
                    else
                    {
                        PopupErrorPassword();
                    }
                }, (error) =>
                {
                    PopupErrorPassword();
                });

                /*
                if (pwd != _consoleBB.Instance.passwordShift)
                {
                    PopupErrorPassword();
                }
                else
                {
                    EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,//Console001PopupConsoleSetPassword,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = I18nMgr.T("Reset Password: Shift"),
                            })
                        );
                    if (res1.value != null)
                    {
                        string pwdNew = (string)res1.value;
                        DebugUtil.Log($"新的密码: {pwdNew}");
                        int pwd01 = int.Parse( pwdNew );

                        MachineDataManager.Instance.RequestChangePassword(pwd01, (res) =>
                        {
                            _consoleBB.Instance.passwordShift = $"{pwd01}"; // 修改成功
                            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password successfully changed"));
                        }, (err) =>
                        {
                            // 修改失败
                            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Account password modification failed"));
                        });
                        //修改密码！
                    }
                } */
            }

        }
        async void OnClickSetPasswordShift001()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Enter Password: Shift/Manager/Admin"),
                        ["isPlaintext"] = false,
                        ["contentLength"] = 8,
                    }));

            if (res.value != null)
            {
                string pwd = (string)res.value;
                DebugUtils.Log($"键盘输入结果 ：{pwd}");

                if (pwd != _consoleBB.Instance.passwordShift
                    && pwd != _consoleBB.Instance.passwordManager
                    && pwd != _consoleBB.Instance.passwordAdmin)
                {
                    PopupErrorPassword();
                }
                else
                {
                    EventData res1 = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetPassword001,//Console001PopupConsoleSetPassword,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = I18nMgr.T("Reset Password: Shift"),
                                ["pwdMustCount"] = 6,
                            })
                        );
                    if (res1.value != null)
                    {
                        string pwdNew = (string)res1.value;
                        DebugUtils.Log($"新的密码: {pwdNew}");
                        _consoleBB.Instance.passwordShift = pwdNew;
                    }
                }
            }

        }


        void PopupErrorPassword()
        {
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
            /*PageManager.Instance.OpenPage(PageName.Console001PopupConsoleCommon,
                new EventData<ErrorPopupInfo>("", new ErrorPopupInfo()
                {
                    isUseXButton = false,
                    buttonAutoClose1 = true,
                    buttonAutoClose2 = true,
                    type = ErrorPopupType.YesNo,
                    text = "Error Password",
                    buttonText1 = "Cancel",
                    buttonText2 = "Confirm",
                }));*/
        }
        #endregion





        async void OnClickMaxCoinInOutRecord()
        {
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Max Coin In Out Record"),
                    ["isPlaintext"] = true,
                }));

            if (res.value != null)
            {
                bool isErr = true;

                int minMaxCoinInOutRecord = DefaultSettingsUtils.Config().minMaxCoinInOutRecord;
                int maxMaxCoinInOutRecord = DefaultSettingsUtils.Config().maxMaxCoinInOutRecord;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;

                    if (val >= minMaxCoinInOutRecord
                        && val <= maxMaxCoinInOutRecord
                        )
                    {
                        isErr = false;
                        _consoleBB.Instance.coinInOutRecordMax = val;
                        btnMaxCoinInOutRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = val.ToString();
                    }

                }
                catch { }

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                        I18nMgr.T("Max Coin In Out Record"),
                        minMaxCoinInOutRecord, maxMaxCoinInOutRecord));
            }
        }

        async void OnClickMaxGameRecord()
        {
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Max Game Record"),
                        ["isPlaintext"] = true,
                    }));

            if (res.value != null)
            {
                bool isErr = true;

                int minMaxGameRecord = DefaultSettingsUtils.Config().minMaxGameRecord;
                int maxMaxGameRecord = DefaultSettingsUtils.Config().maxMaxGameRecord;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;

                    if (val >= minMaxGameRecord
                        && val <= maxMaxGameRecord
                        )
                    {
                        isErr = false;
                        _consoleBB.Instance.gameRecordMax = val;
                        btnMaxGameRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = val.ToString();
                    }
                }
                catch { }

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                        I18nMgr.T("Max Game Record"),
                        minMaxGameRecord, maxMaxGameRecord));
                //TipPopupHandler.Instance.OpenPopup($"The input value must be between {minMaxGameRecord} and {maxMaxGameRecord}");
            }
        }


        async void OnClickMaxEventRecord()
        {
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Max Event Record"),
                        ["isPlaintext"] = true,
                    }));

            if (res.value != null)
            {
                bool isErr = true;

                int minMaxRecord = DefaultSettingsUtils.Config().minMaxEventRecord;
                int maxMaxRecord = DefaultSettingsUtils.Config().maxMaxEventRecord;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;

                    if (val >= minMaxRecord
                        && val <= maxMaxRecord
                        )
                    {
                        isErr = false;
                        _consoleBB.Instance.eventRecordMax = val;
                        btnMaxEventRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = val.ToString();
                    }
                }
                catch { }

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                        I18nMgr.T("Max Event Record"),
                        minMaxRecord, maxMaxRecord));
                //TipPopupHandler.Instance.OpenPopup($"The input value must be between {minMaxGameRecord} and {maxMaxGameRecord}");
            }
        }


        async void OnClickMaxErrorRecord()
        {
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Max Warning Record"),
                        ["isPlaintext"] = true,
                    }));

            if (res.value != null)
            {
                bool isErr = true;

                int minMaxRecord = DefaultSettingsUtils.Config().minMaxErrorRecord;
                int maxMaxRecord = DefaultSettingsUtils.Config().maxMaxErrorRecord;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;

                    if (val >= minMaxRecord
                        && val <= maxMaxRecord
                        )
                    {
                        isErr = false;
                        _consoleBB.Instance.errorRecordMax = val;
                        btnMaxErrorRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = val.ToString();
                    }
                }
                catch { }

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                        I18nMgr.T("Max Warning Record"),
                        minMaxRecord, maxMaxRecord));
                //TipPopupHandler.Instance.OpenPopup($"The input value must be between {minMaxGameRecord} and {maxMaxGameRecord}");
            }
        }


        async void OnClickMaxBusinessDayRecord()
        {
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Max Business Day Record"),
                        ["isPlaintext"] = true,
                    }));

            if (res.value != null)
            {
                bool isErr = true;

                int minMaxRecord = DefaultSettingsUtils.Config().minMaxBusinessDayRecord;
                int maxMaxRecord = DefaultSettingsUtils.Config().maxMaxBusinessDayRecord;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;

                    if (val >= minMaxRecord
                        && val <= maxMaxRecord
                        )
                    {
                        isErr = false;
                        _consoleBB.Instance.businiessDayRecordMax = val;
                        btnMaxBusinessDayRecord.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = val.ToString();
                    }
                }
                catch { }

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                        I18nMgr.T("Max Business Day Record"),
                        minMaxRecord, maxMaxRecord));
                //TipPopupHandler.Instance.OpenPopup($"The input value must be between {minMaxGameRecord} and {maxMaxGameRecord}");
            }
        }


        async void OnClickBillValidatorModel()
        {
            // List<_DeviceInfo>
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleChooseDevice,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Choose Bill Validator Model"),
                        ["deviceLst"] = _consoleBB.Instance.suppoetBillers,
                        ["deviceNumber"] = _consoleBB.Instance.selectBillerNumber,
                    }));

            if (res.value != null)
            {
                int number = (int)res.value;
                _consoleBB.Instance.selectBillerNumber = number;

                OnPropertyChangeIsConnectBiller();

                MachineDeviceCommonBiz.Instance.InitBiller(() =>
                {
                    TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Biller setup successful"));
                }, (err) =>
                {
                    TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Biller setup failed"));
                });
            }
        }


        async void OnClickPrinterModel()
        {
            // List<_DeviceInfo>
            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleChooseDevice,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Choose Printer Model"),
                        ["deviceLst"] = _consoleBB.Instance.suppoetPrinters,
                        ["deviceNumber"] = _consoleBB.Instance.selectPrinterNumber,
                    }));

            if (res.value != null)
            {
                int number = (int)res.value;
                _consoleBB.Instance.selectPrinterNumber = number;
                OnPropertyChangeIsConnectPrinter();

                //OnPropertyChangeIsConnectPrinter();

                MachineDeviceCommonBiz.Instance.InitPrinter(() =>
                {
                    TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Printer setup successful"));
                }, (err) =>
                {
                    TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Printer setup failed"));
                });
            }
        }




        async void OnClickRemoteControlSetting()
        {
            /*Dictionary<string, string> selectLst = new Dictionary<string, string>();
            foreach (KeyValuePair<int, string> kv in _consoleBB.Instance.iotAccessMethodsLst)
            {
                selectLst.Add($"{kv.Key}", I18nMgr.T(kv.Value));
            }*/

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetServerConnect001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Configure Remote Server Connection"),
                    }
                ));

            if (res.value != null)
            {
                string addr = (string)res.value;
                try
                {
                    _consoleBB.Instance.remoteControlSetting = addr;

                    OnPropertyChangeIsConnectRemoteControl();
                    MachineDeviceCommonBiz.Instance.CheckMqttRemoteButtonController();
                }
                catch
                {

                }
            }
        }



        async void OnClickRemoteControlAccount()
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
                        ["title"] = I18nMgr.T("Set Remote Control Account"),
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

                    _consoleBB.Instance.remoteControlAccount = lst[0];
                    _consoleBB.Instance.remoteControlPassword = lst[1];

                    OnPropertyChangeRemoteControlAccount();
                }
                catch
                {

                }
            }
        }


        async void OnClickBonusReportSetting()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetServerConnect001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Configure Server Connection"),
                    }
                ));

            if (res.value != null)
            {
                string addr = (string)res.value;
                try
                {
                    _consoleBB.Instance.bonusReportSetting = addr;
                    MachineDeviceCommonBiz.Instance.CheckBonusReport();
                    OnPropertyChangeIsUseBonusReport();
                }
                catch
                {

                }
            }
        }



        async void OnClickIOTAccessMethods()
        {
            Dictionary<string, string> selectLst = new Dictionary<string, string>();
            foreach (KeyValuePair<int, string> kv in _consoleBB.Instance.iotAccessMethodsLst)
            {
                selectLst.Add($"{kv.Key}", I18nMgr.T(kv.Value));
            }

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleChooseLanguage,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["selectLst"] = selectLst,
                        ["selectNumber"] = $"{_consoleBB.Instance.iotAccessMethods}",
                        ["title"] = "Choose IOT Access Methods",
                        ["selectedDes"] = "Selected Methods: {0}",
                    }
                ));

            if (res.value != null)
            {
                string numberStr = (string)res.value;
                try
                {
                    int idx = int.Parse(numberStr);

                    _consoleBB.Instance.iotAccessMethods = idx;

                    OnPropertyChangeIsConnectIot();


                    MachineDeviceCommonBiz.Instance.CheckIOT();
                }
                catch
                {

                }
            }
        }




        /// <summary>
        /// 点击线号，机台号
        /// </summary>
        async void OnClickAgentIDMachineID()
        {
            //  IsMachineIdReady()

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetMachineID, null);

            if (res.value != null)
            {

                string machineId = (string)res.value;
                string agentId = machineId.Substring(0, 4);
                if (machineId == _consoleBB.Instance.machineID)
                {
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The settings have not changed and do not need to be saved"));
                }
                else
                {

                    UnityAction OnConfirmModify = () =>
                    {
                        MachineDataUtils.RequestSetLineIDMachineID(int.Parse(agentId), int.Parse(machineId),
                        (res) => {
                            SBoxPermissionsData data = res as SBoxPermissionsData;
                            if (data.result == 0)
                                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Successfully saved"));
                            else
                                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Save failed"));

                            //要延时？
                            CheckAgentIDMachineID();
                        },
                        (err) =>
                        {
                            TipPopupHandler.Instance.OpenPopup(I18nMgr.T(err.msg));

                            CheckAgentIDMachineID();
                        });
                    };


                    if (_consoleBB.Instance.isCurAdministrator)
                    {
                        OnConfirmModify();
                    }
                    else
                    {
                        CommonPopupHandler.Instance.OpenPopup(new CommonPopupInfo()
                        {
                            // 只能修改一次线号机台号，确定要修改？
                            type = CommonPopupType.YesNo,
                            text = I18nMgr.T("You can only modify the Agent ID and Machine ID once. Are you sure you want to modify it?"),
                            buttonText1 = I18nMgr.T("Cancel"),
                            buttonText2 = I18nMgr.T("OK"),
                            callback1 = null,
                            callback2 = OnConfirmModify,
                        });
                    }

                }


            }
        }



        async void OnClickJackpotGamePercent()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleSetParameter001,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Set Jackpot Game"),
                        ["paramName1"] = I18nMgr.T("Jackpot Game Percent:"),
                    }
                ));

            if (res.value != null)
            {
                string dataStr = (string)res.value;
                try
                {
                    float val = float.Parse(dataStr);
                    _consoleBB.Instance.jackpotGamePercent = val;
                    btnJackpotGamePercent.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{_consoleBB.Instance.jackpotGamePercent}";
                }
                catch
                {
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The input value must be a number"));
                }
            }

        }


#if false // 已经废弃
        /// <summary>
        /// dll水平
        /// </summary>
        async void OnClickLevel()
        {

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
               new EventData<Dictionary<string, object>>("",
                   new Dictionary<string, object>()
                   {
                       ["title"] = I18nMgr.T("Set Level"),
                       ["isPlaintext"] = true,
                   }));

            if (res.value != null)
            {
                if (res.value != null)
                {
                    bool isErr = true;

                    int minBet = 0;
                    int maxBet = 50;
                    try
                    {
                        int val = int.Parse((string)res.value);  // (long)res.value;

                        if (val >= minBet && val <= maxBet)
                        {
                            isErr = false;

                            _consoleBB.Instance.dllLevel = val;
                            //DllAlgorithmManager.Instance.SetLevel(_consoleBB.Instance.level);
                            MachineDeviceCommonBiz.Instance.SetLevel();

                            btnLevel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{MachineDeviceCommonBiz.Instance.GetLevel()}";
                        }
                    }
                    catch { }

                    if (isErr)
                        TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"), I18nMgr.T("Level"), minBet, maxBet));
                }
            }

        }
#endif

        async void OnClickLevel()
        {
            if (!_consoleBB.Instance.isResetCode)
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("clear first with a reset code."));
                return;
            }

            Dictionary<string, string> selectLst = new Dictionary<string, string>();

            for (int i = 0; i < MachineDeviceCommonBiz.Instance.levelLst.Length; i++)
            {
                //selectLst.Add($"{i}", $"{MachineDeviceCommonBiz.Instance.levelLst[i]}");
                selectLst.Add($"{i}", $"{i}");
            }



            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleChooseLanguage,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["selectLst"] = selectLst,
                    ["selectNumber"] = $"{ConsoleBlackboard02.Instance.dllLevelIndex}",
                    ["title"] = "Choose Level",
                    ["selectedDes"] = "Selected Level: {0}",
                }
            ));

            if (res.value != null)
            {
                bool isErr = true;

                //int minBet = 0;
                //int maxBet = 50;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;


                    Debug.Log($"dll等级修改为：{val}");

                    _consoleBB.Instance.dllLevelIndex = val;
                    MachineDeviceCommonBiz.Instance.SetLevel();
                    btnLevel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{MachineDeviceCommonBiz.Instance.GetLevel()}";
                    
                    isErr = false;
                }
                catch { }

                //if (isErr) TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"), I18nMgr.T("Level"), minBet, maxBet));

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Setting failed"));

            }
        }



        /// <summary>
        /// 波动系数
        /// </summary>
        async void OnClickWaveScore()
        {
            
            Dictionary<string, string> selectLst = new Dictionary<string, string>();
            foreach (int item in MachineDeviceCommonBiz.Instance.waveScoreLst)
            {
                selectLst.Add($"{item}", $"{item}");
            }

            EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleChooseLanguage,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["selectLst"] = selectLst,
                    ["selectNumber"] = $"{MachineDeviceCommonBiz.Instance.GetWaveScore()}",
                    ["title"] = "Choose Wave Score",
                    ["selectedDes"] = "Selected Wave Score: {0}",
                }
            ));

            if (res.value != null)
            {
                bool isErr = true;

                //int minBet = 0;
                //int maxBet = 50;
                try
                {
                    int val = int.Parse((string)res.value);  // (long)res.value;

                    Debug.Log($"波动系数修改为：{val}");

                    MachineDeviceCommonBiz.Instance.SetWaveScore(val);
                    btnWaveScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{MachineDeviceCommonBiz.Instance.GetWaveScore()}";
                    isErr = false;
                }
                catch { }

                //if (isErr) TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"), I18nMgr.T("Level"), minBet, maxBet));

                if (isErr)
                    TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Setting failed"));

            }
        }

    }
}


