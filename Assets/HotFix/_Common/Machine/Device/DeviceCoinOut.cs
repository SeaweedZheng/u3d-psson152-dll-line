
#define SQLITE_ASYNC
using GameMaker;
using SBoxApi;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _consoleBB = PssOn00152.ConsoleBlackboard02;

/// <summary>
/// ��Ʊ
/// </summary>
/// <remarks>
/// * ��ͨ��ť��Ʊ + ���м���
/// </remarks>
public class DeviceCoinOut :  MonoSingleton<DeviceCoinOut> //  CorBehaviour 
{
    const string COR_INIT_PRINTER = "COR_INIT_PRINTER";
    const string COR_IS_COIN_OUT_ING = "COR_IS_COIN_OUT_ING";
    const string COR_COIN_OUT_OUT_TIME = "COR_COIN_OUT_OUT_TIME";
    const string DEVICE_COIN_OUT_ORDER = "device_coin_out_order";


    string orderIdCoinOut;

    /// <summary> 1�Ҷ��ٷ� </summary>
    int coinOutRate;
    int targetCoinOutNum = 0;

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



    public  JSONNode _cacheCoinOutOrder = null;
    public  JSONNode cacheCoinOutOrder
    {
        get
        {
            if (_cacheCoinOutOrder == null)
                _cacheCoinOutOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DeviceCoinOut.DEVICE_COIN_OUT_ORDER, "{}"));
            return _cacheCoinOutOrder;
        }
        //set => _cacheCoinOutOrder = value;
    }


    private void OnEnable()
    {
        //if (!ApplicationSettings.Instance.isMachine)return;

        EventCenter.Instance.AddEventListener<int>(SBoxSanboxEventHandle.COIN_OUT, OnHardwareCoinOut);
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnCLearAllOrderCache);

    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(SBoxSanboxEventHandle.COIN_OUT, OnHardwareCoinOut);
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnCLearAllOrderCache);
    }


    private void OnCLearAllOrderCache(EventData res)
    {
        if (res.name == GlobalEvent.ClearAllOrderCache)
        {
            _cacheCoinOutOrder = null;
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_OUT_ORDER, "{}");
        }
    }



    int m_CoinOutDeviceNumber = 0;
    public int coinOutDeviceNumber
    {
        get => m_CoinOutDeviceNumber;
    }


    void InitParam()
    {
        hashCointOutCreditForTicketPerCredit = 0;
        finishCoinOutNum = 0;
        targetCoinOutNum = 0;
        orderIdCoinOut = "";
        coinOutRate = 0;
    }


    // ��ʱ��Ʊ��������Ʊ����Ϸ����ͬʱ��ʼ
    const string COR_DELAY_COIN_OUT = "COR_DELAY_COIN_OUT";

    /// <summary>
    /// ��Ʊ����Ʊ
    /// </summary>
    public void DoCoinOut()
    {
        DoCor(COR_DELAY_COIN_OUT,DoTask(_DoCoinOut,500)); // ��ʱ��Ʊ��������Ʊ����Ϸ����ͬʱ��ʼ
    }


    public void _DoCoinOut()
    {
        if (!_consoleBB.Instance.isMachineActive)
        {
            DebugUtils.LogWarning("Machine not activated");
            return;
        }

        //if (BlackboardUtils.GetValue<string>("./gameState") != "Idle") // ��������Ϸ�п��ܽ��� idle״̬
        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=24>Cannot coin out during the game period</size>"));
            return;
        }


        if (IsCor(COR_IS_COIN_OUT_ING))
            return;

        DoCor(COR_IS_COIN_OUT_ING, DoTask(() => { }, 4001)); //��ʱ�����ظ�����

        DeviceOrderReship.Instance.DelayReshipOrderRepeat();

        /* ��������ʱֱ�ӿ���
        if (isCoinOutError)
        {
            ShowCoinOutErrorTip();
            return;
        }*/

        // ������
        MaskPopupHandler.Instance.OpenPopup();


        InitParam();

        // �����ܻ����ٸ���
        targetCoinOutNum = DeviceUtils.GetCoinOutNum();

        //��������
        orderIdCoinOut = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinOut); // Guid.NewGuid().ToString();

        // ���� Ӳ����Ʊ
        SBoxSandbox.CoinOutStart(0, this.targetCoinOutNum, 0);
        

        DoCor(COR_COIN_OUT_OUT_TIME,
            DoTask(() =>
            {
                StopCoinOut();
                MaskPopupHandler.Instance.ClosePopup();
                CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                {
                    text = I18nMgr.T("<size=24>Ticket out outtime</size>"),
                    type = CommonPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = null,
                    isUseXButton = false,
                });

                DeviceOrderReship.Instance.DelayReshipOrderRepeat();

            }, 3001)
        );
    }




    /// <summary>
    /// ���м���
    /// </summary>
    public void DoCoinOutImmediately(int credit)
    {

        if (IsCor(COR_IS_COIN_OUT_ING))
            return;

        DoCor(COR_IS_COIN_OUT_ING, DoTask(() => { }, 4001)); //��ʱ�����ظ�����

        DeviceOrderReship.Instance.DelayReshipOrderRepeat();

        // ������(������Ҫ��͸�����֣���)
        MaskPopupHandler.Instance.OpenPopup();

        InitParam();

        // �����ܻ����ٸ���
        targetCoinOutNum = DeviceUtils.GetCoinOutNum(credit);

        //��������
        orderIdCoinOut = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinOut); //Guid.NewGuid().ToString();

        // ���� Ӳ����Ʊ
        SBoxSandbox.CoinOutStart(0, this.targetCoinOutNum, 0);
        

        DoCor(COR_COIN_OUT_OUT_TIME,
            DoTask(() =>
            {
                StopCoinOut();
                MaskPopupHandler.Instance.ClosePopup();

                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=24>Ticket out outtime</size>"));
                /*ErrorPopupHandler.Instance.OpenPopupSingle(new ErrorPopupInfo()
                {
                    text = I18nMgr.T("<size=24>Ticket out outtime</size>"),
                    type = ErrorPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = null,
                    isUseXButton = false,
                });*/

                EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT,
                    new EventData<string>(GlobalEvent.CoinOutError, "Ticket out outtime"));

                DeviceOrderReship.Instance.DelayReshipOrderRepeat();

            }, 3001)
        );
    }



    /// <summary> �Ƿ���Ʊ�� </summary>
    public bool isRegularCoinOuting
    {
        get => IsCor(COR_IS_COIN_OUT_ING) || IsCor(COR_COIN_OUT_OUT_TIME);
    }

    /// <summary> Ӳ����ֹ�˱� </summary>
    private void StopCoinOut()
    {
        if (Application.isEditor)
        {
            //MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_SADNBOX_COIN_OUT_STOP, "0");
            //Զ�̵���
            Debug.LogWarning("û��ʵ��Զ�̵��Եķ���");
        }
        else
        {
            SBoxSandbox.CoinOutStop(0);
        }
        MaskPopupHandler.Instance.ClosePopup();
    }




    bool isCoinOutError;
    int finishCoinOutNum = 0;

    
    /// <summary> ���1�ֶ�Ʊ��ui�·ֶ��� </summary>
    int hashCointOutCreditForTicketPerCredit = 0;
   // int hasCoint

    /// <summary>
    /// Ӳ����Ʊ��Ӧ
    /// </summary>
    /// <param name="coinOutNum01"></param>
    private void OnHardwareCoinOut(int coinOutNum01)
    {
        if (coinOutNum01 <= 0)
            return;

        
        MachineDataManager.Instance.RequestCounter(1, coinOutNum01, 2, (res) =>
        {
            int resault = (int)res;
            if (resault < 0)
                DebugUtils.LogError($"�˱���� : ����״̬��{resault}  �˱Ҹ�����{coinOutNum01}");
            else
                DebugUtils.Log($"�˱���� : ����״̬��{resault}  �˱Ҹ�����{coinOutNum01}");
        });

        //��������Ʊ������
        if (!isRegularCoinOuting) {
            StopCoinOut();
            //�������������̳�Ʊ������
            //isCoinOutError = true;
            //ShowCoinOutErrorTip();
            return;
        }


        DoCor(COR_IS_COIN_OUT_ING, DoTask(() => { }, 4001)); //��ʱ�����ظ�����

        DeviceOrderReship.Instance.DelayReshipOrderRepeat();

        // DebugUtil.LogError($"�����ԡ��˱Ҹ��ۻ�����{finishCoinOutNum}");

        finishCoinOutNum += coinOutNum01;
        if (finishCoinOutNum > this.targetCoinOutNum)
        {
            finishCoinOutNum = this.targetCoinOutNum;

            DebugUtils.LogError($"[Error] currCointOut = {coinOutNum01} targetCointOut = {this.targetCoinOutNum} finishCointOut = {finishCoinOutNum}");

            StopCoinOut();
            //isCoinOutError = true;
            //ShowCoinOutErrorTip();
        }

        if (!cacheCoinOutOrder.HasKey(orderIdCoinOut))
        {
            SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
            nodeOrder.Add("device_number", m_CoinOutDeviceNumber);
            nodeOrder.Add("type", "coin_out");
            nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            nodeOrder.Add("count", finishCoinOutNum); //���ٱ�
            nodeOrder.Add("order_id", orderIdCoinOut);
            nodeOrder.Add("credit_coin_out", DeviceUtils.GetCoinOutCredit(finishCoinOutNum));
            nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
            cacheCoinOutOrder.Add(orderIdCoinOut, nodeOrder);
        }
        else
        {
            cacheCoinOutOrder[orderIdCoinOut]["count"] = finishCoinOutNum;
            cacheCoinOutOrder[orderIdCoinOut]["credit_coin_out"] = DeviceUtils.GetCoinOutCredit(finishCoinOutNum);
        }
        SQLitePlayerPrefs03.Instance.SetString( DEVICE_COIN_OUT_ORDER, cacheCoinOutOrder.ToString());

        // ǰ��ˢ�·�����ʾ
        int outCredit = DeviceUtils.GetCoinOutCredit(coinOutNum01);
        if (MainBlackboardController.Instance.myTempCredit < outCredit)
        {
            // ˢ��ui��������ʾ
            MainBlackboardController.Instance.SetMyTempCredit(0);

            cacheCoinOutOrder[orderIdCoinOut]["credit_after"] = 0;

            ClearCor(COR_COIN_OUT_OUT_TIME);
            StopCoinOut();
            MaskPopupHandler.Instance.ClosePopup();
            HardwareSubCreditSaveOrder();
            return;
        }
        else
        {

            #region ���"��Ʊһ��"���� 1Ʊ��Ӧ��С���֣���int���ι��ˡ�
            hashCointOutCreditForTicketPerCredit += outCredit;
            if(outCredit == 0)
            {
                //����Ҫ�˵�Ʊ
                int targetTatalCreditOut = DeviceUtils.GetCoinOutCredit(finishCoinOutNum);
                if (targetTatalCreditOut - hashCointOutCreditForTicketPerCredit > 0) 
                {
                    outCredit = targetTatalCreditOut - hashCointOutCreditForTicketPerCredit;
                    hashCointOutCreditForTicketPerCredit = targetTatalCreditOut;
                }
            }
            #endregion


            // ˢ��ui��������ʾ
            MainBlackboardController.Instance.MinusMyTempCredit(outCredit, true, true);

            cacheCoinOutOrder[orderIdCoinOut]["credit_after"] =
               (long)cacheCoinOutOrder[orderIdCoinOut]["credit_before"] -
               (long)cacheCoinOutOrder[orderIdCoinOut]["credit_coin_out"];
            //_consoleBB.Instance.myCredit;
        }



        DoCor(COR_COIN_OUT_OUT_TIME,
            DoTask(() =>
            {
                StopCoinOut();
                MaskPopupHandler.Instance.ClosePopup();
                if (finishCoinOutNum < this.targetCoinOutNum)
                {
                    // ������Ʊ���ù� ������ϵ����Ա
                    // ��Ʊ�ˣ�
                    //string msg = "The tickets for the machine have been used up. Please contact the administrator.";
                    CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                    {
                        text = I18nMgr.T("<size=24>The tickets for the machine have been used up,Please contact the administrator</size>"),
                        type = CommonPopupType.OK,
                        buttonText1 = I18nMgr.T("OK"),
                        buttonAutoClose1 = true,
                        callback1 = delegate
                        {
                        },
                        isUseXButton = false,
                    });

                    EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT,
                        new EventData<string>(GlobalEvent.CoinOutError, "Tickets have been used up"));
                }

                HardwareSubCreditSaveOrder();

            }, 3001)
        );
    }


    void HardwareSubCreditSaveOrder()
    {
        DeviceOrderReship.Instance.DelayReshipOrderRepeat();

        // �������
        JSONNode nodeOrder = cacheCoinOutOrder[orderIdCoinOut];

        //Debug.Log("��Ʊ���ж������� : " + cacheCoinOutOrder.ToString());

        //Debug.Log("��Ʊ����id : " + orderIdCoinOut);
        //DebugUtil.LogError($"��ǰ��Ʊ������:{orderIdCoinOut}  ��Ʊ�������� : {nodeOrder.ToString()}");

        long creditCoinOut = (long)nodeOrder["credit_before"] - (long)nodeOrder["credit_after"];

        int coinOutNum = nodeOrder["count"];

        //Debug.Log($"Ҫ�۵�Ǯ : {creditCoinOut}  --- {(long)nodeOrder["credit_coin_out"]}");

        // �㷨����Ǯ��Ǯ
        MachineDataManager.Instance.RequestCoinOut(coinOutNum, (Action<object>)((res) =>
        {
            long creditBefore = _consoleBB.Instance.myCredit;
            _consoleBB.Instance.myCredit -= creditCoinOut;
            //Debug.LogError($"��ȥ����{orderIdCoinOut}: {creditBefore} - {creditCoinOut} = {_consoleBB.Instance.myCredit}");
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

            // ɾ�����嶩��
            cacheCoinOutOrder.Remove(orderIdCoinOut);
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_OUT_ORDER, cacheCoinOutOrder.ToString());


#if !SQLITE_ASYNC

            // �������
            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = nodeOrder["type"],
                    device_number = nodeOrder["device_number"],
                    order_id = nodeOrder["order_id"],
                    count = nodeOrder["count"],
                    credit = creditCoinOut,
                    credit_after = nodeOrder["credit_after"],
                    credit_before = nodeOrder["credit_before"],
                    in_out = 0,
                    created_at = nodeOrder["timestamp"],
                });
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);
#else
            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = nodeOrder["type"],
                    device_number = nodeOrder["device_number"],
                    order_id = nodeOrder["order_id"],
                    count = nodeOrder["count"],
                    credit = creditCoinOut,
                    credit_after = nodeOrder["credit_after"],
                    credit_before = nodeOrder["credit_before"],
                    in_out = 0,
                    created_at = nodeOrder["timestamp"],
                });
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif



            //ÿ��ͳ��
            TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinOut(creditCoinOut, nodeOrder["credit_after"]);

            //��λ����
            InitParam();

            // ��Ʊ����¼�
            EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, new EventData<long>(GlobalEvent.CoinOutSuccess, creditCoinOut));
        }));
    }


    void ShowCoinOutErrorTip()=>
    TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=24>The refund ticket has been damaged. Please contact the administrator.</size>"));



    /// <summary>
    /// ��������
    /// </summary>
    public IEnumerator ReshipOrde()
    {

        bool isNext = false;

        JSONNode cacheOrders = JSONNode.Parse(cacheCoinOutOrder.ToString());
        foreach (KeyValuePair<string, JSONNode> item in cacheOrders)
        {
            JSONNode order = item.Value;
            string orderId = item.Key;
            long creditCoinOut = order["credit_coin_out"];
            int coinOutNum = order["count"];

            long creditBefore = _consoleBB.Instance.myCredit;
            long creditAfter = _consoleBB.Instance.myCredit - creditCoinOut;

            if (creditAfter < 0)
            {
                DebugUtils.LogError($"��OrderReship - CoinOut�� �㵥����ʧ�ܣ� creditAfter = {creditAfter}  order = {order.ToString()}");
                continue;
            }
            /*
            if (creditAfter < 0)
            {
                creditAfter = 0;
            }*/

            //DebugUtil.LogError($"��OrderReship - CoinOut����orderId = {orderId}  creditCoinOut = {creditCoinOut}  coinOutNum = {coinOutNum}");

            MachineDataManager.Instance.RequestCoinOut(coinOutNum, (Action<object>)((res) =>
            {
                // ɾ�����嶩��
                cacheCoinOutOrder.Remove(orderId);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_OUT_ORDER, cacheCoinOutOrder.ToString());



#if !SQLITE_ASYNC

                // �������
                string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = order["type"],
                        device_number = order["device_number"],
                        order_id = order["order_id"],
                        count = coinOutNum,
                        credit = creditCoinOut,
                        credit_after = creditAfter,
                        credit_before = creditBefore,
                        in_out = 0,
                        created_at = order["timestamp"],
                    });
                SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);


#else
                // �������
                string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = order["type"],
                        device_number = order["device_number"],
                        order_id = order["order_id"],
                        count = coinOutNum,
                        credit = creditCoinOut,
                        credit_after = creditAfter,
                        credit_before = creditBefore,
                        in_out = 0,
                        created_at = order["timestamp"],
                    });
                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif




                _consoleBB.Instance.myCredit = creditAfter;
                MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

                TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinOut(creditCoinOut, creditAfter);

                isNext = true;
            }));

            yield return new WaitUntil(()=> isNext == true);
            isNext = false;
        }
    }




}
