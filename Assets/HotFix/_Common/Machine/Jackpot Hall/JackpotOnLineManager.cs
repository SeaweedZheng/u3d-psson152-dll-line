using GameMaker;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
public class JackpotOnLineManager : MonoSingleton<JackpotOnLineManager>
{

    MessageDelegates onPropertyChangedEventDelegates;

    SeverHelper severHelper;

    bool isJackpotHall = false;


    void Awake()
    {
        severHelper = new SeverHelper()
        {
            receiveOvertimeMS = 500,
            requestFunc = requestFunc,
        };

        EventCenter.Instance.AddEventListener<string>(RPCName.jackpotHall, OnJackpotOnLine);
        EventCenter.Instance.AddEventListener(GlobalEvent.ON_INIT_SETTINGS_FINISH_EVENT, OnInitSettingsFinishEvent);

        NetMessageController.Instance.Init();
    }

    /*
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        NetMessageController.Instance.Init();
        OnPropertyChangeIsJackpotOnline(null);
    }*/


    bool isInit = false;

    void OnInitSettingsFinishEvent()
    {
        if (isInit)
            return;
        isInit = true;

        onPropertyChangedEventDelegates = new MessageDelegates
        (
            new Dictionary<string, EventDelegate>
            {
            { "@console/isJackpotOnLine", OnPropertyChangeIsJackpotOnLine},
            }
        );
        EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        OnPropertyChangeIsJackpotOnLine(null);
    }
   


    protected override void  OnDestroy()
    {
        EventCenter.Instance?.RemoveEventListener<string>(RPCName.jackpotHall, OnJackpotOnLine);
        EventCenter.Instance?.RemoveEventListener(GlobalEvent.ON_INIT_SETTINGS_FINISH_EVENT, OnInitSettingsFinishEvent);
        EventCenter.Instance?.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
        base.OnDestroy();
    }

    protected void OnJackpotOnLine(string res)
    {
        severHelper.OnSuccessResponseData(RPCName.jackpotHall, res);
    }

    void requestFunc(string rpcName, object req)
    {

        if (RPCName.jackpotHall == rpcName)
        {
            if (!isJackpotHall)
            {
                severHelper.OnErrorResponseData(RPCName.jackpotHall,"jackpot hall is diable");
                return;
            }

            /*JackBetInfo betInfo = new JackBetInfo
            {
                seat = 1,
                bet = 40000,
                betPercent = 100,
                scoreRate = 10000,
                JPPercent = 5
            };*/
            MsgInfo msgInfo = new MsgInfo
            {
                cmd = (int)C2S_CMD.C2S_JackBet,
                id = int.Parse(_consoleBB.Instance.machineID), //1110001 
                jsonData = JsonConvert.SerializeObject(req as JackBetInfo)
            };
            NetMgr.Instance.SendToServer(JsonConvert.SerializeObject(msgInfo));
        }
        else
        {
            severHelper.OnErrorResponseData(rpcName, $"{rpcName} Invoke is not define");
            return;
        }
    }

    public int RequestsJackpotOnLineData(JackBetInfo req, Action<object> responseCallback, Action<object> errorCallback) =>
        severHelper.RequestData(RPCName.jackpotHall, req, responseCallback, errorCallback);
    public void RemoveRequestAt(int seqID)=> severHelper.RemoveRequestAt(seqID);

    private void Update()
    {
        severHelper.Update();
    }



    void OnPropertyChangeIsJackpotOnLine(EventData res)
    {

        if (res == null)
            isJackpotHall = _consoleBB.Instance.isJackpotOnLine;
        else
            isJackpotHall = ((int)res.value) == 1;

        if (isJackpotHall)
        {
            NetMgr.Instance.SetNetAutoConnect(false);
            DebugUtils.Log("连接大厅彩金");
        }
        else
        {            
            ClientWS.Instance.CloseSocket();
            DebugUtils.Log("关闭大厅彩金");
        }
    }
   
    public void CheckJackpot()
    {

    }
}
public class SBoxJackpotBet
{
    public int MachineId;                       // 机台号
    public int SeatId;                          // 分机号/座位号
    public int Bet;                             // 当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用
    public int BetPercent;                      // 押分比例，目前拉霸默认值传1，同样需要乘以100
    public int ScoreRate;                       // 分值比，1分多少钱，需要乘以1000再往下传
    public int JpPercent;                       // 分机彩金百分比，每次押分贡献给彩金的比例。需要乘以1000再往下传
}

