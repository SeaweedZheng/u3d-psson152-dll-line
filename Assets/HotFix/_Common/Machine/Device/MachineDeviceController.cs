using GameMaker;
using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static SBoxApi.SBoxSandbox;
using Game;
using System.Linq;
using System.Diagnostics;


/***
 * 【按钮逻辑】：
 * 如果后台界面没有打开则，且收到后台打开弹窗按钮事件，则打开弹窗。
 * 
 * 后台打开弹窗后，如果收到游戏通用弹窗、后台通用弹窗。
 * 
 * 后台打开后，如果有优先使用按钮要求，则先响应优先要求。
 * 
 * 如果没有优先响应要求，则进入通用按钮事件处理。+ 普通界面按钮要求。
 */
public class MachineButtonInfo
{
    public MachineButtonKey btnKey;
    public bool isUp;
    public int value;
}
public enum MachineButtonKey
{

    BtnLight,

    //////游戏按钮//////
    BtnSpin,
    BtnPre,
    BtnNext,
    BtnExit,
    BtnSwitch,
    BtnBetUp,
    BtnBetDown,
    BtnBetMax,
    BtnHelp,

    //////功能按钮//////
    BtnTicketOut,
    /// <summary> 上分 </summary>
    BtnCreditUp,
    /// <summary> 下分 </summary>
    BtnCreditDown,

    /// <summary> 管理后台 </summary>
    BtnConsole,

    /// <summary> 门开关 </summary>
    BtnDoor,


