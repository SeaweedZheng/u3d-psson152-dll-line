using Newtonsoft.Json;
using GameMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using Sirenix.OdinInspector;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;



/*
 1. App ���� ��̨ҪҪ����ĳ����Ϣ�� mqtt/clientid/sub  -- ��Ϣ����A ��
 2. ��̨������Ϣ��App �� mqtt/clientid/pub -- ��Ϣ����A �� 
 * 
 * 
 */



/*
 * 
 * 
ָ�����⣺mqtt/clientid/sub
��Ӧ���⣺mqtt/clientid/pub
�豸����֪ͨ���⣺mqtt/clientid/state

*/


public enum MqttAppType
{
    /// <summary> App ���ƶ� </summary>
    IsCtrlApp,
    /// <summary> ��̨�� </summary>
    IsGameApp,
}
public class MqttRemoteCtrlMethod
{

    public const string Off = "Off";
    public const string On = "On";

    //==== App����������


    /// <summary> �˱����� </summary>
    public const string GetCoinCount = "GetCoinCount";
    /// <summary> �ʽ����� </summary>
    public const string GetBonus = "GetBonus";



    /// <summary> ��ο��� </summary>
    public const string Wiper = "Wiper";
    /// <summary> ��Ϸ�Ѷ� </summary>
    public const string Setlevel = "Setlevel";



    public const string PlayGame = "PlayGame";
    



    // ==== ��̨�����ϱ�
    /// <summary> �ʽ������ϱ� </summary>
    public const string Bonus = "Bonus";





    // ==== �°汾 ====��

    // ==== ��ťָ��
    public const string BtnTicketOut = "BtnTicketOut";
    public const string BtnSpin = "BtnSpin";
    public const string BtnAuto = "BtnAuto";
    public const string BtnBetUp = "BtnBetUp";
    public const string BtnBetDown = "BtnBetDown";
    public const string BtnBetMax = "BtnBetMax";
    public const string BtnTable = "BtnTable";
    public const string BtnPrevious = "BtnPrevious";
    public const string BtnNext = "BtnNext";
    public const string BtnExit = "BtnExit";
    public const string BtnSwitch = "BtnSwitch";

    // ==== ��ѯָ��
    public const string GetMachineInfo = "GetMachineInfo";
    public const string GetErrorCode = "GetErrorCode";
    public const string GetGameState = "GetGameState";
    //public const string GetGameJackpot = "GetGameJackpot";

    // ==== Ͷ�˱�ָ��
    /// <summary> Ͷ������ </summary>
    public const string AddCoins = "AddCoins";

    // ==== �豸�ϱ�
    public const string Error = "Error";
    public const string Report = "Report";

    // ==== ����ָ��

}


public partial class MqttRemoteController : MonoSingleton<MqttRemoteController>
{

    public string TOPIC_A2M(string id) => $"mqtt/{id}/sub";
    public string TOPIC_M2A(string id) => $"mqtt/{id}/pub";

    public const string TOPIC_M2A_DEVICE_ON = "mqtt/state/on";
    public const string TOPIC_M2A_DEVICE_OFF = "mqtt/state/off";



    [Header("MQTT Settings")]
    [SerializeField] public string brokerAddress = "192.168.3.174";
    [SerializeField] public int brokerPort = 1883;
    [SerializeField] public string selfClientId = "tomappBB";
    [SerializeField] private float reconnectInterval = 5f;
    public string targetClientId = "";

    public string username = "";
    public string password = "";

    public MqttAppType appType;

    private MqttClient _client;
    bool _isConnected;
    public bool isConnected => _isConnected;

    /// <summary> �������� </summary>
    bool _isUseReconnect = true;


    // -1 : û�У� 0 : ϵͳ
    private int _seqID = 0;
    private int CreatSeqID()
    {
        if (++this._seqID > 10000)
            this._seqID = 1;
        return _seqID;
    }
    /// <summary> �����seqId </summary>
    /// <remarks>
    /// * RESP �� NOTIFY������Ҫ��Ӧ
    /// * REQ ��Ҫ����Ӧ
    /// </remarks>
    Dictionary<int, float> requestSeqIds = new Dictionary<int, float>();


    public void Init(string brokerAddress, int brokerPort, MqttAppType type, string selfClientId , string username, string password)
    {
        string tp = type == MqttAppType.IsGameApp ? "machine" : "app";
        Debug.LogWarning($"��Mqtt Remote Button��Init {brokerAddress} {brokerPort} {tp} {selfClientId} {username} {password}");

        this.brokerAddress = brokerAddress;
        this.brokerPort = brokerPort;
        this.selfClientId = selfClientId;
        this.appType = type;
        this.username = username;
        this.password = password;
        _isUseReconnect = true;

        Connect();
    }


