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
            DebugUtils.Log("���Ӵ����ʽ�");
        }
        else
        {            
            ClientWS.Instance.CloseSocket();
            DebugUtils.Log("�رմ����ʽ�");
        }
    }
   
    public void CheckJackpot()
    {

    }
}
public class SBoxJackpotBet
{
    public int MachineId;                       // ��̨��
    public int SeatId;                          // �ֻ���/��λ��
    public int Bet;                             // ��ǰ��Ѻ��,Ϊ�˱��ⶪʧС������Ҫ����100��Ӳ����ȡ���ֵ�����100��ʹ��
    public int BetPercent;                      // Ѻ�ֱ�����Ŀǰ����Ĭ��ֵ��1��ͬ����Ҫ����100
    public int ScoreRate;                       // ��ֵ�ȣ�1�ֶ���Ǯ����Ҫ����1000�����´�
    public int JpPercent;                       // �ֻ��ʽ�ٷֱȣ�ÿ��Ѻ�ֹ��׸��ʽ�ı�������Ҫ����1000�����´�
}

