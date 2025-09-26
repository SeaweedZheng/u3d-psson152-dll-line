using M2MqttUnity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Events;

namespace IOT
{
    /*
     * �豸�����ڰ�����������ƽ̨����ҵʵ���У���Ӧmqtt����������brokerHostName����
     * iot-06****.mqtt.iothub.aliyuncs.com�˿�(brokerPort)��1883,iot-06****ΪiotInstanceId��
     * �ɻ�ȡ�豸�����Ʋ����ӿڷ���
     */
    public class IoTPayment : MonoSingleton<IoTPayment>
    {
        private IoTDevInfo mDevInfo;
        private List<string> eventMessages = new List<string>();
        private bool _isConnected;
        public bool IsConnected { set
            {
                _isConnected = value;
            }
            get
            { return _isConnected; } }

        private string mTopicSend;  //��Ϣ����
        private string mTopicReply; //��Ϣ����

        void Start()
        {
            M2MqttUnityClient.Instance.ConnectionSucceeded += onConnectionSucceeded;
            M2MqttUnityClient.Instance.ConnectionFailed += onConnectionFailed;

            M2MqttUnityClient.Instance.ActionSubscribeTopics += onSubscribeTopics;
            M2MqttUnityClient.Instance.ActionUnsubscribeTopics += onUnsubscribeTopics;

            M2MqttUnityClient.Instance.ActionDecodeMessage += onDecodeMessage;
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        void onDecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
        }

        void onSubscribeTopics()
        {
            M2MqttUnityClient.Instance.AddSubscribeTopics(mTopicReply);
        }

        void onUnsubscribeTopics()
        {
            M2MqttUnityClient.Instance.RemoveSubscribeTopics(mTopicReply);
        }

        void onConnectionSucceeded()
        {
            Debug.LogWarning("��IOT���ÿ�������");
            IsConnected = true;
        }

        void onConnectionFailed()
        {
            Debug.LogWarning("��IOT���ÿ����ӹر�");
            IsConnected = false;
            IOTModel.Instance.LinkIOT = false;
            IOTModel.Instance.LinkId = null;
        }

        private void ProcessMessage(string strMsg)
        {
            IoTMsg msg = JsonConvert.DeserializeObject<IoTMsg>(strMsg);
            if (msg == null || msg.data == null /*|| !msg.data.success*/) return;
            switch (msg.messagecmd)
            {
                case MessageCmd.ONLINE_REPLY:           //�豸ע�᷵��
                    EventCenter.Instance.EventTrigger(IOTEventHandle.REGISTER_DEV, msg.data.qrcodeUrls);
                    break;
                case MessageCmd.INSERT_COIN:      //ƽ̨Ͷ�ҷ���
                    CoinData coinData = new CoinData();
                    coinData.Num = msg.data.num;
                    coinData.orderNum = msg.data.orderNum;
                    coinData.memberId = msg.data.memberId;
                    IOTModel.Instance.LinkId = msg.data.orderNum;
                    EventCenter.Instance.EventTrigger(IOTEventHandle.COINT_IN, coinData);
                    break;
                case MessageCmd.DEVICE_PRIZE_REPLY:     //�豸�˲�
                    TicketOutData tod = new TicketOutData();
                    tod.code = msg.data.code;
                    tod.success = msg.data.success;
                    tod.message = msg.data.message;
                    tod.num = msg.data.num;
                    tod.orderNum = msg.data.orderNum;
                    tod.type = msg.data.type;
                    tod.seq = msg.data.seq;
                    IOTModel.Instance.LinkId = null;
                    EventCenter.Instance.EventTrigger(IOTEventHandle.TICKET_OUT, tod);
                    break;
                case MessageCmd.NOTICE:                 //ƽ̨��Ϣ
                    NoticeData noticeData = new NoticeData
                    {
                        code = msg.data.code,
                        message = msg.data.message
                    };
                    EventCenter.Instance.EventTrigger(IOTEventHandle.NOTICE, noticeData);
                    break;
                default: break;
            }
        }


        void Update()
        {
            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
        }