    public void Close()
    {
        if (_corConnect != null)
            StopCoroutine(_corConnect);
        _corConnect = null;

        _isUseReconnect = false;

        ClearConnect();
    }


    void ClearConnect()
    {
        try
        {
            if (_client != null)
            {

                if(_isConnected && appType == MqttAppType.IsGameApp)
                {
                    _client.Publish(
                        TOPIC_M2A_DEVICE_OFF,
                        Encoding.UTF8.GetBytes(selfClientId),
                        MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                        false
                    );
                    Debug.LogWarning($"��Mqtt Remote Button��<color=green>rpc up</color>  topic: {TOPIC_M2A_DEVICE_OFF}  payload: {selfClientId}");
                }

                _client.Disconnect();
                _client.MqttMsgPublishReceived -= OnMessageReceived;
                _client.ConnectionClosed -= OnDisconnected;
            }
        }
        catch (Exception ex){ }
        _client = null;

        _isConnected = false;
    }


    float CONNECTION_TIMEOUT = 10f; // 10�볬ʱ


    int _connectNumber = 0;
    public void Connect()
    {
        if (_corConnect != null)
            StopCoroutine(_corConnect);
        _corConnect = StartCoroutine(DoConnect());
    }


    Coroutine _corConnect = null;

    IEnumerator DoConnect()
    {

        if (++_connectNumber > 1000)
            _connectNumber = 0;

        bool shouldReconnect = true;

        StopHeartbeat();
        ClearConnect();

        Debug.LogWarning($"��Mqtt Remote Button������  ip:{brokerAddress}  port:{brokerPort}  number:{_connectNumber}");

        _client = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);
        _client.MqttMsgPublishReceived += OnMessageReceived;
        _client.ConnectionClosed += OnDisconnected;
        int connectNumber = _connectNumber;

        // ������ӳ�ʱ
        bool isNext = false;
        bool isConnected = false;

        Task workerTask = Task.Run(() =>
        {
            try
            {
                _client.Connect(selfClientId, username, password); //�������ɶ���
                isConnected = _client.IsConnected;
                isNext = true; // ��ɼ���
            }
            catch (Exception ex)
            {
                isConnected = false;
                isNext = true;  // �������
            }
        });

        float lastRunTimeS = Time.unscaledTime;
        yield return new WaitUntil(()=> isNext == true || (Time.unscaledTime - lastRunTimeS > CONNECTION_TIMEOUT) );

        if (connectNumber != _connectNumber)  //�û��Ͽ���������
        {
            shouldReconnect = false;
            yield break;
        }

        if (isNext == false && Time.unscaledTime - lastRunTimeS > CONNECTION_TIMEOUT)
        {
            Debug.LogWarning($"��Mqtt Remote Button��MQTT���ӳ�ʱ��{CONNECTION_TIMEOUT}�룩 number:{_connectNumber}");
            shouldReconnect = true;
        }
        else
        {

            if (isConnected)
            {
                Debug.LogWarning($"��Mqtt Remote Button��MQTT���ӳɹ�  number:{_connectNumber}");
                shouldReconnect = false;
                HandleConnectionSuccess();
            }
            else
            {
                Debug.LogWarning($"��Mqtt Remote Button��MQTT����ʧ��  number:{_connectNumber}");
                shouldReconnect = true;
            }
        }

