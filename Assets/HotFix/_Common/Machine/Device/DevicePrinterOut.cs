#define SQLITE_ASYNC
using System.Collections.Generic;
using UnityEngine;
using System;
using SBoxApi;
using SimpleJSON;
using GameMaker;
using Sirenix.OdinInspector;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using Game;


/// <summary>
/// 打印机
/// </summary>
/// <remarks>
/// * 打印机使用退票比例。
/// * 打印机打印多少数值，等于退多少张票。
/// * 打印的数值必须是整数，整数张票。
/// </remarks>
public partial class DevicePrinterOut : CorBehaviour
{
    const string COR_INIT_PRINTER = "COR_INIT_PRINTER";
    const string COR_IS_PRINTER_OUT_ING = "COR_IS_PRINTER_OUT_ING";
    const string COR_CHECK_PRINTER_PAGE = "COR_CHECK_PRINTER_PAGE";
    const string DEVICE_PRINTER_OUT_ORDER = "device_printer_out_order";
    const string COR_CHECK_PRINTER_CONNECT = "COR_CHECK_PRINTER_CONNECT";

    const string MARK_POP_PRINTER_NOT_LINK = "MARK_POP_PRINTER_NOT_LINK";
    //int deviceNumber = 0;




    JSONNode _cachePrinterOutOrder = null;
    JSONNode cachePrinterOutOrder
    {
        get
        {
            if (_cachePrinterOutOrder == null)
                _cachePrinterOutOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_PRINTER_OUT_ORDER, "{}"));
            return _cachePrinterOutOrder;
        }
        //set => _cachePrinterOutOrder = value;
    }

    private void OnEnable()
    {
       // if (!ApplicationSettings.Instance.isMachine) return;


        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_FONTSIZE, OnPrinterFontsize);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_MESSAGE, OnPrinterMessage);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_DATESET, OnPrinterDateSet);
        EventCenter.Instance.AddEventListener<SBoxDate>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_DATEGET, OnPrinterDateGet);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_PAPERCUT, OnPrinterCutPaper);

        RepeatInitPrinter(null,null);
    }
    private void OnDisable()
    {
        MachineDataManager.Instance?.RemoveRequestAt(RPC_MARK_DEVICE_PRINTER_OUT);


        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_FONTSIZE, OnPrinterFontsize);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_MESSAGE, OnPrinterMessage);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_DATESET, OnPrinterDateSet);
        EventCenter.Instance.RemoveEventListener<SBoxDate>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_DATEGET, OnPrinterDateGet);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_PAPERCUT, OnPrinterCutPaper);
    }

    const string RPC_MARK_DEVICE_PRINTER_OUT = "RPC_MARK_DEVICE_PRINTER_OUT";




    /// <summary>
    /// 重复复位打印机，直到复位成功
    /// </summary>
    void RepeatInitPrinter(Action successCallback, Action<string> errorCallback)
    {
        DoCor(COR_INIT_PRINTER, DoTaskRepeat(()=>InitPrinter(null,null), 8000));
    }

    void FirstOrRepeatInitPrinter(Action successCallback, Action<string> errorCallback)
    {
        InitPrinter(); //上电里面初始化（失败时，重复请求）
        DoCor(COR_INIT_PRINTER, DoTaskRepeat(() => InitPrinter(null, null), 8000));
    }

    public void InitPrinter(Action successCallback = null, Action<string> errorCallback = null)
    {

        _consoleBB.Instance.isConnectPrinter = false;
        _consoleBB.Instance.isInitPrinter = false;
        ClearCor(COR_CHECK_PRINTER_CONNECT);

        MachineDataManager.Instance.RequestGetPrinterList(   // 获取列表
            (res) => {
                List<string> printerList = (List<string>)res;

                _consoleBB.Instance.sboxPrinterList = printerList;

                if (_consoleBB.Instance.selectPrinterNumber > printerList.Count - 1)
                    _consoleBB.Instance.selectPrinterNumber = 0;


                if (!_consoleBB.Instance.isUsePrinter)
                {
                    ClearCor(COR_INIT_PRINTER);
                    return;
                }


                MachineDataManager.Instance.RequestSelectPrinter(_consoleBB.Instance.selectPrinterNumber, (res) => {

                    bool isOk = (int)res == 0;
                    if (isOk)
                    {

                        MachineDataManager.Instance.RequestResetPrinter((res) => {
                            if ((int)res == 0)
                            {
                                DebugUtils.LogWarning($"【printer】: 打印机初始化成功，选择 idx: {_consoleBB.Instance.selectPrinterNumber} -- {_consoleBB.Instance.sboxPrinterList[_consoleBB.Instance.selectPrinterNumber]}");
                                ClearCor(COR_INIT_PRINTER);
                                _consoleBB.Instance.isInitPrinter = true;

                                CheckPrinterConnect();

                                successCallback?.Invoke();
                            }
                            else
                            {
                                DebugUtils.LogWarning($"【printer】: 打印机初始化失败,选择 idx: {_consoleBB.Instance.selectPrinterNumber} -- {_consoleBB.Instance.sboxPrinterList[_consoleBB.Instance.selectPrinterNumber]}");
                                errorCallback?.Invoke("打印机初始化失败");
                            }
                        }, (err) => {

                            DebugUtils.LogError(err.msg);
                            errorCallback?.Invoke(err.msg);

                        }, RPC_MARK_DEVICE_PRINTER_OUT);
                    }
                    else
                    {
                        DebugUtils.LogError("打印机选择失败");
                        errorCallback?.Invoke("打印机选择失败");
                    }
               
                }, (err) => {
                    DebugUtils.LogError(err.msg);
                    errorCallback?.Invoke(err.msg);
                },RPC_MARK_DEVICE_PRINTER_OUT);
            },
            (err) =>
            {
                DebugUtils.LogError(err.msg);
                errorCallback?.Invoke(err.msg);
            },RPC_MARK_DEVICE_PRINTER_OUT
        );
    }

    void CheckPrinterConnect()
    {
        _CheckPrinterConnect();
        DoCor(COR_CHECK_PRINTER_CONNECT, DoTaskRepeat(() => _CheckPrinterConnect(), 8000));
    }
    void _CheckPrinterConnect()
    {
        MachineDataManager.Instance.RequestIsPrinterConnect((res) =>
        {
            int data = (int)res;
            _consoleBB.Instance.isConnectPrinter = data == _consoleBB.Instance.selectPrinterNumber;
            //Debug.LogError($"算法卡 打印机编号{data} ， 已选打印机编号： {_consoleBB.Instance.selectPrinterNumber}");

            if (!_consoleBB.Instance.isConnectPrinter)
            {
                if (PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1
                    && PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != -1 
                    && _consoleBB.Instance.isMachineActive == true)
                {
                    //TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Printer not linked."));
                    CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                    {
                        text = I18nMgr.T("Printer not linked."),
                        type = CommonPopupType.OK,
                        buttonText1 = I18nMgr.T("OK"),
                        buttonAutoClose1 = true,
                        isUseXButton = false,
                        mark = MARK_POP_PRINTER_NOT_LINK,
                    });
                    //Debug.LogError("打印机没有连接");
                }
            }
            else
            {
                //Debug.LogWarning("打印机已连接");
                CommonPopupHandler.Instance.ClosePopup(MARK_POP_PRINTER_NOT_LINK);
            }

        });
    }



    /// <summary>
    /// 设置字体大小回调
    /// </summary>
    /// <param name="result"></param>
    void OnPrinterFontsize(int result)
    {
        if (result == 0)
        {
            if (printerTaskFunc != null)
                printerTaskFunc();
        }
        else
        {
            DebugUtils.LogWarning("【printer】: 打印机字体设置失败");
            printerTaskFunc = null;
        }
    }


    /// <summary>
    /// 打印内容回调
    /// </summary>
    /// <param name="result"></param>
    void OnPrinterMessage(int result)
    {
        if (result == 0)
        {
            if (printerTaskFunc != null && !string.IsNullOrEmpty(orderIdPrinterOut))
            {
                DebugUtils.LogWarning(" 延时检查打印机是否缺纸？");
                //延时检查是否缺纸
                //检查是否缺纸： 5秒内检查，但是不能立马检查
                DoCor(COR_CHECK_PRINTER_PAGE, DoTask(() =>
                {
                    int printerState = SBoxSandbox.PrinterState();
                    DebugUtils.LogWarning($"PrinterState = {printerState}");
                    bool isPriniterConnect = printerState >= 0;
                    if (!isPriniterConnect)
                    {
                        //打印机异常
                        cachePrinterOutOrder[orderIdPrinterOut]["code"] = Code.DEVICE_PRINTER_OUT_OF_PAGE;
                        cachePrinterOutOrder[orderIdPrinterOut]["msg"] = "printer out of paper";
                        SQLitePlayerPrefs03.Instance.SetString(DEVICE_PRINTER_OUT_ORDER, cachePrinterOutOrder.ToString());

                        DebugUtils.LogError(" 打印机缺纸！");
                        CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                        {
                            text = I18nMgr.T("<size=24>Printer out of paper</size>"),
                            type = CommonPopupType.OK,
                            buttonText1 = I18nMgr.T("OK"),
                            buttonAutoClose1 = true,
                            callback1 = delegate {
                            },
                            isUseXButton = false,
                        });
                        return;
                    }
                    else
                    {
                        DebugUtils.LogWarning(" 打印机有纸");
                        printerTaskFunc();
                    }
                }, 3000));
            }
            else
            {
                DebugUtils.LogWarning("【printer】打印机被误触发了");
            }
        }
        else
        {
            DebugUtils.LogWarning("【printer】: 打印机打印失败");
            printerTaskFunc = null;
        }
    }

    void OnPrinterDateSet(int result)
    {
        if (result == 0)
        {
            DebugUtils.Log("【printer】: set Date succeed");
        }
    }
    void OnPrinterDateGet(SBoxDate sBoxDate)
    {
        if (sBoxDate.result == 0)
        {
        }
    }
    void OnPrinterCutPaper(int result)
    {
        if (result == 0)
        {
            DebugUtils.Log("【printer】: cut paper succeed");
        }
    }


    Action printerTaskFunc = null;
    string orderIdPrinterOut = "";

    [Button]
    public void DoPrinterOut()
    {
        if (!_consoleBB.Instance.isMachineActive)
        {
            DebugUtils.LogWarning("Machine not activated");
            return;
        }


        if (_consoleBB.Instance.isUsePrinter == false)
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Printer function not enabled."));
            return;
        }

        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            /*ErrorPopupHandler.Instance.OpenPopupSingle(new ErrorPopupInfo()
            {
                text = I18nMgr.T("<size=24>Cannot print out during the game period</size>"),
                type = ErrorPopupType.OK,
                buttonText1 = I18nMgr.T("OK"),
                buttonAutoClose1 = true,
                callback1 = delegate {
                },
                isUseXButton = false,
            });*/

            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=24>Cannot print out during the game period</size>"));
            return;
        }


        if (IsCor(COR_IS_PRINTER_OUT_ING))
            return;

        DoCor(COR_IS_PRINTER_OUT_ING, DoTask(() => { }, 10000)); //延时避免重复触发

        printerTaskFunc = CreatPrinterTask();
        printerTaskFunc();
    }


    Action CreatPrinterTask()
    {
        int _next = -1;
        Action FUNC = null;

        FUNC = () => {

            if (!_consoleBB.Instance.isInitPrinter)
            {
                DebugUtils.LogError(" 打印机初始化失败");

                CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                {
                    text = I18nMgr.T("<size=24>Printer is not init</size>"),
                    type = CommonPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = delegate {
                    },
                    isUseXButton = false,
                });
                return;
            }

            bool isPriniterConnect = SBoxSandbox.PrinterState() >= 0;
            if (!isPriniterConnect)
            {
                DebugUtils.LogError(" 打印机不在线");

                CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                {
                    text = I18nMgr.T("<size=24>Printer not connected</size>"),
                    type = CommonPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = delegate {
                    },
                    isUseXButton = false,
                });
                return;
            }

            _next++;

            switch (_next)
            {
                case 0: //判断是否可以出钞
                    {
                        if (false)
                        {

                        }
                        else  // 可以出钞票
                        {
                            FUNC.Invoke();
                        }
                    }
                    break;
                case 1: //设置字体大小
                    {
                        int fontSize = 3;
                        SBoxSandbox.PrinterFontSize(fontSize);
                    }
                    break;
                case 2: //获取console数据并出票
                    {
                        // 获取console数据
                        //long mycredit = 1001;
                        //double money = mycredit / 100f;

                        //double money = ConsoleBlackboard02.Instance.myCredit / (float)ConsoleBlackboard02.Instance.printOutScale;




                        /*
                        long credit = 0;
                        long count = 0; // 这个只能是整数
                        if (_consoleBB.Instance.coinOutScalePerCredit2Ticket > 1)
                        {
                            count = _consoleBB.Instance.myCredit * _consoleBB.Instance.coinOutScalePerCredit2Ticket;
                            credit = _consoleBB.Instance.myCredit;
                        }
                        else if(_consoleBB.Instance.coinOutScalePerTicket2Credit > 1)
                        {
                            count = _consoleBB.Instance.myCredit / _consoleBB.Instance.coinOutScalePerTicket2Credit;
                            credit = count * _consoleBB.Instance.coinOutScalePerTicket2Credit;
                        }else if (_consoleBB.Instance.coinOutScalePerTicket2Credit ==1 && _consoleBB.Instance.coinOutScalePerTicket2Credit ==1)
                        {
                            count = _consoleBB.Instance.myCredit;
                            credit = _consoleBB.Instance.myCredit;
                        }*/

                        int count = DeviceUtils.GetCoinOutNum();
                        long credit = DeviceUtils.GetCoinOutCredit(count);


                        orderIdPrinterOut = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinOut);
                        //orderIdPrinterOut =  Guid.NewGuid().ToString();

                        string agentName = "1";
                        string businessName = "2";

                        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        // 使用DateTimeOffset将时间戳转换为DateTimeOffset对象
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                        // 如果你只需要DateTime，可以将其转换为UTC时间或本地时间
                        //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                        //string time = utcDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        DateTime localDateTime = dateTimeOffset.LocalDateTime;
                        string time = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        // 打印人类可读的格式
                        //DebugUtil.Log("UTC日期和时间: " + utcDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        //DebugUtil.Log("本地日期和时间: " + localDateTime.ToString("yyyy-MM-dd HH:mm:ss"));



                        //long crefitPrint = _consoleBB.Instance.myCredit;
                        //保存订单到缓存
                        JSONNode nodeOrder = JSONNode.Parse("{}");
                        nodeOrder["type"] = "printer_out";
                        nodeOrder["order_id"] = orderIdPrinterOut;
                        nodeOrder["money"] = count;
                        nodeOrder["agent_name"] = agentName;
                        nodeOrder["business_name"] = businessName;
                        nodeOrder["timestamp"] = timestamp;
                        nodeOrder["credit_before"] = _consoleBB.Instance.myCredit;
                        nodeOrder["credit_after"] = _consoleBB.Instance.myCredit - credit;
                        nodeOrder["credit"] = credit;
                        //nodeOrder["count"] = 1;
                        nodeOrder["count"] = count;  
                        nodeOrder["device_number"] = _consoleBB.Instance.selectPrinterNumber;
                        nodeOrder["code"] = Code.DEVICE_CREAT_ORDER_NUMBER;
                        nodeOrder["msg"] = "";

                        cachePrinterOutOrder[orderIdPrinterOut] = nodeOrder;
                        SQLitePlayerPrefs03.Instance.SetString(DEVICE_PRINTER_OUT_ORDER, cachePrinterOutOrder.ToString());


                        //订单内容
                        string printTitle = $"\t\t{ApplicationSettings.Instance.gameTheme}\r\n";
                        string testMsg = printTitle +   // 平台号
                            //$"Credit: {credit}\r\n" +  // 金钱
                            $"Tickets: {count}\r\n" +
                            $"Order number: \r\n" +
                            $"{orderIdPrinterOut}\r\n" +  //订单号
                            $"Distributor: {agentName}\r\n" +  //代理商
                            $"Business: {businessName}\r\n " + // 厂家
                            $"time: {time}\r\n " +  //时间
                            $"";


                        //检查到打印失败？？
                        /*DoCor(COR_REPEAT_CHECK_PRINTER_PAGE, DoTaskRepeat(() =>
                        {
                            DebugUtil.Log($"轮训检查  PrinterState = {SBoxSandbox.PrinterState()}");
                        }, 300));*/

                        // 开始打印
                        SBoxSandbox.PrinterMessage(testMsg);

                        DebugUtils.Log($"@【printer】打印内容 testMsg = {testMsg}");
                    }
                    break;
                case 3: // 打印成功后，记录订单，并删除订单缓冲
                    {
                        DebugUtils.Log($"@【printer】将打印数据给算法卡");
                        //删除订单到缓存
                        JSONNode nodeOrder = cachePrinterOutOrder[orderIdPrinterOut];

                        long creditCoinOut = (long)nodeOrder["credit"];            
                        int coinOutNum = (int)nodeOrder["count"];

                        MachineDataManager.Instance.RequestCoinOut(coinOutNum, (Action<object>)((res) =>
                        {


                            //删除订单到缓存
                            cachePrinterOutOrder.Remove(orderIdPrinterOut);
                            SQLitePlayerPrefs03.Instance.SetString(DEVICE_PRINTER_OUT_ORDER, cachePrinterOutOrder.ToString());

                            //记录订单


#if !SQLITE_ASYNC
                            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                            ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                            new TableCoinInOutRecordItem()
                            {
                                device_type = nodeOrder["type"],
                                device_number = nodeOrder["device_number"],
                                order_id = nodeOrder["order_id"],
                                count = nodeOrder["count"],
                                as_money = nodeOrder["money"],
                                credit = (long)nodeOrder["credit"],
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
                                 as_money = nodeOrder["money"],
                                 credit = (long)nodeOrder["credit"],
                                 credit_after = nodeOrder["credit_after"],
                                 credit_before = nodeOrder["credit_before"],
                                 in_out = 0,
                                 created_at = nodeOrder["timestamp"],
                             });
                            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif

                            //每日统计
                            //TableBusniessDayRecordManager.Instance.AddTotalPrinterOut(nodeOrder["credit"], nodeOrder["money"], nodeOrder["credit_after"]);
                            TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinOut(nodeOrder["credit"], nodeOrder["credit_after"]);


                            //同步玩家金币
                            _consoleBB.Instance.myCredit -= (long)nodeOrder["credit"];
                            MainBlackboardController.Instance.AutoSyncMyCreditToReel();

                            DebugUtils.Log($"玩家真实金额： {_consoleBB.Instance.myCredit}");
                            orderIdPrinterOut = null;
                            printerTaskFunc = null;

                        }));

                        /*
                        //删除订单到缓存
                        JSONNode nodeOrder = null;
                        if (cachePrinterOutOrder.HasKey(orderIdPrinterOut))
                        {
                            nodeOrder = cachePrinterOutOrder[orderIdPrinterOut];
                            cachePrinterOutOrder.Remove(orderIdPrinterOut);
                            SQLitePlayerPrefs02.Instance.SetString(DEVICE_PRINTER_OUT_ORDER, cachePrinterOutOrder.ToString());
                        }*/

                    }
                    break;
                default:
                    orderIdPrinterOut = null;
                    printerTaskFunc = null;
                    break;
            }

        };

        return FUNC;
    }













    public void TestPrinterTicket()
    {
        if (IsCor(COR_IS_PRINTER_OUT_ING))
            return;

        DoCor(COR_IS_PRINTER_OUT_ING, DoTask(() => { }, 10000)); //延时避免重复触发

        printerTaskFunc = TestCreatPrinterTask();
        printerTaskFunc();
    }


    Action TestCreatPrinterTask()
    {
        int _next = -1;
        Action FUNC = null;

        FUNC = () => {


            bool isPriniterConnect = SBoxSandbox.PrinterState() >= 0;
            if (!isPriniterConnect)
            {
                DebugUtils.LogError(" 打印机不在线");

                CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                {
                    text = I18nMgr.T("<size=24>Printer not connected</size>"),
                    type = CommonPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = delegate {
                    },
                    isUseXButton = false,
                });
                return;
            }

            _next++;

            switch (_next)
            {

                case 0: //设置字体大小
                    {
                        int fontSize = 3;
                        SBoxSandbox.PrinterFontSize(fontSize);
                    }
                    break;
                case 1: //获取console数据并出票
                    {

                        // 使用DateTimeOffset将时间戳转换为DateTimeOffset对象
                        //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                        //string time = utcDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        DateTime localDateTime = DateTimeOffset.UtcNow.LocalDateTime;
                        string time = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        string testMsg =   // 平台号
                            $"测试打印机\r\n" +  // 金钱
                            $"time: {time}\r\n " +  //时间
                            $"";

                        // 开始打印
                        SBoxSandbox.PrinterMessage(testMsg);

                        DebugUtils.Log($"@【printer】打印内容 testMsg = {testMsg}");
                    }
                    break;
                case 2: // 打印成功后，记录订单，并删除订单缓冲
                    {
                        printerTaskFunc = null;
                    }
                    break;
                default:
                    printerTaskFunc = null;
                    break;
            }

        };

        return FUNC;
    }




}






