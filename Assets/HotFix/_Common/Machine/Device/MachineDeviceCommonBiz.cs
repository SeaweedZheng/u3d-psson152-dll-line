using cryRedis;
using Game;
using GameMaker;
using MoneyBox;
using SlotDllAlgorithmG152;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using _consoleBB = PssOn00152.ConsoleBlackboard02;



public enum CoinInOutState
{
    None,
    ScoreUp,
    ScoreDown,
    CoinIn,
    CoinOut,
    BillIn,
    PrintOut,
    IOTIn,
    IOTOut,
    MoneyBoxIn,
    MoneyBoxOut,
}

public class MachineDeviceCommonBiz : MonoSingleton<MachineDeviceCommonBiz>
{
    public DeviceBillIn deviceBillIn;
    public DeviceCoinIn deviceCoinIn;
    public DeviceCoinOut deviceCoinOut;
    public DevicePrinterOut devicePrinterOut;
    //public DeviceIOTPayment deviceIOTPayment;
    public DevicePrinterOutQRCode devicePrinterOutQRCode;

    public DeviceMoneyBox deviceMoneyBox;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 提供投退币状态防止重复触发
    /// </summary>
    /// <returns></returns>
    public CoinInOutState GetCoinInOutState()
    {
        if (deviceCoinOut.isRegularCoinOuting)
        {
            return CoinInOutState.CoinOut;
        }
        return CoinInOutState.None;
    }



    /// <summary>
    /// 打开后台设置界面
    /// </summary>
    public void OpenConsole()
    {

        /*
        if (BlackboardUtils.IsBlackboard("./") && BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            ErrorPopupHandler.Instance.OpenPopup(new ErrorPopupInfo()
            {
                type = ErrorPopupType.OK,
                text = I18nMgr.T("<size=26>Cannot open console during the game period</size>"),
                buttonText1 = I18nMgr.T("OK"),
            });
            return;
        }

        if (PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1)
            PageManager.Instance.OpenPage(PageName.Console001PageConsoleMain);
        */

        // 受到保护时，不允许打开管理后台
        if (GlobalData.isProtectApplication) return;


        if (corOpenConsole != null)
            StopCoroutine(corOpenConsole);
        corOpenConsole = StartCoroutine(DoDelayToOpenConsole());
    }


    #region 温馨提示即将进入设置界面
    Coroutine corOpenConsole;
    IEnumerator DoDelayToOpenConsole()
    {
        if (PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) != -1)
            yield break;

        if (BlackboardUtils.IsBlackboard("./") && BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            if (PageManager.Instance.IndexOf(PageName.PageSysMessage) == -1)
                PageManager.Instance.OpenPage(PageName.PageSysMessage, new EventData<Dictionary<string, object>>("", new Dictionary<string, object>()
                {
                    ["autoCloseTimeS"] = 3f,
                    ["message"] = I18nMgr.T("<color=red>[Note]: We are about to enter the settings page.</color>"),
                }));

            BlackboardUtils.SetValue<bool>("./isRequestToStop", true);
        }
        else
        {
            PageManager.Instance.OpenPage(PageName.Console001PageConsoleMain);
        }

        yield return new WaitUntil(() => BlackboardUtils.GetValue<bool>("./isSpin") == false);

        BlackboardUtils.SetValue<bool>("./isRequestToStop", false);

        if (PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1)
            PageManager.Instance.OpenPage(PageName.Console001PageConsoleMain);