        if (shouldReconnect)
        {
            ClearConnect();
            // ��ʱ�����Ͽ����ӳ���
            ScheduleReconnect();
        }
    }






    /// <summary>
    /// ������
    /// </summary>
    private void HandleConnectionSuccess()
    {
        Debug.LogWarning($"��Mqtt Remote Button��MQTT���ӳɹ� ip: {brokerAddress}  port: {brokerPort}  username: {username}  password: {password}");

        _isConnected = true;

        //Loom.QueueOnMainThread((res) => EventCenter.Instance.EventTrigger(MqttEventHandle.MQTTConnected), null);

        // ������Ϣ
        if (appType == MqttAppType.IsCtrlApp)
        {
            SubscribeTopics(new[] {
                TOPIC_M2A_DEVICE_ON,
                TOPIC_M2A_DEVICE_OFF,
            });
        }
        else
        {
            StartHeartbeat();
            SubscribeTopics(new[] {
                TOPIC_M2A_DEVICE_ON,
                TOPIC_A2M(selfClientId),
                //MQTT_M2A(selfClientId),
                //$"mqtt/{selfClientId}/{MqttEventCmd.Request}",
                //$"mqtt/{selfClientId}/{MqttEventCmd.Respond}",
            });
        }
    }

    /// <summary>
    /// ���Ķ����Ϣ
    /// </summary>
    /// <param name="topics"></param>
    private void SubscribeTopics(string[] topics)
    {
        byte[] qosLevels = new byte[topics.Length];
        for (int i = 0; i < qosLevels.Length; i++)
        {
            qosLevels[i] = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE;
        }
        _client.Subscribe(topics, qosLevels);
    }


    private void SubscribeOneTopic(string additionalTopic = null)
    {
        List<string> topics = new List<string>();
        if (!string.IsNullOrEmpty(additionalTopic))
        {
            topics.Add(additionalTopic);
        }
        byte[] qosLevels = new byte[topics.Count];
        for (int i = 0; i < qosLevels.Length; i++)
        {
            qosLevels[i] = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE;
        }
        _client.Subscribe(topics.ToArray(), qosLevels);
    }



    private void StopHeartbeat()
    {
        if (_corHeartbeat != null)
            StopCoroutine(_corHeartbeat);
        _corHeartbeat = null;
    }

    Coroutine _corHeartbeat = null;
    private void StartHeartbeat()
    {
        StopHeartbeat();
        _corHeartbeat = StartCoroutine(OnSendHeartbeat());
    }

    IEnumerator OnSendHeartbeat()
    {
        while (true)
        {
            SendHeartbeat();
            yield return new WaitForSecondsRealtime(5f);
        }
    }

    private void SendHeartbeat()
    {
        if (!_isConnected) return;

        string topic = TOPIC_M2A_DEVICE_ON;

        _client.Publish(
            //MqttEventCmd.DeviceOn,
            topic,
            Encoding.UTF8.GetBytes(selfClientId),
            MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
            false
        );

        Debug.LogWarning($"��Mqtt Remote Button��<color=green>rpc up</color>  topic: {topic}  payload: {selfClientId}");
    }



    /// <summary>
    /// ���ܵ�����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        // Debug.Log("topic:"+e.Topic+": " +e.Message);
        string topic = e.Topic;
        string payload = Encoding.UTF8.GetString(e.Message);

        Loom.QueueOnMainThread((res) =>
        {
            try
            {
                HandleCommandResponse(topic, payload);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"��Mqtt Remote Button����Ϣ�������: {ex.Message}");
            }
        }, null);

    }

    /// <summary> App�˷��� </summary>
    const string REQ = "req";
    /// <summary> �豸����Ӧ </summary>
    const string RESP = "resp";
    /// <summary> �豸��֪ͨ </summary>
    const string NOTIFY = "notify";

    //======================== ��Ϣ������� ========================//
    private void HandleCommandResponse(string topic, string payload)
    {
        Debug.LogWarning($"��Mqtt Remote Button��<color=yellow>rpc down</color>  topic: {topic}  payload: {payload}");


        if (appType == MqttAppType.IsCtrlApp)  // app
        {
            /*if (msgtype != NOTIFY 
                && msgtype != RESP 
                && topic != MQTT_M2A_DEVICE_OFF 
                && topic != MQTT_M2A_DEVICE_ON) return;*/


            if (topic == TOPIC_M2A_DEVICE_OFF || topic == TOPIC_M2A_DEVICE_ON)
            {
               EventCenter.Instance.EventTrigger<EventData>("ON_MQTT_REMOTE_CONTROL_EVENT",
                   new EventData<string>(  topic == TOPIC_M2A_DEVICE_OFF ? MqttRemoteCtrlMethod.Off : MqttRemoteCtrlMethod.On, payload));
            }
            else
            {

                JObject res = JObject.Parse(payload);
                string msgtype = res["msgtype"].ToObject<string>();
                string method = res["method"].ToObject<string>();
                int seqid = res["seq_id"].ToObject<int>();

                /*bool isOk = true;
                switch (method)
                {
                    case MqttRemoteCtrlMethod.Bonus:
                        break;
                    default:
                        isOk = false;
                        break;
                }

                if (isOk)*/
                    EventCenter.Instance.EventTrigger<EventData>("ON_MQTT_REMOTE_CONTROL_EVENT",
                        new EventData<string>(method, payload));
            }

        }
        else  // ����
        {
            /* if (msgtype != REQ  
                && topic != MQTT_M2A_DEVICE_OFF
                && topic != MQTT_M2A_DEVICE_ON) return;*/

            if (topic == TOPIC_M2A_DEVICE_OFF || topic == TOPIC_M2A_DEVICE_ON)
            {
                //EventCenter.Instance.EventTrigger<EventData>("ON_MQTT_REMOTE_CONTROL_EVENT", new EventData<string>( topic == MQTT_M2A_DEVICE_OFF ? MqttRemoteCtrlMethod.Off : MqttRemoteCtrlMethod.On, payload));
            }
            else
            {


                JObject res = JObject.Parse(payload);
                string msgtype = res["msgtype"].ToObject<string>();
                string method = res["method"].ToObject<string>();
                int seqid = res["seq_id"].ToObject<int>();

               /* bool isOk = true;
                switch (method)
                {
                    // Mqtt
                    case MqttRemoteCtrlMethod.AddCoins:
                        break;
                    case MqttRemoteCtrlMethod.GetCoinCount:
                        break;
                    case MqttRemoteCtrlMethod.GetBonus:
                        break;
                    default:
                        isOk = false;
                        break;
                }
                if (isOk)*/
                    EventCenter.Instance.EventTrigger<EventData>("ON_MQTT_REMOTE_CONTROL_EVENT",
                        new EventData<string>(method, payload));
            }

        }
    }


    public void SetTargetClientId(string clientId)
    {
        if (appType != MqttAppType.IsCtrlApp)
            return;

        if (!string.IsNullOrEmpty(targetClientId))  //ȡ���ɵĶ���
        {
            _client.Unsubscribe(new string[] { $"mqtt/{targetClientId}/pub" });
        }

        targetClientId = clientId;
        SubscribeOneTopic($"mqtt/{targetClientId}/pub");
    }

    public void ClearTargetClientId()
    {
        if (appType != MqttAppType.IsCtrlApp)
            return;
        if (!string.IsNullOrEmpty(targetClientId))  //ȡ���ɵĶ���
        {
            _client.Unsubscribe(new string[] { $"mqtt/{targetClientId}/pub" });
        }
        targetClientId = null;
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="method"></param>
    /// <param name="data"></param>
    public int RequestCommand(string method, object data)
    {
        if (!_isConnected)
        {
            Debug.LogWarning("��Mqtt Remote Button��MQTTδ����");
            return -1;
        }
        if (appType == MqttAppType.IsCtrlApp && string.IsNullOrEmpty(targetClientId))
        {
            Debug.LogWarning("��Mqtt Remote Button��û������ targetClientId");
            return -1;
        }

        JArray body = new JArray();
        if (data != null)
        {
            //string dataStr = JsonConvert.SerializeObject(data);
            string dataStr = data is string ? (string)data : JsonConvert.SerializeObject(data);
            body = JArray.Parse($"[{dataStr}]");
        }



        int seqId = CreatSeqID();
        JObject req = JObject.Parse("{}");
        req["msgtype"] = appType == MqttAppType.IsCtrlApp ? REQ : NOTIFY;
        req["method"] = method;
        req["body"] = body;
        req["seq_id"] = seqId;
        //string topic = $"mqtt/{targetClientId}/{MqttEventCmd.Request}";

        string topic = appType == MqttAppType.IsCtrlApp ?
            TOPIC_A2M(targetClientId) : TOPIC_M2A(selfClientId);


        //string payload = req.ToString(); //���ڸ�ʽ
        string payload = JsonConvert.SerializeObject(req, Formatting.None); //ȥ����ʽ

        _client.Publish(
            topic,
            Encoding.UTF8.GetBytes(payload),
            MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
            false
        );
        Debug.LogWarning($"��Mqtt Remote Button��<color=green>rpc up</color>  topic: {topic}  payload: {payload}");

        return seqId;
    }

    /// <summary>
    /// ��Ӧ����
    /// </summary>
    /// <param name="method"></param>
    /// <param name="data"></param>
    /// <param name="seqId"></param>

    public void RespondCommand(string method, object data, int seqId)
    {
        if (!_isConnected)
        {
            Debug.LogWarning("��Mqtt Remote Button��MQTTδ����");
            return;
        }
        if (appType == MqttAppType.IsCtrlApp && string.IsNullOrEmpty(targetClientId))
        {
            Debug.LogWarning("��Mqtt Remote Button��û������ targetClientId");
            return;
        }


        //JArray body = JArray.Parse("[]");
        //body.Add(data);

        JArray body = new JArray();
        if (data != null)
        {
            string dataStr = data is string? (string)data: JsonConvert.SerializeObject(data);
            body = JArray.Parse($"[{dataStr}]");
        }

        JObject res = JObject.Parse("{}");
        res["msgtype"] = appType == MqttAppType.IsCtrlApp ? REQ : RESP;
        res["method"] = method;
        res["seq_id"] = seqId;
        res["body"] = body;


        string topic = appType == MqttAppType.IsCtrlApp ?
            TOPIC_A2M(targetClientId) : TOPIC_M2A(selfClientId);


        //string payload = res.ToString(); //���ڸ�ʽ
        string payload = JsonConvert.SerializeObject(res, Formatting.None); //ȥ����ʽ

        _client.Publish(
            topic,
            Encoding.UTF8.GetBytes(payload),
            MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
            false
        );
        Debug.LogWarning($"��Mqtt Remote Button��<color=green>rpc up</color>  topic: {topic}  payload: {payload}");
    }

    private void OnDisconnected(object sender, EventArgs e)
    {
        _isConnected = false;

        Loom.QueueOnMainThread((res) =>
        {
            EventCenter.Instance.EventTrigger(MqttEventHandle.MQTTDisconnected);
            ScheduleReconnect();
        }, null);
    }

    private void ScheduleReconnect()
    {
        if (_isUseReconnect)
        {
            Debug.LogWarning("��Mqtt Remote Button��������..");
            Invoke(nameof(Connect), reconnectInterval);
        }
    }


    protected override void OnDestroy()
    {
        ClearConnect();

        CancelInvoke();

        base.OnDestroy();
    }


}


