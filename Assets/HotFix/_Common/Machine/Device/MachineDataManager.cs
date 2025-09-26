#define TEST
#define _TEST02
using Newtonsoft.Json;
using SBoxApi;
using System;
using UnityEngine;
using GameMaker;
using SBoxGodOfWealth = SBoxApi.SBoxGodOfWealth;
using System.Collections.Generic;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class MachineDataManager : MonoSingleton<MachineDataManager>
{
    /// <summary> 获取打码数据 </summary>
    //const string RpcNameMacineCodingInfo = SBoxEventHandle.SBOX_REQUEST_CODER;
    const string RpcNameIsCodingActive = "RpcNameIsCodingActive";
    //const string RpcNameGetPlayerInfo = "RpcNameGetPlayerInfo";
    const string RpcNameSetPlayerInfo = "RpcNameSetPlayerInfo";
    //const string RpcNameWriteConf = "RpcNameWriteConf";
    const string RpcNameClearCodingActive = "RpcNameClearCodingActive";

    const string RpcNameScoreUp = "RpcNameScoreUp";
    const string RpcNameScoreDown = "RpcNameScoreDown";

    const string RpcNameCoinIn = "RpcNameCoinIn";
    const string RpcNameCoinOut = "RpcNameCoinOut";


    const string RpcNameIsPrinterConnect = "RpcNameIsPrinterConnect";
    const string RpcNameIsBillerConnect = "RpcNameIsBillerConnect";


    //public const string RpcNameGameJackpot = "RpcNameGameJackpot";

    //const string RpcNameBillIn = "RpcNameBillIn";
    //const string RpcNamePrintOut = "RpcNamePrintOut";

    private SeverHelper severHelper;



    /// <summary> 机台licnese</summary>
    const string TEST_MACHINE_LICENESE = "TEST_MACHINE_LICENESE";
    const string TEST_MACHINE_LICENESE_OUT_TIME = "TEST_MACHINE_LICENESE_OUT_TIME";
    const string TEST_MACHINE_LICENESE_CREAT_TIME = "TEST_MACHINE_LICENESE_CREAT_TIME";
    /// <summary> 机台设置 </summary>
    const string TEST_MACHINE_SEVER_CONF = "TEST_MACHINE_SEVER_CONF";
    /// <summary> 玩家数据 </summary>
    const string TEST_MACHINE_SEVER_PLAYER = "TEST_MACHINE_SEVER_PLAYER";




    const string TEST_JP_GRAND = "TEST_JP_GRAND";
    const string TEST_JP_MAJOR = "TEST_JP_MAJOR";
    const string TEST_JP_MINOR = "TEST_JP_MINOR";
    const string TEST_JP_MINI = "TEST_JP_MINI";







    const string TEST_PASSWORD_SHIFT = "TEST_PASSWORD_SHIFT";
    const string TEST_PASSWORD_MANAGER = "TEST_PASSWORD_MANAGER";
    const string TEST_PASSWORD_ADMIN = "TEST_PASSWORD_ADMIN";

    private int curPermissions = -1;

    private void Awake()
    {
        severHelper = new SeverHelper()
        {
            receiveOvertimeMS = 1000,
            requestFunc = requestFunc,
            isDebug = true,
            prefix = "【SBox】",
        };
    }


    void Start()
    {
        /*if (!ApplicationSettings.Instance.isMachine)
            return;*/


        //监听下行:打码信息
        EventCenter.Instance.AddEventListener<SBoxCoderData>(SBoxEventHandle.SBOX_REQUEST_CODER, OnResponseMachineCodingInfo);
        //监听下行:打码
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CODER, OnResponseSetCoding);
        //监听下行:读取投退币配置信息
        EventCenter.Instance.AddEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnResponseReadConf);

        // =================================
        // 获取玩家信息
        EventCenter.Instance.AddEventListener<SBoxAccount>(SBoxEventHandle.SBOX_GET_ACCOUNT, OnResponseGetPlayerInfo);
        // Slot Spin
        EventCenter.Instance.AddEventListener<SBoxSlotSpinData>(SBoxEventHandle.SBOX_SLOT_SPIN, OnResponseSlotSpin);




        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_METER_SET, OnResponseCounter);

        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SET_PLAYER_BETS, OnResponseSetPlayerBets);




        //EventCenter.Instance.AddEventListener<int[]>(RpcNameGameJackpot, OnResponseGameJackpot);


        // Sas
        EventCenter.Instance.AddEventListener<int[]>(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP, OnResponseSasCashSeqScoreUp);
        EventCenter.Instance.AddEventListener<int[]>(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN, OnResponseSasCashSeqScoreDown);


        // jackpot
        //EventCenter.Instance.AddEventListener<SBoxGameJackpotData>(SBoxEventHandle.SBOX_GAME_JACKPOT, OnResponseGameJackpot);


        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_WRITE_CONF, OnResponseWriteConf);


        // 版本
        EventCenter.Instance.AddEventListener<string>(SBoxEventHandle.SBOX_IDEA_VERSION, OnResponseGetAlgorithmVersion);
        EventCenter.Instance.AddEventListener<string>(SBoxEventHandle.SBOX_SANDBOX_VERSION, OnResponseGetHardwareVersion);

        //// 打印机
        EventCenter.Instance.AddEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, OnResponseGetPrinterList);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, OnResponseSelectPrinter);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, OnResponseResetPrinter);

        //纸钞机
        EventCenter.Instance.AddEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, OnResponseGetBillerList);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, OnResponseSelectBiller);


        // 用户密码
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CHECK_PASSWORD, OnResponseCheckPassword);
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CHANGE_PASSWORD, OnResponseChangePassword);


    }

    protected override void OnDestroy()
    {
        //监听打码信息
        EventCenter.Instance.RemoveEventListener<SBoxCoderData>(SBoxEventHandle.SBOX_REQUEST_CODER, OnResponseMachineCodingInfo);
        //监听下行:打码
        EventCenter.Instance.RemoveEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CODER, OnResponseSetCoding);
        //监听下行:读取投退币配置信息
        EventCenter.Instance.RemoveEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnResponseReadConf);

        // =================================
        // 获取玩家信息
        EventCenter.Instance.RemoveEventListener<SBoxAccount>(SBoxEventHandle.SBOX_GET_ACCOUNT, OnResponseGetPlayerInfo);
        // Slot Spin
        EventCenter.Instance.RemoveEventListener<SBoxSlotSpinData>(SBoxEventHandle.SBOX_SLOT_SPIN, OnResponseSlotSpin);


        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_METER_SET, OnResponseCounter);

        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SET_PLAYER_BETS, OnResponseSetPlayerBets);


        //EventCenter.Instance.RemoveEventListener<int[]>(RpcNameGameJackpot, OnResponseGameJackpot);

        // Sas
        EventCenter.Instance.RemoveEventListener<int[]>(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP, OnResponseSasCashSeqScoreUp);
        EventCenter.Instance.RemoveEventListener<int[]>(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN, OnResponseSasCashSeqScoreDown);


        // jackpot
        //EventCenter.Instance.RemoveEventListener<SBoxGameJackpotData>(SBoxEventHandle.SBOX_GAME_JACKPOT, OnResponseGameJackpot);


        EventCenter.Instance.RemoveEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_WRITE_CONF, OnResponseWriteConf);


        // 版本
        EventCenter.Instance.RemoveEventListener<string>(SBoxEventHandle.SBOX_IDEA_VERSION, OnResponseGetAlgorithmVersion);
        EventCenter.Instance.RemoveEventListener<string>(SBoxEventHandle.SBOX_SANDBOX_VERSION, OnResponseGetHardwareVersion);

        ////打印机
        EventCenter.Instance.RemoveEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, OnResponseGetPrinterList);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, OnResponseSelectPrinter);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, OnResponseResetPrinter);

        //纸钞机
        EventCenter.Instance.RemoveEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, OnResponseGetBillerList);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, OnResponseSelectBiller);

        // 用户密码
        EventCenter.Instance.RemoveEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CHECK_PASSWORD, OnResponseCheckPassword);
        EventCenter.Instance.RemoveEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CHANGE_PASSWORD, OnResponseChangePassword);

        base.OnDestroy();
    }



    // EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, OnPrinterSelect);
    //EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, OnPrinterReset);




    /* void testWrite()
    {
        if (Application.isEditor)
        {
            SBoxConfData data = new SBoxConfData();
            MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_WRITE_CONF, JSONNodeUtil.ObjectToJsonStr(data));
        }
        else
        {
            SBoxConfData data = new SBoxConfData();
            SBoxIdea.WriteConf(data);
        }
    }
     */



    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">退币器编号，0~1</param>
    /// <param name="counts">退币数量</param>
    /// <param name="type">退币类型，0：退币，1：脉冲打印</param>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestCoinOut(int id, int counts, int type,Action<object> successCallback, Action<BagelCodeError> errorCallback)
    {
        SBoxSandbox.CoinOutStart(0, this.targetCoinOutNum, 0);
        return severHelper.RequestData(SBoxEventHandle.SBOX_CASH_SEQ, null, successCallback, null);
    }
    void OnRequestCoinOut(string res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_IDEA_VERSION, res);
    */





    /// <summary>
    /// 硬件版本
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <param name="mark"></param>
    /// <returns></returns>
    public int RequestGetHardwareVersion(Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_SANDBOX_VERSION, null, successCallback, errorCallback, mark);
    }
    void OnResponseGetHardwareVersion(string res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SANDBOX_VERSION, res);


    /// <summary>
    /// 算法卡版本
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <param name="mark"></param>
    /// <returns></returns>
    public int RequestGetAlgorithmVersion(Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_IDEA_VERSION, null, successCallback, errorCallback, mark);
    }
    void OnResponseGetAlgorithmVersion(string res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_IDEA_VERSION, res);


    /// <summary>
    /// 打印机是否链接
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestIsPrinterConnect(Action<object> successCallback, string mark = null)
    {
        return severHelper.RequestData(RpcNameIsPrinterConnect, null, successCallback, null, mark);
    }
    void OnResponseIsPrinterConnect(int res) =>
        severHelper.OnSuccessResponseData(RpcNameIsPrinterConnect, res);



    /// <summary>
    /// 纸钞机是否链接
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestIsBillerConnect(Action<object> successCallback,  string mark = null)
    {
        return severHelper.RequestData(RpcNameIsBillerConnect, null, successCallback, null, mark);
    }
    void OnResponseIsBillerConnect(int res) =>
        severHelper.OnSuccessResponseData(RpcNameIsBillerConnect, res);




    /// <summary>
    /// 获纸钞机列表
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestGetBillerList(Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, null, successCallback, errorCallback, mark);
    }
    void OnResponseGetBillerList(List<string> res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, res);

    /// <summary>
    /// 选择纸钞机
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestSelectBiller(int index, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, index, successCallback, errorCallback, mark);
    }
    void OnResponseSelectBiller(int res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, res);




    /// <summary>
    /// 获取打印机列表
    /// </summary>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestGetPrinterList(Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, null, successCallback, errorCallback, mark);
    }
    void OnResponseGetPrinterList(List<string> res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, res);



    /// <summary>
    /// 选择打印机
    /// </summary>
    /// <param name="index"></param>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestSelectPrinter(int index, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, index, successCallback, errorCallback);
    }
    void OnResponseSelectPrinter(int res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, res);


    /// <summary>
    /// 复位打印机
    /// </summary>
    /// <param name="index"></param>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public int RequestResetPrinter(Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, null, successCallback, errorCallback);
    }
    void OnResponseResetPrinter(int res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, res);



    /// <summary>
    /// Sas纸钞机进钞票时，请求seq
    /// </summary>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    /// <remarks>
    /// res[0]=0 成功<br/>
    /// cashSeq = res[1] 高32位 + res[2] 底32位
    /// </remarks>
    public int RequestSasCashSeqScoreUp(Action<object> successCallback)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP, null, successCallback, null);
    }
    void OnResponseSasCashSeqScoreUp(int[] res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP, res);  // cashSeq = ((long)res[1] << 32) | (uint)res[2];



    /// <summary>
    /// Sas打印出票时，请求seq
    /// </summary>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    /// <remarks>
    /// res[0]=0 成功<br/>
    /// cashSeq = res[1] 高32位 + res[2] 底32位
    /// </remarks>
    public int RequestSasCashSeqScoreDown(Action<object> successCallback)
    {
        return severHelper.RequestData(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN, null, successCallback, null);
    }
    void OnResponseSasCashSeqScoreDown(int[] res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN, res);  // cashSeq = ((long)res[1] << 32) | (uint)res[2];




    /*
    /// <summary> 获取游戏彩金 </summary>
    public int RequestGameJackpot(Action<object> successCallback, Action<BagelCodeError> errorCallback)
        => severHelper.RequestData(SBoxEventHandle.SBOX_GAME_JACKPOT, null, successCallback, errorCallback);
    void OnResponseGameJackpot(SBoxGameJackpotData res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_GAME_JACKPOT, res);
    */



    /// <summary>
    /// 外设彩金通知算法卡
    /// </summary>
    /// <param name="winJpCredit"></param>
    /// <returns></returns>
    public void NotifyGameJackpot(int winJpCredit)
    {
        if (ApplicationSettings.Instance.isMock)
        {
            RequestSetPlayerCreditWhenMock(-1, 0, winJpCredit);
        }
        else
        {
            // SBoxGodOfWealth.GameJackpot(winJpCredit);
            RequestScoreUp(winJpCredit, null);
        }
    }



    /// <summary>
    /// 设置玩家当前压住金额
    /// </summary>
    /// <param name="balance">玩家积分，回传</param>
    /// <param name="totalbet">当前压住金额</param>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    public int RequestSetPlayerBets(long balance, long totalbet, Action<object> successCallback)
    {
        SBoxPlayerBetsData req = new SBoxPlayerBetsData()
        {
            PlayerId = 0,                 // 玩家ID
            balance = (int)balance,       // 当前余分
            rfu = 0,                      // 保留
            Bets = new int[15] { (int)totalbet, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },     ///new int[15] { 0 }, // 本局15门押分
        };
        return severHelper.RequestData(SBoxEventHandle.SBOX_SET_PLAYER_BETS, req, successCallback, null);
    }
    void OnResponseSetPlayerBets(int res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SET_PLAYER_BETS, res);


    /// <summary>
    /// 码表
    /// </summary>
	/// <param name="id">码表编号,0:投币码表,1:退币码表,2:上分码表,3:下分码表</param>
    /// <param name="counts">码表走数</param>
    /// <param name="type">走数类型,0:无䇅,1:counts为绝对值,2:counts为追加值,3:中止走数,</param>
    /// <param name="successCallback"></param>	
    /// <returns>
    /// result = 0：成功
    /// result 《 0：发送参数错误
    /// result 》 0：状态码(保留)
    /// </returns>
    public int RequestCounter(int id, int counts, int type, Action<object> successCallback)
    {
        Dictionary<string, object> req = new Dictionary<string, object>()
        {
            ["id"] = id,
            ["counts"] = counts,
            ["type"] = type,
        };
        return severHelper.RequestData(SBoxEventHandle.SBOX_SADNBOX_METER_SET, req, successCallback, null);
    }
    void OnResponseCounter(int res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_SADNBOX_METER_SET, res);


    /// <summary> 密码校验 </summary>
    public int RequestCheckPassword(int password, Action<object> successCallback, Action<BagelCodeError> errorCallback)
        => severHelper.RequestData(SBoxEventHandle.SBOX_CHECK_PASSWORD, password, successCallback, errorCallback);
    void OnResponseCheckPassword(SBoxPermissionsData res) =>
        severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_CHECK_PASSWORD, res);//有可能会超时，所以要errorCallback
                                                                                    //severHelper.OnResponsData(SBoxEventHandle.SBOX_CHECK_PASSWORD, res, res.result != 0);  //有可能会超时

    /*
        /// <summary> 密码校验 </summary>
        public int RequestCheckPassword(int password, Action<object> successCallback)
            => severHelper.RequestData(SBoxEventHandle.SBOX_CHECK_PASSWORD, password, successCallback, null);
        void OnResponseCheckPassword(SBoxPermissionsData res) =>
            severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_CHECK_PASSWORD, res);  */



    /// <summary> 修改密码 </summary>
    public int RequestChangePassword(int password, Action<object> successCallback, Action<BagelCodeError> errorCallback)
        => severHelper.RequestData(SBoxEventHandle.SBOX_CHANGE_PASSWORD, password, successCallback, errorCallback);
    void OnResponseChangePassword(SBoxPermissionsData res) =>
        severHelper.OnResponsData(SBoxEventHandle.SBOX_CHANGE_PASSWORD, res, res.result != 0);





    /// <summary>
    /// 上分
    /// </summary>
    /// <param name="credit">分数</param>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    public int RequestScoreUp(int credit, Action<object> successCallback)
        => severHelper.RequestData(RpcNameScoreUp, credit, successCallback, null);
    void OnResponseScoreUp(int credit) => severHelper.OnSuccessResponseData(RpcNameScoreUp, credit);


    /// <summary>
    /// 下分
    /// </summary>
    /// <param name="credit">分数</param>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    public int RequestScoreDown(int credit, Action<object> successCallback)
        => severHelper.RequestData(RpcNameScoreDown, credit, successCallback, null);
    void OnResponseScoreDown(int credit) => severHelper.OnSuccessResponseData(RpcNameScoreDown, credit);



    /// <summary>
    /// 投币
    /// </summary>
    /// <param name="num">个数</param>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    public int RequestCoinIn(int num, Action<object> successCallback)
    => severHelper.RequestData(RpcNameCoinIn, num, successCallback, null);
    void OnResponseCoinIn(int num) => severHelper.OnSuccessResponseData(RpcNameCoinIn, num);



    /// <summary>
    /// 退票
    /// </summary>
    /// <param name="num">个数</param>
    /// <param name="successCallback"></param>
    /// <returns></returns>
    public int RequestCoinOut(int num, Action<object> successCallback)
    => severHelper.RequestData(RpcNameCoinOut, num, successCallback, null);
    void OnResponseCoinOut(int num) => severHelper.OnSuccessResponseData(RpcNameCoinOut, num);





    /// <summary> 请求投退币配置 </summary>
    public int RequestSlotSpin(int bet, Action<object> successCallback, Action<BagelCodeError> errorCallback)
        => severHelper.RequestData(SBoxEventHandle.SBOX_SLOT_SPIN, bet, successCallback, errorCallback);
    void OnResponseSlotSpin(SBoxSlotSpinData res) => severHelper.OnResponsData(SBoxEventHandle.SBOX_SLOT_SPIN, res, res.result != 0);


    /// <summary> 请求配置 </summary>
    public int RequestReadConf(Action<object> successCallback, Action<BagelCodeError> errorCallback)
        => severHelper.RequestData(SBoxEventHandle.SBOX_READ_CONF, null, successCallback, errorCallback);
    void OnResponseReadConf(SBoxConfData res) => severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_READ_CONF, res);


    /// <summary> 修改配置 </summary>
    public int RequestWriteConf(SBoxConfData data, Action<object> successCallback, Action<BagelCodeError> errorCallback) =>
                severHelper.RequestData(SBoxEventHandle.SBOX_WRITE_CONF, data, successCallback, errorCallback);
    void OnResponseWriteConf(SBoxPermissionsData res) => severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_WRITE_CONF, res);



    /// <summary> 打码 </summary>
    public int RequestSetCoding(ulong code, Action<object> successCallback, Action<BagelCodeError> errorCallback) =>
        severHelper.RequestData(SBoxEventHandle.SBOX_CODER, code, successCallback, errorCallback);
    void OnResponseSetCoding(SBoxPermissionsData res) => severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_CODER, res);


    /// <summary>设置为未激活</summary>
    public int RequestClearCodingActive(Action<object> successCallback, Action<BagelCodeError> errorCallback)
        => severHelper.RequestData(RpcNameClearCodingActive, null, successCallback, errorCallback);
    void OnResponseClearCodingActive(object res) => severHelper.OnSuccessResponseData(RpcNameClearCodingActive, res);


    /// <summary> 是否激活 </summary>
    public int RequestIsCodingActive(Action<object> successCallback) =>
        severHelper.RequestData(RpcNameIsCodingActive, null, successCallback, null);
    void OnResponseIsCodingActive(int code) => severHelper.OnSuccessResponseData(RpcNameIsCodingActive, code);


    /// <summary> 请求打码数据 </summary>
    public int RequestMachineCodingInfo(Action<object> successCallback, Action<BagelCodeError> errorCallback) =>
        severHelper.RequestData(SBoxEventHandle.SBOX_REQUEST_CODER, null, successCallback, errorCallback);
    void OnResponseMachineCodingInfo(SBoxCoderData res) => severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_REQUEST_CODER, res);


    /// <summary> 获取玩家信息 </summary>
    public int RequestGetPlayerInfo(Action<object> successCallback, Action<BagelCodeError> errorCallback) =>
        severHelper.RequestData(SBoxEventHandle.SBOX_GET_ACCOUNT, null, successCallback, errorCallback);
    void OnResponseGetPlayerInfo(SBoxAccount res) => severHelper.OnSuccessResponseData(SBoxEventHandle.SBOX_GET_ACCOUNT, res);


    /// <summary> 修改玩家信息 </summary>
    public int RequestSetPlayerInfo(SBoxPlayerAccount req, Action<object> successCallback)
         => severHelper.RequestData(RpcNameSetPlayerInfo, req, successCallback, null);
    void OnResponseSetPlayerInfo(SBoxPlayerAccount res) => severHelper.OnSuccessResponseData(RpcNameSetPlayerInfo, res);



    #region Mock接口

    /// <summary> 类似算法卡设置玩家金额 调试用 </summary>
    /// <remarks>
    /// mock模式下，没有算方法开，需要自己维护玩家金额
    /// </remarks>
    public SBoxPlayerAccount RequestSetPlayerCreditWhenMock(long credit, long bet, long win)
        => RequestSetPlayerAccountWhenMock(credit, bet, win, 0, 0, 0, 0);

    public SBoxPlayerAccount RequestSetCoinInCoinOutWhenMock(long credit, long coinInNum, long coinOutNum, long scoreInCredit, long scoreOutCredit)
        => RequestSetPlayerAccountWhenMock(credit, 0, 0, coinInNum, coinOutNum, scoreInCredit, scoreOutCredit);

    public SBoxPlayerAccount RequestSetPlayerAccountWhenMock(long credit, long bet, long win, long coinInNum, long coinOutNum, long scoreInCredit, long scoreOutCredit)
    {
        if (
            bet < 0
            && win < 0
            && coinInNum < 0
            && coinOutNum < 0
            && scoreInCredit < 0
            && scoreOutCredit < 0
            )
        {
            Debug.LogError("SetPlayerAccountWhenMock : data is error");
            return null;
        }


        if (!ApplicationSettings.Instance.isMock)
            return null;

        SBoxPlayerAccount PlayerAccount = _RequestGetPlayerAccountWhenMock();

        if (credit >= 0)
            PlayerAccount.Credit = (int)credit;
        else
        {
            PlayerAccount.Credit = PlayerAccount.Credit + (int)win - (int)bet
                + (int)coinInNum * _consoleBB.Instance.coinInScale + (int)scoreInCredit
                 - DeviceUtils.GetCoinOutCredit((int)coinOutNum) - (int)scoreOutCredit;
        }

        PlayerAccount.Bets += (int)bet;
        PlayerAccount.Wins += (int)win;
        PlayerAccount.CoinIn += (int)coinInNum;
        PlayerAccount.CoinOut += (int)coinOutNum;
        PlayerAccount.ScoreIn += (int)scoreInCredit;
        PlayerAccount.ScoreOut += (int)scoreOutCredit;
        SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_SEVER_PLAYER, JSONNodeUtil.ObjectToJsonStr((SBoxPlayerAccount)PlayerAccount));

        //EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_MOCK_EVENT, new EventData<SBoxPlayerAccount>(GlobalEvent.PlayerAccountChange, PlayerAccount));

        return PlayerAccount;
    }


    public SBoxPlayerAccount RequestClearPlayerAccountWhenMock()
    {
        SBoxPlayerAccount PlayerAccount = new SBoxPlayerAccount()
        {
            PlayerId = 0,    // 默认id为0
            CoinIn = 0,    // 总投币分
            CoinOut = 0,     // 总退币分
            ScoreIn = 0,     // 总上分
            ScoreOut = 0,    // 总下分                       
            Credit = 0,      // 余额分
            Bets = 0,        // 历史总押分
            Wins = 0,        // 历史总赢分
        };
        SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_SEVER_PLAYER, JSONNodeUtil.ObjectToJsonStr((SBoxPlayerAccount)PlayerAccount));

        //EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_MOCK_EVENT, new EventData<SBoxPlayerAccount>(GlobalEvent.PlayerAccountChange, PlayerAccount));

        return PlayerAccount;
    }

    SBoxPlayerAccount _RequestGetPlayerAccountWhenMock()
    {
        if (!ApplicationSettings.Instance.isMock)
            return null;
        SBoxPlayerAccount PlayerAccount = new SBoxPlayerAccount()
        {
            PlayerId = 0,    // 默认id为0
            CoinIn = 0,    // 总投币分
            CoinOut = 0,     // 总退币分
            ScoreIn = 0,     // 总上分
            ScoreOut = 0,    // 总下分                       
            Credit = 0,      // 余额分
            Bets = 0,        // 历史总押分
            Wins = 0,        // 历史总赢分
        };
        string cache = SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_SEVER_PLAYER, JsonConvert.SerializeObject(PlayerAccount));
        PlayerAccount = JsonConvert.DeserializeObject<SBoxPlayerAccount>(cache);

        //EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_MOCK_EVENT, new EventData<SBoxPlayerAccount>(GlobalEvent.PlayerAccountChange, PlayerAccount ));

        return PlayerAccount;

    }



    /// <summary>
    /// Mock状态下，模拟大厅彩金
    /// </summary>
    public void RequestJackpotOnLineWhenMock()
    {
        if (!ApplicationSettings.Instance.isMock)
            return;

        int idx = UnityEngine.Random.Range(0, 20);
        if (idx <= 3 && idx >= 0)
        {
            int count = UnityEngine.Random.Range(1, 20);
            string res = JsonConvert.SerializeObject(new WinJackpotInfo()
            {
                macId = int.Parse(_consoleBB.Instance.machineID),
                seat = 0,
                win = count,
                jackpotId = idx,
                orderId = -1,
                time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            });

            // 模拟算法卡，同步玩家金额。(这一不在投币方法里已经实现)

            EventCenter.Instance.EventTrigger<string>(RPCName.jackpotHall, res);
        }
    }


    #endregion







    /// <summary>
    /// 删除某条请求
    /// </summary>
    /// <param name="seqID"></param>
    public void RemoveRequestAt(int seqID) => severHelper.RemoveRequestAt(seqID);

    public void RemoveRequestAt(string mark) => severHelper.RemoveRequestAt(mark);

    int testActiveMinute = (int)(60f * 24 * 3f); //3天

    void requestFunc(string rpcName, object req)
    {

#if TEST

        switch (rpcName)
        {
            case RpcNameScoreUp:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    RequestSetCoinInCoinOutWhenMock(-1, 0, 0, (int)req, 0);
                    // 上分逻辑
                    OnResponseScoreUp((int)req);
                }
                return;
            case RpcNameScoreDown:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    RequestSetCoinInCoinOutWhenMock(-1, 0, 0, 0, (int)req);
                    // 下分逻辑
                    OnResponseScoreDown((int)req);
                }
                return;
            case RpcNameCoinIn:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    RequestSetCoinInCoinOutWhenMock(-1, (int)req, 0, 0, 0);
                    // 上分逻辑
                    OnResponseCoinIn((int)req);
                }
                return;
            case RpcNameCoinOut:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    RequestSetCoinInCoinOutWhenMock(-1, 0, (int)req, 0, 0);
                    // 下分逻辑
                    OnResponseCoinOut((int)req);
                }
                return;
            case SBoxEventHandle.SBOX_REQUEST_CODER:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    SBoxCoderData reqDefault = new SBoxCoderData()
                    {
                        result = 0,
                        Bets = 100,
                        Wins = 2,
                        MachineId = ConfigUtils.curGameId,
                        CoderCount = 0,
                        CheckValue = 567,
                        RemainMinute = -1, //(60 * 1 + 3),         // 当前剩余时间（分钟）
                    };
                    //MACHINE_LICENESE_OUT_TIME
                    SBoxCoderData req01 = JsonConvert.DeserializeObject<SBoxCoderData>(SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_LICENESE, JSONNodeUtil.ObjectToJsonStr(reqDefault)));
                    long nowTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    if (req01.RemainMinute == -1)
                    {
                        req01.RemainMinute = testActiveMinute;

                        long outtime = nowTimeMS + (long)(req01.RemainMinute * 60 * 1000);
                        SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_LICENESE_OUT_TIME, outtime.ToString());
                        //SQLitePlayerPrefs02.Instance.SetString(MACHINE_LICENESE_CREAT_TIME, nowTimeMS.ToString()); 
                    }

                    long difMS = long.Parse(SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_LICENESE_OUT_TIME, nowTimeMS.ToString()))
                        - nowTimeMS;
                    // long.Parse(SQLitePlayerPrefs02.Instance.GetString(MACHINE_LICENESE_CREAT_TIME, nowTimeMS.ToString()));
                    int difMinute = (int)(difMS / 1000 / 60);


                    if (difMinute > 0)
                    {
                        req01.RemainMinute = difMinute;
                    }
                    else
                    {
                        req01.RemainMinute = 0;
                    }

                    DebugUtils.Log($"RemainMinute = {req01.RemainMinute}");
                    SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_LICENESE, JSONNodeUtil.ObjectToJsonStr(req01));

                    //下行:
                    EventCenter.Instance.EventTrigger<SBoxCoderData>(SBoxEventHandle.SBOX_REQUEST_CODER, req01);
                }
                return;

            case SBoxEventHandle.SBOX_CODER: //请求打码
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {

                    ulong code = (ulong)req;
                    if (code == 666)
                    {
                        SBoxCoderData req01 = JsonConvert.DeserializeObject<SBoxCoderData>(SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_LICENESE, "{}"));
                        req01.RemainMinute = testActiveMinute;

                        long nowTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        long outtime = nowTimeMS + (long)(req01.RemainMinute * 60 * 1000);
                        SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_LICENESE_OUT_TIME, outtime.ToString());


                        SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_LICENESE, JSONNodeUtil.ObjectToJsonStr(req01));

                        // 打码成功
                        EventCenter.Instance.EventTrigger<SBoxPermissionsData>(SBoxEventHandle.SBOX_CODER, new SBoxPermissionsData()
                        {

                            result = 0,
                            permissions = 2001,
                        });
                    }
                    else
                    {
                        // 打码失败
                        EventCenter.Instance.EventTrigger<SBoxPermissionsData>(SBoxEventHandle.SBOX_CODER, new SBoxPermissionsData()
                        {
                            result = 1,
                            permissions = 2001,
                        });
                    }
                }
                return;
            case RpcNameIsCodingActive: //是否激活
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    //非0时,需要激活
                    long nowTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    long lastTimeMS = long.Parse(SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_LICENESE_OUT_TIME, nowTimeMS.ToString()));
                    long difMS = nowTimeMS - lastTimeMS;
                    OnResponseIsCodingActive(difMS < 0 ? 0 : 1);
                }
                return;

            case RpcNameClearCodingActive:
                {
                    SBoxCoderData reqDefault = new SBoxCoderData()
                    {
                        result = 0,

                        Bets = 100,
                        Wins = 2,
                        MachineId = ConfigUtils.curGameId,
                        CoderCount = 0,
                        CheckValue = 567,
                        RemainMinute = -1, //(60 * 1 + 3),         // 当前剩余时间（分钟）
                    };
                    SBoxCoderData data = JsonConvert.DeserializeObject<SBoxCoderData>(SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_LICENESE, JSONNodeUtil.ObjectToJsonStr(reqDefault)));
                    data.RemainMinute = 0;
                    long outtime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 1000;
                    SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_LICENESE_OUT_TIME, outtime.ToString());

                    OnResponseClearCodingActive(null);
                }
                return;


            // 【check】
            case SBoxEventHandle.SBOX_GET_ACCOUNT: // 获取玩家信息
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {

                    List<SBoxPlayerAccount> PlayerAccountList = new List<SBoxPlayerAccount>();

                    SBoxPlayerAccount PlayerAccount = _RequestGetPlayerAccountWhenMock();
                    PlayerAccountList.Add(PlayerAccount);
                    SBoxAccount res = new SBoxAccount()
                    {
                        result = 0,
                        PlayerAccountList = PlayerAccountList,
                    };
                    EventCenter.Instance.EventTrigger<SBoxAccount>(SBoxEventHandle.SBOX_GET_ACCOUNT, res);

                }
                return;
            case RpcNameSetPlayerInfo: // 设置玩家信息
                {
                    SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_SEVER_PLAYER, JSONNodeUtil.ObjectToJsonStr((SBoxPlayerAccount)req));
                    OnResponseSetPlayerInfo((SBoxPlayerAccount)req);
                }
                return;
            case SBoxEventHandle.SBOX_READ_CONF:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    SBoxConfData confDefault = new SBoxConfData()
                    {
                        result = 0,
                        PwdType = 0,                         // 0:无任何修改参数的权限,1:普通密码权限,2:管理员密码权限,3:超级管理员密码权限
                        PlaceType = 0,                       // 场地类型,0:普通,1:技巧,2:专家
                        difficulty = 0,                      // 难度,0~8
                        odds = 0,                            // 倍率,0:低倍率,1:高倍率,2:随机

                        WinLock = 100,                         // 盈利宕机
                        MachineId = 11109001,                       // 机台编号,8位有效十进制数
                        LineId = 1110,                          // 线号,4位有效十进制数

                        TicketMode = 0,                      // 退票模式,0:即中即退,1:退票
                        TicketValue = 100,                   // 1票对应几分（彩票比例）  (投票:1票多少分？)
                        scoreTicket = 100,                   // 1分对应几票
                        CoinValue = 1000,                    // 投币比例 (投币:1币多少分？)
                        MaxBet = 0,                          // 最大押注
                        MinBet = 0,                          // 最小押注
                        CountDown = 0,                       // 例计时
                        MachineIdLock = 0,                   // 1:机台号已锁定,除超级管理员外,无法更改,0:机台号未锁定
                        BetsMinOfJackpot = 0,                // 中彩金最小押分值
                        JackpotStartValue = 0,               // 彩金初始值

                        //LostLockCustom = 0,                  // 当轮爆机分数:默认300000


                        LimitBetsWins = 0,// 限红值,默认3000
                        ReturnScore = 0,// 返分值,500
                        SwitchBetsUnitMin = 0,// 切换单位小,默认10
                        SwitchBetsUnitMid = 0, // 切换单位中,默认50
                        SwitchBetsUnitMax = 0,// 切换单位大,默认100
                        ScoreUpUnit = 0, // 上分单位
                        PrintMode = 0, // 打单模式,0:不打印,1:正常打印,2:伸缩打印
                        ShowMode = 0,
                        CheckTime = 0,// 对单时间
                        OpenBoxTime = 0,// 开箱时间
                        PrintLevel = 0, // 打印深度,0,1,2三级,0时最
                        PlayerWinLock = 0,// 分机爆机分数:默认100000
                        LostLock = 0,// 全台爆机分数:默认500000
                        PulseValue = 0,// 脉冲比例 
                        NewGameMode = 0,// 开始新一轮游戏模式,0:自动开始,1:手动开始
                        NetJackpot = 0,
                    };
                    SBoxConfData conf = JsonUtility.FromJson<SBoxConfData>(
                        SQLitePlayerPrefs03.Instance.GetString(TEST_MACHINE_SEVER_CONF, JSONNodeUtil.ObjectToJsonStr(confDefault)));

                    EventCenter.Instance.EventTrigger<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, conf);
                }
                return;
            case SBoxEventHandle.SBOX_WRITE_CONF:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    SQLitePlayerPrefs03.Instance.SetString(TEST_MACHINE_SEVER_CONF, JSONNodeUtil.ObjectToJsonStr((SBoxConfData)req));

                    SBoxPermissionsData sBoxPermissionsData = new SBoxPermissionsData()
                    {
                        result = 0,
                        permissions = 1,
                    };
                    EventCenter.Instance.EventTrigger<SBoxPermissionsData>(SBoxEventHandle.SBOX_WRITE_CONF, sBoxPermissionsData);
                }
                return;

            // 【check】
            case SBoxEventHandle.SBOX_SLOT_SPIN:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    DebugUtils.Log("@假数据 : g152__RESULT_RECALL__7.json");
                    SBoxSlotSpinData sBoxSlotSpinData = new SBoxSlotSpinData()
                    {
                        result = 0,
                        pid = _consoleBB.Instance.pid,
                        bet = (int)req, //玩家压注
                        totalPay = 1220, // 玩家本局总赢
                        credit = 0,  // 玩家本局之后的积分
                        icons = new int[] { 8, 4, 7, 0, 4, 7, 4, 4, 4, 5, 4, 0, 4, 7, 3 }, //画面
                        curState = 0,
                        nextState = 0,
                        freeTimes = 0,
                        scatterLines = new WinLine[] { },
                        changeLines = new WinLine[] { },
                        winLines = new WinLine[]
                        {
                            new WinLine{
                                lineId = 1,
                                iconId = 4,
                                multiplier = 12,
                                winpos = new int[] { 11, 31, 41, 12, 22, 32, 3, 13, 23},
                                totalPay = 1200,
                                winLineType = 0,
                            },
                            new WinLine{
                                lineId = 2,
                                iconId = 7,
                                multiplier = 2,
                                winpos = new int[] {21, 31, 2, 13, 33},
                                totalPay = 20,
                                winLineType = 0,
                            },
                        },
                    };
                    EventCenter.Instance.EventTrigger<SBoxSlotSpinData>(SBoxEventHandle.SBOX_SLOT_SPIN, sBoxSlotSpinData);

                }
                return;
            case SBoxEventHandle.SBOX_CHECK_PASSWORD:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    SBoxPermissionsData sBoxPermissionsData = new SBoxPermissionsData()
                    {
                        result = 1,
                        permissions = -1,
                    };

                    string passwordStr = $"{req}";

                    _consoleBB.Instance.passwordShift = PlayerPrefs.GetString(TEST_PASSWORD_SHIFT, "666666");
                    _consoleBB.Instance.passwordManager = PlayerPrefs.GetString(TEST_PASSWORD_MANAGER, "88888888");
                    _consoleBB.Instance.passwordAdmin = PlayerPrefs.GetString(TEST_PASSWORD_ADMIN, "187653214");
                    if (_consoleBB.Instance.passwordShift == passwordStr)
                    {
                        sBoxPermissionsData.result = 0;
                        sBoxPermissionsData.permissions = 1;
                    }
                    else if (_consoleBB.Instance.passwordManager == passwordStr)
                    {
                        sBoxPermissionsData.result = 0;
                        sBoxPermissionsData.permissions = 2;
                    }
                    else if (_consoleBB.Instance.passwordAdmin == passwordStr)
                    {
                        sBoxPermissionsData.result = 0;
                        sBoxPermissionsData.permissions = 3;
                    }

                    curPermissions = sBoxPermissionsData.permissions;

                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_CHECK_PASSWORD, sBoxPermissionsData);
                }
                return;
            case SBoxEventHandle.SBOX_CHANGE_PASSWORD:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    //修改玩家钥匙
                    SBoxPermissionsData sBoxPermissionsData = new SBoxPermissionsData()
                    {
                        result = 0,
                        permissions = 0,
                    };
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_CHANGE_PASSWORD, sBoxPermissionsData);
                }
                return;


            case SBoxEventHandle.SBOX_SADNBOX_METER_SET:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    //码表
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SADNBOX_METER_SET, 0);
                }
                return;


            case SBoxEventHandle.SBOX_SET_PLAYER_BETS:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SET_PLAYER_BETS, 0);
                }
                return;



            case SBoxEventHandle.SBOX_IDEA_VERSION:  //获取算法版本
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_IDEA_VERSION, "-9.9.9");
                }
                return;

            case SBoxEventHandle.SBOX_SANDBOX_VERSION:  //获取算法版本
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SANDBOX_VERSION, "-8.8.8");
                }
                return;

