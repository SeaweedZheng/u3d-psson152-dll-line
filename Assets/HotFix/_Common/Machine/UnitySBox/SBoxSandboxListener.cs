using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static SBoxApi.SBoxSandbox;
using static SBoxSandboxListener;

public class SBoxSanboxEventHandle
{
    public const string COIN_IN = "COIN_IN";
    public const string COIN_OUT = "COIN_OUT";
    public const string COIN_OUT_TIMEOUT = "COIN_OUT_TIMEOUT";
    public const string BILL_IN = "BILL_IN";
    public const string BILL_STACKED = "BILL_STACKED";

    //����Ʊ
    public const string TICKET_IN = "TICKET_IN";
}
public class SwitchClass
{
    public bool triggerPointerDown;
    public bool startPress;
    public bool longPressTrigger;
    public float curPointDownTime;
    public ButtonPointerDownEvent onPointerDown = new ButtonPointerDownEvent();
    public ButtonPointerUpEvent onPointerUp = new ButtonPointerUpEvent();
    public ButtonClickEvent onClick = new ButtonClickEvent();
    public ButtonLongPressEvent onLongPress = new ButtonLongPressEvent();
}
public class SBoxSandboxListener : MonoSingleton<SBoxSandboxListener>
{
    private bool isInit;

    private const float longPressTime = 3f;

    private readonly Dictionary<ulong, SwitchClass> switchClassDic = new Dictionary<ulong, SwitchClass>();

    public void Init()
    {
        if (isInit)
            return;
        Type t = typeof(SBOX_SWITCH);
        var fields = t.GetFields();
        foreach (var fieldInfo in fields)
            switchClassDic.Add((ulong)fieldInfo.GetRawConstantValue(), new SwitchClass());
        isInit = true;
    }

    private void Update()
    {
        CheckNumberOfCointIn();
        CheckNumberOfCoinOut();
        CheckCoinOutTimeOut();
        CheckBillIn();
        CheckTicketIn();
        CheckBillStacked();
        CheckButtonState();
    }

    private void CheckNumberOfCointIn()
    {
        for (int i = 0; i < 4; i++)
        {
            int data = NumberOfCoinIn(i);
            if (data > 0)
            {
                CoinInData coinData = new CoinInData
                {
                    id = i,
                    coinNum = data,
                };
                EventCenter.Instance.EventTrigger(SBoxSanboxEventHandle.COIN_IN, coinData);
            }
        }
    }

    private void CheckBillIn()
    {
        int data = BillCredit();
        if (data > 0)
            EventCenter.Instance.EventTrigger(SBoxSanboxEventHandle.BILL_IN, data);
    }

    private void CheckTicketIn()
    {
        byte[] data = BillBarCodeData();
        if (data != null && data.Length > 0)
        {
            //Debug.LogError("SBoxSandboxListener.CheckTicketIn==========>" + String.Join("-", data));
            //��ʱ���� type = 0x01�����
            //int dlen = (int)data[0];
            List<int> intLst = new List<int>();
            if (data[1] == 0x01)
            {
                for (int i = 4; i < (data.Length - 4); i++)
                {
                    if (((data[i] >> 4) & 0x0F) == 3)
                    {
                        int inval = data[i] & 0x0F;
                        intLst.Add(inval);
                    }
                }
            }
            if (intLst.Count > 0)
            {
                string ticketNo = String.Join("", intLst);
                EventCenter.Instance.EventTrigger(SBoxSanboxEventHandle.TICKET_IN, ticketNo);

            }

        }
           
    }

    private void CheckBillStacked()
    {
        bool data = IsBillStacked();
        if (data)
            EventCenter.Instance.EventTrigger(SBoxSanboxEventHandle.BILL_STACKED);
    }

    private void CheckNumberOfCoinOut()
    {
        for (int i = 0; i < 2; i++)
        {
            int data = NumberOfCoinOut(i);
            if (data > 0)
                EventCenter.Instance.EventTrigger(SBoxSanboxEventHandle.COIN_OUT, data);
        }
    }

    private void CheckCoinOutTimeOut()
    {
        for (int i = 0; i < 2; i++)
            if (IsCoinOutTimeout(i))
                EventCenter.Instance.EventTrigger(SBoxSanboxEventHandle.COIN_OUT_TIMEOUT, i);
    }

    private void CheckButtonState()
    {
        ulong data = SwitchInState();
        foreach (ulong switchKey in switchClassDic.Keys)
        {
            if (!switchClassDic[switchKey].startPress && (data & (ulong)switchKey) == (ulong)switchKey)
            {
                switchClassDic[switchKey].curPointDownTime = Time.time;
                switchClassDic[switchKey].startPress = true;
                switchClassDic[switchKey].longPressTrigger = false;
                if (!switchClassDic[switchKey].triggerPointerDown)
                {
                    switchClassDic[switchKey]?.onPointerDown.Invoke();
                    switchClassDic[switchKey].triggerPointerDown = true;
                }
            }

            if (switchClassDic[switchKey].startPress && (data & (ulong)switchKey) == 0)
            {
                switchClassDic[switchKey].startPress = false;
                switchClassDic[switchKey]?.onPointerUp.Invoke();
                switchClassDic[switchKey].triggerPointerDown = false;
                CheckClick(switchKey);
            }
        }
        CheckIsLongPress();
    }

    private void CheckIsLongPress()
    {
        foreach (ulong switchKey in switchClassDic.Keys)
        {
            if (switchClassDic[switchKey].startPress && !switchClassDic[switchKey].longPressTrigger)
            {
                if (Time.time > switchClassDic[switchKey].curPointDownTime + longPressTime)
                {
                    switchClassDic[switchKey].longPressTrigger = true;
                    switchClassDic[switchKey].onLongPress?.Invoke();
                }
            }
        }
    }

    private void CheckClick(ulong switchKey)
    {
        if (!switchClassDic[switchKey].longPressTrigger
            && Time.time <= switchClassDic[switchKey].curPointDownTime + longPressTime)
        {
            Debug.Log("Click: " + switchKey);
            switchClassDic[switchKey].onClick?.Invoke();
        }
    }

    public void AddButtonDown(ulong sboxSwtich, UnityAction unityAction)
    {
        switchClassDic[sboxSwtich].onPointerDown.AddListener(unityAction);

    }

    public void AddButtonUp(ulong sboxSwtich, UnityAction unityAction)
    {
        switchClassDic[sboxSwtich].onPointerUp.AddListener(unityAction);
    }
    public void AddButtonClick(ulong sboxSwtich, UnityAction unityAction)
    {
        switchClassDic[sboxSwtich].onClick.AddListener(unityAction);
    }

    public void AddButtonLongPress(ulong sboxSwtich, UnityAction unityAction)
    {
        switchClassDic[sboxSwtich].onLongPress.AddListener(unityAction);
    }

    public class ButtonPointerDownEvent : UnityEvent { }

    public class ButtonPointerUpEvent : UnityEvent { }

    public class ButtonClickEvent : UnityEvent { }

    public class ButtonLongPressEvent : UnityEvent { }
}