public partial class MqttRemoteController : MonoSingleton<MqttRemoteController>
{

    public void TestA2MRequestCommand(string method, object data)
    {
        if (!_isConnected)
        {
            Debug.LogWarning("��Mqtt Remote Button��MQTTδ����");
            return;
        }

        JArray body01 = new JArray();
        body01.Add(new JObject
        {
            ["code"] = 0
        });

        //JArray body = new JArray.Parse("[]");
        /* ������뱨��
        JArray body = new JArray();
        if (data != null) {
            string dataStr = JsonConvert.SerializeObject(data);
            JObject dat = JObject.Parse(dataStr);
            body.Add(dat);
        }*/
        JArray body = new JArray();
        if (data != null)
        {
            //string dataStr = JsonConvert.SerializeObject(data);
            string dataStr = data is string ? (string)data : JsonConvert.SerializeObject(data);
            body = JArray.Parse($"[{dataStr}]");
        }


        JObject req = JObject.Parse("{}");
        req["msgtype"] = REQ;
        req["method"] = method;
        req["body"] = body;
        req["seq_id"] = CreatSeqID();

        targetClientId = selfClientId;
        string topic = TOPIC_A2M(targetClientId);

        //string payload = req.ToString(); //���ڸ�ʽ
        string payload = JsonConvert.SerializeObject(req, Formatting.None); //ȥ����ʽ

        _client.Publish(
            topic,
            Encoding.UTF8.GetBytes(payload),
            MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
            false
        );
        Debug.LogWarning($"��Mqtt Remote Button Test A2M��rpc up  topic: {topic}  payload: {payload}");
    }





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