        corOpenConsole = null;
    }
    #endregion






    #region 投退币


    public void DoCoinOut()
    {
        if (PageManager.Instance.IsHasPopupOrOverlayPage())
            return;


        // 有sas退票先用sas
        if (PlayerPrefsUtils.isUseSas)
        {
            DeviceSasTicketInOut.Instance.DoSasTicketOut();
            return;
        }


        // 有好酷优先用好酷
        if (_consoleBB.Instance.isUseIot)
        {

            if (!DeviceIOTPayment.Instance.isIOTConneted)
            {
                TipPopupHandler.Instance.OpenPopupOnce(
                    string.Format(I18nMgr.T("IOT connection failed [{0}]"), Code.DEVICE_IOT_MQTT_NOT_CONNECT));
                return;
            }
            if (!DeviceIOTPayment.Instance.isCoinInBindWeChatAccount)
            {
                TipPopupHandler.Instance.OpenPopupOnce(
                    string.Format(I18nMgr.T("IOT connection failed [{0}]"), Code.DEVICE_IOT_COIN_IN_NOT_BIND_WECHAT_ACCOUNT));
                return;
            }
            DeviceIOTPayment.Instance.DoIotTickerOut();
            return;
        }

        //普通退币
        deviceCoinOut.DoCoinOut();
    }

    /// <summary>
    /// 即中即退
    /// </summary>
    /// <param name="credit"></param>
    public void DoCoinOutImmediately(int credit) => deviceCoinOut.DoCoinOutImmediately(credit);

    #endregion

    /// <summary>
    /// 中联网彩金
    /// </summary>
    /// <param name="count">中多少个币</param>
    public void ConInWhenHitJackpotOnline(int count) => deviceCoinIn.OnJackpotOnlineCoinIn(count);


    #region 选择打印机、纸钞机
    /// <summary> 重复初始化打印机，直到初始化成功 </summary>
    public void InitPrinter(Action successCallback, Action<string> errorCallback) => devicePrinterOut.InitPrinter(successCallback, errorCallback);

    /// <summary> 重复初始化纸钞机，直到初始化成功 </summary>
    public void InitBiller(Action successCallback, Action<string> errorCallback) => deviceBillIn.InitBiller(successCallback, errorCallback);
    #endregion

    public void TestTicketOut()
    {
        deviceCoinOut.DoCoinOut();
        EventCenter.Instance.EventTrigger<int>(SBoxSanboxEventHandle.COIN_OUT, 1);
    }



    public void TestPrintQRCodeInfo() => devicePrinterOutQRCode.PrintQRCodeInfo();

    public void TestPrinterTicket() => devicePrinterOut.TestPrinterTicket();





    public void PrinterJCM950(string orderId = "011058314280279645", double money = 512.32, Action<int, string> onSuccessCallback = null) =>
        DeviceSasTicketInOut.Instance.PrinterJCM950(orderId, money, (code , msg) =>
        {
            Debug.Log($"JCM950 , code: {code} , msg: {msg}");
        });

    public void PrinterTRANSACT950(string orderId = "011058314280279645", double money = 512.32, Action<int, string> onSuccessCallback = null) =>
        DeviceSasTicketInOut.Instance.PrinterTRANSACT950(orderId, money, (code, msg) =>
        {
            Debug.Log($"JCM950 , code: {code} , msg: {msg}");
        });



