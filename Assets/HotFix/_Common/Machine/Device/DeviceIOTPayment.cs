#define SQLITE_ASYNC
using Game;
using GameMaker;
using IOT;
using SBoxApi;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class DeviceIOTPayment : MonoSingleton<DeviceIOTPayment>  //MonoBehaviour
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

    void Start()
    {
        // if (!ApplicationSettings.Instance.isMachine) return;

        // 注册获取二维码
        EventCenter.Instance.AddEventListener<List<QrCodeData>>(IOTEventHandle.REGISTER_DEV, OnEventRegisterQrCode);
        // 二维码码过期重新刷新
        //EventCenter.Instance.AddEventListener<>(EventHandle.REFRESH_QRCORD,);
        // 监听投币信息
        EventCenter.Instance.AddEventListener<CoinData>(IOTEventHandle.COINT_IN, OnEventQrCoinIn);
        //退票
        EventCenter.Instance.AddEventListener<TicketOutData>(IOTEventHandle.TICKET_OUT, OnEventIotTicketOut);


    }
    protected override void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<List<QrCodeData>>(IOTEventHandle.REGISTER_DEV, OnEventRegisterQrCode);
        EventCenter.Instance.RemoveEventListener<CoinData>(IOTEventHandle.COINT_IN, OnEventQrCoinIn);
        EventCenter.Instance.RemoveEventListener<TicketOutData>(IOTEventHandle.TICKET_OUT, OnEventIotTicketOut);

        base.OnDestroy();
    }



    /// <summary> 断网重连iot</summary>
    const string COR_RECONNECT_IOT = "COR_RECONNECT_IOT";

    /// <summary> 检查IOT是否已经链接并注册</summary>
    const string COR_CHECK_IOT_CONNECT = "COR_CHECK_IOT_CONNECT";


    void CheckIotSignIn()
    {
        _CheckIotSignIn();
        DoCor(COR_CHECK_IOT_CONNECT,DoTaskRepeat(_CheckIotSignIn,5000));
    }
    void _CheckIotSignIn()
    {
        if(_consoleBB.Instance.isConnectIot != isIOTSignInGetQRCode)
        {
            _consoleBB.Instance.isConnectIot = isIOTSignInGetQRCode;
        }

        if(_consoleBB.Instance.isUseIot == false)
        {
            ClearCor(COR_CHECK_IOT_CONNECT);
        }
    }

    /// <summary>
    /// 链接或关闭好酷
    /// </summary>
    public void CheckIOT()
    {
        //开启或关闭好酷
        if (_consoleBB.Instance.isUseIot)
            Init();
        else
            Close();

        //断网重连
        if (_consoleBB.Instance.isUseIot)
            DoCor(COR_RECONNECT_IOT, DoTaskRepeat(() =>
            {
                if (_consoleBB.Instance.isUseIot && !isIOTSignInGetQRCode)
                    Init();
            }, 10000));
        else
            ClearCor(COR_RECONNECT_IOT);
    }





    [Button]
    void Init()
    {
        StartCoroutine(_InitIOT());
    }
    void Close()
    {
        IoTPayment.Instance.Disconnect();

        ClearCor(COR_CHECK_IOT_CONNECT);
        _consoleBB.Instance.isConnectIot = false;
    }


    /// <summary> 好酷是否链接 </summary>
    public bool isIOTConneted => IoTPayment.Instance.IsConnected;


    /// <summary>
    ///  是否登录好酷注册二维码
    /// </summary>
    public bool isIOTSignInGetQRCode
    {
        get => isIOTConneted && IOTModel.Instance.LinkIOT;  // 注册二维码
    }

    /// <summary>
    /// 是否已经投币绑定微信号
    /// </summary>
    public bool isCoinInBindWeChatAccount
    {
        get => !string.IsNullOrEmpty(IOTModel.Instance.LinkId);  // 已经先投币
    }

    /// <summary>
    /// 已经登录好酷账号，且链接
    /// </summary>
    public bool isAllowIotCoinOut
    {
        get => isIOTSignInGetQRCode   // 注册二维码
            && isCoinInBindWeChatAccount;  // 已经先投币
    }





    IEnumerator _InitIOT()
    {
        _consoleBB.Instance.isConnectIot = false;

        //M2MqttUnityClient.Instance.is
        //原来连接上，先关掉
        while (isIOTConneted)
        {
            IoTPayment.Instance.Disconnect();
            yield return new WaitForSeconds(3f);

            DebugUtils.Log($"【IOT】等待好酷关闭");
        }

        DebugUtils.Log($"【IOT】好酷初始化 {IoTConst.GetDevParamURL}");

        if (PlayerPrefsUtils.isUseReleaseIot)
        {
            //正式账号 _consoleBB.Instance.pid,
            IoTPayment.Instance.Init(_consoleBB.Instance.machineID, _consoleBB.Instance.iotPort, _consoleBB.Instance.iotAccessMethods, 32, 1 , 
            (err) =>
            {
                if(PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1)
                    TipPopupHandler.Instance.OpenPopupOnce("【好酷】：" + err);
            });
        }
        else
        {

            IoTPayment.Instance.Init("10600001", 1, 1, 32, 1,
            (err) =>
            {
                TipPopupHandler.Instance.OpenPopupOnce("【好酷提醒】：" + err);
            });
        }

        CheckIotSignIn();

    }




    /// <summary> 注册，获取二维码 </summary> 
    void OnEventRegisterQrCode(List<QrCodeData> res)
    {
       
        if(res != null && res.Count > 0)
        {
            IOTModel.Instance.LinkIOT = true;
            IOTModel.Instance.qrCodeDatas = res;
            DebugUtils.Log("【IOT】好酷二维码注册成功");
        }
        else
        {
            DebugUtils.Log("【IOT】好酷二维码注册失败");
        }

    }

    const string DEVICE_IOT_COIN_OUT_ORDER = "device_iot_coin_out_order";
    const string DEVICE_IOT_COIN_IN_ORDER = "device_iot_coin_in_order";
    const string COR_CHECK_IOT_COIN_IN = "COR_CHECK_IOT_COIN_IN";


    JSONNode _cacheIOTCoinOutOrder;

    JSONNode cacheIOTCoinInOrder
    {
        get
        {
            if (_cacheIOTCoinInOrder == null)
                _cacheIOTCoinInOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_IOT_COIN_IN_ORDER, "{}"));
            return _cacheIOTCoinInOrder;
        }
        //set => _cacheCoinInOrder = value;

    }



    JSONNode _cacheIOTCoinInOrder;

    JSONNode cacheIOTCoinOutOrder
    {
        get
        {
            if (_cacheIOTCoinOutOrder == null)
                _cacheIOTCoinOutOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_IOT_COIN_OUT_ORDER, "{}"));
            return _cacheIOTCoinOutOrder;
        }
        //set => _cacheCoinInOrder = value;
    }



    /// <summary>
    /// 每次好酷冲钱时，获取linkId
    /// </summary>
    string curPlayerLinkId = "";

    #region 好酷投币
    /// <summary>
    /// 二维码充值
    /// </summary>
    /// <param name="data"></param>
    void OnEventQrCoinIn(CoinData data)
    {
        //收到投币信息，数据存入算法卡

        if (data.Num <= 0)
            return;

        string LinkId = data.orderNum;
        IOTModel.Instance.LinkId = LinkId;
        curPlayerLinkId = LinkId;

        string orderId = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.IOTCoinIn); // Guid.NewGuid().ToString();

        int credit = data.Num * _consoleBB.Instance.coinInScale;

        JSONNode nodeOrder = JSONNode.Parse("{}");
        nodeOrder.Add("device_number", 0);
        nodeOrder.Add("type", "iot_coin_in");
        nodeOrder.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        nodeOrder.Add("count", data.Num); //多少币
        nodeOrder.Add("order_id", orderId);
        nodeOrder.Add("link_id", curPlayerLinkId);
        nodeOrder.Add("scale", _consoleBB.Instance.coinInScale); //1币多少分
        nodeOrder.Add("credit", credit);
        nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
        nodeOrder.Add("credit_after", _consoleBB.Instance.myCredit + credit);
        nodeOrder.Add("code", Code.DEVICE_IOT_COIN_IN_SUCCESS);
        nodeOrder.Add("msg","");
        cacheIOTCoinInOrder[orderId] = nodeOrder;
        SQLitePlayerPrefs03.Instance.SetString(DEVICE_IOT_COIN_IN_ORDER, cacheIOTCoinInOrder.ToString());


        DebugUtils.Log($"【Iot Coin In】好酷数据：{JSONNodeUtil.ObjectToJsonStr(data)}");
        DebugUtils.Log($"【Iot Coin In】订单号：{nodeOrder.ToString() }");

        // 加钱超时：
        DoCor(COR_CHECK_IOT_COIN_IN, DoTask(() =>
        {
            IoTPayment.Instance.ReplyCoinIn(
                IOTModel.Instance.PortId,  //IOTModel.Instance.qrCodeDatas[0].portid,
                data.Num,
                data.orderNum,
                false,
                "请求超时");

            // 弹出提示
        },10000));

        MachineDataManager.Instance.RequestCoinIn (data.Num, (Action<object>)((res) =>
        {

            DebugUtils.Log($"好酷上分成功 : {credit}");

            ClearCor(COR_CHECK_IOT_COIN_IN);

            JSONNode nodeOrder = cacheIOTCoinInOrder[orderId];

            int iotCoinInCredit = (int)nodeOrder["credit"];
            /*
            (int)nodeOrder["credit"];
            long myCredit = BlackboardUtils.GetValue<long>("@console/myCredit");//_consoleBB.Instance.myCredit;
            cacheIOTCoinInOrder[orderIdCoinIn]["credit_before"] = myCredit;
            cacheIOTCoinInOrder[orderIdCoinIn]["credit_after"] = myCredit + coinInCredit;
            */


            // 清掉缓存订单
            cacheIOTCoinInOrder.Remove(orderId);
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_IOT_COIN_IN_ORDER, cacheIOTCoinInOrder.ToString());




#if !SQLITE_ASYNC

            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = nodeOrder["type"],
                    device_number = nodeOrder["device_number"],
                    order_id = nodeOrder["order_id"],
                    count = nodeOrder["count"],
                    credit = nodeOrder["credit"],
                    credit_after = _consoleBB.Instance.myCredit + iotCoinInCredit,
                    credit_before = _consoleBB.Instance.myCredit,
                    in_out = 1,
                    created_at = nodeOrder["timestamp"],
                });
            DebugUtil.Log($"【SQL - Iot Coin In】 : {sql}");
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
                    credit = nodeOrder["credit"],
                    credit_after = _consoleBB.Instance.myCredit + iotCoinInCredit,
                    credit_before = _consoleBB.Instance.myCredit,
                    in_out = 1,
                    created_at = nodeOrder["timestamp"],
                });
            DebugUtils.Log($"【SQL - Iot Coin In】 : {sql}");
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


            //加钱动画
            _consoleBB.Instance.myCredit += iotCoinInCredit;
            MainBlackboardController.Instance.AddOrSyncMyCreditToReal(iotCoinInCredit);


            //每日统计
            TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinIn((long)iotCoinInCredit, _consoleBB.Instance.myCredit);


            //刷新缓存
            if (true)
            {
                IoTPayment.Instance.ReplyCoinIn(
                    IOTModel.Instance.PortId,
                    data.Num,
                    data.orderNum,
                    true,
                    "");
            }
            else
            {
                // 可能失败？？
                /*IoTPayment.Instance.ReplyCoinIn(
                    IOTModel.Instance.PortId,
                    //IOTModel.Instance.qrCodeDatas[0].portid,
                    data.Num,
                    data.orderNum,
                    false,
                    "报错信息");*/
            }

            EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT,
                new EventData<int>(GlobalEvent.IOTCoinInCompleted, iotCoinInCredit));
        }));


    }


    /// <summary>
    /// 好酷充值
    /// </summary>
    public void DoQrCoinIn()
    {
        if (!isIOTSignInGetQRCode)
        {
            TipPopupHandler.Instance.OpenPopupOnce(string.Format(I18nMgr.T("IOT connection failed [{0}]"), Code.DEVICE_IOT_NOT_SIGN_IN));
            //TipPopupHandler.Instance.OpenPopup("\"二维码投退币功能\"未开启");
            return;
        }

        PageManager.Instance.OpenPage(PageName.P015PopupQrCoinIn,
            new EventData<Dictionary<string, object>>(
                "",
                new Dictionary<string, object>() {
                    ["qrcodeUrl"] = IOTModel.Instance.qrCodeDatas[0].qrcodeUrl,
                }
            ));
    }
    #endregion

















    #region 好酷退票
    const string COR_IS_IOT_COIN_OUT_ING = "COR_IS_IOT_COIN_OUT_ING";
    const string COR_IOT_COIN_OUT_TIMEOUT = "COR_IOT_COIN_OUT_TIMEOUT";
    public bool isRegularIOTCoinOuting
    {
        get => IsCor(COR_IS_IOT_COIN_OUT_ING) || IsCor(COR_IOT_COIN_OUT_TIMEOUT);
    }


    string COR_DELAY_IOT_TICKER_OUT = "COR_DELAY_IOT_TICKER_OUT";
    string orderIdIOTCoinOut;
    public void DoIotTickerOut()
    {
        DoCor(COR_DELAY_IOT_TICKER_OUT, DoTask(_DoIotTickerOut, 500)); // 延时退票，避免退票和游戏开完同时开始
    }

    public void _DoIotTickerOut()
    {
        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=24>Cannot coin out during the game period</size>"));
            return;
        }

        Debug.Log($"是否连接好酷: {isIOTConneted}， 是否登录注册: {isIOTSignInGetQRCode} 是否绑定微信号: {isCoinInBindWeChatAccount}  isAllowIotCoinOut:{isAllowIotCoinOut}");
        if (!isAllowIotCoinOut)
        {
            //TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("IOT connection failed"));
            return;
        }



        int coinOutNum = DeviceUtils.GetCoinOutNum();
        if (coinOutNum  <= 0)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Too few game credits, ticket refund failed."));
            return;
        }


        if (IsCor(COR_IS_IOT_COIN_OUT_ING))
            return;

        DoCor(COR_IS_IOT_COIN_OUT_ING, DoTask(() => { }, 4001)); //延时避免重复触发



        long credit = DeviceUtils.GetCoinOutCredit(coinOutNum);

        // 打开遮罩
        MaskPopupHandler.Instance.OpenPopup();

        //好酷退票超时。
        DoCor(COR_IOT_COIN_OUT_TIMEOUT, DoTask(() => {
            MaskPopupHandler.Instance.ClosePopup();
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("IOT refund timeout"));
            CheckIOT(); //重连好酷
        }, 40001));

        long timeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        orderIdIOTCoinOut = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.IOTCoinOut);  // Guid.NewGuid().ToString();

        JSONNode nodeOrder = JSONNode.Parse("{}");
        nodeOrder.Add("device_number", 0);
        nodeOrder.Add("type", "iot_coin_out");
        nodeOrder.Add("timestamp", timeMS);
        nodeOrder.Add("count", coinOutNum); //多少票
        nodeOrder.Add("order_id", orderIdIOTCoinOut);
        nodeOrder.Add("scale_credit_per_ticket", _consoleBB.Instance.coinOutScaleCreditPerTicket); //1币多少分
        nodeOrder.Add("scale_ticket_per_credit", _consoleBB.Instance.coinOutScaleTicketPerCredit); //1币多少分
        nodeOrder.Add("credit", credit);
        nodeOrder.Add("credit_before", _consoleBB.Instance.myCredit);
        nodeOrder.Add("credit_after", _consoleBB.Instance.myCredit - credit);
        //nodeOrder.Add("link_id", IOTModel.Instance.LinkId);
        nodeOrder.Add("link_id", curPlayerLinkId);
        nodeOrder.Add("code", Code.DEVICE_CREAT_ORDER_NUMBER);
        nodeOrder.Add("msg", "");
        cacheIOTCoinOutOrder[orderIdIOTCoinOut] = nodeOrder;
        SQLitePlayerPrefs03.Instance.SetString(DEVICE_IOT_COIN_OUT_ORDER, cacheIOTCoinOutOrder.ToString());

        DebugUtils.Log($"【Iot Coin Out】创建本地订单：{nodeOrder.ToString()}");



        TicketOutData data = new TicketOutData();
        data.type = (int)IOTModel.Instance.ticketOutType;
        data.num = coinOutNum;
        data.seq = $"{timeMS / 1000}"; //(Utils.GetTimeStamp()/1000).ToString();
        data.orderNum = IOTModel.Instance.LinkId;

        DebugUtils.Log($"【Iot Coin Out】给好酷发数据：{JSONNodeUtil.ObjectToJsonStr(data)}");

        IOTModel.Instance.unfinishTicketOutDatas.Add(data);
        IoTPayment.Instance.DeviceTicketOut(
            IOTModel.Instance.PortId, 
            data,
            (err) => {

                CommonPopupHandler.Instance.OpenPopupSingle(
                new CommonPopupInfo()
                {
                    isUseXButton = false,
                    buttonAutoClose1 = true,
                    type = CommonPopupType.OK,
                    text = string.Format(
                                    I18nMgr.T("Request failed: [{0}]"),
                                    Code.DEVICE_IOT_COIN_OUT_API_ERR),
                    buttonText1 = I18nMgr.T("OK"),
                });

                cacheIOTCoinOutOrder.Remove(orderIdIOTCoinOut);
                OnOverIOTCoinOut();
            });
    }

    void OnOverIOTCoinOut()
    {
        MaskPopupHandler.Instance.ClosePopup();
        ClearCor(COR_IS_IOT_COIN_OUT_ING);
        ClearCor(COR_IOT_COIN_OUT_TIMEOUT);
        orderIdIOTCoinOut = "";
    }
    void OnEventIotTicketOut(TicketOutData data)
    {
        if (data.num <= 0)
            return;

        string linkId = curPlayerLinkId;

        //退票成功 。这里要订单补发（这里要订单确认）
        cacheIOTCoinOutOrder[orderIdIOTCoinOut]["code"] = Code.DEVICE_IOT_COIN_OUT_SUCCESS;
        SQLitePlayerPrefs03.Instance.SetString(DEVICE_IOT_COIN_OUT_ORDER, cacheIOTCoinOutOrder.ToString());


        //DebugUtil.Log($"【Iot Ticket out】 : 好酷数据 = {JSONNodeUtil.ObjectToJsonStr(data)}");
        //DebugUtil.Log($"【Iot Ticket out】 : 缓存 = {JSONNodeUtil.ObjectToJsonStr(IOTModel.Instance.unfinishTicketOutDatas)}");
        //DebugUtil.Log($"【Iot Ticket out】 : 本地订单号 = {cacheIOTCoinOutOrder[orderIdIOTCoinOut].ToString()}");
        //DebugUtil.Log($"【Iot Ticket out】 : @@ data.seq = {data.seq} ; IOTModel.Instance.LinkId= {linkId}");
        //删除缓存订单
        TicketOutData lastData = null;
        for (int i = 0; i < IOTModel.Instance.unfinishTicketOutDatas.Count; i++)
        {
            var item = IOTModel.Instance.unfinishTicketOutDatas[i];
            //DebugUtil.Log($"【Iot Ticket out】 : item.seq = {item.seq} ; item.orderNum = {item.orderNum}");
            if (item.seq == data.seq && item.orderNum == linkId)
            {
                lastData = item;
                IOTModel.Instance.unfinishTicketOutDatas.Remove(item);
                break;
            }
        }

        IOTModel.Instance.LinkId = null;
        curPlayerLinkId = null;

        if (lastData != null)
        {

                int creditOut = cacheIOTCoinOutOrder[orderIdIOTCoinOut]["credit"];
                int coinOutNum = cacheIOTCoinOutOrder[orderIdIOTCoinOut]["count"];

                MachineDataManager.Instance.RequestCoinOut(coinOutNum, (Action<object>)((res) =>
                {

                    DebugUtils.Log($"好酷退票成功 : {creditOut}");
                    JSONNode nodeOrder = cacheIOTCoinOutOrder[orderIdIOTCoinOut];

                    // 清掉缓存订单
                    cacheIOTCoinOutOrder.Remove(orderIdIOTCoinOut);
                    SQLitePlayerPrefs03.Instance.SetString(DEVICE_IOT_COIN_OUT_ORDER, cacheIOTCoinOutOrder.ToString());

                    if (_consoleBB.Instance.myCredit - creditOut < 0)
                    {
                        Debug.LogError($"好酷退的积分: {creditOut}  超过自身积分： {_consoleBB.Instance.myCredit}");
                        creditOut = (int)_consoleBB.Instance.myCredit;
                    }


#if !SQLITE_ASYNC
               string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                    ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                    new TableCoinInOutRecordItem()
                    {
                        device_type = nodeOrder["type"],
                        device_number = nodeOrder["device_number"],
                        order_id = nodeOrder["order_id"],
                        count = nodeOrder["count"],
                        credit = creditOut,
                        credit_after = _consoleBB.Instance.myCredit - creditOut, //nodeOrder["credit_after"],
                        credit_before = _consoleBB.Instance.myCredit,//nodeOrder["credit_before"],
                        in_out = 0,
                        created_at = nodeOrder["timestamp"],
                    });

                DebugUtil.Log($"【SQL - Iot Coin Out】 : {sql}");
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
                            credit = creditOut,
                            credit_after = _consoleBB.Instance.myCredit - creditOut, //nodeOrder["credit_after"],
                            credit_before = _consoleBB.Instance.myCredit,//nodeOrder["credit_before"],
                            in_out = 0,
                            created_at = nodeOrder["timestamp"],
                        });

                    DebugUtils.Log($"【SQL - Iot Coin Out】 : {sql}");
                    SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


                    //加钱动画
                    _consoleBB.Instance.myCredit -= creditOut;
                    MainBlackboardController.Instance.MinusOrSyncMyCreditToReal(creditOut);


                    //每日统计
                    TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinOut((long)creditOut, _consoleBB.Instance.myCredit);

                    OnOverIOTCoinOut();


                    EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT,
                        new EventData<int>(GlobalEvent.IOTCoinOutCompleted, creditOut));
                }));

        }
        else
        {

            CommonPopupHandler.Instance.OpenPopupSingle(
            new CommonPopupInfo()
            {
                isUseXButton = false,
                buttonAutoClose1 = true,
                type = CommonPopupType.OK,
                text = string.Format(
                                I18nMgr.T("Request failed: [{0}]"),
                                Code.DEVICE_IOT_COIN_OUT_CACHE_ORDER_IS_NOT_FIND),
                buttonText1 = I18nMgr.T("OK"),
            });


            DebugUtils.LogWarning("iot cache order is not find");

            cacheIOTCoinOutOrder[orderIdIOTCoinOut]["code"] = Code.DEVICE_IOT_COIN_OUT_CACHE_NOT_FIND;
            cacheIOTCoinOutOrder[orderIdIOTCoinOut]["msg"] = "找不到对应的缓存的订单";
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_IOT_COIN_OUT_ORDER, cacheIOTCoinOutOrder.ToString());
            OnOverIOTCoinOut();
        }

    }
    #endregion 
}
