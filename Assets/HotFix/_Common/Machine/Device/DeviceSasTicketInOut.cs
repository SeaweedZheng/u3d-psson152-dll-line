using CryPrinter;
using cryRedis;
using GameMaker;
using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;


/// <summary>
/// 公共方法
/// </summary>
public partial class DeviceSasTicketInOut : MonoSingleton<DeviceSasTicketInOut>
{

    private CorController _corCtrl;
    private CorController corCtrl
    {
        get
        {
            if (_corCtrl == null)
                _corCtrl = new CorController(this);
            return _corCtrl;
        }
    }

    public void ClearCor(string name) => corCtrl.ClearCor(name);
    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);
    public bool IsCor(string name) => corCtrl.IsCor(name);
    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);

    public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);


    const string COR_CHECK_SAS_CONNECT = "COR_CHECK_SAS_CONNECT";

        //isSasConnect
    private void OnEnable()
    {
        //if (!ApplicationSettings.Instance.isMachine)  return;
        EventCenter.Instance.AddEventListener(SBoxSanboxEventHandle.BILL_STACKED, OnHardwareBillStacked);
        EventCenter.Instance.AddEventListener<string>(SBoxSanboxEventHandle.TICKET_IN, OnHardwareSasTicketIn);

    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(SBoxSanboxEventHandle.BILL_STACKED, OnHardwareBillStacked);
        EventCenter.Instance.RemoveEventListener<string>(SBoxSanboxEventHandle.TICKET_IN, OnHardwareSasTicketIn);
    }


    List<Action> tasks = new List<Action>();
    private void Update()
    {
        if (tasks.Count > 0)
        {
            Action task = tasks[0];
            tasks.RemoveAt(0);
            task.Invoke();
        }
    }
    /// <summary>
    /// Sas-Redis 下行数据
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="data"></param>
    public void OnSasRedisRpcDownHandler(int eventId, string data)
    {
        Debug.LogWarning($"【SasRedis】<color=yellow>rpc down</color>  eventId: {eventId} , data:{data} ");
        tasks.Add(() =>
        {
            switch (eventId)
            {
                case 3: // 入票上分，获取票号对应的金额
                    {
                        string[] res = data.Split(',');
                        bool isApprove = res[0] == "0";
                        int money = isApprove?int.Parse(res[1]):0;
                        OnSasRedisRpcDownGetTicketInMoney(isApprove, money);
                    }
                    break;
                case 4: // 下分时监听sas-redis下发的票号：
                    {
                        string _orderId = data;
                        OnSasRedisRpcDownGetTicketOutOrder(_orderId);
                    }
                    break;
            }
        });
    }


    void CheckSasConnect()
    {
        _consoleBB.Instance.isSasConnect = RedisMgr.Instance.isConnectRedis;

        DoCor(COR_CHECK_SAS_CONNECT, DoTaskRepeat(() =>
        {
            _consoleBB.Instance.isSasConnect =  RedisMgr.Instance.isConnectRedis;
            Debug.LogWarning($"【Divice Sas Ticket In Out】CheckSasConnect: {RedisMgr.Instance.isConnectRedis}");
        }, 5000));
    }


    public void CheckSas()
    {
        if (PlayerPrefsUtils.isUseSas)
        {
            /*
                RedisMgr.Instance.Init("192.168.3.234", 6379, "sas@2024", 0, 50, "", (eventId, data) =>
                    {
                        DeviceSasTicketInOut.Instance.OnSasRedisRpcDownHandler(eventId, data);
                    });
             */

            _consoleBB.Instance.isSasConnect = false;

            string[] addr = _consoleBB.Instance.sasConnection.Split(':');
            string pwd = _consoleBB.Instance.sasPassword;
            RedisMgr.Instance.Init(addr[0], int.Parse(addr[1]), pwd, 0, 50, "", (eventId, data) =>
            {
                DeviceSasTicketInOut.Instance.OnSasRedisRpcDownHandler(eventId, data);
            });
            CheckSasConnect();
        }
        else
        {
            _consoleBB.Instance.isSasConnect = false;
            ClearCor(COR_CHECK_SAS_CONNECT);
            RedisMgr.Instance.Close();
        }

    }
}