#if false
            case SBoxEventHandle.SBOX_GAME_JACKPOT:  //旧方案算法卡的彩金，已经停用 
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {

                    int grandJPNow = PlayerPrefs.GetInt(TEST_JP_GRAND, 1000000);
                    int majorJPNow = PlayerPrefs.GetInt(TEST_JP_MAJOR, 100000);
                    int minorJPNow = PlayerPrefs.GetInt(TEST_JP_MINOR, 10000);
                    int miniJPNow = PlayerPrefs.GetInt(TEST_JP_MINI, 1000);

                    int winFlg = 0;

                    if (BlackboardUtils.GetValue<bool>("./isSpin"))
                    {

                        grandJPNow = grandJPNow == 0 ? 1000000 : grandJPNow;
                        majorJPNow = majorJPNow == 0 ? 100000 : majorJPNow;
                        minorJPNow = minorJPNow == 0 ? 10000 : minorJPNow;
                        miniJPNow = miniJPNow == 0 ? 1000 : miniJPNow;


                        grandJPNow += UnityEngine.Random.Range(10, 2000);
                        majorJPNow += UnityEngine.Random.Range(10, 2500);
                        minorJPNow += UnityEngine.Random.Range(10, 3000);
                        miniJPNow += UnityEngine.Random.Range(10, 3500);

                        grandJPNow = grandJPNow > 5000000 ? 1000000 : grandJPNow;
                        majorJPNow = majorJPNow > 500000 ? 100000 : majorJPNow;
                        minorJPNow = minorJPNow > 50000 ? 10000 : minorJPNow;
                        miniJPNow = miniJPNow > 5000 ? 1000 : miniJPNow;


                        int idx = UnityEngine.Random.Range(0, 100);
                        // 0001 == grand
                        // 0010 == majorJP
                        // 0100 == minorJP
                        // 1000 == miniJP
                        bool isWin = false;

                        if (idx >= 0 && idx <= 3)
                        {
                            isWin = true;
                            winFlg = 1 << idx;
                        }
                    }

                    int grandJPWin = winFlg != 0b0001 ? 0 : grandJPNow / 7;
                    int majorJPWin = winFlg != 0b0010 ? 0 : majorJPNow / 6;
                    int minorJPWin = winFlg != 0b0100 ? 0 : minorJPNow / 5;
                    int miniJPWin = winFlg != 0b1000 ? 0 : miniJPNow / 4;

                    int grandJPWhenWin = winFlg != 0b0001 ? 0 : grandJPNow;
                    int majorJPWhenWin = winFlg != 0b0010 ? 0 : majorJPNow;
                    int minorJPWhenWin = winFlg != 0b0100 ? 0 : minorJPNow;
                    int miniJPWhenWin = winFlg != 0b1000 ? 0 : miniJPNow;


                    // 这个数值是乘以100的， 倒数两位是小数部分，将其置为 .00
                    grandJPWin = grandJPWin / 100 * 100;
                    majorJPWin = majorJPWin / 100 * 100;
                    minorJPWin = minorJPWin / 100 * 100;
                    miniJPWin = miniJPWin / 100 * 100;


                    grandJPNow = grandJPNow - grandJPWin;
                    majorJPNow = majorJPNow - majorJPWin;
                    minorJPNow = minorJPNow - minorJPWin;
                    miniJPNow = miniJPNow - miniJPWin;

                    PlayerPrefs.SetInt(TEST_JP_GRAND, grandJPNow);
                    PlayerPrefs.SetInt(TEST_JP_MAJOR, majorJPNow);
                    PlayerPrefs.SetInt(TEST_JP_MINOR, minorJPNow);
                    PlayerPrefs.SetInt(TEST_JP_MINI, miniJPNow);


                    int totaWin = grandJPWin / 100 + majorJPWin / 100 + minorJPWin / 100 + miniJPWin / 100;

                    RequestSetPlayerCreditWhenMock(-1, 0, totaWin);


                    //PlayerPrefs.SetInt(TEST_JP_GRAND, 2000000);
                    //PlayerPrefs.SetInt(TEST_JP_MAJOR, 200000);
                    //PlayerPrefs.SetInt(TEST_JP_MINOR, 20000);
                    //PlayerPrefs.SetInt(TEST_JP_MINI, 2000);

                    int[] data = new int[] {
                        grandJPNow , majorJPNow, minorJPNow, miniJPNow,
                        winFlg,
                        grandJPWin,majorJPWin,minorJPWin,miniJPWin,
                        grandJPWhenWin,  majorJPWhenWin, minorJPWhenWin, miniJPWhenWin
                    };

                    SBoxGameJackpotData res = new SBoxGameJackpotData();
                    res.result = 0;
                    res.data = data;
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_GAME_JACKPOT, res);
                }
                return;
