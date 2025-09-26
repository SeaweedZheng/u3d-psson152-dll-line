#define SQLITE_ASYNC
using GameMaker;
using MoneyBox;
using Newtonsoft.Json;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;


/// <summary>
/// ������̨�·�
/// </summary>
public partial class DeviceMoneyBox : MonoSingleton<DeviceMoneyBox> 
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


    void Start()
    {

    }



    #region ����̨�·ֲ���ӡ��ά��
    const string RPC_MARK_MACHINR_QRCODE_OUT = "RPC_MARK_MACHINR_QRCODE_OUT";


    /// <summary> ��ӡ��Ʊ�� </summary>
    const string COR_IS_QRCODE_OUT_ING = "COR_IS_QRCODE_OUT_ING";


    [Button]
    public void DoQRCodeOut(int credit)
    {
        if (!MoneyBoxManager.Instance.isLogin)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Failed to link to the money box server.")); // Failed to link to the cash box server
            return;
        }

        if (!DeviceUtils.IsCurQRCodePrinter())
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Please link the QR code printer first."));
            //����ѡ���ά���ӡ��
            return;
        }


        if (_consoleBB.Instance.isUsePrinter == false)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Printer function not enabled."));
            return;
        }


        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Cannot score down during the game period"));
            return;
        }


        if (IsCor(COR_IS_QRCODE_OUT_ING))
            return;

        DoCor(COR_IS_QRCODE_OUT_ING, DoTask(() => { }, 8001)); //��ʱ�����ظ�����


        DeviceOrderReship.Instance.DelayReshipOrderRepeat();


        MoneyBoxMoneyInfo moneyOutInfo = DeviceUtils.GetMoneyOutInfo(credit);

        DebugUtils.Log($"Ҫ�˵���Ϸ���֣�{credit}   ��Ǯ��Ϣ��{JsonConvert.SerializeObject(moneyOutInfo)}");
        if (moneyOutInfo.money > 0)
        {

            MaskPopupHandler.Instance.OpenPopup();


            MoneyBoxManager.Instance.RequesCreateOrderIdWhenPrintQRCodeOut(moneyOutInfo.money,
            (res) =>
            {
                JSONNode data = res as JSONNode;
                //DebugUtil.LogError($"@@@@ �յ����� �� {data.ToString()}");


                JSONNode storeInfo = data["Data"]["storeInfo"];

                string qrcode = (string)data["Data"]["qrcode"];
                string showNo = (string)data["Data"]["showno"];
                string ticketType = "Money Type";
                long money = (long)data["Data"]["money"];
                //string companyName = $"{(string)storeInfo["tStoreName"]}:" + (string)storeInfo["storeName"];
                string companyName = (string)storeInfo["storeName"];
                string companyCity = $"{(string)storeInfo["tCityAddr"]}:" + (string)storeInfo["storeAddr"];
                string companyAddress = $"{(string)storeInfo["tStreetAddr"]}:" + (string)storeInfo["storeDetailAddr"];
                string companyEmail = $"{(string)storeInfo["tStoreEmail"]}:" + (string)storeInfo["storeEmail"];
                string telephone = $"{(string)storeInfo["tStoreTel"]}:" + (string)storeInfo["storeTel"];
                string deviceName = ApplicationSettings.Instance.gameTheme;
                //bool isCheckIsBusy = true;

                MachineDeviceCommonBiz.Instance.devicePrinterOutQRCode.
                    PrintTicket(
                    qrcode,
                    showNo,
                    ticketType,
                    money,
                    companyName,
                    companyCity,
                    companyAddress,
                    companyEmail,
                    telephone,
                    deviceName,
                    (code, msg) => {

                        if (code ==0)
                        {
                            //���ɶ���-����
                            string orderIdQRCodeOut = qrcode;

                            // �����涩�����桿��ά���·� 
                            if (!cacheQRCodeOutOrder.HasKey(orderIdQRCodeOut))
                            {
                                SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
                                nodeOrder.Add("type", "money_box_qrcode_out");
                                nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                                nodeOrder.Add("order_id", orderIdQRCodeOut);
                                nodeOrder.Add("as_coin_in_count", moneyOutInfo.asCoinInCount); //������Ʊ
                                nodeOrder.Add("as_credit", moneyOutInfo.asCredit);
                                nodeOrder.Add("money", moneyOutInfo.money);
                                nodeOrder.Add("money_per_coin_in", MoneyBoxModel.Instance.moneyPerCoinIn);
                                nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
                                nodeOrder.Add("in_out", 0);
                                cacheQRCodeOutOrder.Add(orderIdQRCodeOut, nodeOrder);
                                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_OUT_ORDER, cacheQRCodeOutOrder.ToString());
                            }
                            else
                            {
                                DebugUtils.LogError($"���㵥���桿�� ��ά���·� - �������ظ��� {orderIdQRCodeOut}");
                            }

                            //DebugUtil.Log($"��ά���Ʊ����-���ػ������ݣ�{cacheQRCodeOutOrder.ToString()}");

                            OnSendResultQRCodeOut(orderIdQRCodeOut, (code,msg) =>
                            {

                                MaskPopupHandler.Instance.ClosePopup();

                                // ��Ʊ�ɹ����Ѷһ� {0} ���֡�  ��Ʊʧ��

                                string txt = code == 0 ? string.Format(I18nMgr.T("Ticket issuance successful, {0} credits have been redeemed."), moneyOutInfo.asCredit) :
                                    I18nMgr.T("Ticket issuance failed.");
                                TipPopupHandler.Instance.OpenPopupOnce(txt);

                                ClearCor(COR_IS_QRCODE_OUT_ING);
                            });
                        }
                        else
                        {
                            TipPopupHandler.Instance.OpenPopupOnce("qrcode print failed !");
                        }

                        DebugUtils.LogError($"��ά���ӡ����� {code} , {msg}");
                    });

            }, (error) =>
            {
                // ��ʾ��������
                TipPopupHandler.Instance.OpenPopupOnce(string.Format(I18nMgr.T("Request failed: [{0}]"),
                    Code.DEVICE_MB_GET_ORDER_ID_FAIL));

                //DebugUtil.LogError($"@@@@ �յ����� ����������{error.msg} ");


            }, RPC_MARK_MACHINR_QRCODE_OUT);
        }
        else
        {
            //tip: ��ѡ�����Ľ��
        }
    }


    const string DEVICE_QRCODE_OUT_ORDER = "device_qrcode_out_order";

    public JSONNode _cacheQRCodeOutOrder = null;

    public JSONNode cacheQRCodeOutOrder
    {
        get
        {
            if (_cacheQRCodeOutOrder == null)
                _cacheQRCodeOutOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_QRCODE_OUT_ORDER, "{}"));
            return _cacheQRCodeOutOrder;
        }
        //set => _cacheCoinOutOrder = value;
    }

    //string orderIdQRCodeOut;


    void OnSendResultQRCodeOut(string orderIdQRCodeOut, Action<int,string> onFinishCallBack)
    {

        if (!cacheQRCodeOutOrder.HasKey(orderIdQRCodeOut))
        {
            string msg = $"can not find order: {orderIdQRCodeOut} at cache";
            onFinishCallBack?.Invoke(1, msg);
            return;
        }
        JSONNode order01 = cacheQRCodeOutOrder[orderIdQRCodeOut];
        int creditOut = (int)order01["as_credit"];

        if (_consoleBB.Instance.myCredit < creditOut)
        {
            string msg = $" Error SendResultQRCodeOut   order: {order01.ToString()}";
            DebugUtils.LogError(msg);
            onFinishCallBack?.Invoke(1, msg);
            return;
        }

        // ���������ϱ�
        MoneyBoxManager.Instance.RequestSendOrderResultWhenPrintQRCodeOut(orderIdQRCodeOut, (Action<object>)((res) =>
        {
            JSONNode resNode = res as JSONNode;

            if (!resNode.HasKey("Data"))
            {
                string msg = $"can not find field: 'Data' at rpc down";
                DebugUtils.LogError(msg + $" res:{resNode.ToString()}");
                onFinishCallBack?.Invoke(1, msg);
                return;
            }

            if (!resNode["Data"].HasKey("qrcode"))
            {
                string msg = $"can not find field: 'qrcode' at rpc down";
                DebugUtils.LogError(msg + $" res:{resNode.ToString()}");
                onFinishCallBack?.Invoke(1, msg);
                return;
            }

            string orderId = (string)resNode["Data"]["qrcode"];

            if (!cacheQRCodeOutOrder.HasKey(orderId))
            {
                string msg = $"can not find order: {orderId} at cache";
                DebugUtils.LogError(msg +  $"rpc down res:{resNode.ToString()}    ");
                onFinishCallBack?.Invoke(1, msg);
                return;
            }

            // ���ػ���
            JSONNode order = cacheQRCodeOutOrder[orderId];
            int creditOut = (int)order["as_credit"];

            // ֪ͨ�㷨��
            MachineDataManager.Instance.RequestScoreDown(creditOut, (Action<object>)((res) =>
            {

                // ɾ�����嶩��
                cacheQRCodeOutOrder.Remove(orderId);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_OUT_ORDER, cacheQRCodeOutOrder.ToString());


#if !SQLITE_ASYNC

                // �������
                string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = order["type"],
                        device_number = 0,
                        order_id = order["order_id"],
                        count = order["as_coin_in_count"],
                        credit = creditOut,
                        credit_after = _consoleBB.Instance.myCredit - creditOut,
                        credit_before = _consoleBB.Instance.myCredit,
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
                        device_number = 0,
                        order_id = order["order_id"],
                        count = order["as_coin_in_count"],
                        credit = creditOut,
                        credit_after = _consoleBB.Instance.myCredit - creditOut,
                        credit_before = _consoleBB.Instance.myCredit,
                        in_out = 0,
                        created_at = order["timestamp"],
                    });
                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif



                // �޸���ҽ��
                _consoleBB.Instance.myCredit -= creditOut;
                MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

                // ÿ��ͳ��
                TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreDown((long)creditOut, _consoleBB.Instance.myCredit);

                onFinishCallBack?.Invoke(0,"");
            }));

        }),
        (error) =>
        {
            if (error.response != null)
            {
                JSONNode resNode = error.response as JSONNode;

                if (resNode.HasKey("qrcode"))
                {
                    string orderIdQRCodeOut = (string)resNode["qrcode"];

                    // ɾ����������
                    if (cacheQRCodeOutOrder.HasKey(orderIdQRCodeOut))
                    {
                        cacheQRCodeOutOrder.Remove(orderIdQRCodeOut);
                        SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_OUT_ORDER, cacheQRCodeOutOrder.ToString());
                    }
                }
            }
            onFinishCallBack?.Invoke(1, error.msg);
        });
    }
    #endregion


}