/// <summary>
/// 旧版本打印机选择代码
/// </summary>
public partial class DevicePrinterOut : CorBehaviour
{


    private void OnEnable01()
    {
        // if (!ApplicationSettings.Instance.isMachine) return;

        EventCenter.Instance.AddEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, OnPrinterListGet);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, OnPrinterSelect);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, OnPrinterReset);

        DoCor(COR_INIT_PRINTER, DoTaskRepeat(GetPrintList, 5000));
    }

    private void OnDisable01()
    {
        EventCenter.Instance.RemoveEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, OnPrinterListGet);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, OnPrinterSelect);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, OnPrinterReset);
    }

    public void GetPrintList()
    {
        _consoleBB.Instance.isInitPrinter = false;

        if (Application.isEditor)
        {
            MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET);
        }
        else
        {
            SBoxSandbox.PrinterListGet();
        }
    }

    void OnPrinterListGet(List<string> strList)
    {

        for (int i = 0; i < strList.Count; i++)
        {
            Debug.Log("支持的打印机 - " + strList[i]);
        }
        strList.ForEach(str => DebugUtils.Log(str));

        SBoxSandbox.PrinterSelect(_consoleBB.Instance.selectPrinterNumber);
    }


    void OnPrinterSelect(int result)
    {
        if (result == 0)
        {

            if (Application.isEditor)
            {
                MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET);
            }
            else
            {
                SBoxSandbox.PrinterReset();
            }
        }
        else
        {
            DebugUtils.LogWarning($"【printer】: : 打印机选择失败  index = 0 ");
        }
    }



    void OnPrinterReset(int result)
    {
        if (result == 0)
        {
            ClearCor(COR_INIT_PRINTER);
            _consoleBB.Instance.isInitPrinter = true;
        }
        else
        {
            DebugUtils.LogWarning("【printer】: : 打印机复位失败");
        }
    }

}