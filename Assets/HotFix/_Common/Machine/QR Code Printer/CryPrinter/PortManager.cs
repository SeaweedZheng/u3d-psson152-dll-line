#if UNITY_ANDROID
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY_ANDROID
using System.IO.Ports;
#endif
using System.Text;
using System.Threading;
using UnityEngine;

namespace CryPrinter
{
#if !UNITY_ANDROID
    public class PortManager
    {
        private SerialPort sp;
        #region ɨ��˿�
        //ʹ��APIɨ��
        public string[] ScanPorts_API()
        {
            string[] portList = SerialPort.GetPortNames();
            return portList;
        }
        //ʹ��ע�����Ϣɨ��
        public string[] ScanPorts_Regedit()
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            string[] SubKeys = keyCom.GetValueNames();
            string[] portList = new string[SubKeys.Length];
            for (int i = 0; i < SubKeys.Length; i++)
            {
                portList[i] = (string)keyCom.GetValue(SubKeys[i]);
            }
            return portList;
        }
        //�Դ��ʽɨ��
        public string[] ScanPorts_TryFail()
        {
            List<string> tempPost = new List<string>();
            bool mark = false;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    SerialPort sp = new SerialPort("COM" + (i + 1).ToString());
                    sp.Open();
                    sp.Close();
                    tempPost.Add("COM" + (i + 1).ToString());
                    mark = true;
                }
                catch (System.Exception)
                {
                    continue;
                }

            }
            if (mark)
            {
                string[] portList = tempPost.ToArray();
                return portList;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region �򿪴���/�رմ���
        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="_portName">�˿ں�</param>
        /// <param name="_baudRate">������</param>
        /// <param name="_parity">У��λ</param>
        /// <param name="dataBits">����λ</param>
        /// <param name="_stopbits">ֹͣλ</param>
        public void OpenSerialPort(string _portName, int _baudRate, Parity _parity, int dataBits, StopBits _stopbits)
        {
            try
            {
                sp = new SerialPort(_portName, _baudRate, _parity, dataBits, _stopbits);//�󶨶˿�
                sp.Open();
                //ʹ��ί��
                //sp.DataReceived += DataReceived;
                //ʹ���߳�
                Thread thread = new Thread(new ThreadStart(DataReceived));
                thread.Start();
            }
            catch (Exception ex)
            {
                sp = new SerialPort();
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public void CloseSerialPort()
        {
            sp.Close();
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="_info">string����</param>
        public void SendData(string _info)
        {
            try
            {
                if (sp.IsOpen)
                {
                    sp.WriteLine(_info);
                }
                else
                {
                    sp.Open();
                    sp.WriteLine(_info);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="send">byte����</param>
        /// <param name="offSet">��ʼλ</param>
        /// <param name="count">byte����</param>
        public void SendData(byte[] send, int offSet, int count)
        {
            try
            {
                if (sp.IsOpen)
                {
                    sp.Write(send, offSet, count);
                }
                else
                {
                    sp.Open();
                    sp.Write(send, offSet, count);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
        #endregion

        #region ��������
        /// <summary>
        /// �������� �ص�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] ReDatas = new byte[sp.BytesToRead];
            sp.Read(ReDatas, 0, ReDatas.Length);//��ȡ����
            DataProcessing(ReDatas);//���ݴ���
        }
        /// <summary>
        /// �������� �߳�
        /// </summary>
        public void DataReceived()
        {
            while (true)
            {
                if (sp.IsOpen)
                {
                    int count = sp.BytesToRead;
                    if (count > 0)
                    {
                        byte[] readBuffer = new byte[count];
                        try
                        {
                            sp.Read(readBuffer, 0, count);
                            DataProcessing(readBuffer);//���ݴ���
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
        /// <summary>
        /// ���ݴ���
        /// </summary>
        /// <param name="data">�ֽ�����</param>
        public void DataProcessing(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat("{0:x2}" + "", data[i]);
            }
            Debug.Log(sb.ToString());
        }
        #endregion
    }
#endif
}


#endif
