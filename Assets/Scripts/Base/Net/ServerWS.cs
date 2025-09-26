using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class ServerWS : MonoBehaviour
{
    private WebSockets.WebSocketServer mServer;

    //主机相关
    private UdpClient mUdpclient = null; //主机和分机的udpclient
    private IPEndPoint endpoint;
    ServerInfo serverinfo;

    private bool IsStop = false;
    private Thread RcvThread = null;

    public void StartServer(int port, int broadcastPort)
    {
        serverinfo = new ServerInfo();
        serverinfo.IP = Utils.LocalIP();
        serverinfo.port = port;
        StartUdp(broadcastPort);
        InitSocket(port);
    }

    public void StopServer()
    {
        if(mServer != null)
        {
            mServer.Stop();
            mServer = null;
        }
    }

    protected void StartUdp(int broadcastPort)
    {
        mUdpclient = new UdpClient(new IPEndPoint(IPAddress.Any, broadcastPort));
        endpoint = new IPEndPoint(IPAddress.Any, 0);
        IsStop = false;
        RcvThread = new Thread(new ThreadStart(ReciveUdpMsg))
        {
            IsBackground = true
        };
        RcvThread.Start();
    }

    public void InitSocket(int port)
    {
        StopServer();
        mServer = new WebSockets.WebSocketServer(IPAddress.Any,port);
        mServer.OnClientConnected += OnClientConnected;
        mServer.Start();
    }

    private void ReciveUdpMsg()
    {
        while (!IsStop && mUdpclient != null)
        {
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buf = mUdpclient.Receive(ref endpoint);
            if (buf != null)
            {
                string msg = Encoding.UTF8.GetString(buf);
                if (!string.IsNullOrEmpty(msg))
                {
                    ServerInfo srvInfo = new ServerInfo
                    {
                        IP = serverinfo.IP,
                        port = serverinfo.port
                    };
                    SendUpdMsg(JsonConvert.SerializeObject(srvInfo), endpoint);
                }
            }
            Thread.Sleep(500);
        }
    }

    //使用udp发送消息
    public void SendUpdMsg(string strMsg, IPEndPoint endPoint)
    {
        if (mUdpclient != null)
        {
            byte[] bf = Encoding.UTF8.GetBytes(strMsg);
            mUdpclient.Send(bf, bf.Length, endPoint);
        }
    }

    private void OnClientConnected(WebSockets.ClientConnection client)
    {
        client.ReceivedTextualData += OnReceivedTextualData;
        client.Disconnected += OnClientDisconnected;
        client.StartReceiving();

        Debug.Log(string.Format("Client {0} Connected...", client.Id));
    }

    private void OnClientDisconnected(WebSockets.ClientConnection client)
    {
        client.ReceivedTextualData -= OnReceivedTextualData;
        client.Disconnected -= OnClientDisconnected;
        Debug.Log(string.Format("Client {0} Disconnected...",client.Id));
        EventCenter.Instance.EventTrigger(EventHandle.PLAYER_DISCONNECT, client);
    }

    private void OnReceivedTextualData(WebSockets.ClientConnection client, string data)
    {
        WSSrvMsgData wmd = new WSSrvMsgData
        {
            Client = client,
            Data = data
        };
        Loom.QueueOnMainThread((wmd) =>
        {
            Messenger.Broadcast<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, (WSSrvMsgData)wmd);
            wmd = null;
        }, wmd);
    }

    public void SendToClient(WebSockets.ClientConnection client,string msg)
    {
        client.Send(msg);
    }

    public void SendToAllClient(string msg)
    {
        if(mServer != null)
        {
            mServer.SendToAllClient(msg);
        }
    }

    private void OnDestroy()
    {
        IsStop = true;
        if (RcvThread != null)
        {
            RcvThread.Abort();
            RcvThread = null;
        }
        // StopCoroutine(CheckHostServerInfo(3.0f));
        if (mUdpclient != null)
        {
            mUdpclient.Close();
            mUdpclient = null;
        }
        StopServer();
    }
}
