using GameMaker;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using _contentBB = PssOn00152.ContentBlackboard;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using _mainBB = GameMaker.MainBlackboard;
using SimpleJSON;


public class NetCmdHandle
{

    public string cmdName;

    public Action<NetCmdInfo> onInvoke;

    public string mark;

}

public class NetCmdInfo
{
    public string dataType = "";
    public object data;
    public Action<object> onCallback;
}



public partial class NetCmdManager : MonoSingleton<NetCmdManager>
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


    List<NetCmdHandle> handles = new List<NetCmdHandle>();
    public void AddHandles(NetCmdHandle info)
    {
        handles.Add(info);
    }
    public void ReomveHandles(NetCmdHandle info)
    {
        ClearCorStartsWith($"{info.mark}__{info.cmdName}");

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmdName">那个按钮</param>
    /// <param name="info">NetCmdInfo数据</param>
    private void OnInvoke(string cmdName, NetCmdInfo info)
    {
        for (int i = 0; i < handles.Count; i++)
        {
            if (handles[i].cmdName == cmdName)
                handles[i].onInvoke.Invoke(info);
        }
    }
}


public partial class NetCmdManager : MonoSingleton<NetCmdManager>
{
    /// <summary> 指令：投币</summary>
    public const string CMD_COIN_IN = "CMD_COIN_IN";
    /// <summary> 指令：投币</summary>
    public const string CMD_COIN_OUT = "CMD_COIN_OUT";
    /// <summary> 指令：游戏彩金当前值</summary>
    public const string CMD_JACKPOT_CUR_DATA = "CMD_JACKPOT_CUR_DATA";


    void Start()
    {
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_MQTT_REMOTE_CONTROL_EVENT, OnMqttRemoteControlButton);
    }

    protected override void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_MQTT_REMOTE_CONTROL_EVENT, OnMqttRemoteControlButton);

        base.OnDestroy();
    }


    [Button]
    public void TestCmdCoinIn()
    {
        OnInvoke(CMD_COIN_IN, new NetCmdInfo()
        {
            dataType = "",
            data = 8,
            onCallback = (isOk) =>
            {
                Debug.Log($"TestCmdCoinIn: {isOk}");
            }
        });
    }

    [Button]
    public void TestCmdCoinOut()
    {
        OnInvoke(CMD_COIN_OUT, new NetCmdInfo()
        {
            dataType = "",
            data = null,
            onCallback = (coinOutCount) =>
            {
                Debug.Log($"退票个数： {coinOutCount}");
            }
        });
    }
}




public partial class NetCmdManager : MonoSingleton<NetCmdManager>
{

    /// <summary> mqtt 网页按钮数据 </summary>
    public const string DATA_MQTT_REMOTE_CONTROL = "DATA_MQTT_REMOTE_CONTROL";