#if false


    const string PRINTER_NUMBER = "PRINTER_NUMBER";

    Coroutine _corPrinterState = null;

    //public void TestPrinterJCM950(string orderId = "011058314280279645", double money = 1100, Action onSuccessCallback = null)
    public void PrinterJCM950(string orderId = "011058314280279645", double money = 512.32, Action onSuccessCallback = null)
    {
        /*
        int cashSeq = PlayerPrefs.GetInt(PRINTER_NUMBER, 0);
        cashSeq++;
        PlayerPrefs.SetInt(PRINTER_NUMBER, cashSeq);
        */

        // 和算法卡拿seqid
        MachineDataManager.Instance.RequestSasCashSeqScoreDown((res) =>
        {
            int[] data = res as int[];

            if (data[0] == 0)
            {
                long cashSeq = ((long)data[1] << 32) | (uint)data[2];

                //Debug.LogError($"i am JCM950   orderId:{orderId}  money:{money}");
                //1.如果接了JCM的950打印机:
                string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
                Epic950Printer printer = new Epic950Printer(port, false);//"/dev/ttyS1") ;
                printer.PrintTicket(orderId, money, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, $"{cashSeq}", System.DateTime.Now);

                DoCheckPrinterIsBusy(printer, onSuccessCallback);
            }
            else
            {
                Debug.LogError("get seq number is fail; when sas score down");
                return;
            }
        });

    }

    public void PrinterTRANSACT950(string orderId = "011058314280279645", double money = 512.32, Action onSuccessCallback = null)
    {
        /*
        int cashSeq = PlayerPrefs.GetInt(PRINTER_NUMBER, 0);
        cashSeq++;
        PlayerPrefs.SetInt(PRINTER_NUMBER, cashSeq);
        */

        MachineDataManager.Instance.RequestSasCashSeqScoreDown((res) =>
        {
            int[] data = res as int[];

            if (data[0] == 0)
            {
                long cashSeq = ((long)data[1] << 32) | (uint)data[2];


                //Debug.LogError($"i am TRANSACT950   orderId:{orderId}  money:{money}");

                //2.如果接了TRANSACT的950打印机:
                string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
                Epic950Printer printer = new Epic950Printer(port, true);//"/dev/ttyS1") ;
                printer.PrintTicket(orderId, money, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, $"{cashSeq}", System.DateTime.Now);

                DoCheckPrinterIsBusy(printer, onSuccessCallback);
            }
            else
            {
                Debug.LogError("get seq number is fail; when sas score down");
                return;
            }
        });
    }



    public void RequestScoreDown(int credit)
    {
        MachineDataManager.Instance.RequestScoreDown(credit, (res) => {

            long myCredit = _consoleBB.Instance.myCredit;
                if (credit > myCredit)
                    _consoleBB.Instance.myCredit = 0;
                else
                    _consoleBB.Instance.myCredit = myCredit - credit;

                        //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));

                MainBlackboardController.Instance.SyncMyCreditToReal(true);

            // 数据库统计
            // 每日统计
         });
    }


    public void RequestScoreUp(int credit)
    {
        MachineDataManager.Instance.RequestScoreUp(credit, (res) => {

            _consoleBB.Instance.myCredit += credit;

            //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));

            MainBlackboardController.Instance.SyncMyCreditToReal(true);

            // 数据库统计
            // 每日统计
        });
    }


    public void SasBillerApprove(int money) => deviceBillIn.SasApproveTicketIn(money);

    public void SasRejectTicketIn() => deviceBillIn.SasRejectTicketIn();

