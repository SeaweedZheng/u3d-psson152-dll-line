using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;
using SimpleJSON;
using System.Threading.Tasks;
using System.Collections;

namespace MoneyBox
{

    /// <summary>
    /// Ǯ�������
    /// </summary>
    /// <remarks>
    /// * Ǯ�������Ƿ����Ӳ��ϱ�־λ������¼�ɹ���־λ��  ������ ���˿��������
    ///  ��WebSocket������ �� ��û�յ����������� �������¼�ɹ���־λ"
    /// * ��¼�󱣴棬token �� device_id �� device_name.��λ����¼�ɹ���־λ"
    ///   ���ߺ�ÿ��5���Զ�ʹ��device_id �� device_name ������ �����»�ȡ��token
    /// * ��¼�ɹ��󣬿�ʼ��������
    /// * token���ڣ�ʹ��ʹ��device_id �� device_name �Զ������� ��ȡ�»�ȡ��token ��λ����¼�ɹ���־λ"��
    /// * ÿ��10�뷢���������� ���������������ء�
    ///   ���������20��û�е����̿������Զ�������ʹ��device_id �� device_name ������ �����»�ȡ��token��
    /// * ������WebSocket-close�����¼����¼�ɹ���־λ = 0��  �� 
    ///   ��������ʱ��ǰ�˵��ýӿڸ������������ݡ�Ҫ������ʧ��-���������С�
    /// </remarks>
    public partial class WebSocketClient : MonoBehaviour
    {

        public ClientWebSocket webSocket;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationTokenSource reconnectCancellationTokenSource;

        public bool isEnableReconnect = true;


        public bool isForbidWS = false;

        public string logicUrl = "";

        /// <summary> ��¼������ </summary>
        public bool isLogin => _isLogin;

        bool _isLogin = false;

        //private readonly Dictionary<string, Action<JSONNode>> protocolFunctionDict = new Dictionary<string, Action<JSONNode>>();

        Action<string, JSONNode> rpcDownHandle;



        /*private void _DoBeforeInit()
        {
            // TODO: Add any initialization logic here
            OnServerCloseCallback = OnWebSocketClosed;
        }

        void OnWebSocketClosed()
        {
            //CloseClient();
            Debug.LogWarning("222��⵽�����������ر����ӣ�ִ����Ӧ�����߼�");
        }*/



        private int seqId = 0;

        private int CreatSeqID()
        {
            if (--seqId < -999 || seqId > -900)
                seqId = -900;
            return seqId;
        }
        private void PopSeqID(int id){ }


        /// <summary>
        /// ��¼Э��
        /// </summary>
        public void SendLoginMessage()
        {
            JSONNode data = JSONNode.Parse("{}");
            data["device_id"] = MoneyBoxModel.Instance.machineId; 
            data["device_name"] = MoneyBoxModel.Instance.machineName; 
            data["action_type"] = "login";
            data["msgid"] = CreatSeqID(); 
            SendMessageForce("login", data);
        }

        public async void ConnectWS()
        {
            string url = string.Format("ws://{0}", this.logicUrl);

            Debug.LogWarning($"��money box websocket��start connected: {url}");

            if (isForbidWS)
            {
                Debug.LogWarning($"��money box websocket��websocket is forbid");
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();
            reconnectCancellationTokenSource = new CancellationTokenSource();
            isEnableReconnect = true;
            try
            {
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(url), cancellationTokenSource.Token); //����

                Debug.LogWarning("��money box websocket��WebSocket connected");

                OnConnectWS();

                //_StartListeningForMessages();  //�첽����


                // ����WebSocket��������
                byte[] buffer = new byte[65535];
                StringBuilder messageBuilder = new StringBuilder();
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);

                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        messageBuilder.Append(receivedData);

                        if (result.EndOfMessage)
                        {
                            // This means we have received the complete message
                            string completeMessage = messageBuilder.ToString();
                            //Debug.LogWarning($"WebSocket message received: {completeMessage}");
                            _HandleReceivedMessage(completeMessage);

                            // Reset the StringBuilder for the next message
                            messageBuilder.Clear();
                        }
                    }

