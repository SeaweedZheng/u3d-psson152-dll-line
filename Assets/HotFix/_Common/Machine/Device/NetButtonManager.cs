using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using GameMaker;
using Newtonsoft.Json.Linq;


public class NetButtonHandle
{

    public string buttonName;

    public Action<NetButtonInfo> onClick;

    public string mark;

}

public class NetButtonInfo
{
    public string dataType = "";
    public string toDo;
    public object data;
    public Action<bool> onCallback;
}


public partial class NetButtonManager : MonoSingleton<NetButtonManager>
{
    private CorController _corCtrl;
    private CorController corCtrl
    {
        get
        {
            if (_corCtrl == null)
            {
                _corCtrl = new CorController(this);
            }
            return _corCtrl;
        }
    }

    public void ClearCorStartsWith(string prefix) => corCtrl.ClearCorStartsWith(prefix);
    public void ClearCor(string name) => corCtrl.ClearCor(name);

    public void ClearAllCor() => corCtrl.ClearAllCor();

    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);

    public bool IsCor(string name) => corCtrl.IsCor(name);

    public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);

    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_MQTT_REMOTE_CONTROL_EVENT, OnMqttRemoteControlButton);
    }

    protected override void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_MQTT_REMOTE_CONTROL_EVENT, OnMqttRemoteControlButton);

        base.OnDestroy();
    }


    List<NetButtonHandle> handles = new List<NetButtonHandle>();
    public void AddHandles(NetButtonHandle info)
    {
        handles.Add(info);
    }
    public void ReomveHandles(NetButtonHandle info)
    {
        ClearCorStartsWith($"{info.mark}__{info.buttonName}");

        handles.Remove(info);
    }
    public void ReomveHandles(string mark)
    {
        ClearCorStartsWith(mark);

        int idx = handles.Count;
        while (--idx >= 0)
        {
            if (handles[idx].mark == mark)
                handles.RemoveAt(idx);
        }
    }


    /// <summary> 加注 </summary>
    public const string BTN_BET_UP = "BTN_BET_UP";
    /// <summary> 减注 </summary>
    public const string BTN_BET_DOWN = "BTN_BET_DOWN";
    /// <summary> 最大注 </summary>
    public const string BTN_BET_MAX = "BTN_BET_MAX";
    /// <summary> 说明页 </summary>
    public const string BTN_TABLE = "BTN_TABLE";
    /// <summary> 开玩 </summary>
    public const string BTN_SPIN = "BTN_SPIN";
    /// <summary> 停止 </summary>
    public const string BTN_STOP = "BTN_STOP";
    /// <summary> 自动 </summary>
    public const string BTN_AUTO = "BTN_AUTO";
    /// <summary> 上一页 </summary>
    public const string BTN_PREV = "BTN_PREV";
    /// <summary> 下一页 </summary>
    public const string BTN_NEXT = "BTN_NEXT";
    /// <summary> 退出 </summary>
    public const string BTN_EXIT = "BTN_EXIT";
    /// <summary> 切换游戏配置 </summary>
    public const string BTN_SWITCH = "BTN_SWITCH";
    /// <summary> 退票 </summary>
    public const string BTN_TICKET = "BTN_TICKET";



    [Button]
    public void TestBtnTABLE() => DoButton(BTN_TABLE);
    [Button]
    public void TestBtnPREV() => DoButton(BTN_PREV);
    [Button]
    public void TestBtnNEXT() => DoButton(BTN_NEXT);

    [Button]
    public void TestBtnBET_UP() => DoButton(BTN_BET_UP);

    [Button]
    public void TestBtnBET_DOWN() => DoButton(BTN_BET_DOWN);


    [Button]
    public void TestBtnBET_MAX() => DoButton(BTN_BET_MAX);

    [Button]
    public void TestBtnEXIT() => DoButton(BTN_EXIT);


    [Button]
    public void TestBtnSPIN() => DoButton(BTN_SPIN);

    [Button]
    public void TestBtnSTOP() => DoButton(BTN_STOP);

    [Button]
    public void TestBtnAUTO() => DoButton(BTN_AUTO);

    [Button]
    public void TestBtnTICKET() => DoButton(BTN_TICKET);

    


    [Button]
    public void TestCommonPopOne()
    {
        CommonPopupHandler.Instance.OpenPopupSingle(
           new CommonPopupInfo()
           {
               isUseXButton = false,
               buttonAutoClose1 = true,
               buttonAutoClose2 = true,
               type = CommonPopupType.YesNo,
               text = I18nMgr.T("Error Password"),
               buttonText1 = I18nMgr.T("Cancel"),
               buttonText2 = I18nMgr.T("Confirm"),
           });
    }

    /// <summary> mqtt 网页按钮数据 </summary>
    public const string DATA_MACHINE_BUTTON_CONTROL = "DATA_MACHINE_BUTTON_CONTROL";

    public void DoButton(string btnName)
    {
        switch (btnName)
        {
            case BTN_BET_UP: 
                break;
            case BTN_BET_DOWN: 
                break;
            case BTN_BET_MAX: 
                break;
            case BTN_TABLE: 
                break;
            case BTN_SPIN: 
                break;
            case BTN_STOP: 
                break;
            case BTN_AUTO: 
                break;
            case BTN_PREV: 
                break;
            case BTN_NEXT: 
                break;
            case BTN_EXIT: 
                break;
            case BTN_SWITCH: 
                break;
            case BTN_TICKET: 
                break;
        }

        NetButtonInfo info = new NetButtonInfo()
        {
            dataType = DATA_MACHINE_BUTTON_CONTROL,
        };

        OnInvoke(btnName, info);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="btnName">那个按钮</param>
    /// <param name="dataType">数据类型：普通、mqtt数据、视讯</param>
    /// <param name="data"> 数据</param>
    /// <param name="onCallback">回调函数</param>
    private void OnInvoke(string btnName, NetButtonInfo info)
    {
        for (int i=0; i< handles.Count; i++ )
        {
            if(handles[i].buttonName == btnName)
                handles[i].onClick.Invoke(info);
        }
    }


    #region 播放UI按钮按下的动画
    public void ShowUIAminButtonClick(Button btn , string mark, string btnName) // mark__btnName;
    {
        if (btn.interactable)
        {
            DoCor($"{mark}__{btnName}", DoBtnClick(btn));
        }
    }
    IEnumerator DoBtnClick(Button btn)
    {
        //只有按下动画，不触发事件
        btn.OnPointerDown(new PointerEventData(null)
        {
            button = PointerEventData.InputButton.Left,
        });

        yield return new WaitForSecondsRealtime(0.15f);

        //只有弹起动画，不触发事件
        btn.OnPointerUp(new PointerEventData(null)
        {
            button = PointerEventData.InputButton.Left,
        });

        btn.onClick.Invoke();//或 btn.OnSubmit(null); 
    }


    //IPointerDownHandler, IPointerUpHandler
    public void ShowUIAminButtonClick(Action onPointerDown, Action onPointerUp, string mark, string btnName) // mark__btnName;
    {
         DoCor($"{mark}__{btnName}", DoBtnClick(onPointerDown, onPointerUp, 0.15f));
    }
    IEnumerator DoBtnClick(Action onPointerDown, Action onPointerUp, float timeS)
    {
        //只有按下动画，不触发事件
        onPointerDown.Invoke();

        yield return new WaitForSecondsRealtime(timeS);//0.15f
        //只有弹起动画，不触发事件
        onPointerUp.Invoke();
    }

    public void ShowUIAminButtonLongClick(Action onPointerDown, Action onPointerUp, string mark, string btnName) // mark__btnName;
    {
        DoCor($"{mark}__{btnName}", DoBtnClick(onPointerDown, onPointerUp, 1f));
    }


    #endregion


}