/// <summary>
/// ������̨�Ϸ�
/// </summary>
public partial class DeviceMoneyBox : MonoSingleton<DeviceMoneyBox>
{
    const string RPC_MARK_MACHINR_QRCODE_IN = "RPC_MARK_MACHINR_QRCODE_IN";

    const string DEVICE_QRCODE_IN_ORDER = "device_qrcode_in_order";

    public JSONNode _cacheQRCodeInOrder = null;
    public JSONNode cacheQRCodeInOrder
    {
        get
        {
            if (_cacheQRCodeInOrder == null)
                _cacheQRCodeInOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_QRCODE_IN_ORDER, "{}"));
            return _cacheQRCodeInOrder;
        }
        //set => _cacheQRCodeInOrder = value;
    }

    /// <summary> ��ӡ��Ʊ�� </summary>
    const string COR_IS_QRCODE_IN_ING = "COR_IS_QRCODE_IN_ING";


    /// <summary> ��ǵ�����ȷ�϶�ά���Ϸ֡� </summary>
    const string MARK_POPUP_CONTINUE_QRCODE_IN = "MARK_POPUP_CONTINUE_QRCODE_IN";

    #region ����̨ɨ���ά�벢�Ϸ�


    public void DoQRCodeIn(string qrcodeScan)
    {
        if (CommonPopupHandler.Instance.isOpen(MARK_POPUP_CONTINUE_QRCODE_IN))
        {
            DebugUtils.Log("��money box ui�������ܴ˶�ά�룬����ά���Ϸ֡�ȷ�ϵ����Ѵ� ");
            return;
        }

        if (IsCor(COR_IS_QRCODE_IN_ING))
        {
            DebugUtils.Log("��money box ui�������ܴ˶�ά�룬����ά���Ϸ֡����������� ");
            return;
        }

        DoCor(COR_IS_QRCODE_IN_ING, DoTask(() => { }, 4001)); //��ʱ�����ظ�����

        DeviceOrderReship.Instance.DelayReshipOrderRepeat();


        if (!MoneyBoxManager.Instance.isLogin)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Failed to link to the money box server.")); // Failed to link to the cash box server
            return;
        }



        /*
        "money": 2921, // ���
        "qrcode": "bank:1743597275871507300_743", // ��ά������
        "money_per_coin_in": 0.5,
        "money_per_coin_out": 0.5
        */
        MoneyBoxManager.Instance.RequestCheckQRCodeWhenScanQRCode(qrcodeScan,
        (res) =>
        {
            DebugUtils.Log("��money box ui��ɨ���ά�룬����ĵ������ݣ� " + res.ToString());

            JSONNode resNode = res as JSONNode;
            int money = resNode["Data"]["money"];
            float moneyPerCoinIn = resNode["Data"]["money_per_coin_in"];
            string qrcode = resNode["Data"]["qrcode"];

            MoneyBoxMoneyInfo moneyInfo = DeviceUtils.GetMoneyInInfo(money, moneyPerCoinIn);



            DeviceOrderReship.Instance.ClearReshipOrderRepeat();

            CommonPopupHandler.Instance.OpenPopup(new CommonPopupInfo()
            {
                mark = MARK_POPUP_CONTINUE_QRCODE_IN,
                text = string.Format(
                    I18nMgr.T("Using this OR Bank Code, you can add {0} points to your current account. Do you want to continue?"), moneyInfo.asCredit),
                type = CommonPopupType.YesNo,  
                isUseXButton = false,

                buttonText1 = I18nMgr.T("Cancel"),
                buttonAutoClose1 = true,
                callback1 = () =>
                {
                    DeviceOrderReship.Instance.DelayReshipOrderRepeat();
                    DebugUtils.Log("��money box ui�����ȡ�������һ���ά��");
                },

                buttonAutoClose2 = true,
                buttonText2 = I18nMgr.T("OK"),
                callback2 = () =>
                {
                    DebugUtils.Log("��money box ui���һ���ά��");

                    //���ɶ���-����
                    string orderIdQRCodeIn = qrcode;

                    // �����涩�����桿��ά���·� 
                    if (cacheQRCodeInOrder.HasKey(orderIdQRCodeIn))
                    {
                        cacheQRCodeInOrder.Remove(orderIdQRCodeIn);
                    }

                    SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
                    nodeOrder.Add("type", "money_box_qrcode_in");
                    nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    nodeOrder.Add("order_id", orderIdQRCodeIn);
                    nodeOrder.Add("as_coin_in_count", moneyInfo.asCoinInCount); //������Ʊ
                    nodeOrder.Add("as_credit", moneyInfo.asCredit);
                    nodeOrder.Add("money", moneyInfo.money);
                    nodeOrder.Add("money_per_coin_in", moneyPerCoinIn);
                    nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
                    nodeOrder.Add("in_out", 1);
                    cacheQRCodeInOrder.Add(orderIdQRCodeIn, nodeOrder);
                    SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_IN_ORDER, cacheQRCodeInOrder.ToString());

                    DoCor(COR_IS_QRCODE_IN_ING, DoTask(() => { }, 8001)); //��ʱ�����ظ�����

                    OnCacheQRCodeIn(orderIdQRCodeIn, (code,msg) => {       
                        TipPopupHandler.Instance.OpenPopupOnce(msg);
                        ClearCor(COR_IS_QRCODE_IN_ING);
                        DeviceOrderReship.Instance.DelayReshipOrderRepeat();
                    });
                    
                },
            });
            
        },
        (error) =>
        {
            JSONNode node = error.response as JSONNode;
            InvalidQRCodeIn(node);

            TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The QR code is invalid. Please check and try again."));
        }, RPC_MARK_MACHINR_QRCODE_IN);
    }



    void OnCacheQRCodeIn(string orderIdQRCodeIn, Action<int,string> onFinishCallback)
    {
        if (!cacheQRCodeInOrder.HasKey(orderIdQRCodeIn))
        {
            //onFinish?.Invoke(false, I18nMgr.T("The QR code is invalid. Please check and try again.")+"[1]");
            onFinishCallback?.Invoke(1, I18nMgr.T("The QR code is invalid. Please check and try again."));
            return;
        }


        MoneyBoxManager.Instance.RequestConsumeQRCode(orderIdQRCodeIn,
        (Action<object>)((res) =>
        {
            JSONNode data = res as JSONNode;

            string qrcode = (string)data["Data"]["qrcode"];
            int money = (int)data["Data"]["money"];
            float moneyPerCoinIn = (float)data["Data"]["money_per_coin_in"];


            JSONNode order = cacheQRCodeInOrder[orderIdQRCodeIn];

            string qrcodeFromOrder = order["order_id"];
            int asCreditFromOrder = order["as_credit"];
            int asCoinInCountFromOrder = order["as_coin_in_count"];
            int moneyFromOrder = order["money"];
            float moneyPerCoinInFromOrder = order["money_per_coin_in"];

            if (qrcodeFromOrder != qrcode 
            || money != moneyFromOrder 
            || moneyPerCoinIn != moneyPerCoinInFromOrder) 
            {
                //��鶩���ڵĽ�� ��id �ͷ������Ĳ����ϣ�����
                DebugUtils.LogError($"���������-��ά��һ��Ϸ֣��������ݺͱ��ض��������ϣ� �������ݣ�{order.ToString()}");
                DebugUtils.LogError($"���������-��ά��һ��Ϸ֣��������ݺͱ��ض��������ϣ� �������������ݣ�{data.ToString()}");
                onFinishCallback?.Invoke(1, I18nMgr.T("The QR code is invalid. Please check and try again."));
                return;
            }

            // ֪ͨ�㷨��
            MachineDataManager.Instance.RequestScoreUp(asCreditFromOrder, (Action<object>)((res) =>
            {
                // ɾ�����嶩��
                cacheQRCodeInOrder.Remove(orderIdQRCodeIn);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_IN_ORDER, cacheQRCodeInOrder.ToString());



#if !SQLITE_ASYNC
                // �������
                string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = order["type"],
                        device_number = 0,
                        order_id = qrcodeFromOrder,
                        count = asCoinInCountFromOrder,
                        credit = asCreditFromOrder,
                        credit_after = _consoleBB.Instance.myCredit + asCreditFromOrder,
                        credit_before = _consoleBB.Instance.myCredit,
                        in_out = 1,
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
                        device_number = 0,
                        order_id = qrcodeFromOrder,
                        count = asCoinInCountFromOrder,
                        credit = asCreditFromOrder,
                        credit_after = _consoleBB.Instance.myCredit + asCreditFromOrder,
                        credit_before = _consoleBB.Instance.myCredit,
                        in_out = 1,
                        created_at = order["timestamp"],
                    });
                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif




                // �޸���ҽ��
                _consoleBB.Instance.myCredit += asCreditFromOrder;
                MainBlackboardController.Instance.AddOrSyncMyCreditToReal(asCreditFromOrder);


                // ÿ��ͳ��
                TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreUp((long)asCreditFromOrder, _consoleBB.Instance.myCredit);

                onFinishCallback?.Invoke(0,  string.Format(I18nMgr.T("QR code recharge {0} game credits completed."), asCreditFromOrder));
            }));

        }), (error) =>
        {

            JSONNode node = error.response as JSONNode;

            InvalidQRCodeIn(node);

            onFinishCallback?.Invoke(1, I18nMgr.T("The QR code is invalid. Please check and try again."));

        }, RPC_MARK_MACHINR_QRCODE_IN);

    }

    void InvalidQRCodeIn(JSONNode res)
    {
        if (!res.HasKey("Data"))
        {
            DebugUtils.LogError($"���������-��ά��һ��Ϸ֣���������û��Data�ֶ�: {res.ToString()}");

        }
        else if (!res["Data"].HasKey("qrcode"))
        {
            DebugUtils.LogError($"���������-��ά��һ��Ϸ֣���������û��Data/qrcode�ֶ�: {res.ToString()}");
        }
        else
        {
            string qrcode = res["Data"]["qrcode"];

            if (cacheQRCodeInOrder.HasKey(qrcode))
            {
                // ɾ�����嶩��
                cacheQRCodeInOrder.Remove(qrcode);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_IN_ORDER, cacheQRCodeInOrder.ToString());

                DebugUtils.Log("ɾ���Ϸֶ�ά�뱾�ػ��棺 {qrcode}");
            }
        }
    }
    #endregion



    #region Ӳ����ά��Ӳ��ɨ��


    /// <summary> ��̨��Ǯ�� ��ӡ��ά���ǰ׺</summary>
    string patternBank = @"bank:(.*?)&QRCodeEnd&";
    /// <summary> �ֻ����ɶ�ά�루�ֱ�Ǯ�� </summary>
    //string patternQRCode = @"qr_code:(.*?)&QRCodeEnd&";
    private CheckInputStatus _CheckInputStatus = CheckInputStatus.None;
    private string inputValue = "";


    void Update()
    {
        OnUpdateScanQRCode();
    }

    /// <summary>
    /// ѭ��ɨ���ά��
    /// </summary>
    void OnUpdateScanQRCode()
    {
        if (ApplicationSettings.Instance.isMachine || Application.isEditor)
        {
            if (Input.anyKeyDown)
            {
                if (_CheckInputStatus == CheckInputStatus.Using)
                {
                    inputValue = "";
                    return;
                }
                string temp = Input.inputString;
                if (!string.IsNullOrEmpty(temp))
                {
                    inputValue += temp;
                    Match match  = Regex.Match(inputValue, patternBank);
                    if (match.Success)
                    {
                        string bankValue = match.Groups[1].Value;
                        inputValue = "";
                        CheckBankOrderInputHandle(bankValue);
                    }
                }
            }
        }
    }


    /// <summary>   
    /// ���յ�����̨��bank��ά����Ϣ�Ĵ���
    /// </summary>
    /// <param name="message">��Ϣ����(�Ѿ�ȥ����ǰ׺�ͺ�׺)</param>
    public void CheckBankOrderInputHandle(string message)
    {
        //if (CheckCanShowUsePop())
        if (true)
        {
            try
            {
                string qrcode = $"bank:{MoneyBoxUtils.DecryptQRCode(message)}";
                DebugUtils.Log($"ɨ���ά�����ݣ�{message}  ���ܣ� {qrcode}");
                DoQRCodeIn(qrcode);
            }
            catch
            {

            }
        }
        else
        {
            inputValue = "";
        }
    }
    #endregion



}