#endif

            case SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP, new int[] { 0, 333, 444 });
                }
                return;

            case SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN, new int[] { 0, 333, 444 });
                }
                return;

            case SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET:
#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    List<string> printerList = new List<string>()
                    {
                        "ICT:GP-58CR",
                        "PTI:Phoenix",
                        "ITHACA:Epic950"
                    };
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET, printerList);
                }
                return;


            case SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT: // 选择打印机

#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT, 0);
                }
                return;


            case SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET: // 打印机复位

#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET, 0);
                }
                return;



            case SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET: //获取纸钞机列表

#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    List<string> billerList = new List<string>()
                    {
                        "MEI:AE2831 D5",
                        "Pyramid:APEX 5000 SERIES",
                        "Pyramid:APEX 7000 SERIES",
                        "ICT:PA&/TAO",
                    };
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, billerList);
                }
                return;

            case SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT: // 选择纸钞机

#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, 0);
                }
                return;


            case RpcNameIsPrinterConnect: 

#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    OnResponseIsPrinterConnect(0);
                }
                return;


            case RpcNameIsBillerConnect:

#if TEST02
                requestFunc02(rpcName, req);
                return;
#endif
                if (!ApplicationSettings.Instance.isMock)
                {
                    requestFunc02(rpcName, req);
                }
                else
                {
                    OnResponseIsBillerConnect(0);
                }
                return;


        }
        return;