/// <summary>
/// 检查打印机状态
/// </summary>
/// <param name="printer"></param>
/// <param name="onSuccessCallback"></param>
public void DoCheckPrinterIsBusy(Epic950Printer printer, Action onSuccessCallback)
{
    if (_corPrinterState != null)
        StopCoroutine(_corPrinterState);
    _corPrinterState = StartCoroutine(CheckPrinterIsBusy(printer, onSuccessCallback));
}



    /// <summary>
    /// 这个脚本有问题！
    /// </summary>
    /// <param name="printer"></param>
    /// <param name="onSuccessCallback"></param>
    /// <returns></returns>
    private IEnumerator CheckPrinterIsBusy(Epic950Printer printer , Action onSuccessCallback)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(4.0f);
            var offlineCode = printer.GetStatus(StatusTypes.OfflineStatus);
            if (!offlineCode.IsBusy)
            {
                yield return new WaitForSecondsRealtime(1.0f);
                object[] res = GetPrintIsSuccess(printer);
                bool isSuccess = (bool)res[0];
                if (isSuccess)
                {
                    Debug.LogWarning("【printer qr】success: " + (string)res[1]);
                    //发送打印完成事件 - 或弹窗
                    //MessageDispatcher.Dispatch(EVTType.ON_CONTENT_EVENT, new ParadoxNotion.EventData("PrintFinish"));

                    onSuccessCallback?.Invoke();
                }
                else
                {
                    Debug.LogWarning("【printer qr】error: " + (string)res[1]);
                    //发送失败事件 - 或弹窗

                    onSuccessCallback?.Invoke();
                }
                break;
            }
        }

    }




    public object[] GetPrintIsSuccess(Epic950Printer printer)
    {

        int state = SBoxSandbox.PrinterState();///先检测有无连接打印机
        //Debug.Log("printer state.............." + state.ToString());
        var offlineCode = printer.GetStatus(StatusTypes.OfflineStatus);
        //Debug.Log("新款打印机状态 IsOnline:" + offlineCode.IsOnline.ToString() + " HasError: " + offlineCode.HasError.ToString() + " IsPaperPresent: " + offlineCode.IsPaperPresent.ToString() + "IsPaperLevelOkay:" + offlineCode.IsPaperLevelOkay);
        if (offlineCode.HasError)///打印机有错误
        {
            if (!offlineCode.IsPaperPresent)
            {
                return new object[] { false, "【打印机打印结果】： Printer Out Of Paper" };
            }
            if (!offlineCode.IsOnline)
            {
                return new object[] { false, "【打印机打印结果】： Disconnected Printer" };
            }
            return new object[] { false, "【打印机打印结果】： Printer Exception, please contact the administrator" };
        }
        if (!offlineCode.IsPaperLevelOkay)///为fasle，代表少纸
        {
            return new object[] { false, "【打印机打印结果】： Printer Out Of Paper" };
        }
        if (!offlineCode.IsPaperPresent)
        {
            return new object[] { false, "【打印机打印结果】： Printer Out Of Paper" };
        }
        if (!offlineCode.IsOnline)
        {
            return new object[] { false, "【打印机打印结果】： Disconnected Printer" };
        }
        if (state == -2)
        {
            return new object[] { false, "【打印机打印结果】： Printer Exception, please contact the administrator" };
        }
        return new object[] { true, "" };
    }



#endif








    #region 钱箱业务逻辑接口

    /// <summary>
    /// 机器下分
    /// </summary>
    /// <param name="credit"></param>
    public void DoMoneyOut(int credit) => deviceMoneyBox.DoQRCodeOut(credit);
    #endregion





    /// <summary> 检查sas，尝试链接</summary>
    public void CheckSas()=> DeviceSasTicketInOut.Instance.CheckSas();

    /// <summary> 检查钱箱，尝试链接</summary>
    public void CheckMoneyBox()
    {
        if (_consoleBB.Instance.isConnectMoneyBox)
            MoneyBoxManager.Instance.Init(_consoleBB.Instance.machineID, ApplicationSettings.Instance.gameTheme);
        else
            MoneyBoxManager.Instance.Close();
    }




    /// <summary> 检查好酷，尝试链接</summary>
    public void CheckIOT() => DeviceIOTPayment.Instance.CheckIOT();




    #region Mqtt 远端控制逻辑
    public void CheckMqttRemoteButtonController() => DeviceRemoteControl.Instance.CheckMqttRemoteControl();
    #endregion

    public void CheckBonusReport() => DeviceBonusReport.Instance.CheckBonusReport();


    public void SetLevel()=> SlotDllAlgorithmG152Manager.Instance.SetLevel(_consoleBB.Instance.dllLevelIndex);
    
    public int GetLevel()=> SlotDllAlgorithmG152Manager.Instance.GetLevel();

    public string GetLevelName()
    {
         int levelIndex = SlotDllAlgorithmG152Manager.Instance.GetLevel();

        return levelIndex >= 0 && levelIndex < levelLst.Length ? levelLst[levelIndex] : $"index: {levelIndex}";
    }

    //dll算法里的波动系数
    public void SetWaveScore(int waveScore) => SlotDllAlgorithmG152Manager.Instance.SetWaveScore(waveScore);
    /*
    * 获取波动分数
    */
    public int GetWaveScore() => SlotDllAlgorithmG152Manager.Instance.GetWaveScore();

    public int[] waveScoreLst => SlotDllAlgorithmG152Manager.Instance.waveScoreLst;


    /// <summary> 等级列表  </summary>
    public string[] levelLst => SlotDllAlgorithmG152Manager.Instance.levelLst;
}