/// <summary>
/// Ǯ��ָ������̨�Ϸ�
/// </summary>
public partial class DeviceMoneyBox : MonoSingleton<DeviceMoneyBox>
{


    const string DEVICE_MB_REQ_QRCODE_IN_ORDER = "device_mb_req_qrcode_in_order";

    public JSONNode _cacheMBReqQRCodeInOrder = null;
    public JSONNode cacheMBReqQRCodeInOrder
    {
        get
        {
            if (_cacheMBReqQRCodeInOrder == null)
                _cacheMBReqQRCodeInOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_MB_REQ_QRCODE_IN_ORDER, "{}"));
            return _cacheMBReqQRCodeInOrder;
        }
    }


    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_MONEY_BOX_EVENT, OnRpcDownRequestMachineQRCodeUp);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_MONEY_BOX_EVENT, OnRpcDownRequestMachineQRCodeUp);
    }


    const string COR_IS_MB_RQYUEST_MACH_QRCODE_IN_ING = "";
    void OnRpcDownRequestMachineQRCodeUp(EventData res)
    {
        if (res.name == GlobalEvent.MoneyBoxRequestMachineQRCodeUp)
        {

            /*
             ��money box��rpc down: CassetteRechange 
            data: {"device_id":"11009001","orderno":"PRC_1745237618351453900_9_Qp8Ws6TQlv","money":5,"action_type":"cashin_rechange"}
            */

            DeviceOrderReship.Instance.DelayReshipOrderRepeat();

            JSONNode data = res.value as JSONNode;
            string orderIdQRCodeIn = data["orderno"];
            int money = data["money"];

            // ����ȥ��(���������ظ��·�ȷ�ϻ�̨�յ�������)
            if (cacheMBReqQRCodeInOrder.HasKey(orderIdQRCodeIn))
            {
                MoneyBoxManager.Instance.RequestReturnQrderIdWhenMBReqMacQrIn(orderIdQRCodeIn, money);
                return;
            }

            if (IsCor(COR_IS_MB_RQYUEST_MACH_QRCODE_IN_ING))  //��ֹ��������ֻ����һ��������
            {
                return;
            }
            DoCor(COR_IS_MB_RQYUEST_MACH_QRCODE_IN_ING, DoTask(() => { }, 4001)); //��ʱ�����ظ�����

            //����id
            MoneyBoxManager.Instance.RequestReturnQrderIdWhenMBReqMacQrIn(orderIdQRCodeIn, money);

            float moneyPerCoinIn = MoneyBoxModel.Instance.moneyPerCoinIn;

            MoneyBoxMoneyInfo moneyInfo = DeviceUtils.GetMoneyInInfo(money, moneyPerCoinIn);

            //���ɶ���-����
            SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
            nodeOrder.Add("type", "money_box_qrcode_in");
            nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            nodeOrder.Add("order_id", orderIdQRCodeIn);
            nodeOrder.Add("as_coin_in_count", moneyInfo.asCoinInCount); //������Ʊ
            nodeOrder.Add("as_credit", moneyInfo.asCredit);
            nodeOrder.Add("money", moneyInfo.money);
            nodeOrder.Add("money_per_coin_in", moneyPerCoinIn);
            nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
            nodeOrder.Add("in_out", 1);
            cacheMBReqQRCodeInOrder.Add(orderIdQRCodeIn, nodeOrder);
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_MB_REQ_QRCODE_IN_ORDER, cacheMBReqQRCodeInOrder.ToString());

            OnCacheMBReqQRCodeIn(orderIdQRCodeIn, (code, msg) =>
            {
                if (code == 0)
                {
                    CommonPopupHandler.Instance.OpenPopupSingle(
                       new CommonPopupInfo()
                       {
                           isUseXButton = false,
                           buttonAutoClose1 = true,
                           type = CommonPopupType.OK,
                           text = string.Format(I18nMgr.T("Money Box recharge {0} game credits completed."), moneyInfo.asCredit),
                           buttonText1 = I18nMgr.T("OK"),
                       });
                }
                else{
                    //TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Failed to link to the money box server."));
                }

                DeviceOrderReship.Instance.DelayReshipOrderRepeat();
            });

        }
    }


    void OnCacheMBReqQRCodeIn(string orderIdQRCodeIn, Action<int, string> onFinishCallback)
    {

        JSONNode cacheOrder = cacheMBReqQRCodeInOrder[orderIdQRCodeIn];

        int asCredit = cacheOrder["as_credit"];
        int asCoinInCount = cacheOrder["as_coin_in_count"];

        // ֪ͨ��������������
        MoneyBoxManager.Instance.RequestSendOrderWhenMBReqMacQrIn(orderIdQRCodeIn, cacheOrder["money"], (Action<object>)((res) =>
        {

            // ֪ͨ�㷨��
            MachineDataManager.Instance.RequestScoreUp(asCredit, (Action<object>)((res) =>
            {

                // ɾ�����嶩��
                cacheMBReqQRCodeInOrder.Remove(orderIdQRCodeIn);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_MB_REQ_QRCODE_IN_ORDER, cacheMBReqQRCodeInOrder.ToString());



#if !SQLITE_ASYNC

                // �������
                string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = cacheOrder["type"],
                        device_number = 0,
                        order_id = orderIdQRCodeIn,
                        count = asCoinInCount,
                        credit = asCredit,
                        credit_after = _consoleBB.Instance.myCredit + asCredit,
                        credit_before = _consoleBB.Instance.myCredit,
                        in_out = 1,
                        created_at = cacheOrder["timestamp"],
                    });
                SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else
                // �������
                string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = cacheOrder["type"],
                        device_number = 0,
                        order_id = orderIdQRCodeIn,
                        count = asCoinInCount,
                        credit = asCredit,
                        credit_after = _consoleBB.Instance.myCredit + asCredit,
                        credit_before = _consoleBB.Instance.myCredit,
                        in_out = 1,
                        created_at = cacheOrder["timestamp"],
                    });
                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif



                // �޸���ҽ��
                _consoleBB.Instance.myCredit += asCredit;
                //MainBlackboardController.Instance.SyncMyTempCreditToReal(true);
                MainBlackboardController.Instance.AddOrSyncMyCreditToReal(asCredit);

                // ÿ��ͳ��
                TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreUp((long)asCredit, _consoleBB.Instance.myCredit);


                onFinishCallback?.Invoke(0,"");

            }));

        }), (error) =>
        {
            JSONNode node = error.response as JSONNode;

            InvalidQRCodeIn(node);

            onFinishCallback?.Invoke(1, "");
        });
    }




    /// <summary>
    /// ��������
    /// </summary>
    public IEnumerator ReshipOrde()
    {

        bool isNext = false;

        // �����·�
        JSONNode cacheOrders = JSONNode.Parse(cacheQRCodeOutOrder.ToString());
        foreach (KeyValuePair<string, JSONNode> item in cacheOrders)
        {
            string orderId = item.Key;
            OnSendResultQRCodeOut(orderId, (code, msg) =>
            {
                isNext = true;
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;
        }


        // �����Ϸ�
        cacheOrders = JSONNode.Parse(cacheQRCodeInOrder.ToString());
        foreach (KeyValuePair<string, JSONNode> item in cacheOrders)
        {
            string orderId = item.Key;
            OnCacheQRCodeIn(orderId, (code, msg) =>
            {
                isNext = true;
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;
        }


        // Ǯ��ָ���Ϸ�
        cacheOrders = JSONNode.Parse(cacheMBReqQRCodeInOrder.ToString());
        foreach (KeyValuePair<string, JSONNode> item in cacheOrders)
        {

            string orderId = item.Key;
            OnCacheMBReqQRCodeIn(orderId, (code, msg) =>
            {
                isNext = true;
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;
        }
    }



}









