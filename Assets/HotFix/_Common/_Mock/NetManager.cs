#define MOCK
using GameMaker;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text.RegularExpressions;
using UnityEngine;




/// <summary>
/// ISocket
/// </summary>
public partial class NetManager : MonoSingleton<NetManager>, IHttp
{
    // Start is called before the first frame update
    void Start()
    {


    NetManager.Instance.Connect(new NetConnectOptions("mock://127.0.0.1:88", 0));

    }

    // Update is called once per frame
    void Update()
    {

    }




    private ISocket m_Socket = null;

    private ISocket socket
    {
        get
        {
            if (m_Socket == null)
            {
#if MOCK
                    //m_Socket = MockManager.Instance;
#else
                    m_Socket = null;
#endif

            }
            return m_Socket;
        }
        set
        {
            m_Socket = value;
        }
    }


    /// <summary>�Ƿ��Ѿ���ʼ��socket</summary>
    bool _isSocketInit = false;

    /// <summary>����״̬</summary>
    protected NetNodeState _state = NetNodeState.Closed;


    /// <summary>�������</summary>
    [SerializeField]
    protected NetConnectOptions _connectOptions = null;

    /// <summary>��������</summary>
    protected int _autoReconnect = 0;


    private void _InitSocket()
    {
        if (this.socket != null)
        {
            this.socket.onConnected = this.OnConnected;
            this.socket.onMessage = this.OnMessage;
            this.socket.onError = this.OnError;
            this.socket.onClosed = this.onClosed;
            this._isSocketInit = true;
        }
    }

    private void OnConnected(object evt) {}
    private void OnError(object evt){ }

    private void onClosed(object evt){ }

    public bool Connect(NetConnectOptions options)
    {
        if (this.socket != null && this._state == NetNodeState.Closed)
        {
            if (!this._isSocketInit)
            {
                this._InitSocket();
            }
            this._state = NetNodeState.Connecting;

            if (!this.socket.Connect(options))
            {
                return false;
            }
            if (this._connectOptions == null)
            {
                this._autoReconnect = options.autoReconnect; //ˢ����������
            }
            this._connectOptions = options;
            return true;
        }
        return false;
    }

    private void OnMessage(object aesEvt)
    {
        string evt = "";
        JSONNode dataDict = null;
        try
        {
            evt = aesEvt as string;
            dataDict = SimpleJSON.JSONNode.Parse(evt as string);
        }
        catch (Exception ex)
        {
            DebugUtils.LogError($"@ ERROR :{ex}");
            DebugUtils.LogError($"@ ����������û���� : evt = {aesEvt}");

            // �ص���¼����
            return;
        }

        try
        {
            if (!dataDict.HasKey("protocol_key"))
            {
                DebugUtils.LogError($"�������������ݣ�û��protocol_key�ֶ� ��{evt}");
                return;
            }
            else if (!dataDict.HasKey("data"))
            {
                DebugUtils.LogError($"�������������ݣ�û��data�ֶ� ��{evt}");
                return;
            }
            else if (!dataDict["data"].HasKey("err"))
            {
                DebugUtils.LogError($"�������������ݣ�û��data.err�ֶ� ��{evt}");
                return;
            }

            if (dataDict["protocol_key"] == RPCName.ping)
            {
                DebugUtils.Log($"== �����յ��������� ��{evt}");
            }
            else
            {
                DebugUtils.Log($"==@ �����յ��������� ��{evt}");
            }

            this.OnWebSocketMessage((string)dataDict["protocol_key"], dataDict["data"]);
        }
        catch (Exception ex)
        {
            DebugUtils.LogError($"@������: {ex}\n{evt}");
            // ���ܵ��� ���ܷ��ش���
            //ReturnToLoginPage($"Error {ex}   evt = {evt}");
        }
    }
}