#endif
        requestFunc02(rpcName, req);
    }


    void requestFunc02(string rpcName, object req)
    {
        switch (rpcName)
        {
            case RpcNameScoreUp:
                {
                    int credit = (int)req;
                    List<SBoxPlayerCoinInfo> sBoxPlayerCoinInfos = new List<SBoxPlayerCoinInfo>();
                    sBoxPlayerCoinInfos.Add(new SBoxPlayerCoinInfo()
                    {
                        PlayerId = _consoleBB.Instance.pid,
                        ScoreUp = credit,
                    });
                    DebugUtils.Log($"@SBoxIdea.SetPlayerCoinInfo({JSONNodeUtil.ObjectToJsonStr(sBoxPlayerCoinInfos)})");
                    SBoxIdea.SetPlayerCoinInfo(sBoxPlayerCoinInfos);


                    // 自己的回调
                    OnResponseScoreUp(credit);
                }
                return;
            case RpcNameScoreDown:
                {
                    int credit = (int)req;
                    List<SBoxPlayerCoinInfo> sBoxPlayerCoinInfos = new List<SBoxPlayerCoinInfo>();
                    sBoxPlayerCoinInfos.Add(new SBoxPlayerCoinInfo()
                    {
                        PlayerId = _consoleBB.Instance.pid,
                        ScoreDown = credit,
                    });
                    SBoxIdea.SetPlayerCoinInfo(sBoxPlayerCoinInfos);

                    // 自己的回调
                    OnResponseScoreDown(credit);
                }
                return;
            case RpcNameCoinIn:
                {
                    int num = (int)req;
                    List<SBoxPlayerCoinInfo> sBoxPlayerCoinInfos = new List<SBoxPlayerCoinInfo>()
                        {
                            new SBoxPlayerCoinInfo()
                            {
                                PlayerId = _consoleBB.Instance.pid,
                                CoinIn = num,
                            }
                        };
                    SBoxIdea.SetPlayerCoinInfo(sBoxPlayerCoinInfos);

                    // 自己的回调
                    OnResponseCoinIn((int)req);
                }
                return;
            case RpcNameCoinOut:
                {
                    int num = (int)req;
                    List<SBoxPlayerCoinInfo> sBoxPlayerCoinInfos = new List<SBoxPlayerCoinInfo>()
                        {
                            new SBoxPlayerCoinInfo()
                            {
                                PlayerId = _consoleBB.Instance.pid,
                                CoinOut = num,
                            }
                        };
                    //Debug.LogWarning($"通知算法卡退票积分： {credit}");
                    SBoxIdea.SetPlayerCoinInfo(sBoxPlayerCoinInfos);


                    // 自己的回调
                    OnResponseCoinOut((int)req);
                }
                return;

            case SBoxEventHandle.SBOX_REQUEST_CODER:
                {
                    SBoxIdea.RequestCoder(0);
                }
                return;
            case SBoxEventHandle.SBOX_CODER: //请求打码
                {
                    ulong code = (ulong)req;
                    //SBoxIdea.Coder(0, ulong.Parse((string)res.value));
                    DebugUtils.Log($"@SBoxIdea.Coder({0},{code})");
                    SBoxIdea.Coder(0, code);
                }
                return;

            case RpcNameIsCodingActive: //是否激活
                {
                    int code = SBoxIdea.NeedActivated();
                    DebugUtils.Log($"@SBoxIdea.NeedActivated() =  {code}");
                    OnResponseIsCodingActive(code);
                }
                return;
            case SBoxEventHandle.SBOX_GET_ACCOUNT: // 获取玩家信息
                {
                    SBoxGodOfWealth.GetAccount();
                }
                return;
            case RpcNameSetPlayerInfo: // 设置玩家信息
                {
                    DebugUtils.LogError("【待实现】设置玩家信息");
                }
                return;
            case SBoxEventHandle.SBOX_READ_CONF:
                {
                    SBoxIdea.ReadConf();
                }
                return;
            case SBoxEventHandle.SBOX_WRITE_CONF: //写配置
                {
                    SBoxIdea.WriteConf((SBoxConfData)req);
                }
                return;
            case SBoxEventHandle.SBOX_SLOT_SPIN:
                {
                    SBoxGodOfWealth.SlotSpin(_consoleBB.Instance.pid, (int)req);
                }
                return;
            case SBoxEventHandle.SBOX_CHECK_PASSWORD:
                {
                    int password = (int)req;
                    SBoxIdea.CheckPassword(password);
                }
                return;
            case SBoxEventHandle.SBOX_CHANGE_PASSWORD:
                {
                    int password = (int)req;
                    SBoxIdea.ChangePassword(password);
                }
                return;

            case SBoxEventHandle.SBOX_SADNBOX_METER_SET:
                {
                    Dictionary<string, object> dic = (Dictionary<string, object>)req;
                    //码表设置
                    SBoxSandbox.MeterSet((int)dic["id"], (int)dic["counts"], (int)dic["type"]);
                }
                return;

            case SBoxEventHandle.SBOX_SET_PLAYER_BETS:
                {
                    SBoxIdea.SetPlayerBets((SBoxPlayerBetsData)req);
                }
                return;


            case SBoxEventHandle.SBOX_IDEA_VERSION:  //获取算法版本
                {
                    SBoxIdea.Version();
                }
                return;


            case SBoxEventHandle.SBOX_SANDBOX_VERSION:  //获取硬件版本
                {
                    SBoxSandbox.Version();
                }
                return;


          
            /*case SBoxEventHandle.SBOX_GAME_JACKPOT:  //游戏彩金
                {
                    SBoxGodOfWealth.GameJackpot();
                }
                return;*/

            case SBoxEventHandle.SBOX_CASH_SEQ_SCORE_UP:  // sas 的seq
                {
                    SBoxIdea.RequestCashSeq(1);
                }
                return;

            case SBoxEventHandle.SBOX_CASH_SEQ_SCORE_DOWN:
                {
                    SBoxIdea.RequestCashSeq(2);
                }
                return;

            case SBoxEventHandle.SBOX_SADNBOX_PRINTER_LIST_GET: // 可选打印机列表
                {
                    SBoxSandbox.PrinterListGet();
                }
                return;
            case SBoxEventHandle.SBOX_SADNBOX_PRINTER_SELECT: // 选择打印机
                {
                    SBoxSandbox.PrinterSelect((int)req);
                }
                return;



            case SBoxEventHandle.SBOX_SADNBOX_PRINTER_RESET: // 打印机复位
                {
                    SBoxSandbox.PrinterReset();
                }
                return;

            case SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET: //获取纸钞机列表
                {
                    SBoxSandbox.BillListGet();
                }
                return;

            case SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT:  // 选择纸钞机
                {
                    SBoxSandbox.BillSelect((int)req);
                }
                return;

            case RpcNameIsBillerConnect:  // 选择纸钞机
                {
                    OnResponseIsBillerConnect(SBoxSandbox.BillState());
                }
                return;

            case RpcNameIsPrinterConnect:  // 选择纸钞机
                {
                    OnResponseIsPrinterConnect(SBoxSandbox.PrinterState());
                }
                return;

        }
    }

    private void Update()
    {
        severHelper.Update();
    }
}

