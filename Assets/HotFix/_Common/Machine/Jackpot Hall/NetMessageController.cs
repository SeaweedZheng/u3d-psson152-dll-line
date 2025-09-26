using Newtonsoft.Json;
using System.Text;
using UnityEngine;

public class NetMessageController : BaseManager<NetMessageController>
{
    public void Init()
    {
        AddEventListener();
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener<bool>(EventHandle.NETWORK_STATUS, OnNetworkStatus);
        Messenger.AddListener<byte[]>(MessageName.Event_ClientNetworkRecv, OnClientNetworkRecv);
    }

    private void OnNetworkStatus(bool isConnected)
    {
        if (isConnected)
            Login();
        else
            DebugUtils.LogError("网络连接失败");
    }

    public void Login()
    {
        MsgInfo msgInfo = new MsgInfo()
        {
            cmd = (int)C2S_CMD.C2S_Login,
            id = 10800001,//IOCanvasModel.Instance.CfgData.MachineId,
        };
        string msg = JsonConvert.SerializeObject(msgInfo);
        NetMgr.Instance.SendToServer(msg);
    }

    private void OnClientNetworkRecv(byte[] data)
    {
        if (data.Length == 0)
            return;

        string singlePacket = Encoding.UTF8.GetString(data);
        MsgInfo info = JsonConvert.DeserializeObject<MsgInfo>(singlePacket);

        if (info == null)
            return;
        int macId = info.id;
        S2C_CMD netCmd = (S2C_CMD)info.cmd;

        if (macId != -1 && macId != 10800001/*IOCanvasModel.Instance.CfgData.MachineId*/) return;

        switch (netCmd)
        {
            case S2C_CMD.S2C_HeartHeat:
                NetMgr.Instance.SetLastHeartHeat();
                break;
            case S2C_CMD.S2C_WinJackpot:
                WinJackpot(info.jsonData);
                EventCenter.Instance.EventTrigger<string>(RPCName.jackpotHall, info.jsonData);
                break;
            case S2C_CMD.S2C_Error:
                MessageError(info.jsonData);
                break;
            default:
                DebugUtils.LogError($"未设置 {info.cmd} 响应");
                break;
        }
    }

    private void WinJackpot(string jsonData)
    {
        var winJackpotInfo = JsonConvert.DeserializeObject<WinJackpotInfo>(jsonData);
        DebugUtils.Log($"{winJackpotInfo.macId}号机  {winJackpotInfo.seat}分机 彩金id: {winJackpotInfo.jackpotId}  中奖金额：{winJackpotInfo.win}");

        NetMgr.Instance.SendToServer(JsonConvert.SerializeObject(new MsgInfo()
        {
            cmd = (int)C2S_CMD.C2S_ReceiveJackpot,
            id = 10800001,//IOCanvasModel.Instance.CfgData.MachineId,
            jsonData = winJackpotInfo.orderId.ToString(),
        }));
    }

    private void MessageError(string jsonData)
    {
        var errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(jsonData);
        DebugUtils.Log($"错误码：{errorInfo.errCode}，错误信息：{errorInfo.errString}");
    }
}