/// <summary>
/// Sas投票
/// </summary>
/// <remarks>
/// # 【Sas纸钞机进票】<br/>
/// * sas 机进票。获取票号通知（事件：TICKET_IN）<br/>
/// * 告诉sas-redis票号;  监听sas-redis通道(eventId = 3)，得到 （"0允许接受该票,票对应的金额"）或 （"1不允许接受该票,票对应的金额"）。<br/>
/// * [允许收票]通知"sas机(硬件)"收取该票。 在硬件收取票响应方法中（BILL_STACKED），给算法卡加分，并上报结果给 sas-redis（已经收取该票）<br/>
/// * [拒绝收票]通知"sas机(硬件)"拒绝该票。并上报结果给 sas-redis（已拒绝收票）	<br/> 
/// </remarks>
public partial class DeviceSasTicketInOut {



    #region sas进票



    List<string> orderIdCache = new List<string>();

    string lastTicketInOrderId = "";

    const string COR_DELAY_CLEAR_ORDER_CACHE = "COR_DELAY_CLEAR_ORDER_CACHE";

    const string IS_SAS_TICKET_IN_ING = "IS_SAS_TICKET_IN_ING";

    /// <summary>
    /// 进票信息
    /// </summary>
    /// <param name="ticketInOrderId"></param>
    /// <remarks>
    /// * 机器进票 得到 orderId  (面值（分），序列号 )???
    /// * 票号orderId发给sas 
    /// </remarks>
    private void OnHardwareSasTicketIn(string ticketInOrderId)
    {
        //Debug.LogError($"OnTicketIn  orderId: 点单 {ticketInOrderId}");

        if (!DeviceUtils.IsCurSasBiller())
        {
            Debug.LogError($"当前非sas纸钞机");
            return;
        }


        // 票号去重
        if (orderIdCache.Contains(ticketInOrderId))
        {
            //Debug.LogError($"OnTicketIn  orderId: 点单重复过滤 {ticketInOrderId}");
            return;
        }
        orderIdCache.Add(ticketInOrderId);

        DoCor(COR_DELAY_CLEAR_ORDER_CACHE, DoTask(() =>
        {
            orderIdCache.Remove(ticketInOrderId);
        }, 200));


        if (IsCor(IS_SAS_TICKET_IN_ING))
            return;
        DoCor(IS_SAS_TICKET_IN_ING, DoTask(() => { }, 4000));


        InitParamSasTicketIn();

        // 保存票号
        lastTicketInOrderId = ticketInOrderId;

        // 通知sas-redis票号
        //Debug.LogError($"OnTicketIn  orderId: {ticketInOrderId}");
        SasCommand.Instance.PushGeneralBillerTicketIn(ticketInOrderId);
    }

    void OnSasRedisRpcDownGetTicketInMoney(bool isApprove, int money)
    {
        if (isApprove)
        {
            SasApproveTicketIn(money);
        }
        else
        {
            SasRejectTicketIn();
        }
    }


    int moneyIn = 0;
    /// <summary>
    /// redis允许接受该票
    /// </summary>
    public void SasApproveTicketIn(int moneyFromTicket)
    {
        moneyIn = moneyFromTicket;

        // 等待硬件响应已接收到票
        SBoxSandbox.BillApprove();
    }

    /// <summary>
    /// Sas-redis不允许接受该票
    /// </summary>
    public void SasRejectTicketIn()
    {
        //通知sas进票机，拒绝此票。
        SBoxSandbox.BillReject();

        // 通知sas-redis，已拒绝此票。
        SasCommand.Instance.PushGeneralSasBillerInApproveResualt(0, lastTicketInOrderId);
        lastTicketInOrderId = "";
    }

    /// <summary> 正常的进票过程 </summary>
    bool isRegularSasTicketIn => !string.IsNullOrEmpty(lastTicketInOrderId) && moneyIn > 0;

    void InitParamSasTicketIn()
    {
        lastTicketInOrderId = "";
        moneyIn = 0;
    }

