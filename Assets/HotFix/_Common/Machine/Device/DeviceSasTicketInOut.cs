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
/// ��������
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
    /// Sas-Redis ��������
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="data"></param>
    public void OnSasRedisRpcDownHandler(int eventId, string data)
    {
        Debug.LogWarning($"��SasRedis��<color=yellow>rpc down</color>  eventId: {eventId} , data:{data} ");
        tasks.Add(() =>
        {
            switch (eventId)
            {
                case 3: // ��Ʊ�Ϸ֣���ȡƱ�Ŷ�Ӧ�Ľ��
                    {
                        string[] res = data.Split(',');
                        bool isApprove = res[0] == "0";
                        int money = isApprove?int.Parse(res[1]):0;
                        OnSasRedisRpcDownGetTicketInMoney(isApprove, money);
                    }
                    break;
                case 4: // �·�ʱ����sas-redis�·���Ʊ�ţ�
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
            Debug.LogWarning($"��Divice Sas Ticket In Out��CheckSasConnect: {RedisMgr.Instance.isConnectRedis}");
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
/// SasͶƱ
/// </summary>
/// <remarks>
/// # ��Sasֽ������Ʊ��<br/>
/// * sas ����Ʊ����ȡƱ��֪ͨ���¼���TICKET_IN��<br/>
/// * ����sas-redisƱ��;  ����sas-redisͨ��(eventId = 3)���õ� ��"0������ܸ�Ʊ,Ʊ��Ӧ�Ľ��"���� ��"1��������ܸ�Ʊ,Ʊ��Ӧ�Ľ��"����<br/>
/// * [������Ʊ]֪ͨ"sas��(Ӳ��)"��ȡ��Ʊ�� ��Ӳ����ȡƱ��Ӧ�����У�BILL_STACKED�������㷨���ӷ֣����ϱ������ sas-redis���Ѿ���ȡ��Ʊ��<br/>
/// * [�ܾ���Ʊ]֪ͨ"sas��(Ӳ��)"�ܾ���Ʊ�����ϱ������ sas-redis���Ѿܾ���Ʊ��	<br/> 
/// </remarks>
public partial class DeviceSasTicketInOut {



    #region sas��Ʊ



    List<string> orderIdCache = new List<string>();

    string lastTicketInOrderId = "";

    const string COR_DELAY_CLEAR_ORDER_CACHE = "COR_DELAY_CLEAR_ORDER_CACHE";

    const string IS_SAS_TICKET_IN_ING = "IS_SAS_TICKET_IN_ING";

    /// <summary>
    /// ��Ʊ��Ϣ
    /// </summary>
    /// <param name="ticketInOrderId"></param>
    /// <remarks>
    /// * ������Ʊ �õ� orderId  (��ֵ���֣������к� )???
    /// * Ʊ��orderId����sas 
    /// </remarks>
    private void OnHardwareSasTicketIn(string ticketInOrderId)
    {
        //Debug.LogError($"OnTicketIn  orderId: �㵥 {ticketInOrderId}");

        if (!DeviceUtils.IsCurSasBiller())
        {
            Debug.LogError($"��ǰ��sasֽ����");
            return;
        }


        // Ʊ��ȥ��
        if (orderIdCache.Contains(ticketInOrderId))
        {
            //Debug.LogError($"OnTicketIn  orderId: �㵥�ظ����� {ticketInOrderId}");
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

        // ����Ʊ��
        lastTicketInOrderId = ticketInOrderId;

        // ֪ͨsas-redisƱ��
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
    /// redis������ܸ�Ʊ
    /// </summary>
    public void SasApproveTicketIn(int moneyFromTicket)
    {
        moneyIn = moneyFromTicket;

        // �ȴ�Ӳ����Ӧ�ѽ��յ�Ʊ
        SBoxSandbox.BillApprove();
    }

    /// <summary>
    /// Sas-redis��������ܸ�Ʊ
    /// </summary>
    public void SasRejectTicketIn()
    {
        //֪ͨsas��Ʊ�����ܾ���Ʊ��
        SBoxSandbox.BillReject();

        // ֪ͨsas-redis���Ѿܾ���Ʊ��
        SasCommand.Instance.PushGeneralSasBillerInApproveResualt(0, lastTicketInOrderId);
        lastTicketInOrderId = "";
    }

    /// <summary> �����Ľ�Ʊ���� </summary>
    bool isRegularSasTicketIn => !string.IsNullOrEmpty(lastTicketInOrderId) && moneyIn > 0;

    void InitParamSasTicketIn()
    {
        lastTicketInOrderId = "";
        moneyIn = 0;
    }

    /// <summary>
    /// ��Ʊ����Ǯ��
    /// </summary>
    private void OnHardwareBillStacked()
    {
        if (!isRegularSasTicketIn) return;

        if (!DeviceUtils.IsCurSasBiller())
            return;

        int credit = moneyIn * _consoleBB.Instance.sasInOutScale; 

        //Debug.LogError($"Ҫ�ϷֵĽ�� {credit}");

        //MachineDeviceCommonBiz.Instance.RequestScoreUp(credit);

        SasCommand.Instance.PushGeneralSasBillerInApproveResualt(1, lastTicketInOrderId);

        // �������
        MachineDataManager.Instance.RequestScoreUp(credit, (Action<object>)((res) => {

            // �������

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

            // ÿ��ͳ��
            TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreUp((long)credit, _consoleBB.Instance.myCredit);

            InitParamSasTicketIn();

        }));
    }


    #endregion

}




/// <summary>
/// Sas��ӡ��Ʊ
/// </summary>
public partial class DeviceSasTicketInOut
{


    #region sas��Ʊ ����Ʊ��ť��sas��Ʊ����

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
            // ��ʾsas����ʧ��
            return;
        }

        if (IsCor(COR_IS_SAS_TICKET_OUT_ING))
            return;
        DoCor(COR_IS_SAS_TICKET_OUT_ING, DoTask(() => { },4000));  // ��ֹ�ظ��󴥷�

        // ������
        MaskPopupHandler.Instance.OpenPopup();

        // ����Ҫ�и����ʵ�λ�� ��һ�����ת�ɶ���Ǯ��
        // moneyOut = 1000;


        moneyOut = (int)_consoleBB.Instance.myCredit / _consoleBB.Instance.sasInOutScale;

        SasCommand.Instance.PushGeneralScoreDown(moneyOut);
    }

    void OnSasRedisRpcDownGetTicketOutOrder(string orderId)
    {
        //��ӡƱ
        PrinterJCM950(orderId, moneyOut, (code, msg) =>
        {
            MaskPopupHandler.Instance.ClosePopup();

            if (code != 0)
            {
                Debug.LogError($"JCM950 , code: {code} , msg: {msg}");
                return;
            }

            SasCommand.Instance.PushGeneralScoreDownResualt(orderId);

            //�㷨�����·�
            //int creditOut =  moneyOut * 1;
            int creditOut = moneyOut * _consoleBB.Instance.sasInOutScale;

            MachineDataManager.Instance.RequestScoreDown(creditOut, (Action<object>)((res) => {

                // �������

                long creditBefore = _consoleBB.Instance.myCredit;
                long creditAfter = _consoleBB.Instance.myCredit - creditOut;

                Debug.LogError($"Sas��ӡ�·� creditBefore={creditBefore} creditAfter={creditAfter} ");

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

                // ÿ��ͳ��
                TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreDown((long)creditOut, _consoleBB.Instance.myCredit);

            }));

        });

    }


    #endregion

}


/// <summary>
/// ���·֡���ӡ��Ӳ������
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

        // ���㷨����seqid
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
        // ���㷨����seqid
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
    /// ����ӡ��״̬
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

        int state = SBoxSandbox.PrinterState();///�ȼ���������Ӵ�ӡ��
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

         �Ȳ����״̬
        if (state == -2)
        {
            return new object[] { 1, "Printer Exception, please contact the administrator" };
        }*/
        return new object[] { 0, "" };



        /* ���������Ѿ�������
        //Debug.Log("�¿��ӡ��״̬ IsOnline:" + offlineCode.IsOnline.ToString() + " HasError: " + offlineCode.HasError.ToString() + " IsPaperPresent: " + offlineCode.IsPaperPresent.ToString() + "IsPaperLevelOkay:" + offlineCode.IsPaperLevelOkay);
        if (offlineCode.HasError)///��ӡ���д���
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
        if (!offlineCode.IsPaperLevelOkay)///Ϊfasle��������ֽ
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