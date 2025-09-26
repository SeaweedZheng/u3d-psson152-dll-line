using GameMaker;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace MoneyBox
{
    public partial class MoneyBoxManager : MonoBehaviour
    {
        private static MoneyBoxManager _instance;

        public static MoneyBoxManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var founds = FindObjectsOfType(typeof(MoneyBoxManager));
                    if (founds.Length > 0)
                    {
                        _instance = (MoneyBoxManager)founds[0];
                        if (_instance.transform.parent == null) // �ж��Ƿ��Ǹ��ڵ㣬
                            DontDestroyOnLoad(_instance.gameObject);
                    }
                    else
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<MoneyBoxManager>();
                        singleton.name = "MoneyBoxManager";    //"(Singleton)" + typeof(MoneyBoxManager).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                }
                return _instance;
            }
        }
    }


    public partial class MoneyBoxManager : MonoBehaviour
    {
        private int seqID = 0;
        private int CreatSeqID()
        {
            List<int> temp = new List<int>();
            foreach (KeyValuePair<string, ResponseInfo> kv in this.dicResponse)
                temp.Add(kv.Value.seqID);
            do
            {
                if (++this.seqID > 10000)
                    this.seqID = 1;
            } while (temp.Contains(seqID));
            return seqID;
        }

        Dictionary<string, ResponseInfo> dicResponse = new Dictionary<string, ResponseInfo>();


        public void OnSuccessResponseData(string eventName, object res)
        {

            //OnDebugRpcDown(eventName, res);

            if (dicResponse.ContainsKey(eventName))
            {
                ResponseInfo info = dicResponse[eventName];
                dicResponse.Remove(eventName);
                info.successCallback?.Invoke(res);
            }
        }

        public void OnErrorResponseData(string eventName, object res)
        {
            //OnDebugRpcDown(eventName, res);

            if (dicResponse.ContainsKey(eventName))
            {
                ResponseInfo info = dicResponse[eventName];
                dicResponse.Remove(eventName);
                info.errorCallback?.Invoke(new BagelCodeError { response = res });
            }
        }



        public void RemoveRequestAt(int seqID)
        {
            int idx = dicResponse.Count;
            while (--idx >= 0)
            {
                KeyValuePair<string, ResponseInfo> item = dicResponse.ElementAt(idx);
                if (item.Value.seqID == seqID)
                {
                    dicResponse.Remove(item.Key);
                }
            }
        }

        public void RemoveRequestAt(string mark)
        {
            int idx = dicResponse.Count;
            while (--idx >= 0)
            {
                KeyValuePair<string, ResponseInfo> item = dicResponse.ElementAt(idx);
                if (item.Value.mark == mark)
                {
                    dicResponse.Remove(item.Key);
                }
            }
        }
    }

    public partial class MoneyBoxManager : MonoBehaviour
    {



        WebSocketClient compWSC;
        UdpClientManager compUDP;



        private void Awake()
        {
            compUDP = gameObject.GetComponent<UdpClientManager>();
            if (compUDP == null)
            {
                compUDP = gameObject.AddComponent<UdpClientManager>();
            }

            compWSC = gameObject.GetComponent<WebSocketClient>();
            if (compWSC == null)
            {
                compWSC = gameObject.AddComponent<WebSocketClient>();
            }

        }




        void Start001()
        {

            /*
            compWSC.RegisterProtocol("login", (res) =>
            {
                OnRpcDown("CassetteReturn", res);

            });

            compWSC.RegisterProtocol("ping", (res) =>
            {
                OnRpcDown("CassetteReturn", res);
            });

            compWSC.RegisterProtocol("CassetteRechange", (res) =>
            {
                OnRpcDown("CassetteReturn", res);
            });

            compWSC.RegisterProtocol("CassetteReturn", (res) =>
            {
                OnRpcDown("CassetteReturn", res);
            });*/
            compWSC.SetRpcDownHandle(OnRpcDown);
        }


        /// <summary> �Ƿ��¼Ǯ������� </summary>
        public bool isLogin => compWSC.isLogin;


        public void Init(string deviceId, string deviceName)
        {
            Start001();

            MoneyBoxModel.Instance.machineName = deviceName;
            MoneyBoxModel.Instance.machineId = deviceId;
            //UdpClientManager udp_client_manager = camera_object.GetComponent<UdpClientManager>();

            // ���� UDP �ͻ���
            compUDP.StartClient();

            // �ͻ��˷��͹㲥��Ϣ
            compUDP.SendBroadcast("Hello, Server!");
        }

        /// <summary> �ر�Ǯ�书�� </summary>
        public void Close()
        {
            compUDP.CloseClient();
            compWSC.ForceClose();
        }



        /*�ψ�Ͷ�n�ϷֽY��
         * 
     {
     "access_token": "gm_token:17",
     "action_data": {
         "money": 1129,
         "orderno": "PRC_1744359846162524200_2959_wXSNefmC46"
     },
     "action_type": "notify_cashin_result",
     "device_id": "test502",
     "device_name": "warrior",
     "msgid": 152
 }
   =======================
{
    "Code": 0,
    "ErrMsg": "HandleSuccess",
    "Data": [],
    "Action": "",
    "action_type": "notify_cashin_result",
    "device_id": "test502",
    "device_name": "warrior",
    "msgid": 152
}
        
         */

        public void OnRpcDown(string rpcName, JSONNode data)
        {

            string actionType = rpcName == MoneyBoxRPCName.CassetteReturn?
                        (string)data["action_type"]:rpcName;

            //Debug.LogError($"��mony box  rpc down��rpc name: {rpcName}  data: {data.ToString()}");

            if ((int)data[ "Code"] != 0)
            {

                //Debug.LogError($"��Money Box Mgr��rpc down: {rpcName}; Err: {data.ToString()}");

                //������
                OnErrorResponseData(actionType, data);
            }
            else
            {
                // ��������
                switch (rpcName)
                {
                    case MoneyBoxRPCName.login:

                        MoneyBoxModel.Instance.moneyPerCoinIn = (float)(data["Data"]["money_per_coin_in"]);
                        MoneyBoxModel.Instance.accessToken = (string)(data["Data"]["access_token"]);
                        MoneyBoxModel.Instance.bankQRCodeAesKey = (string)(data["Data"]["qrcode_key"]);
                        MoneyBoxModel.Instance.coinInNumLst.Clear();

                        if(data["Data"].HasKey("preset_coin_list"))
                            foreach (JSONNode item in data["Data"]["preset_coin_list"])
                            {
                                MoneyBoxModel.Instance.coinInNumLst.Add((int)item);
                            }
                        //Debug.LogWarning($"��Money OK�����յ����ݣ�{data.ToString()}");
                        break;
                    case MoneyBoxRPCName.ping:

                        break;
                    case MoneyBoxRPCName.CassetteReturn:
                        actionType = (string)data["action_type"];
                        break;

                    case MoneyBoxRPCName.CassetteRechange:
                        EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_MONEY_BOX_EVENT, 
                            new EventData<JSONNode> (GlobalEvent.MoneyBoxRequestMachineQRCodeUp, data));
                        break;
                    case MoneyBoxRPCName.create_qrcode:
                    case MoneyBoxRPCName.notify_decr_success:
                    case MoneyBoxRPCName.scan_qrcode:
                    case MoneyBoxRPCName.consume_qrcode:
                    case MoneyBoxRPCName.notify_cashin_result:
                        break;
                }

                //��������
                OnSuccessResponseData(actionType, data);
            }
        }



   
        public void OnRpcUp(string rpcName, JSONNode data, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
        {
            //Debug.LogWarning($"��money box��<color=green>rpc up</color>: {rpcName} data: {data.ToString()}");

            int id = CreatSeqID();

            if (!dicResponse.ContainsKey(rpcName))
                dicResponse.Add(rpcName, new ResponseInfo() { });
            else
            { //���е�ɾ��
              //int seqID = dicResponse[eventName].seqID;
                BagelCodeError res = new BagelCodeError() { msg = "Request is repeated", };
                dicResponse[rpcName].errorCallback?.Invoke(res);
            }
            dicResponse[rpcName].successCallback = successCallback;
            dicResponse[rpcName].errorCallback = errorCallback;
            dicResponse[rpcName].seqID = id;
            dicResponse[rpcName].time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            dicResponse[rpcName].mark = mark;


            // ��������

            JSONNode req = JSONNode.Parse("{}");
            req["access_token"] = MoneyBoxModel.Instance.accessToken;
            req["device_id"] = MoneyBoxModel.Instance.machineId;
            req["device_name"] = MoneyBoxModel.Instance.machineName;
            req["action_type"] = rpcName;
            req["action_data"] = data;
            req["msgid"] = id;

            compWSC.SendMessage(MoneyBoxRPCName.CassetteProcess, req);//rpcName


            /*
            WebSocketClient websocket_client = MoneyBoxManager.Instance.GetComponent<WebSocketClient>();  //camera_object.GetComponent<WebSocketClient>();
            JSONNode comment = JSONNode.Parse("{}");
            JSONNode data = JSONNode.Parse("{}");
            comment["device_id"] = GlobalsManager.Instance.GetStrAttribute(GlobalsManager.DeviceID);
            comment["device_name"] = "";
            comment["access_token"] = access_token;
            data["credit"] = amount;
            data["precode"] = prcode;
            comment["action_data"] = data;
            comment["action_type"] = "recive_cassette_rechange";
            websocket_client.SendMessage("CassetteProcess", comment);*/
        }


        #region ����̨�·ֲ���ӡ��ά��
       
       /// <summary>
       /// �����Ʊ��ά��
       /// </summary>
       /// <param name="money"></param>
       /// <param name="successCallback"></param>
       /// <param name="errorCallback"></param>
       /// <param name="mark"></param>
       public void RequesCreateOrderIdWhenPrintQRCodeOut(int money, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
        {
            JSONNode data = JSONNode.Parse("{}");
            data["money"] = money;
            OnRpcUp(MoneyBoxRPCName.create_qrcode, data, successCallback, errorCallback, mark);
            //compWSC.SendMessage(MoneyBoxRPCName.CassetteProcess, comment);
        }

        /// <summary>
        /// ��ά���Ʊ����ϱ�
        /// </summary>
        /// <param name="qrcodeAsOrderId"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="mark"></param>
        public void RequestSendOrderResultWhenPrintQRCodeOut(string qrcodeAsOrderId, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
        {
            JSONNode data = JSONNode.Parse("{}");
            data["qrcode"] = qrcodeAsOrderId;
            OnRpcUp(MoneyBoxRPCName.notify_decr_success, data, successCallback, errorCallback, mark);
            //compWSC.SendMessage(MoneyBoxRPCName.CassetteProcess, comment);
        }


        #endregion



        //scan_qrcode



        #region ɨ���ά��(��̨��ӡ)�ڱ���̨�Ϸ֡�

        /// <summary>
        /// ɨ�赽��ά��-������������ȷ���Ƿ�����Ч�Ķ�ά�룡
        /// </summary>
        /// <param name="qrcode"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="mark"></param>
        public void RequestCheckQRCodeWhenScanQRCode(string qrcodeAsOrderId, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
        {
            JSONNode data = JSONNode.Parse("{}");
            data["qrcode"] = qrcodeAsOrderId;
            OnRpcUp(MoneyBoxRPCName.scan_qrcode, data, successCallback, errorCallback, mark);
        }


        /// <summary>
        /// ʹ�ö�ά��������Ϸ�
        /// </summary>
        /// <param name="qrcodeAsOrderId"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="mark"></param>
        public void RequestConsumeQRCode(string qrcodeAsOrderId, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
        {
            JSONNode data = JSONNode.Parse("{}");
            data["qrcode"] = qrcodeAsOrderId;
            OnRpcUp(MoneyBoxRPCName.consume_qrcode, data, successCallback, errorCallback, mark);
        }

        #endregion



        #region Ǯ��ָ����̨�Ϸ�

        /// <summary>
        /// ���߽��յ�Ǯ�䷢�͵Ķ�ά��id������֪Ǯ�䡣
        /// </summary>
        /// <param name="qrcodeAsOrderId"></param>
        /// <param name="money"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="mark"></param>
        public void RequestReturnQrderIdWhenMBReqMacQrIn(string qrcodeAsOrderId, int money, string mark = null)
        {
            JSONNode data = JSONNode.Parse("{}");
            data["orderno"] = qrcodeAsOrderId;
            data["money"] = money;
            OnRpcUp(MoneyBoxRPCName.cashin_rechange, data, null, null, mark);
            //compWSC.SendMessage(MoneyBoxRPCName.CassetteProcess, comment);
        }



        /// <summary>
        /// ���ϷֵĶ������ݣ����߸���������
        /// </summary>
        /// <param name="qrcodeAsOrderId"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        /// <param name="mark"></param>
        public void RequestSendOrderWhenMBReqMacQrIn(string qrcodeAsOrderId, int money, Action<object> successCallback, Action<BagelCodeError> errorCallback, string mark = null)
        {
            JSONNode data = JSONNode.Parse("{}");
            data["preset_orderno"] = qrcodeAsOrderId;
            data["money"] = money;
            OnRpcUp(MoneyBoxRPCName.notify_cashin_result, data, successCallback, errorCallback, mark);
            //compWSC.SendMessage(MoneyBoxRPCName.CassetteProcess, comment);
        }




        #endregion






        const string RPC_MARK_MACHINR_MONEY_OUT = "RPC_MARK_MACHINR_MONEY_OUT";

        [Button]
        public void TestRequesCreateBankOrderNumber()
        {

            RequesCreateOrderIdWhenPrintQRCodeOut(1200,
                (res) =>
                {
                    JSONNode data = res as JSONNode;
                    
                }, (error) =>
                {


                }, RPC_MARK_MACHINR_MONEY_OUT);
        }
    }

}




/*
 * �����С�
compWSC.SendMessage("notify_cashin_result", 
{
    "access_token": "gm_token:17",
    "action_data": {
        "money": 1129,
        "orderno": "PRC_1744359846162524200_2959_wXSNefmC46"
    },
    "action_type": "notify_cashin_result",
    "device_id": "test502",
    "device_name": "warrior",
    "msgid": 152
});


"CassetteProcess"

"notify_cashin_result"

{
    "ctxt": "6RQm245KN972pnG3JBKLTwLKzzqBUVvUn8ccR0iJ4l/yRRKaZw4CJvi2bXZep2mBIv/04lZbpOR8hhPqdLuymT9Ip8Ajpw1o+7HfrAPr62U=",
    "lang": "cn"
}


 * �����С�
{
    "Code": 0,
    "ErrMsg": "Success",
    "Data": {
        "money": 596,
        "qrcode": "bank:1744339759495362700_981"
    },
    "Action": "",
    "action_type": "consume_qrcode",
    "device_id": "test502",
    "device_name": "warrior",
    "msgid": 233
}
*/