        string MakeMsgId()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="macId">������̨��</param>
        /// <param name="portId">�ֻ���̨��</param>
        /// <param name="accessType">accessType���豸���뷽ʽ��1�����ں�(Ĭ��Ϊ1) 2��С���� 3��Լսƽ̨</param>
        /// <param name="signal">�ź�ֵ������0~32֮�䣬32���ź���ã�һ�������Ϊ32�������wifi��������Ĺ����ź�ֵ13-16Ϊǿ���ź�ֵ9-12Ϊ�У��ź�ֵ8����Ϊ��</param>
        /// <param name="version"></param>
        /// <param name="isConnectTestSever">�Ƿ�������ʽ��</param>
        /// <param name="onErrorCallback">����ص�</param>
        public void Init(string macId, int portId, int accessType, int signal, int version ,Action<string> onErrorCallback)
        {

            onConnectionFailed();

            Debug.LogWarning($"��IOT��connect iot;  machineID: {macId} - portId: {portId} - accessType: {accessType} - signal: {signal} - version: {version}");

            bool checkSuccess = CheckDeviceInfo(macId, IoTConst.Secret,out string msg);
            if (checkSuccess) {
                StartCoroutine(RegistDevcie(macId, portId, accessType, signal, version));
            }
            else
            {
                //Debug.LogError($"msg: {msg}");
                onErrorCallback?.Invoke(msg);
            }
        }

        public void Disconnect(){
            //M2MqttUnityClient.Instance.ConnectionSucceeded -= onConnectionSucceeded;
            //M2MqttUnityClient.Instance.ConnectionFailed -= onConnectionFailed;
            //M2MqttUnityClient.Instance.ActionSubscribeTopics -= onSubscribeTopics;
            //M2MqttUnityClient.Instance.ActionUnsubscribeTopics -= onUnsubscribeTopics;
            //M2MqttUnityClient.Instance.ActionDecodeMessage -= onDecodeMessage;
            M2MqttUnityClient.Instance.Disconnect();
        }

        private IEnumerator RegistDevcie(string macId, int portId, int accessType, int signal, int version)
        {
            Debug.LogWarning("��IOT��: �ȴ�Mqtt����...");
            yield return new WaitUntil(() => IsConnected);
            Debug.LogWarning("��IOT��: Mqtt�����ϣ���ʼע��!");
            RegisterDevice(macId, portId, accessType, signal, version);
        }

        /// <summary>
        /// �豸ÿ�ο�����ѯһ�Σ���ѯ�ɹ��󱣴��豸�����������ѯ���µ��豸�����������²����滻�ɲ���
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="secret"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckDeviceInfo(string hid,string secret, out string msg)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param["hardwareId"] = hid;
            param["secret"] = secret;

            string url = IoTConst.GetDevParamURL;
            string strRsp = Utils.Post(url, param); // psot����
            //string strRsp = Utils.Post("http://i.gzhaoku.com/admin/resp/firmware/query", param); // psot����
            DevParamRsp rsp = JsonConvert.DeserializeObject<DevParamRsp>(strRsp);
            Debug.Log($"��IOT��post ����: {url}�� {JsonConvert.SerializeObject(param)}");
            Debug.Log($"��IOT��post ��Ӧ: {strRsp}");
            if (rsp.success)
            {
                Debug.LogWarning("��IOT��: post �ɹ�����ʼmqtt���� ");
                SaveDeviceInfo(rsp);  // post�ɹ��󣬿�ʼmqtt����
            }
            else
            {
                Debug.LogError($"��IOT��: post ʧ�ܣ� strRsp: {strRsp} ");
            }
            msg = rsp.message;
            return rsp.success;
        }

        private void SaveDeviceInfo(DevParamRsp rsp)
        {
            mDevInfo = rsp.data;

            mTopicReply = String.Format("/{0}/{1}/user/s2c/play", mDevInfo.ProductKey,mDevInfo.FirmwareName);
            mTopicSend = String.Format("/{0}/{1}/user/c2s/play", mDevInfo.ProductKey, mDevInfo.FirmwareName);
            //����mqtt
            M2MqttUnityClient.Instance.InitMqtt(mDevInfo);
        }

