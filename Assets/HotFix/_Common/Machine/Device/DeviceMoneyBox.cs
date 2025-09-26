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
/// 本机机台下分
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



    #region 本机台下分并打印二维码
    const string RPC_MARK_MACHINR_QRCODE_OUT = "RPC_MARK_MACHINR_QRCODE_OUT";


    /// <summary> 打印出票中 </summary>
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
            //请先选择二维码打印机
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

        DoCor(COR_IS_QRCODE_OUT_ING, DoTask(() => { }, 8001)); //延时避免重复触发


        DeviceOrderReship.Instance.DelayReshipOrderRepeat();


        MoneyBoxMoneyInfo moneyOutInfo = DeviceUtils.GetMoneyOutInfo(credit);

        DebugUtils.Log($"要退的游戏积分：{credit}   退钱信息：{JsonConvert.SerializeObject(moneyOutInfo)}");
        if (moneyOutInfo.money > 0)
        {

            MaskPopupHandler.Instance.OpenPopup();


            MoneyBoxManager.Instance.RequesCreateOrderIdWhenPrintQRCodeOut(moneyOutInfo.money,
            (res) =>
            {
                JSONNode data = res as JSONNode;
                //DebugUtil.LogError($"@@@@ 收到数据 ： {data.ToString()}");


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
                            //生成订单-缓存
                            string orderIdQRCodeOut = qrcode;

                            // 【保存订单缓存】二维码下分 
                            if (!cacheQRCodeOutOrder.HasKey(orderIdQRCodeOut))
                            {
                                SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
                                nodeOrder.Add("type", "money_box_qrcode_out");
                                nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                                nodeOrder.Add("order_id", orderIdQRCodeOut);
                                nodeOrder.Add("as_coin_in_count", moneyOutInfo.asCoinInCount); //出多少票
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
                                DebugUtils.LogError($"【点单缓存】： 二维码下分 - 订单号重复： {orderIdQRCodeOut}");
                            }

                            //DebugUtil.Log($"二维码出票订单-本地缓存数据：{cacheQRCodeOutOrder.ToString()}");

                            OnSendResultQRCodeOut(orderIdQRCodeOut, (code,msg) =>
                            {

                                MaskPopupHandler.Instance.ClosePopup();

                                // 出票成功，已兑换 {0} 积分。  出票失败

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

                        DebugUtils.LogError($"二维码打印结果： {code} , {msg}");
                    });

            }, (error) =>
            {
                // 显示报错提醒
                TipPopupHandler.Instance.OpenPopupOnce(string.Format(I18nMgr.T("Request failed: [{0}]"),
                    Code.DEVICE_MB_GET_ORDER_ID_FAIL));

                //DebugUtil.LogError($"@@@@ 收到数据 ：报错！！！{error.msg} ");


            }, RPC_MARK_MACHINR_QRCODE_OUT);
        }
        else
        {
            //tip: 请选择更大的金额
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

        // 订单数据上报
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

            // 本地缓存
            JSONNode order = cacheQRCodeOutOrder[orderId];
            int creditOut = (int)order["as_credit"];

            // 通知算法卡
            MachineDataManager.Instance.RequestScoreDown(creditOut, (Action<object>)((res) =>
            {

                // 删除缓冲订单
                cacheQRCodeOutOrder.Remove(orderId);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_OUT_ORDER, cacheQRCodeOutOrder.ToString());


#if !SQLITE_ASYNC

                // 订单入库
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
                // 订单入库
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



                // 修改玩家金额
                _consoleBB.Instance.myCredit -= creditOut;
                MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

                // 每日统计
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

                    // 删除订单缓存
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
/// 本机机台上分
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

    /// <summary> 打印出票中 </summary>
    const string COR_IS_QRCODE_IN_ING = "COR_IS_QRCODE_IN_ING";


    /// <summary> 标记弹窗“确认二维码上分” </summary>
    const string MARK_POPUP_CONTINUE_QRCODE_IN = "MARK_POPUP_CONTINUE_QRCODE_IN";

    #region 本机台扫描二维码并上分


    public void DoQRCodeIn(string qrcodeScan)
    {
        if (CommonPopupHandler.Instance.isOpen(MARK_POPUP_CONTINUE_QRCODE_IN))
        {
            DebugUtils.Log("【money box ui】不接受此二维码，“二维码上分”确认弹窗已打开 ");
            return;
        }

        if (IsCor(COR_IS_QRCODE_IN_ING))
        {
            DebugUtils.Log("【money box ui】不接受此二维码，“二维码上分”已正在运行 ");
            return;
        }

        DoCor(COR_IS_QRCODE_IN_ING, DoTask(() => { }, 4001)); //延时避免重复触发

        DeviceOrderReship.Instance.DelayReshipOrderRepeat();


        if (!MoneyBoxManager.Instance.isLogin)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Failed to link to the money box server.")); // Failed to link to the cash box server
            return;
        }



        /*
        "money": 2921, // 金额
        "qrcode": "bank:1743597275871507300_743", // 二维码内容
        "money_per_coin_in": 0.5,
        "money_per_coin_out": 0.5
        */
        MoneyBoxManager.Instance.RequestCheckQRCodeWhenScanQRCode(qrcodeScan,
        (res) =>
        {
            DebugUtils.Log("【money box ui】扫描二维码，请求的到的数据： " + res.ToString());

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
                    DebugUtils.Log("【money box ui】点击取消，不兑换二维码");
                },

                buttonAutoClose2 = true,
                buttonText2 = I18nMgr.T("OK"),
                callback2 = () =>
                {
                    DebugUtils.Log("【money box ui】兑换二维码");

                    //生成订单-缓存
                    string orderIdQRCodeIn = qrcode;

                    // 【保存订单缓存】二维码下分 
                    if (cacheQRCodeInOrder.HasKey(orderIdQRCodeIn))
                    {
                        cacheQRCodeInOrder.Remove(orderIdQRCodeIn);
                    }

                    SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
                    nodeOrder.Add("type", "money_box_qrcode_in");
                    nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    nodeOrder.Add("order_id", orderIdQRCodeIn);
                    nodeOrder.Add("as_coin_in_count", moneyInfo.asCoinInCount); //出多少票
                    nodeOrder.Add("as_credit", moneyInfo.asCredit);
                    nodeOrder.Add("money", moneyInfo.money);
                    nodeOrder.Add("money_per_coin_in", moneyPerCoinIn);
                    nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
                    nodeOrder.Add("in_out", 1);
                    cacheQRCodeInOrder.Add(orderIdQRCodeIn, nodeOrder);
                    SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_IN_ORDER, cacheQRCodeInOrder.ToString());

                    DoCor(COR_IS_QRCODE_IN_ING, DoTask(() => { }, 8001)); //延时避免重复触发

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
                //检查订单内的金额 或id 和服务器的不符合？？？
                DebugUtils.LogError($"请求服务器-二维码兑换上分：下行数据和本地订单不符合！ 本地数据：{order.ToString()}");
                DebugUtils.LogError($"请求服务器-二维码兑换上分：下行数据和本地订单不符合！ 服务型下行数据：{data.ToString()}");
                onFinishCallback?.Invoke(1, I18nMgr.T("The QR code is invalid. Please check and try again."));
                return;
            }

            // 通知算法卡
            MachineDataManager.Instance.RequestScoreUp(asCreditFromOrder, (Action<object>)((res) =>
            {
                // 删除缓冲订单
                cacheQRCodeInOrder.Remove(orderIdQRCodeIn);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_IN_ORDER, cacheQRCodeInOrder.ToString());



#if !SQLITE_ASYNC
                // 订单入库
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
                // 订单入库
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




                // 修改玩家金额
                _consoleBB.Instance.myCredit += asCreditFromOrder;
                MainBlackboardController.Instance.AddOrSyncMyCreditToReal(asCreditFromOrder);


                // 每日统计
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
            DebugUtils.LogError($"请求服务器-二维码兑换上分：下行数据没有Data字段: {res.ToString()}");

        }
        else if (!res["Data"].HasKey("qrcode"))
        {
            DebugUtils.LogError($"请求服务器-二维码兑换上分：下行数据没有Data/qrcode字段: {res.ToString()}");
        }
        else
        {
            string qrcode = res["Data"]["qrcode"];

            if (cacheQRCodeInOrder.HasKey(qrcode))
            {
                // 删除缓冲订单
                cacheQRCodeInOrder.Remove(qrcode);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_QRCODE_IN_ORDER, cacheQRCodeInOrder.ToString());

                DebugUtils.Log("删除上分二维码本地缓存： {qrcode}");
            }
        }
    }
    #endregion



    #region 硬件二维码硬件扫描


    /// <summary> 机台或钱箱 打印二维码的前缀</summary>
    string patternBank = @"bank:(.*?)&QRCodeEnd&";
    /// <summary> 手机生成二维码（分变钱） </summary>
    //string patternQRCode = @"qr_code:(.*?)&QRCodeEnd&";
    private CheckInputStatus _CheckInputStatus = CheckInputStatus.None;
    private string inputValue = "";


    void Update()
    {
        OnUpdateScanQRCode();
    }

    /// <summary>
    /// 循环扫描二维码
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
    /// 当收到（机台）bank二维码消息的处理
    /// </summary>
    /// <param name="message">消息内容(已经去除了前缀和后缀)</param>
    public void CheckBankOrderInputHandle(string message)
    {
        //if (CheckCanShowUsePop())
        if (true)
        {
            try
            {
                string qrcode = $"bank:{MoneyBoxUtils.DecryptQRCode(message)}";
                DebugUtils.Log($"扫描二维码内容：{message}  解密： {qrcode}");
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
/// 钱箱指定本机台上分
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
             【money box】rpc down: CassetteRechange 
            data: {"device_id":"11009001","orderno":"PRC_1745237618351453900_9_Qp8Ws6TQlv","money":5,"action_type":"cashin_rechange"}
            */

            DeviceOrderReship.Instance.DelayReshipOrderRepeat();

            JSONNode data = res.value as JSONNode;
            string orderIdQRCodeIn = data["orderno"];
            int money = data["money"];

            // 订单去重(服务器会重复下发确认机台收到订单号)
            if (cacheMBReqQRCodeInOrder.HasKey(orderIdQRCodeIn))
            {
                MoneyBoxManager.Instance.RequestReturnQrderIdWhenMBReqMacQrIn(orderIdQRCodeIn, money);
                return;
            }

            if (IsCor(COR_IS_MB_RQYUEST_MACH_QRCODE_IN_ING))  //防止并发处理（只处理一个订单）
            {
                return;
            }
            DoCor(COR_IS_MB_RQYUEST_MACH_QRCODE_IN_ING, DoTask(() => { }, 4001)); //延时避免重复触发

            //返回id
            MoneyBoxManager.Instance.RequestReturnQrderIdWhenMBReqMacQrIn(orderIdQRCodeIn, money);

            float moneyPerCoinIn = MoneyBoxModel.Instance.moneyPerCoinIn;

            MoneyBoxMoneyInfo moneyInfo = DeviceUtils.GetMoneyInInfo(money, moneyPerCoinIn);

            //生成订单-缓存
            SimpleJSON.JSONNode nodeOrder = SimpleJSON.JSONNode.Parse("{}");
            nodeOrder.Add("type", "money_box_qrcode_in");
            nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            nodeOrder.Add("order_id", orderIdQRCodeIn);
            nodeOrder.Add("as_coin_in_count", moneyInfo.asCoinInCount); //出多少票
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

        // 通知服务器订单数据
        MoneyBoxManager.Instance.RequestSendOrderWhenMBReqMacQrIn(orderIdQRCodeIn, cacheOrder["money"], (Action<object>)((res) =>
        {

            // 通知算法卡
            MachineDataManager.Instance.RequestScoreUp(asCredit, (Action<object>)((res) =>
            {

                // 删除缓冲订单
                cacheMBReqQRCodeInOrder.Remove(orderIdQRCodeIn);
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_MB_REQ_QRCODE_IN_ORDER, cacheMBReqQRCodeInOrder.ToString());



#if !SQLITE_ASYNC

                // 订单入库
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
                // 订单入库
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



                // 修改玩家金额
                _consoleBB.Instance.myCredit += asCredit;
                //MainBlackboardController.Instance.SyncMyTempCreditToReal(true);
                MainBlackboardController.Instance.AddOrSyncMyCreditToReal(asCredit);

                // 每日统计
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
    /// 订单补发
    /// </summary>
    public IEnumerator ReshipOrde()
    {

        bool isNext = false;

        // 本机下分
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


        // 本机上分
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


        // 钱箱指定上分
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









