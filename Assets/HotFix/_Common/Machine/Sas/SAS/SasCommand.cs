using CryPrinter;
using cryRedis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/*public class SasCommand 
{
    /*static SasCommand instance;
    public static SasCommand Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SasCommand();
            }
            return instance;
        }
}*/
public class SasCommand :MonoSingleton<SasCommand> 
{



    /*
     * Send General Poll Exception codes
     */
    public long PushGeneralPoll(SASMessage msg)
    {

        string data = JsonConvert.SerializeObject(msg);
        long ret = cryRedis.RedisMgr.Instance.PublishNoneMessageId(SasConstant.SAS_EXCEPTION, data);
        Debug.LogWarning($"【SasRedis】<color=green>rpc up</color> Publish General; data:{data} result: {ret}");

        return ret;
    }

    //meter 在redis的值，例如47 Q 7-15 $1.00 bill accepted (non-RTE only)，在redis里key为47，value为1
    public bool SetMeter(string code, string message)
    {
        if (!RedisMgr.Instance.isConnectRedis)
            return false;
        
        bool ret = cryRedis.RedisMgr.Instance.Set(code, message);
        Debug.LogWarning($"【SasRedis】<color=green>rpc up</color> SetMeter; code:{code}  message:{message}  result: {ret}");

        return ret;
    }