        public IoTDevInfo GetDeviceInfo()
        {
            return mDevInfo;
        }

        /// <summary>
        /// ע���豸
        /// </summary>
        /// <param name="hid">������̨��</param>
        /// <param name="portid">�ֻ���̨��</param>
        /// <param name="accessType">accessType���豸���뷽ʽ��1�����ں�(Ĭ��Ϊ1) 2��С���� 3��Լսƽ̨</param>
        /// <param name="signal">�ź�ֵ������0~32֮�䣬32���ź���ã�һ�������Ϊ32�������wifi��������Ĺ����ź�ֵ13-16Ϊǿ���ź�ֵ9-12Ϊ�У��ź�ֵ8����Ϊ��</param>
        /// <param name="version">�汾��Ϊ����</param>
        public void RegisterDevice(string hid,int portid,int accessType,int signal,int version)
        {
            IoTData data = new IoTData
            {
                accessType = accessType,
                signal = signal,
                hardwareId = hid,
                version = version
            };

            IoTMsg msg = new IoTMsg
            {
                data = data,
                messageid = MakeMsgId(),
                messagetype = MessageType.C2S,
                messagecmd = MessageCmd.ONLINE,
                portid = portid,
                timestamp = (Utils.GetTimeStamp() / 1000).ToString()
            };

            string jsonMsg = JsonConvert.SerializeObject(msg);
            M2MqttUnityClient.Instance.PublishMsg(mTopicSend, jsonMsg);
        }

        /// <summary>
        /// �ظ�ƽ̨Ͷ��(��ָ���豸����ظ�������ظ�ʧ�� ���� ���ظ������˱�)
        /// </summary>
        /// <param name="portid">������̨��</param>
        /// <param name="num">ʵ��Ͷ�ҵ�����</param>
        /// <param name="orderNum">��Ӧ�Ķ�����</param>
        /// <param name="bSuccess">�ɹ�����ʧ��</param>
        /// <param name="strMessage">ʧ��ԭ�� ����ɹ�����д�� "�����ɹ���"</param>
        public void ReplyCoinIn(int portid,int num,string orderNum,bool bSuccess,string strMessage)
        {
            IoTData data = new IoTData
            {
                code = 0,
                success = bSuccess,
                message = strMessage,
                num = num,
                orderNum = orderNum
            };

            IoTMsg msg = new IoTMsg
            {
                data = data,
                messageid = MakeMsgId(),
                messagetype = MessageType.C2S,
                messagecmd = MessageCmd.INSERT_COIN_REPLY,
                portid = portid
            };

            string jsonMsg = JsonConvert.SerializeObject(msg);
            M2MqttUnityClient.Instance.PublishMsg(mTopicSend, jsonMsg);
        }

        /// <summary>
        /// �豸�˲�
        /// ƽ̨�ظ���ʾ�·ּ�¼���յ������û���յ�ƽ̨�ظ���һ�������粻ͨ��������·ּ�¼
        /// ������������������ʱ���ϴ����ٴ��ϴ�ʱ������orderNum �� seq���ܱ䣬ƽ̨����2���ֶ������ء�
        /// </summary>
        /// <param name="portid">������̨��</param>
        /// <param name="ticketOutData">�˱�����</param>
        public void DeviceTicketOut(int portid, TicketOutData ticketOutData, UnityAction<int> errorAction)
        {
            if (string.IsNullOrEmpty(IOTModel.Instance.LinkId))
                errorAction(ticketOutData.num);

            IoTData data = new IoTData
            {
                type = ticketOutData.type,
                ext = ticketOutData.ext,
                seq = ticketOutData.seq,
                num = ticketOutData.num,
                orderNum = IOTModel.Instance.LinkId
            };

            IoTMsg msg = new IoTMsg
            {
                data = data,
                messageid = MakeMsgId(),
                messagetype = MessageType.C2S,
                messagecmd = MessageCmd.DEVICE_PRIZE,
                portid = portid
            };

            string jsonMsg = JsonConvert.SerializeObject(msg);
            M2MqttUnityClient.Instance.PublishMsg(mTopicSend, jsonMsg);
        }
    }

}
