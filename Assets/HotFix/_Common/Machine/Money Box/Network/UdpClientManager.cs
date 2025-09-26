using SimpleJSON;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace MoneyBox
{
    public class UdpClientManager : MonoBehaviour
    {
        private UdpClient udpClient;
        private int port = 12346;
        private string serverIP = "255.255.255.255"; // �㲥��ַ

        private Queue<string> receivedMessages = new Queue<string>();
        private bool isReceiving = false;
        private bool isClosing = false; // ������־λ�����ڿ����Ƿ�ر�

        void Start()
        {
           // WebSocketClient websocket_client = WebSocketGameObject.GetComponent<WebSocketClient>();
           // websocket_client.OnServerCloseCallback = OnWebSocketClosed;

           // StartClient();
        }

        void OnWebSocketClosed()
        {
            //CloseClient();
            Debug.Log("111��⵽�����������ر����ӣ�ִ����Ӧ�����߼�");

        }

        public void StartClient()
        {
            isReceiving = false;
            isClosing = false; 
            try
            {
                udpClient = new UdpClient();
                udpClient.EnableBroadcast = true; // ����㲥
                StartReceiving();
            }
            catch (Exception ex)
            {
                Debug.LogError("��money box udp��Failed to start UDP client: " + ex.Message);
            }
        }

        public void SendBroadcast(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
                udpClient.Send(data, data.Length, endPoint);
                Debug.LogWarning("��money box udp��<color=green>rpc up</color>  message: " + message);
            }
            catch (Exception ex)
            {
                Debug.LogError("��money box udp��Failed to send broadcast message: " + ex.Message);
            }
        }

        private void StartReceiving()
        {
            if (isReceiving) return;
            isReceiving = true;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            udpClient.BeginReceive(ReceiveCallback, endPoint);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (isClosing) return; // ������ڹرգ�ֱ�ӷ���

            try
            {
                IPEndPoint endPoint = (IPEndPoint)ar.AsyncState;
                byte[] receivedData = udpClient.EndReceive(ar, ref endPoint);
                string receivedMessage = Encoding.UTF8.GetString(receivedData);

                lock (receivedMessages)
                {
                    receivedMessages.Enqueue(receivedMessage);
                }

                // ���������µ���Ϣ
                if (!isClosing) // ����Ƿ����ڹر�
                {
                    udpClient.BeginReceive(ReceiveCallback, endPoint);
                }
            }
            catch (Exception ex)
            {
                if (!isClosing) // ����������ڹرգ��ż�¼������־
                {
                    Debug.LogError("Failed to receive message: " + ex.Message);
                }
            }
        }

        private void Update()
        {
            lock (receivedMessages)
            {
                while (receivedMessages.Count > 0)
                {
                    string message = receivedMessages.Dequeue();
                    ProcessReceivedMessage(message);
                }
            }
        }


        /// <summary>
        /// ������udp����
        /// </summary>
        /// <param name="message"></param>
        private void ProcessReceivedMessage(string message)
        {
            Debug.LogWarning("��money box udp��<color=yellow>rpc down</color>  message: " + message);

            JSONNode jsondata = JSONNode.Parse(message);
            if (jsondata != null && jsondata.HasKey("control_main_ip"))
            {
                string control_main_ip = jsondata["control_main_ip"];

                WebSocketClient websocket_client = GetComponent<WebSocketClient>();
                if (websocket_client != null)
                {                    
                    Debug.LogWarning("��money box udp��Received from control_panel_main: " + control_main_ip);
                    
                    websocket_client.ForceClose();
                    websocket_client.logicUrl = control_main_ip;  // ��¼������ url
                    websocket_client.ConnectWS();
                }
                else
                {
                    Debug.LogError("��money box udp��WebSocketClient component not found.");
                }
            }
            else
            {
                Debug.LogError("��money box udp��Invalid JSON data or missing 'control_main_ip' key.");
            }
        }

        public void CloseClient()
        {
            isClosing = true; // ���ùرձ�־λ
            try
            {
                if (udpClient != null)
                {
                    udpClient.Close();
                    udpClient = null;
                    Debug.LogWarning("��money box udp��close.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to close UDP client: " + ex.Message);
            }
        }

        private void OnDestroy()
        {
            Debug.Log("---UdpClientManager          OnDestroy---");
            CloseClient();
        }
    }
}