    [Button]
    void TestA2MSendBtnTicketOut()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnTicketOut, null);
    }


    [Button]
    void TestA2MSendBtnSpin()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnSpin, null);
    }

    [Button]
    void TestA2MSendBtnAuto()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnAuto, null);
    }

    [Button]
    void TestA2MSendBtnExit()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnExit, null);
    }



    [Button]
    void TestA2MSendBtnBetUp()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnBetUp, null);
    }

    [Button]
    void TestA2MSendBtnBetDown()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnBetDown, null);
    }
    [Button]
    void TestA2MSendBtnBetMax()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnBetMax, null);
    }

    [Button]
    void TestA2MSendBtnTable()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnTable, null);
    }

    [Button]
    void TestA2MSendBtnPrevious()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnPrevious, null);
    }

    [Button]
    void TestA2MSendBtnNext()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnNext, null);
    }


    [Button]
    void TestA2MSendBtnSwitch()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.BtnSwitch, null);
    }



    [Button]
    void TestA2MSendCmdGetGameState()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.GetGameState, null);
    }

    [Button]
    void TestA2MSendCmdGetMachineInfo()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.GetMachineInfo, null);
    }


    [Button]
    void TestA2MSendCmdGetErrorCode()
    {
        targetClientId = selfClientId;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.GetErrorCode, null);
    }


    /*
    [Button]
    void TestA2MReqGetCoinCount()
    {
        targetClientId = selfClientId;
        Dictionary<string, object> data = null;
        TestA2MRequestCommand(MqttRemoteCtrlMethod.GetCoinCount, data);
    }*/

    [Button]
    void TestA2MReqAddCoins()
    {
        targetClientId = selfClientId;
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            ["num"] = 3,
        };
        TestA2MRequestCommand(MqttRemoteCtrlMethod.AddCoins, data);
    }

}