    public void OnMqttRemoteControlButton(EventData req)
    {


        if (req.name.StartsWith("Btn") || req.name == MqttRemoteCtrlMethod.On || req.name == MqttRemoteCtrlMethod.Off)
        {
            return;
        }

        JObject data = JObject.Parse((string)req.value);
        int seqId = data["seq_id"].ToObject<int>();

        switch (req.name)
        {

            case MqttRemoteCtrlMethod.AddCoins:
                {

                    JObject node = JObject.Parse((string)req.value);
                    int coinInCount = (int)node["body"][0]["num"];

                    OnInvoke(CMD_COIN_IN, new NetCmdInfo()
                    {
                        dataType = DATA_MQTT_REMOTE_CONTROL,
                        data = coinInCount,
                        onCallback = (isOk) =>
                        {
                            /*
                            int code = isOk ? 1 : 0;
                            string resStr = $@"{{
                                ""code"": {code}
                            }}";
                            JObject res = JObject.Parse(resStr);*/

                            Dictionary<string, object> res = new Dictionary<string, object>()
                            {
                                ["code"] = (bool)isOk ? 0 : 1,
                            };
                            MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.AddCoins, res, seqId);
                        }
                    });
                }
                break;
            /*case MqttRemoteCtrlMethod.GetCoinCount:
                {

                    OnInvoke(CMD_COIN_OUT, new NetCmdInfo()
                    {
                        dataType = DATA_MQTT_REMOTE_CONTROL,
                        data = null,
                        onCallback = (coinOutCount) =>
                        {
                            Dictionary<string, object> res = new Dictionary<string, object>()
                            {
                                ["code"] = 0,
                                ["CoinCount"] = coinOutCount,
                            };
                            MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.AddCoins, res, seqId);
                        }
                    });
                }
                break;
            case MqttRemoteCtrlMethod.GetBonus:
                {
                    OnInvoke(CMD_JACKPOT_CUR_DATA, new NetCmdInfo()
                    {
                        dataType = DATA_MQTT_REMOTE_CONTROL,
                        data = null,
                        onCallback = (data) =>
                        {

                            Dictionary<string, object> res = new Dictionary<string, object>()
                            {
                                ["code"] = 0,
                                ["jp1"] = 10000,
                                ["jp2"] = 1000,
                                ["jp3"] = 100,
                                ["jp4"] = 10,
                            };
                            MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.AddCoins, res, seqId);
                        }
                    });
                }
                break;*/
            case MqttRemoteCtrlMethod.GetErrorCode:
                {

                    TextAsset jsn = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>(ConfigUtils.GetErrorCode());

                    JSONNode node = JSONNode.Parse("{}");
                    node.Add("code",0);
                    node.Add("msg", 0);
                    node.Add("error_code", JSONNode.Parse(jsn.text));

                    MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.GetErrorCode, node.ToString(), seqId);
                }
                break;
            case MqttRemoteCtrlMethod.GetMachineInfo:
                {
                    Dictionary<string, object> res = new Dictionary<string, object>()
                    {
                        ["code"] = 0,
                        ["msg"] = "",
                        ["machine_name"] = ApplicationSettings.Instance.gameTheme,
                        ["active"] = _consoleBB.Instance.isMachineActive,
                        ["avatar_url"] = Path.Combine(PathHelper.hotfixDirWEBURL, "PssOn00152.png"),
                    };


                    MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.GetMachineInfo, res, seqId);
                }
                break;
            case MqttRemoteCtrlMethod.GetGameState:
                {
                    Dictionary<string, object> res = new Dictionary<string, object>()
                    {
                        ["code"] = 0,
                        ["msg"] = "",
                        ["game_state"] = _contentBB.Instance.gameState,
                        ["credit"] = _mainBB.Instance.myCredit,
                        ["bet"] = _contentBB.Instance.totalBet,
                        ["is_spin"] = _contentBB.Instance.isSpin,
                        ["is_auto"] = _contentBB.Instance.isAuto,
                        ["jp1"] = _contentBB.Instance.uiGrandJP.curCredit,
                        ["jp2"] = _contentBB.Instance.uiMajorJP.curCredit,
                        ["jp3"] = _contentBB.Instance.uiMinorJP.curCredit,
                        ["jp4"] = _contentBB.Instance.uiMiniJP.curCredit,
                    };


                    MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.GetGameState, res, seqId);
                }
                break;
            case MqttRemoteCtrlMethod.Report:
                {
                    JObject node = JObject.Parse((string)req.value);
                    string tp = (string)node["body"][0]["type"];

                    if (tp == "SqlSelect")
                    {
                        string sqlSelect = (string)node["body"][0]["sql"];
                        string result = ReportDB.Instance.DoSQLSelect(sqlSelect);

                        Dictionary<string, object> res = new Dictionary<string, object>()
                        {
                            ["code"] = 0,
                            ["msg"] = "",
                            ["game_state"] = _contentBB.Instance.gameState,
                            ["type"] = tp,
                            ["data"] = result,
                        };
                        MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.Report, res, seqId);
                    }
                    else
                    {
                        Dictionary<string, object> res = new Dictionary<string, object>()
                        {
                            ["code"] = 1,
                            ["msg"] = "无法识别指令",
                            ["data"] = node.ToString(),
                        };
                        MqttRemoteController.Instance.RespondCommand(MqttRemoteCtrlMethod.Report, res, seqId);
                    }

                }
                break;
        }

    }
}