    /// <summary>
    /// 钞票进入钱箱
    /// </summary>
    private void OnHardwareBillStacked()
    {
        if (!isRegularSasTicketIn) return;

        if (!DeviceUtils.IsCurSasBiller())
            return;

        int credit = moneyIn * _consoleBB.Instance.sasInOutScale; 

        //Debug.LogError($"要上分的金额 {credit}");

        //MachineDeviceCommonBiz.Instance.RequestScoreUp(credit);

        SasCommand.Instance.PushGeneralSasBillerInApproveResualt(1, lastTicketInOrderId);

        // 订单入库
        MachineDataManager.Instance.RequestScoreUp(credit, (Action<object>)((res) => {

            // 订单入库

            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = "sas_ticket_in",
                    device_number = 0,
                    order_id = lastTicketInOrderId,
                    count = 1,
                    credit = credit,
                    credit_after = _consoleBB.Instance.myCredit + credit,
                    credit_before = _consoleBB.Instance.myCredit,
                    in_out = 1,
                    created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                });
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);

            _consoleBB.Instance.myCredit += credit;
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

            // 每日统计
            TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreUp((long)credit, _consoleBB.Instance.myCredit);

            InitParamSasTicketIn();

        }));
    }


    #endregion

}




/// <summary>
/// Sas打印出票
/// </summary>
public partial class DeviceSasTicketInOut
{


    #region sas出票 （出票按钮接sas出票机）

    const string COR_IS_SAS_TICKET_OUT_ING = "COR_IS_SAS_TICKET_OUT_ING";


    int moneyOut;
    public void DoSasTicketOut()
    {

        if (!_consoleBB.Instance.isUsePrinter)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Printer function not enabled."));
            return;
        }



        if (!PlayerPrefsUtils.isUseSas)
        {
            // 提示sas链接失败
            return;
        }

        if (IsCor(COR_IS_SAS_TICKET_OUT_ING))
            return;
        DoCor(COR_IS_SAS_TICKET_OUT_ING, DoTask(() => { },4000));  // 防止重复误触发

        // 打开遮罩
        MaskPopupHandler.Instance.OpenPopup();

        // 这里要有个倍率单位， 玩家积分能转成多少钱。
        // moneyOut = 1000;


        moneyOut = (int)_consoleBB.Instance.myCredit / _consoleBB.Instance.sasInOutScale;

        SasCommand.Instance.PushGeneralScoreDown(moneyOut);
    }

    void OnSasRedisRpcDownGetTicketOutOrder(string orderId)
    {
        //打印票
        PrinterJCM950(orderId, moneyOut, (code, msg) =>
        {
            MaskPopupHandler.Instance.ClosePopup();

            if (code != 0)
            {
                Debug.LogError($"JCM950 , code: {code} , msg: {msg}");
                return;
            }

            SasCommand.Instance.PushGeneralScoreDownResualt(orderId);

            //算法卡走下分
            //int creditOut =  moneyOut * 1;
            int creditOut = moneyOut * _consoleBB.Instance.sasInOutScale;

            MachineDataManager.Instance.RequestScoreDown(creditOut, (Action<object>)((res) => {

                // 订单入库

                long creditBefore = _consoleBB.Instance.myCredit;
                long creditAfter = _consoleBB.Instance.myCredit - creditOut;

                Debug.LogError($"Sas打印下分 creditBefore={creditBefore} creditAfter={creditAfter} ");

                string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = "sas_ticket_out",
                        device_number = 0,
                        order_id = orderId,
                        count = 1,
                        credit = creditOut,
                        credit_after = creditAfter,
                        credit_before = creditBefore,
                        in_out = 0,
                        created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    });
                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);


                _consoleBB.Instance.myCredit = creditAfter;
                MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

                // 每日统计
                TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreDown((long)creditOut, _consoleBB.Instance.myCredit);

            }));

        });

    }


    #endregion

}


/// <summary>
/// 【下分】打印机硬件部分
/// </summary>
public partial class DeviceSasTicketInOut : MonoSingleton<DeviceSasTicketInOut>
{


    const string PRINTER_NUMBER = "PRINTER_NUMBER";
    public void PrinterJCM950(string orderId = "011058314280279645", double money = 512.32, Action<int, string> onSuccessCallback = null)
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

                string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
                Epic950Printer printer = new Epic950Printer(port, false);//"/dev/ttyS1") ;
                printer.PrintTicket(orderId, money, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, $"{cashSeq}", System.DateTime.Now);