/// <summary>
/// ҵ���
/// </summary>
public partial class NetManager : MonoSingleton<NetManager>, IHttp
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rpcName"></param>
    /// <param name="data"> Dictionary<string, object> ��List<Object>�� JSONNode �� Class </param>
    /// <param name="seq_id"></param>
    private void SendMsg(string rpcName, object data, int seq_id = -1)
    {
        string buffer = null;

        JSONNode node = JSONNode.Parse("{}");
        node.Add("protocol_key", rpcName);

        if (data is JSONNode)
        {
            node.Add("data", (JSONNode)data);
        }
        else
        {
            node.Add("data", JSONNode.Parse(JSONNodeUtil.ObjectToJsonStr(data)));
        }

        if (seq_id == -1)
            seq_id = CreatSeqID();

        node["data"].Add("seq_id", seq_id);
        buffer = node.ToString();

        //this._Send(rpcName, buffer);
        _Send(rpcName, buffer);
    }


    private int _Send(string rpcName, string buffer, bool force = false)
    {
        if (rpcName == RPCName.ping)
        {
            DebugUtils.Log($"== ������������ ��{buffer}");
        }
        else
        {
            DebugUtils.Log($"==@ ������������ ��{buffer}");
        }

        try
        {
            return socket.Send(buffer);
        }
        catch (Exception ex)
        {
            DebugUtils.LogException(ex);
            return -1;
        }

    }



        private void OnWebSocketMessage(string rpcName, JSONNode data)
    {
        //this._requests.RemoveAll(item => item.rpcName == rpcName);
        switch (rpcName)
        {
            case RPCName.login:
                break;
            case RPCName.lobby:
                break;
            case RPCName.enterGame:
                MainBlackboard.Instance.gameID = ConfigUtils.curGameId;
                break;
        }

        this._Emit(rpcName, data as JSONNode);
    }

    private void _Emit(string rpcName, JSONNode data)
    {
        var eventData = new EventData<JSONNode>(rpcName, data as JSONNode);
        EventCenter.Instance.EventTrigger<EventData>(rpcName, eventData);


        if (this._onceEventHandlerLst.ContainsKey(rpcName))
        {

            EventHandlerInfo[] ehLst2Handle = new EventHandlerInfo[] { };

            if (data.HasKey("seq_id"))
            {
                int seqID = data["seq_id"];
                if (seqID == -1)  //�������seq_id
                {

                    EventHandlerInfo eh2Remain = null;
                    /*
                    RequestType req = this._requests.Find(item => item.rpcName == rpcName);
                    if (req != null)
                    {
                        Match match = Regex.Match((string)req.buffer, "\"seq_id\":\\s*(\\d+)");
                        if (match.Success)
                        {
                            string str = match.Groups[1].Value;
                            int seq_id = int.Parse(str);
                            eh2Remain = this._onceEventHandlerLst[rpcName].Find(item => item.seqID == seq_id);
                        }
                    }*/

                    if (eh2Remain != null)
                        this._onceEventHandlerLst[rpcName].Remove(eh2Remain);

                    ehLst2Handle = this._onceEventHandlerLst[rpcName].ToArray();
                    this._onceEventHandlerLst[rpcName].Clear();

                    if (eh2Remain != null)
                        this._onceEventHandlerLst[rpcName].Add(eh2Remain);
                }
                else
                {
                    EventHandlerInfo eh = this._onceEventHandlerLst[rpcName].Find(item => item.seqID == seqID);
                    if (eh != null)
                    {
                        this._onceEventHandlerLst[rpcName].Remove(eh);
                        ehLst2Handle = new EventHandlerInfo[] { eh };
                    }
                }
            }
            else
            {
                ehLst2Handle = this._onceEventHandlerLst[rpcName].ToArray();
                this._onceEventHandlerLst[rpcName].Clear();
            }


            if ((int)data["err"] == 0)
            {
                foreach (var eh in ehLst2Handle)
                {
                    eh.responseCallback(data);
                }
            }
            else
            {
                BagelCodeHTTPError errMsg = new BagelCodeHTTPError();
                errMsg.code = (int)data["err"];
                errMsg.msg = data["msg"];  
                //errMsg.response = data.ToString();

                DebugUtils.LogWarning($"rpc name = {rpcName} ,err = {errMsg.code} , msg = {errMsg.msg}");
                
                foreach (var eh in ehLst2Handle)
                {
                    if (data.HasKey("seq_id"))  //��"seq_id" = -1 �滻Ϊ��Ӧid
                    {
                        data["seq_id"] = eh.seqID;
                    }
                    errMsg.response = data.ToString();
                    eh.errorCallback(errMsg);
                }
            }
        }
    }
}


/// <summary>
/// IHttp
/// </summary>
public partial class NetManager : MonoSingleton<NetManager>, IHttp
{

    private int seqID = 0;
    private Dictionary<string, List<EventHandlerInfo>> _onceEventHandlerLst = new Dictionary<string, List<EventHandlerInfo>>();

    private int CreatSeqID()
    {
        List<int> temp = new List<int>();
        foreach (KeyValuePair<string, List<EventHandlerInfo>> kv in this._onceEventHandlerLst)
        {
            foreach (EventHandlerInfo item in kv.Value)
            {
                temp.Add(item.seqID);
            }
        }

        do
        {
            if (++this.seqID > 10000)
                this.seqID = 1;

        } while (temp.Contains(seqID));

        return seqID;
    }



    /// <summary>
    /// post��������
    /// </summary>
    /// <param name="url">Э������</param>
    /// <param name="data">Dictionary&lt;string,object> / List&lt;object> / �� �磺RPCKenoClassic.ReqKenoSpin </param>
    /// <param name="responseCallback">Action<JSONNode> / �ɹ��ص�</param>
    /// <param name="errorCallback"> HTTPErrorCallback / ʧ�ܻص� </param>
    /// <returns>void</returns>
    public int Post(string url, object data, Action<JSONNode> responseCallback, HTTPErrorCallback errorCallback)
    {

        if (!this._onceEventHandlerLst.ContainsKey(url))
        {
            this._onceEventHandlerLst.Add(url, new List<EventHandlerInfo>());
        }

        int _seqID = CreatSeqID();

        this._onceEventHandlerLst[url].Add(new EventHandlerInfo(url, data, responseCallback, errorCallback, _seqID));
        this.SendMsg(url, data, _seqID);
        return _seqID;
    }


    public int Get(string url, Action<JSONNode> responseCallback, HTTPErrorCallback errorCallback)
    {
        string rpcName = url.Split('?')[0];
        string pattern = @"(\?|&)([^=]+)=([^&]*)";

        MatchCollection matches = Regex.Matches(url, pattern);

        Dictionary<string, string> parameters = new Dictionary<string, string>();

        foreach (Match match in matches)
        {
            string key = match.Groups[2].Value;// ƥ�䵽�Ĳ�����  
            string value = match.Groups[3].Value;// ƥ�䵽�Ĳ���ֵ  
            parameters.Add(key, value);
        }

        return this.Post(rpcName, parameters, responseCallback, errorCallback);
    }

    public void RemoveRequestAt(int mark)
    {
        foreach (var kv in this._onceEventHandlerLst)
        {
            int i = 0;
            while (i < this._onceEventHandlerLst[kv.Key].Count)
            {
                if (this._onceEventHandlerLst[kv.Key][i].seqID == mark)
                {
                    this._onceEventHandlerLst[kv.Key].RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}