    /// <summary> 打印 (没有机台按钮)</summary>
    // BtnPrinterOut,
}
public class MachineDeviceController : MonoSingleton<MachineDeviceController>  // 
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


    protected void ClearCor(string name) => corCtrl.ClearCor(name);
    protected void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);


    public readonly Dictionary<ulong, MachineButtonKey> keyMap = new Dictionary<ulong, MachineButtonKey>()
    {


        //游戏按钮：
        { SBOX_SWITCH.SWITCH_ENTER ,MachineButtonKey.BtnSpin},
        //{ SBOX_SWITCH.SWITCH_YELLOW, MachineButtonKey.BtnPre},
        //{ SBOX_SWITCH.SWITCH_BET4, MachineButtonKey.BtnNext},
        { SBOX_SWITCH.SWITCH_SWITCH, MachineButtonKey.BtnExit},
        //{ SBOX_SWITCH.SWITCH_BET5, MachineButtonKey.BtnSwitch},
       // { SBOX_SWITCH.SWITCH_RED, MachineButtonKey.BtnBetUp},
       // { SBOX_SWITCH.SWITCH_GREEN, MachineButtonKey.BtnBetDown},
       // { SBOX_SWITCH.SWITCH_AUTO,MachineButtonKey.BtnBetMax},
        { SBOX_SWITCH.SWITCH_ESC,MachineButtonKey.BtnHelp},

         //管理按钮：
        { SBOX_SWITCH.SWITCH_PAYOUT ,MachineButtonKey.BtnTicketOut},
        { SBOX_SWITCH.SWITCH_SCORE_UP ,MachineButtonKey.BtnCreditUp},
        { SBOX_SWITCH.SWITCH_SCORE_DOWN ,MachineButtonKey.BtnCreditDown},
        { SBOX_SWITCH.SWITCH_SET ,MachineButtonKey.BtnConsole},
        { SBOX_SWITCH.SWITCH_DOOR_SWITCH ,MachineButtonKey.BtnDoor}
    };

    public ulong GetButtonValue(MachineButtonKey btnState)
    {
        for (int i = 0; i < keyMap.Count; i++)
        {
            KeyValuePair<ulong, MachineButtonKey> kv = keyMap.ElementAt(i);
            if (kv.Value == btnState)
            {
                return kv.Key;
            }

        }
        return 0;
    }


    readonly List<MachineButtonKey> defaultLightBtn = new List<MachineButtonKey>() {
        //上一排（从左到右）
        MachineButtonKey.BtnBetUp,
        MachineButtonKey.BtnBetDown,
        MachineButtonKey.BtnPre,
        MachineButtonKey.BtnNext,
        //下一排（从左到右）
        MachineButtonKey.BtnBetMax,
        MachineButtonKey.BtnHelp,
        MachineButtonKey.BtnExit,
        MachineButtonKey.BtnSwitch,
    };



    public DeviceBillIn deviceBillIn;
    public DeviceCoinIn deviceCoinIn;
    public DevicePrinterOut devicePrinterOut;


    public const string MACHINE_BUTTON_EVENT = "MACHINE_BUTTON_EVENT";
    public const string LightButton = "LightButton";
    const string COR_BTN_LIGHT = "COR_BTN_LIGHT";
    const string COR_CREDIT_UP_LONG_CLICK = "COR_CREDIT_UP_LONG_CLICK";
    const string COR_CREDIT_DOWN_LONG_CLICK = "COR_CREDIT_DOWN_LONG_CLICK";

    MachineCustomButton curBtnInfo;


    //SoundHelper soundHelper;
    private void Start()
    {

        /*
                Type type = typeof(SBOX_SWITCH);
                FieldInfo[] staticFields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo field in staticFields)
                    DebugUtil.Log($"@@@ Static readonly field: {field.Name}, Type: {field.FieldType}, Value: {field.GetValue(null)}");
        */



        if (!ApplicationSettings.Instance.IsMachine())   return;

        //if (!ApplicationSettings.Instance.isMachine) return;

        if (Application.isEditor)
        {
            EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_DOWN, OnKeyDown);
            EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_UP, OnKeyUp);
        }
        else
        {
            foreach (KeyValuePair<ulong, MachineButtonKey> kv in keyMap)
            {
                ulong key = kv.Key;
                SBoxSandboxListener.Instance.AddButtonDown(key, () => { OnKeyDown(key); });
                SBoxSandboxListener.Instance.AddButtonUp(key, () => { OnKeyUp(key); });
            }
        }
    }

    private void OnEnable()
    {

        AddNetButtonHandle();
        AddNetCmdHandle();

        if (!ApplicationSettings.Instance.IsMachine()) return;

        //if (!ApplicationSettings.Instance.isMachine) return;

        EventCenter.Instance.AddEventListener<EventData>(MachineCustomButton.MACHINE_CUSTOM_BUTTON_FOCUS_EVENT, OnEventMachineCustomButton);
    }

    private void OnDisable()
    {
        RemoveNetButtonHandle();
        RemoveNetCmdHandle();

        EventCenter.Instance.RemoveEventListener<EventData>(MachineCustomButton.MACHINE_CUSTOM_BUTTON_FOCUS_EVENT, OnEventMachineCustomButton);
    }


    public void DoPrinterOut() => devicePrinterOut.DoPrinterOut();



    Dictionary<MachineButtonKey, float> longClickTime = new Dictionary<MachineButtonKey, float>();

    private void OnKeyDown(ulong key)
    {
        MachineButtonKey value = keyMap[key];
        string keyName = Enum.GetName(typeof(MachineButtonKey), value);
#if UNITY_EDITOR
        DebugUtils.LogWarning($"【machine】KeyDown Value = {key} ;  KeyDown Name = {keyName};");
#endif
        OnKeyDown(value);
    }

    private void OnKeyDown(MachineButtonKey value)
    {
        /*
                Type type = typeof(SBOX_SWITCH);
                FieldInfo[] staticFields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo field in staticFields)
                    DebugUtil.Log($"@@@ Static readonly field: {field.Name}, Type: {field.FieldType}, Value: {field.GetValue(null)}");
        */  

/*
        MachineButtonKey value = keyMap[key];
        string keyName = Enum.GetName(typeof(MachineButtonKey), value);
#if UNITY_EDITOR
        DebugUtil.LogWarning($"【machine】KeyDown Value = {key} ;  KeyDown Name = {keyName};");
#endif
   */
        //按钮时间记录
        if (!longClickTime.ContainsKey(value))
            longClickTime.Add(value, Time.unscaledTime);
        else
            longClickTime[value] = Time.unscaledTime;



        if (IsSysPriority(value))
        {
            return;
        }

        if (curBtnInfo == null || !curBtnInfo.isPriority)
            switch (value)
            {
                /*case MachineButtonKey.BtnSpin: 
                    break;
                case MachineButtonKey.BtnPre: 
                    break;
                case MachineButtonKey.BtnNext: 
                    break;
                case MachineButtonKey.BtnExit: 
                    break;
                case MachineButtonKey.BtnSwitch: 
                    break;
                case MachineButtonKey.BtnBetUp: 
                    break;
                case MachineButtonKey.BtnBetDown: 
                    break;
                case MachineButtonKey.BtnBetMax: 
                    break;
                case MachineButtonKey.BtnHelp: 
                    break;*/
                case MachineButtonKey.BtnDoor:
                    return;
                case MachineButtonKey.BtnConsole:
                    return;
                case MachineButtonKey.BtnCreditUp:
                    DoCor(COR_CREDIT_UP_LONG_CLICK, DoCreditUpLongClick());
                    return;
                case MachineButtonKey.BtnCreditDown:
                    DoCor(COR_CREDIT_DOWN_LONG_CLICK, DoCreditDownLongClick());
                    return;
                case MachineButtonKey.BtnTicketOut:
                    return;

            }

        if (curBtnInfo != null)// && curBtnInfo.isShowBtn)
        {
            if (curBtnInfo.btnType == MachineButtonType.Light)
            {
                List<MachineButtonKey> lst = GetLightBtnLst();
                if (lst.Contains(value))
                {
                    EventCenter.Instance.EventTrigger<EventData>(MACHINE_BUTTON_EVENT,
                        new EventData<MachineButtonInfo>(
                            curBtnInfo.mark,
                            new MachineButtonInfo()
                            {
                                isUp = false,
                                btnKey = MachineButtonKey.BtnLight,
                                value = lst.IndexOf(value),
                            }
                        ));
                }
            }
            else
            {
                EventCenter.Instance.EventTrigger<EventData>(MACHINE_BUTTON_EVENT,
                    new EventData<MachineButtonInfo>(
                        curBtnInfo.mark,
                        new MachineButtonInfo()
                        {
                            isUp = false,
                            btnKey = value, //$"{keyName}_Down",
                        }
                    ));
            }
        }
    }


    bool IsSysPriority(MachineButtonKey value)
    {
        if (value == MachineButtonKey.BtnConsole && PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1)
        {
            return true;
        }

        return false;
    }


    private void OnKeyUp(ulong key)
    {
        MachineButtonKey value = keyMap[key];
        string keyName = Enum.GetName(typeof(MachineButtonKey), value);
#if UNITY_EDITOR
        DebugUtils.LogWarning($"【machine】KeyUp Value = {key} ;  KeyDown Name = {keyName};");
#endif
        OnKeyUp(value);
    }

    private void OnKeyUp(MachineButtonKey value)
    {
/*
#if UNITY_EDITOR
        DebugUtil.LogWarning("【machine】KeyUp value = " + key);
#endif

        MachineButtonKey value = keyMap[key];
        string keyName = Enum.GetName(typeof(MachineButtonKey), value);
#if UNITY_EDITOR
        DebugUtil.LogWarning("【machine】KeyUp " + keyName);
#endif
*/
        if (IsSysPriority(value))
        {

            //应用被保护中
            // if (GlobalData.isProtectApplication) return;


            switch (value)
            {
                case MachineButtonKey.BtnConsole:
                    {
                        MachineDeviceCommonBiz.Instance.OpenConsole();
                    }
                    return;
            }
            return;
        }


        if (curBtnInfo == null || !curBtnInfo.isPriority)
            switch (value)
            {
                /*
                case MachineButtonKey.BtnSpin:
                    break;
                case MachineButtonKey.BtnPre:
                    break;
                case MachineButtonKey.BtnNext:
                    break;
                case MachineButtonKey.BtnExit:
                    break;
                case MachineButtonKey.BtnSwitch:
                    break;
                case MachineButtonKey.BtnBetUp:
                    break;
                case MachineButtonKey.BtnBetDown:
                    break;
                case MachineButtonKey.BtnBetMax:
                    break;
                case MachineButtonKey.BtnHelp:
                    break;
                */


                case MachineButtonKey.BtnCreditUp:
                    {
                        ClearCor(COR_CREDIT_UP_LONG_CLICK);

                        bool isLongClick = Time.unscaledTime - longClickTime[MachineButtonKey.BtnCreditUp] > 5;
                        if (!isLongClick)
                        {
                            DeviceCreditUpDown.Instance.CreditUp();
                        }
                        return;
                    }
                case MachineButtonKey.BtnCreditDown:
                    {
                        ClearCor(COR_CREDIT_DOWN_LONG_CLICK);

                        bool isLongClick = Time.unscaledTime - longClickTime[MachineButtonKey.BtnCreditDown] > 5;
                        if (!isLongClick)
                        {
                            DeviceCreditUpDown.Instance.CreditDown();
                        }
                        return;
                    }
                case MachineButtonKey.BtnTicketOut:
                    {  /*
                        if (PlayerPrefsUtils.isConnectSas) // sas 退票出票
                        {
                          
                            Debug.Log("【machine】sas 退票机退票");
                            int money = 1100;
                            Debug.LogWarning($"开始 sas 退钱： {money}");   //LogWaring("");

                        }
                        else
                        {
                            if (PageManager.Instance.IsHasPopupOrOverlayPage())
                                return;
                            MachineDeviceCommonBiz.Instance.DoCoinOut();
                        }*/

                        if (PageManager.Instance.IsHasPopupOrOverlayPage())
                            return;
                        MachineDeviceCommonBiz.Instance.DoCoinOut();
                    }
                    return;
            }

        if (curBtnInfo != null)/// && curBtnInfo.isShowBtn)
        {

            if (curBtnInfo.btnType == MachineButtonType.Light)
            {
                List<MachineButtonKey> lst = GetLightBtnLst();
                if (lst.Contains(value))
                {
                    EventCenter.Instance.EventTrigger<EventData>(MACHINE_BUTTON_EVENT,
                        new EventData<MachineButtonInfo>(
                            curBtnInfo.mark,
                            new MachineButtonInfo()
                            {
                                isUp = true,
                                btnKey = MachineButtonKey.BtnLight,
                                value = lst.IndexOf(value),
                            }
                        ));

                }
            }
            else
            {
                EventCenter.Instance.EventTrigger<EventData>(MACHINE_BUTTON_EVENT,
                    new EventData<MachineButtonInfo>(
                        curBtnInfo.mark,
                        new MachineButtonInfo()
                        {
                            isUp = true,
                            btnKey = value, // $"{keyName}_Up",
                        }
                    ));
            }
        }

    }



    #region 场景等亮控制

    private IEnumerator InitAllLight()
    {
        LightOn(defaultLightBtn);
        yield return new WaitForSecondsRealtime(1f);
        LightOff(defaultLightBtn);
    }

    /*private void OnEventMachineCustomButton(EventData evt)
    {
        curBtnInfo = (MachineCustomButton)evt.value;

        ClearCor(COR_BTN_LIGHT);

        LightOff(defaultLightBtn);

        if (curBtnInfo == null)// || !curBtnInfo.isShowBtn)
            return;
        

        if (curBtnInfo.btnType == MachineButtonType.Light)
        {
            DoCor(COR_BTN_LIGHT, OnLightBtnShow());
        }
        else
        {
            List<MachineButtonKey> lst = curBtnInfo.btnShowLst;
            LightOn(lst);
        }
    }*/
    private void OnEventMachineCustomButton(EventData evt)
    {
        DoCor(COR_BTN_LIGHT, SetButtonLigth(evt));
    }

    bool isInit = false;

    List<MachineButtonKey> lastBtnShowLst = new List<MachineButtonKey>();
    IEnumerator SetButtonLigth(EventData evt)
    {

        curBtnInfo = (MachineCustomButton)evt.value;

        if (isInit == false)
        {
            LightAllOn();
            yield return new WaitForSecondsRealtime(2f);
            LightAllOff();
            isInit = true;
            yield return new WaitForSecondsRealtime(2f);
        }

        if (curBtnInfo == null)
        {
            LightAllOff();
            CacheBtnShowLst(null);
            yield break;
        }
        else if (curBtnInfo.btnType == MachineButtonType.Regular)
        {
            List<MachineButtonKey> lst = curBtnInfo.btnShowLst;

            if (lst == null || lst.Count == 0)
            {
                LightAllOff();
                CacheBtnShowLst(null);
                yield break;
            }
            else
            {
                yield return CloseExcludeLight(lst);

                LightOn(lst);
                CacheBtnShowLst(lst);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        else
        {
            List<MachineButtonKey> lst = GetLightBtnLst();

            yield return CloseExcludeLight(lst);

            while (true)
            {
                LightOn(lst);
                CacheBtnShowLst(lst);
                yield return new WaitForSecondsRealtime(1f);

                LightOff(lst);
                CacheBtnShowLst(null);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

    }

    IEnumerator CloseExcludeLight(List<MachineButtonKey> lst)
    {
        List<MachineButtonKey> toClose = new List<MachineButtonKey>();
        foreach (MachineButtonKey item in lastBtnShowLst)
        {
            if (!lst.Contains(item))
                toClose.Add(item);
        }
        if (toClose.Count > 0)
        {
            LightOff(toClose);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    void CacheBtnShowLst(List<MachineButtonKey> lst)
    {
        lastBtnShowLst.Clear();
        if (lst == null || lst.Count == 0)
            return;

        foreach (MachineButtonKey item in lst)
            lastBtnShowLst.Add(item);
    }

    private List<MachineButtonKey> GetLightBtnLst()
    {
        List<MachineButtonKey> lst = new List<MachineButtonKey>();
        if (curBtnInfo.btnShowLst != null && curBtnInfo.btnShowLst.Count > 0)
        {
            lst = curBtnInfo.btnShowLst;
        }
        else
        {
            for (int i = 0; i < curBtnInfo.numlightBtn; i++)
            {
                lst.Add(defaultLightBtn[i]);
            }
        }
        return lst;
    }
    #endregion


    void LightOn(List<MachineButtonKey> keys)
    {
        List<ulong> datas = new List<ulong>();
        foreach (KeyValuePair<ulong, MachineButtonKey> kv in keyMap)
        {
            if (keys.Contains(kv.Value))
                datas.Add(kv.Key);
        }
        foreach (ulong data in datas)
        {
            LightOn(data);
        }
    }
    void LightOn(ulong data)
    {
        SBoxSandbox.SwitchOutStateOn(data);
    }

    void LightOff(List<MachineButtonKey> keys)
    {
        List<ulong> datas = new List<ulong>();
        foreach (KeyValuePair<ulong, MachineButtonKey> kv in keyMap)
        {
            if (keys.Contains(kv.Value))
                datas.Add(kv.Key);
        }
        foreach (ulong data in datas)
        {
            LightOff(data);
        }
    }

    void LightAllOn()
    {
        List<ulong> datas = new List<ulong>();
        Type t = typeof(SBOX_SWITCH);
        var fields = t.GetFields();
        foreach (var fieldInfo in fields)
            datas.Add((ulong)fieldInfo.GetRawConstantValue());
        foreach (ulong data in datas)
        {
            LightOn(data);
        }
    }
    void LightAllOff()
    {
        List<ulong> datas = new List<ulong>();
        Type t = typeof(SBOX_SWITCH);
        var fields = t.GetFields();
        foreach (var fieldInfo in fields)
            datas.Add((ulong)fieldInfo.GetRawConstantValue());
        foreach (ulong data in datas)
        {
            LightOff(data);
        }
        return;
    }

    void LightOff(ulong data)
    {
        SBoxSandbox.SwitchOutStateOff(data);
    }

    /// <summary>
    /// 长按上分
    /// </summary>
    /// <returns></returns>
    IEnumerator DoCreditUpLongClick()
    {

        yield return new WaitForSecondsRealtime(3f);

        while (true)
        {
            DeviceCreditUpDown.Instance.CreditUp(true);
            yield return new WaitForSecondsRealtime(0.7f);
        }
    }

    /// <summary>
    /// 长按下分清零
    /// </summary>
    /// <returns></returns>
    IEnumerator DoCreditDownLongClick()
    {

        yield return new WaitForSecondsRealtime(3f);

        DeviceCreditUpDown.Instance.CreditAllDown();
    }









    #region 网络按钮
    const string MARK_NET_BTN_MACHINE_DEVICE = "MARK_NET_BTN_MACHINE_DEVICE";
    void AddNetButtonHandle()
    {
        NetButtonManager.Instance.AddHandles(new NetButtonHandle()
        {
            buttonName = NetButtonManager.BTN_TICKET,
            mark = MARK_NET_BTN_MACHINE_DEVICE,
            onClick = OnNetBtnTicket,
        });
    }

    void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_MACHINE_DEVICE);

    void OnNetBtnTicket(NetButtonInfo info)
    {
        if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
        if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

        NetButtonManager.Instance.ShowUIAminButtonClick(() =>
        {
            OnKeyDown(MachineButtonKey.BtnTicketOut);
        }, () => {
            OnKeyUp(MachineButtonKey.BtnTicketOut);
        }, MARK_NET_BTN_MACHINE_DEVICE, NetButtonManager.BTN_TABLE);

        info.onCallback?.Invoke(true);
    }

    #endregion




    #region 网络命令


    const string MARK_NET_CMD_MACHINE_DEVICE = "MARK_NET_CMD_MACHINE_DEVICE";

    void AddNetCmdHandle()
    {
        NetCmdManager.Instance.AddHandles(new NetCmdHandle()
        {
            cmdName = NetCmdManager.CMD_COIN_IN,
            mark = MARK_NET_CMD_MACHINE_DEVICE,
            onInvoke = OnNetCmdCoinIn,
        });
    }

    void RemoveNetCmdHandle() => NetCmdManager.Instance.ReomveHandles(MARK_NET_CMD_MACHINE_DEVICE);


    void OnNetCmdCoinIn(NetCmdInfo info)
    {
        MachineDeviceCommonBiz.Instance.deviceCoinIn.DoCmdCoinIn((int)info.data, info.onCallback);
    }
    #endregion
}