                DoCheckPrinterIsBusy(printer, onSuccessCallback);
            }
            else
            {
                Debug.LogError("get seq number is fail; when sas score down");
                onSuccessCallback?.Invoke(1, "get seq number is fail; when sas score down");
                return;
            }
        });

    }

    public void PrinterTRANSACT950(string orderId = "011058314280279645", double money = 512.32, Action<int, string> onSuccessCallback = null)
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

                string port = Application.isEditor ? "COM4" : "/dev/ttyS1";
                Epic950Printer printer = new Epic950Printer(port, true);//"/dev/ttyS1") ;
                printer.PrintTicket(orderId, money, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, $"{cashSeq}", System.DateTime.Now);

                DoCheckPrinterIsBusy(printer, onSuccessCallback);
            }
            else
            {
                Debug.LogError("get seq number is fail; when sas score down");
                onSuccessCallback?.Invoke(1, "get seq number is fail; when sas score down");
                return;
            }
        });
    }


    /// <summary>
    /// 检查打印机状态
    /// </summary>
    /// <param name="printer"></param>
    /// <param name="onSuccessCallback"></param>
    public void DoCheckPrinterIsBusy(Epic950Printer printer, Action<int, string> onFinishCallback)
    {
        if (_corPrinterState != null)
            StopCoroutine(_corPrinterState);
        _corPrinterState = StartCoroutine(CheckPrinterIsBusy(printer, onFinishCallback));
    }
    Coroutine _corPrinterState;


    private IEnumerator CheckPrinterIsBusy(Epic950Printer printer, Action<int,string> onFinishCallback)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(4.0f);
            var offlineCode = printer.GetStatus(StatusTypes.OfflineStatus); 
            if (!offlineCode.IsBusy)
            {
                yield return new WaitForSecondsRealtime(1.0f);
                object[] res = GetPrintIsSuccess(printer);
                onFinishCallback?.Invoke((int)res[0], (string)res[1]);
                break;
            }
        }
    }

    /*
     * 
     * 
            rts.IsOnline = (data & 0x01) == 0;

            rts.HasError = (data & 0x04) != 0;

            rts.IsPaperPresent = (data & 0x10) == 0;
    */

    public object[] GetPrintIsSuccess(Epic950Printer printer)
    {

        int state = SBoxSandbox.PrinterState();///先检测有无连接打印机
        //Debug.Log("printer state.............." + state.ToString());
        StatusReport offlineCode = printer.GetStatus(StatusTypes.OfflineStatus);
        Debug.LogWarning($"printer state.........{JsonConvert.SerializeObject(offlineCode)}");
        if (offlineCode.IsInvalidReport)
        {
            return new object[] { 1, "Printer Invalid Report" };

        }
        /*
        else if (offlineCode.HasError)
        {
            if (!offlineCode.IsPaperPresent)
            {
                return new object[] { 1, "Printer Out Of Paper" };
            }
            if (!offlineCode.IsOnline)
            {
                return new object[] { 1, "Disconnected Printer" };
            }
            return new object[] { 1, "Printer Exception, please contact the administrator" };
        }
        else if (!offlineCode.IsPaperPresent)
        {
            return new object[] { 1, "Printer Out Of Paper" };
        }
        else if (!offlineCode.IsOnline)
        {
            return new object[] { 1, "Disconnected Printer" };
        }

         先不检查状态
        if (state == -2)
        {
            return new object[] { 1, "Printer Exception, please contact the administrator" };
        }*/
        return new object[] { 0, "" };



        /* 【这块代码已经废弃】
        //Debug.Log("新款打印机状态 IsOnline:" + offlineCode.IsOnline.ToString() + " HasError: " + offlineCode.HasError.ToString() + " IsPaperPresent: " + offlineCode.IsPaperPresent.ToString() + "IsPaperLevelOkay:" + offlineCode.IsPaperLevelOkay);
        if (offlineCode.HasError)///打印机有错误
        {
            if (!offlineCode.IsPaperPresent)
            {
                return new object[] { 1, "Printer Out Of Paper" };
            }
            if (!offlineCode.IsOnline)
            {
                return new object[] { 1, "Disconnected Printer" };
            }
            return new object[] { 1, "Printer Exception, please contact the administrator" };
        }
        if (!offlineCode.IsPaperLevelOkay)///为fasle，代表少纸
        {
            return new object[] { 1, "Printer Out Of Paper" };
        }
        if (!offlineCode.IsPaperPresent)
        {
            return new object[] { 1, "Printer Out Of Paper" };
        }
        if (!offlineCode.IsOnline)
        {
            return new object[] { 1, "Disconnected Printer" };
        }
        if (state == -2)
        {
            return new object[] { 1, "Printer Exception, please contact the administrator" };
        }
        return new object[] { 0, "" };
        */

    }


}