                    if (result.MessageType == WebSocketMessageType.Close || webSocket.State != WebSocketState.Open)
                    {
                        Debug.LogWarning($"��money box websock��WebSocketMessageType: {result.MessageType.ToString()}; WebSocketState: {webSocket.State.ToString()}");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"��money box websocket��WebSocket connection error: {ex.Message}");

            }
            finally
            {
                OnCloseWS();
            }
        }


        /*
        private async void _StartListeningForMessages()
        {
            try
            {
                byte[] buffer = new byte[65535];
                StringBuilder messageBuilder = new StringBuilder();

                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);

                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        messageBuilder.Append(receivedData);

                        if (result.EndOfMessage)
                        {
                            // This means we have received the complete message
                            string completeMessage = messageBuilder.ToString();
                            //Debug.LogWarning($"WebSocket message received: {completeMessage}");
                            _HandleReceivedMessage(completeMessage);

                            // Reset the StringBuilder for the next message
                            messageBuilder.Clear();
                        }
                    }

                    if (result.MessageType == WebSocketMessageType.Close  || webSocket.State != WebSocketState.Open)
                    {

                        Debug.LogError($"��money box websock��WebSocketMessageType: {result.MessageType.ToString()}; WebSocketState: {webSocket.State.ToString()}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.LogError($"��money box websock��WebSocket receive error: {ex.Message}\n{ex.StackTrace}");
            }
        }*/



        private void _HandleReceivedMessage(string message)
        {
            //string content = string.Format("_HandleReceivedMessage = {0}", message);
            //Debug.LogWarning(content);
            //lastHeartbeatTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            DealProtocolFunc(message);
            // TODO: Handle the received message
        }




        public void SendMessage(string protocol_key, JSONNode req)
        {
            if (!isLogin)
            {
                Debug.LogWarning($"��money box��<color=green>rpc up</color>: {protocol_key} data: {req.ToString()}");

                DoLastTask();
                task = () =>
                {
                    JSONNode res = JSONNode.Parse("{}");
                    res["Code"] = -99;
                    res["ErrMsg"] = "failed to send data without logging into the server";
                    res["msgid"] = req["msgid"];
                    if (req.HasKey("action_type"))
                    {
                        res["action_type"] = req["action_type"];
                    }

                    Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: {protocol_key} data: {res.ToString()}");


                    rpcDownHandle?.Invoke(protocol_key, res);

                    /*if (protocolFunctionDict.ContainsKey(protocol_key))
                    {
                        var protocolFunction = protocolFunctionDict[protocol_key];
                        protocolFunction(res);
                    }*/
                };
                return;
            }

            SendMessageForce(protocol_key, req);
        }

        public void SendMessageForce(string protocol_key, JSONNode req)
        {

            JSONNode jsonNode = JSONNode.Parse("{}");
            jsonNode["protocol_key"] = protocol_key;
            jsonNode["data"] = req;

            //AesManager manager = AesManager.Instance;
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                string message = jsonNode.ToString();

                //message = manager.TryEncrypt(message);
                byte[] iv = AESScript.GenerateIv();
                message = AESScript.Encrypt(message, AESScript.AppPostBase64Key, iv);
                byte[] buffer = Encoding.UTF8.GetBytes(message);

                Debug.LogWarning($"��money box��<color=green>rpc up</color>: {protocol_key} data: {req.ToString()}");

                webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
            }
            else
            {
                // �·���������ʧ�ܣ�
                Debug.LogWarning("��money box websocket��WebSocket connection is not open.");
            }
        }

        void CloseWebSocket()
        {
            _isLogin = false;

            AesManager.Instance.initAesLocal();

            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by user", cancellationTokenSource.Token);
                Debug.LogWarning("��money box websocket��close websocket.");
            }
        }



        const float heatbeatOvertimeS = 22f;
        const float heatbeatIntervalTimeS = 5f;

        /// <summary> �������յ�ʱ�� </summary>
        float heartbeatLastReceptTimeS = 0;
        /// <summary> �������յ�ʱ�� </summary>
        float heartbeatNextSendTimeS = 0;

        Action task = null;

        void DoLastTask()
        {
            if (task != null)
            {
                task.Invoke();
                task = null;
            }
        }


        
        void Update()
        {
            DoLastTask();

            if (_isLogin)
            {
                if ( Time.unscaledTime - heartbeatLastReceptTimeS > heatbeatOvertimeS)
                {
                    StopHeartbeat();  //������ʱ���ر�websocket��������
                }
                else if (Time.unscaledTime - heartbeatNextSendTimeS > 0) 
                {
                    heartbeatNextSendTimeS = Time.unscaledTime + heatbeatIntervalTimeS;
                    _SendHeartbeat();  // ��������
                }
            }
        }

        private void _SendHeartbeat()
        {
            JSONNode data = JSONNode.Parse("{}");
            data["device_name"] = MoneyBoxModel.Instance.machineName;
            data["device_id"] = MoneyBoxModel.Instance.machineId; 
            data["access_token"] = MoneyBoxModel.Instance.accessToken; 
            data["msgid"] = CreatSeqID();

            data["action_data"] = JSONNode.Parse("{}");
            data["action_data"]["time_stamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            SendMessage("ping", data);
        }



        public void StopHeartbeat()
        {
            Debug.LogWarning("��money box websocket��stop ping��close websocket");
            //isHeartbeat = false;
            //lastHeartbeatTime = 0;
            CloseWebSocket();
        }


        private void OnDestroy()
        {
            Debug.LogWarning("��money box websocket��on destroy");

            isForbidWS = true;
            isEnableReconnect = false;
            _isLogin = false;

            CloseWebSocket();
            Dispose();
        }

        public void Dispose()
        {
            //Debug.LogWarning("WebSocketClient Dispose");
            //StopHeartbeat();
            if (webSocket != null)
            {
                webSocket.Dispose();
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Dispose();
            }

            if (reconnectCancellationTokenSource != null)
            {
                reconnectCancellationTokenSource.Dispose();
            }
        }

        /*public void RegisterProtocol(string protocolKey, Action<JSONNode> protocolFunc)
        {
            protocolFunctionDict[protocolKey] = protocolFunc;
        }*/

        public void SetRpcDownHandle(Action<string, JSONNode> func)
        {
            Debug.LogWarning($"��money box websocket�� set rpc down handle");
            rpcDownHandle = func;
        }


        public void ClearRpcDownHandle() {
            Debug.LogWarning($"��money box websocket�� clear rpc down handle");
            rpcDownHandle = null;
        } 

        private void DealProtocolFunc(string messageStr)
        {

            JSONNode dataDict = null;
            try
            {
                string targetStr = AESScript.Decrypt(messageStr, AESScript.AppPostBase64Key);
                dataDict = JSONNode.Parse(targetStr);
            }
            catch (Exception ex)
            {
                // ����ʧ��
                Debug.LogError($"��money box��<color=yellow>rpc down</color>: Error reading message: {messageStr}\n{ex}");
                return;
            }

            if (dataDict == null)
            {
                Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'protocol_key' not find: {dataDict.ToString()}");
                return;
            }
  


            /* ## ���ݸ�ʽ���:
            if (!dataDict.HasKey("protocol_key"))
            {
                Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'protocol_key' not find: {dataDict.ToString()}");
                return;
            }

            if (!dataDict.HasKey("data"))
            {
                Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'data' not find: {dataDict.ToString()}");
                return;
            }

            if (!dataDict["data"].HasKey("Code"))
            {
                Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'data/Code' not find: {dataDict.ToString()}");
                return;
            }

            if (!dataDict["data"].HasKey("msgid"))
            {
                Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'data/msgid' not find: {dataDict.ToString()}");
                return;
            }

            if (!dataDict["data"].HasKey("ErrMsg"))
            {
                Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'data/ErrMsg' not find: {dataDict.ToString()}");
                return;
            }*/


            var protocolKey = (string)dataDict["protocol_key"];
            JSONNode res = dataDict.HasKey("data") ? dataDict["data"] : null;

            // ���������ܵ����ݷ���
            if (res.HasKey("ResentData"))
            {
                string DecryptStr = AESScript.Decrypt(res["ResentData"], AESScript.AppPostBase64Key);
                JSONNode ReturnData = JSONNode.Parse(DecryptStr);
                res["Data"] = ReturnData["Data"];
            }

            Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: {protocolKey} data: {res.ToString()}");

            //string rpcName = res.HasKey("action_type") ? (string)res["action_type"] : protocolKey;
            // rpcName == MoneyBoxRPCName.CassetteReturn ?  (string)data["action_type"] : rpcName;

            OnWebSockDown(protocolKey, res);

            rpcDownHandle?.Invoke(protocolKey, res);

            /*
            if (protocolFunctionDict.ContainsKey(protocolKey))
            {
                var protocolFunction = protocolFunctionDict[protocolKey];
                protocolFunction(res);
            }*/
        }

        /*
        public void DealProtocolFunc001(string messageStr)
        {
            try
            {
                // ��������ݷ���
                string targetStr = AESScript.Decrypt(messageStr, AESScript.AppPostBase64Key);
                JSONNode dataDict = JSONNode.Parse(targetStr);

                if (dataDict.HasKey("protocol_key"))
                {
                    var protocolKey = (string)dataDict["protocol_key"];
                    JSONNode res = dataDict.HasKey("data") ? dataDict["data"] : null;


                    // ���������ܵ����ݷ���
                    if (res.HasKey("ResentData"))
                    {
                        string DecryptStr = AESScript.Decrypt(res["ResentData"], AESScript.AppPostBase64Key);
                        JSONNode ReturnData = JSONNode.Parse(DecryptStr);
                        res["Data"] = ReturnData["Data"];
                    }

                    Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: {protocolKey} data: {res.ToString()}");


                    //string rpcName = res.HasKey("action_type") ? (string)res["action_type"] : protocolKey;
                    // rpcName == MoneyBoxRPCName.CassetteReturn ?  (string)data["action_type"] : rpcName;


                    OnWebSockDown(protocolKey, res);

                    if (protocolFunctionDict.ContainsKey(protocolKey))
                    {
                        var protocolFunction = protocolFunctionDict[protocolKey];
                        protocolFunction(res);
                    }

                }
                else
                {
                    Debug.LogWarning($"��money box��<color=yellow>rpc down</color>: 'protocol_key' not registered: {targetStr}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"��money box��<color=yellow>rpc down</color>: Error reading message: {messageStr}\n{ex}");
            }
        }
        */


        public void ForceClose()
        {
            _isLogin = false;
            isEnableReconnect = false;
            CloseWebSocket();
        }

        private void StartReconnect()
        {
            if (isEnableReconnect)
            {
                Debug.LogWarning("��money box websocket��Start reconnecting...");
                Reconnect();
            }
        }
  
        private async void Reconnect()
        {
            try
            {
                // ��ʱ5��
                await Task.Delay(TimeSpan.FromSeconds(5), reconnectCancellationTokenSource.Token);

                /*if (webSocket.State != WebSocketState.Open)
                {
                    ConnectWS();
                }*/
                Debug.LogWarning("��money box websocket��Reconnection...");
                ConnectWS();

            }
            catch (TaskCanceledException)
            {
                //Debug.LogWarning("��money box websocket��Reconnection canceled");
            }
        }
    }


    public partial class WebSocketClient : MonoBehaviour
    {

        /// <summary>  ������� </summary>
        private void OnConnectWS()
        {
            Debug.LogWarning("��money box websocket��OnConnectWS");

            _isLogin = false;
            SendLoginMessage();
        }

        /// <summary> ���ӹر� </summary>
        private void OnCloseWS()
        {
            Debug.LogWarning("��money box websocket��OnCloseWS");

            _isLogin = false;

            AesManager.Instance.initAesLocal();

            StartReconnect();
        }


        /// <summary>
        /// web socket ��������
        /// </summary>
        /// <param name="rpcName"></param>
        /// <param name="data"></param>
        private void OnWebSockDown(string rpcName, JSONNode data)
        {
            string _rpcName = data.HasKey("action_type") ? (string)data["action_type"] : rpcName;

            int code = (int)data["Code"];

            PopSeqID(code);

            switch (_rpcName)
            {
                case "ping":
                    {
                        //## if(code == 0)
                            heartbeatLastReceptTimeS = Time.unscaledTime;
                    }
                    break;
                case "login":
                    {
                        if (code == 0  && data["Data"].HasKey("access_token"))
                        {
                            heartbeatLastReceptTimeS = Time.unscaledTime;
                            if(_corLogin != null)
                            {
                                StopCoroutine(_corLogin);
                                _corLogin = null;
                            }

                            Debug.LogWarning($"��money box websocket��login succeeded");

                            string token = data["Data"]["access_token"];
                            MoneyBoxModel.Instance.accessToken = token;
                            _isLogin = true;
                        }
                        else 
                        {
                            if (!data["Data"].HasKey("access_token"))
                            {
                                //Debug.LogError("");
                            }
                            _corLogin = StartCoroutine(DelayTask(SendLoginMessage,2));
                        }
                    }
                    break;
            }   
        }


        Coroutine _corLogin;
        IEnumerator DelayTask(Action tk , float tiemS = 0.2f)
        {
            yield return new WaitForSecondsRealtime(tiemS);
            tk();
        }
      
    }
}

// # �����⡿��
// * ������дû��
// * ����û�����µ�¼������
// * ������Ӧ�����ڵ�¼����������
// * ��¼������ʧ��û���ظ���¼


/*
 * 
// ����
{
	"protocol_key":"ping",
	"data":{
		"device_name":"GOOD FORTUNE RETURNS",
		"device_id":"11009001",
		"access_token":"gm_token:520",
		"msgid":-901,
		"action_data":{"time_stamp":1744800173228}
	}
}

// ����
{ 
	"protocol_key":"ping",
	"data":{
		"Code": 0,
		"ErrMsg": "",
		"msgid": -902,
		"Data":{}, 
	}
}

//  {"protocol_key":"ping","data":{}}

 * 
 */