using Newtonsoft.Json;
using UnityEngine;


public class NetMgr : MonoSingleton<NetMgr>
{
    private readonly int port = 7789;
    public int broadcastPort = 10999;
    private bool IsHost = false;

    //WebSocket
    ServerWS serverWS;
    ClientWS clientWS;


    private void Awake()
    {
        serverWS = this.transform.GetComponent<ServerWS>();
        clientWS = this.transform.GetComponent<ClientWS>();

        Messenger.AddListener<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, OnWSServerData);
        Messenger.AddListener<byte[]>(MessageName.Event_NetworkClientData, OnClientData);
    }


    public void SetLastHeartHeat()
    {
        if (clientWS != null)
            clientWS.LastHeartHeatTime = Time.time;
    }

    void OnClientData(byte[] data)
    {
        Messenger.Broadcast<byte[]>(MessageName.Event_ClientNetworkRecv, data);
    }

    public void SetNetAutoConnect(bool Host)
    {
        IsHost = Host;
        if (IsHost)
        {
            if (serverWS == null)
                serverWS = gameObject.AddComponent<ServerWS>();
            serverWS.StartServer(port, broadcastPort);
        }
        else
        {
            if (clientWS == null)
                clientWS = gameObject.AddComponent<ClientWS>();
            clientWS.StartUdp(broadcastPort);
        }
    }

    //�ͻ��˷������ݸ�������
    public void SendToServer(string strMsg)
    {
        clientWS?.SendToServer(strMsg);
    }

    //�������������ݸ��ͻ���
    public void SendToClient(WebSockets.ClientConnection client,string strMsg)
    {
        serverWS?.SendToClient(client, strMsg);
    }

    //�����������пͻ��˷�����Ϣ
    public void SendToAllClient(string strMsg)
    {
        serverWS?.SendToAllClient(strMsg);
    }

    //����WS�������յ�����Ϣ
    void OnWSServerData(WSSrvMsgData data)
    {
        if (data.Data.Length == 0)
            return;
        string singlePacket = data.Data;
        MsgInfo info = null;
        try
        {
            info = JsonConvert.DeserializeObject<MsgInfo>(singlePacket);
        }
        catch (System.Exception ex)
        {
            Debug.Log("JSON : " + ex.Message);
        }

        if (info != null)
        {
            switch ((C2S_CMD)info.cmd)
            {
                case C2S_CMD.C2S_HeartHeat:
                    info.cmd = (int)S2C_CMD.S2C_HeartHeat;
                    info.id = info.id;
                    SendToClient(data.Client, JsonConvert.SerializeObject(info));
                    break;
                default:
                    Messenger.Broadcast(MessageName.Event_ServerNetworkRecv, info, data.Client);
                    break;
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Messenger.RemoveListener<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, OnWSServerData);
        Messenger.RemoveListener<byte[]>(MessageName.Event_NetworkClientData, OnClientData);
    }
}