    /// <summary>
    /// 自己的彩金值
    /// </summary>
    /// <param name="myCrdit"></param>
    /// <returns></returns>
    public bool SetMeterMyCredit(int myCrdit)
    {
        string code = string.Format("{0}", SasConstant.CURRENT_CREDITS);
        string message1 = string.Format("{0}", myCrdit);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 历史总投币
    /// </summary>
    /// <param name="totalCoinIn"></param>
    /// <returns></returns>
    public bool SetMeterTotalCoinIn(int totalCoinIn)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_COIN_IN_CREDITS);
        string message1 = string.Format("{0}", totalCoinIn);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 代表机器输出的硬币总积分
    /// </summary>
    /// <param name="totalCoinOut"></param>
    /// <returns></returns>
    public bool SetMeterTotalCoinOut(int totalCoinOut)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_COIN_OUT_CREDITS);
        string message1 = string.Format("{0}", totalCoinOut);
        return SasCommand.Instance.SetMeter(code, message1);
    }




    /// <summary>
    /// 累积的头奖积分
    /// </summary>
    /// <param name="totalJcakpotCredits"></param>
    /// <returns></returns>
    public bool SetMeterTotalJcakpotCredits(int totalJcakpotCredits)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_JACKPOT_CREDITS);
        string message1 = string.Format("{0}", totalJcakpotCredits);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 已玩的游戏次数
    /// </summary>
    /// <param name="totalGamePlayed"></param>
    /// <returns></returns>
    public bool SetMeterTotalGamePlayed(int totalGamePlayed)
    {
        string code = string.Format("{0}", SasConstant.GAMES_PLAYED);
        string message1 = string.Format("{0}", totalGamePlayed);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 代表手动支付的总积分（手动支付已取消积分和头奖积分总和）
    /// </summary>
    /// <param name="totalHeadPaidCredits"></param>
    /// <returns></returns>
    public bool SetMeterTotalHeadPaidCredits(int totalHeadPaidCredits)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_HAND_PAID_CREDITS);
        string message1 = string.Format("{0}", totalHeadPaidCredits);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 代表包含可兑现、非受限及受限票券在内的总输入票券积分
    /// </summary>
    /// <param name="totalTicketInCredits"></param>
    /// <returns></returns>
    public bool SetMeterTotalTicketInCredits(int totalTicketInCredits)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_TICKET_IN_CREDITS);
        string message1 = string.Format("{0}", totalTicketInCredits);
        return SasCommand.Instance.SetMeter(code, message1);
    }





    /// <summary>
    /// 代表包含可兑现、非受限、受限及借记票券在内的总输出票券积分
    /// </summary>
    /// <param name="totalTicketOutCredits"></param>
    /// <returns></returns>
    public bool SetMeterTotalTicketOutCredits(int totalTicketOutCredits)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_TICKET_OUT_CREDITS);
        string message1 = string.Format("{0}", totalTicketOutCredits);
        return SasCommand.Instance.SetMeter(code, message1);
    }



    /// <summary>
    /// 代表向游戏机进行的电子转账总积分，包括各种类型且不论转账到积分表还是票券
    /// </summary>
    /// <param name="totalElectronicTransfers"></param>
    /// <returns></returns>
    public bool SetMeterTotalElectronicTransfersToMachine(int totalElectronicTransfers)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_ELECTRONIC_TRANSFERS_TO_GAMING_MACHINE_CREDITS);
        string message1 = string.Format("{0}", totalElectronicTransfers);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 代表向主机进行的电子转账总积分，包含各种类型及中奖金额等情况
    /// </summary>
    /// <param name="totalElectronicTransfersCredits"></param>
    /// <returns></returns>
    public bool SetMeterTotalElectronicTransfersToHost(int totalElectronicTransfersCredits)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_ELECTRONIC_TRANSFERS_TO_HOST_CREDITS);
        string message1 = string.Format("{0}", totalElectronicTransfersCredits);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 代表已接受纸币对应的总积分
    /// </summary>
    /// <param name="totalCreditsFromBills"></param>
    /// <returns></returns>
    public bool SetMeterTotalCreditsFromBills(int totalCreditsFromBills)
    {
        string code = string.Format("{0}", SasConstant.TOTAL_CREDITS_FROM_BILLS_ACCEPTED);
        string message1 = string.Format("{0}", totalCreditsFromBills);
        return SasCommand.Instance.SetMeter(code, message1);
    }


    /// <summary>
    /// 普通进钞机，数据统计
    /// </summary>
    /// <param name="type"></param>
    /// <param name="cent">多少分</param>
    /// <param name="seq"></param>
    /// <returns></returns>
    public bool SetMeterBillInCash(int value, long seq, int centMultiple = 100)  => SetMeterBillIn(BILL_IN_CASH, value, seq, centMultiple);


    public bool SetMeterBillInTicket(int value, long seq, int centMultiple = 100) => SetMeterBillIn(BILL_IN_TICKET, value, seq, centMultiple);


    bool SetMeterBillIn(int billInType, int value, long seq, int centMultiple = 100)
    {

        /*if (!mapCash.ContainsKey(value))
        {
            Debug.LogError($"{value} is no support");
            return false;
        }*/

        int cashValue = value * centMultiple;

        string code = string.Format("{0}", 0x48);

        JObject dat = new JObject();
        dat.Add("cashValue", cashValue);
        dat.Add("index", seq);

        //JObject req = new JObject();
        //req.Add("eventId", billInType);
        //req.Add("data", dat);

        return SasCommand.Instance.SetMeter(code, dat.ToString());

    }




    const int BILL_IN_CASH = 1;
    const int BILL_IN_TICKET = 2;


    Dictionary<int, int> mapCash = new Dictionary<int, int>()
    {
        [1] = SasConstant.BILL_ACCEPTED_1_DOLLAR,
        [2] = SasConstant.BILL_ACCEPTED_2_DOLLARS,
        [5] = SasConstant.BILL_ACCEPTED_5_DOLLARS,
        [10] = SasConstant.BILL_ACCEPTED_10_DOLLARS,
        [20] = SasConstant.BILL_ACCEPTED_20_DOLLARS,
        [50] = SasConstant.BILL_ACCEPTED_50_DOLLARS,
        [100] = SasConstant.BILL_ACCEPTED_100_DOLLARS,
        [200] = SasConstant.BILL_ACCEPTED_200_DOLLARS,
        [500] = SasConstant.BILL_ACCEPTED_500_DOLLARS,
    };





    /// <summary>
    /// 进票
    /// </summary>
    /// <param name="totalCoinOut"></param>
    /// <returns></returns>
    public bool SetMeterTicketInserted(string ticketNo)
    {
        Debug.Log($"TicketInserted - ticketNo: {ticketNo}");

        JObject dat = new JObject();
        dat.Add("ticketNo", ticketNo);
        JObject req01 = new JObject();
        req01.Add("TikcetRequest", dat);

        string code = string.Format("{0}", SasConstant.TICKET_INSERTED);



        return SasCommand.Instance.SetMeter(code, req01.ToString()); /**/

    }


    public class TicketVo
    {
        /**
            00 Valid cashable ticket
            01 Valid restricted promotional ticket
            02 Valid nonrestricted promotional ticket
            80 Unable to validate (no reason given / other)
            81 Not a valid validation number
            82 Validation number not in system
            83 Ticket marked pending in system
            84 Ticket already redeemed
            85 Ticket expired
            86 Validation information not available
            87 Ticket amount does not match system amount
            88 Ticket amount exceeds auto redemption limit
         */
        private string status;
        /**
         * 只要当status =0 ,这个才有值，以分为单位
         */
        private long money;
    }












    int moneyOut = 999;


    /// <summary>
    /// 代表已接受纸币对应的总积分
    /// </summary>
    /// <param name="totalCreditsFromBills"></param>
    /// <remarks>
    /// * 给sas-redis发生要出的金额<br/>
    /// * 监听eventId=4,获取票号
    /// </remarks>
    /// <returns></returns>
    public long PushGeneralScoreDown(int money) // money: 美分
    {
        moneyOut = money;

        //string code = string.Format("{0}", SasConstant.TOTAL_CREDITS_FROM_BILLS_ACCEPTED);
        //string message1 = string.Format("{0}", totalCreditsFromBills);

        //JObject message = new JObject();
        //message.Add("Amount", cashValue);
        //message.Add("Index", seq);

        //JObject MessageRequest = new JObject();
        //MessageRequest.Add("messageCode", "TICKETOUT");
        //MessageRequest.Add("message", $"{money}");
        //
        //JObject data = new JObject();
        //data.Add("MessageRequest", MessageRequest);

        SASMessage req =  new SASMessage();
        req.eventId = 2;
        req.data = $"{money}";

        return SasCommand.Instance.PushGeneralPoll(req);
    }


    /// <summary>
    /// 上报sas出票机结果
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public long PushGeneralScoreDownResualt(string orderId) 
    {
        SASMessage req = new SASMessage();
        req.eventId = 5;
        req.data = orderId;
        return SasCommand.Instance.PushGeneralPoll(req);
    }


    /// <summary>
    /// 通知sas-redis 进钞结果
    /// </summary>
    /// <remarks>
    /// * "1，票号"<br/>
    /// * 如："0,5675765761236575645" 表示票没进; "1,5675765761236575645" 则表示票进去了
    /// </remarks>
    /// <param name="isSuccess"></param>
    /// <param name="orderId">票号</param>
    /// <returns></returns>
    public long PushGeneralSasBillerInApproveResualt(int isSuccess, string orderId) //  成功 "1,票号" 如："0,票号"
    {
        SASMessage req = new SASMessage();
        req.eventId = 6;
        req.data = $"{isSuccess},{orderId}";
        return SasCommand.Instance.PushGeneralPoll(req);
    }



    /*
    public void rpcDownHandler(int eventId , string data)
    {

        Debug.LogError($"@ 1===  {eventId} , {data} ");
        if (eventId == 4 )  //出票下分
        {
            Debug.LogError("i am here 4");
            //Debug.LogError("【redis rpc down】 4 == " + JsonConvert.SerializeObject(res));

            //得到票号   data :票号
            //JSONNode dat = JSONNode.Parse(res.Body);

            string _orderId = data;
            double _moneyOut = moneyOut;
            task = () =>
            {
                Debug.Log("rpcDownHandler--->_orderId  + " + _orderId); //$"{_orderId}"
                MachineDeviceCommonBiz.Instance.PrinterJCM950(_orderId.ToString(), _moneyOut, () =>
                {
                    // 接受钞票

                    Debug.LogError($"@ 4===  {eventId} , {data}  moneyOut: {moneyOut}");
                    PushGeneralScoreDownResualt(_orderId);

                    //算法卡走下分
                    int credit = (int)_moneyOut;

                    MachineDeviceCommonBiz.Instance.RequestScoreDown(credit);
                });
            };
        }
        else if (eventId == 3)  // 入票上分
        {
            Debug.LogError("【redis rpc down】  3 == " + data);

            task = () =>
            {
                string[] res01 = data.Split(',');
                if (res01[0] == "0")
                {
                    int money = int.Parse(res01[1]);
                    MachineDeviceCommonBiz.Instance.SasBillerApprove(money);
                }
                else
                {
                    MachineDeviceCommonBiz.Instance.SasRejectTicketIn();
                }
            };
        }
    }

    Action task = null;
    private void Update()
    { 
        if(task != null)
        {
            task.Invoke();
            task = null;
        }
    }
*/


    /// <summary>
    /// sas纸钞机入票-通知redis
    /// </summary>
    /// <remarks>
    /// * 监听通道3，获取"结果，金额" (如："0，12345" 表示合法票，金额是123.45 。如果第一位不是0，就非法票，不用上分)<br/>
    /// * 发送id=6，告知redis是否接收票<br/>
    /// </remarks>
    /// <param name="ticketInOrderId">纸钞机，输入的票号</param>
    /// <returns></returns>
    public long PushGeneralBillerTicketIn(string ticketInOrderId) // money: 美分
    {
        SASMessage req = new SASMessage();
        req.eventId = 1;
        req.data = ticketInOrderId;
        return SasCommand.Instance.PushGeneralPoll(req);
    }


    /// <summary>
    /// sas入钞-上分（最终：走算法卡投币接口给游戏上分）
    /// </summary>
    /// <param name="money">美元</param>
    /// <param name="cashSeq"></param>
    /// <returns></returns>
    public long PushGeneralBillInDetails(int money,long cashSeq, int centMultiple = 100) 
    {
        SASMessage req = new SASMessage();
        req.eventId = 0;
        req.data =  $"{money* centMultiple}, {cashSeq}";  // "分，seqId"  (这里要传入美分)
        return SasCommand.Instance.PushGeneralPoll(req);
    }





    /// <summary>
    /// 游戏开始
    /// </summary>
    /// <returns></returns>
    public bool SetMeterGameStart()
    {
        string code = string.Format("{0}", SasConstant.GAME_STARTED);
        //string message1 = string.Format("{0}", totalJcakpotCredits);
        return SasCommand.Instance.SetMeter(code, null);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <returns></returns>
    public bool SetMeterGameEnd()
    {
        string code = string.Format("{0}", SasConstant.GAME_ENDED);
        //string message1 = string.Format("{0}", totalJcakpotCredits);
        return SasCommand.Instance.SetMeter(code, null);
    }


}