/// <summary>
///  针对Mqtt的按钮控制
/// </summary>
public partial class NetButtonManager : MonoSingleton<NetButtonManager>
{

    /// <summary> mqtt 网页按钮数据 </summary>
    public const string DATA_MQTT_REMOTE_CONTROL = "DATA_MQTT_REMOTE_CONTROL";



    Dictionary<string, string> btnMap = new Dictionary<string, string>()
    {
        [MqttRemoteCtrlMethod.BtnTicketOut] = BTN_TICKET,
        [MqttRemoteCtrlMethod.BtnSpin] = BTN_SPIN,
        [MqttRemoteCtrlMethod.BtnAuto] = BTN_AUTO,
        [MqttRemoteCtrlMethod.BtnBetUp] = BTN_BET_UP,
        [MqttRemoteCtrlMethod.BtnBetDown] = BTN_BET_DOWN,
        [MqttRemoteCtrlMethod.BtnBetMax] = BTN_BET_MAX,
        [MqttRemoteCtrlMethod.BtnTable] = BTN_TABLE,
        [MqttRemoteCtrlMethod.BtnPrevious] = BTN_PREV,
        [MqttRemoteCtrlMethod.BtnNext] = BTN_NEXT,
        [MqttRemoteCtrlMethod.BtnExit] = BTN_EXIT,
        [MqttRemoteCtrlMethod.BtnSwitch] = BTN_SWITCH,
    };



    public void OnMqttRemoteControlButton(EventData res)
    {
        if (res.name.StartsWith("Btn"))
        {
            JObject data = JObject.Parse((string)res.value);
            int seqId = data["seq_id"].ToObject<int>();

            string btnTarget = btnMap[res.name];

            OnInvoke(btnTarget,
                new NetButtonInfo()
                {
                    dataType = DATA_MACHINE_BUTTON_CONTROL,
                    //toDo = res.name,
                    //data = res.value,
                    onCallback = (isOk) =>
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>()
                        {
                            ["code"] = (bool)isOk ? 0 : 1
                        };
                        MqttRemoteController.Instance.RespondCommand(res.name, data, seqId);
                    }
                });
        }

        switch (res.name)
        {
            /*
            case MqttRemoteCtrlMethod.PlayGame:
                {
                    int data = int.Parse((string)res.value);
                    switch (data)
                    {
                        case 0:  //Spin
                            {
                                OnInvoke(BTN_SPIN, 
                                    new NetButtonInfo()
                                    {
                                       dataType = DATA_MQTT_REMOTE_CONTROL,
                                       toDo = "Spin",
                                       data = res.value,
                                       onClick = (isOk) =>
                                       {
                                           MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.PlayGame, (bool)isOk ? 1 : 2);
                                       }
                                    });

                            }
                            break;
                        case 1: //Auto
                            {
                                OnInvoke(BTN_AUTO, new NetButtonInfo()
                                {
                                    dataType = DATA_MQTT_REMOTE_CONTROL,
                                    toDo = "Auto",
                                    data = res.value,
                                    onClick = (isOk) =>
                                    {
                                        MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.PlayGame, (bool)isOk ? 1 : 2);
                                    }
                                });

                            }
                            break;
                        case 2: //Stop Auto
                            {
                                OnInvoke(BTN_STOP, new NetButtonInfo()
                                {
                                    dataType = DATA_MQTT_REMOTE_CONTROL,
                                    toDo = "StopAuto",
                                    data = res.value,
                                    onClick = (isOk) =>
                                    {
                                        MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.PlayGame, (bool)isOk ? 1 : 2);
                                    }
                                });
                            }
                            break;
                    }
                }
                break;
            */

            default:

                break;
        }
    }

}