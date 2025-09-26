using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

namespace IOT
{
    //��Ҫע����Ӧ����Ϣ
    public class IOTEventHandle
    {
        public const string REGISTER_DEV = "REGISTER_DEV";                  //��Ӧ�豸ע��    ����List<QrCodeData>
        public const string COINT_IN = "IOT_COIN_IN";                       //��Ӧƽ̨Ͷ��
        public const string TICKET_OUT = "TICKET_OUT";                      //��Ӧ�豸�˲�
        public const string NOTICE = "NOTICE";                              //��Ӧƽ̨��Ϣ
    }
    //����ṹ��Ҫ�洢�ڱ���
    public class IoTDevInfo
    {
        public String ProductKey { get; set; }
        public String DeviceName { get; set; }
        public String FirmwareName { get; set; }
        public String DeviceSecret { get; set; }
        public String IotInstanceId { get; set; }
    }

    public class MessageType
    {
        public static string C2S = "c2s";
        public static string S2C = "s2c";
    }

    public class MessageCmd
    {
        //1.�豸ע��
        public const string ONLINE = "firmware-online";                        //����
        public const string ONLINE_REPLY = "firmware-online-reply";            //����

        //2.ƽ̨Ͷ��
        public const string INSERT_COIN = "insert-coin";                       //����
        public const string INSERT_COIN_REPLY = "insert-coin-reply";           //����

        //3.�豸�˲�
        public const string DEVICE_PRIZE = "device-prize";                     //����
        public const string DEVICE_PRIZE_REPLY = "device-prize-reply";         //����

        //4.ƽ̨��Ϣ֪ͨ
        public const string NOTICE = "NOTICE";                                 //����
    }

    /*
     * ���Ի�����http://i.test.gzhaoku.com/admin/resp/firmware/query
     * ��ʽ������http://i.gzhaoku.com/admin/resp/firmware/query
     */
    public class IoTConst
    {
        public static string Secret = "EoZ2mFUdWngwE2s2JgutswVtp4RZFtma";
        //public static string GetDevParamURL =  ApplicationSettings.Instance.isRelease?  "http://i.gzhaoku.com/admin/resp/firmware/query" : "http://i.test.gzhaoku.com/admin/resp/firmware/query";

        public static string GetDevParamURL => PlayerPrefsUtils.isUseReleaseIot ?
           "http://i.gzhaoku.com/admin/resp/firmware/query" :  "http://i.test.gzhaoku.com/admin/resp/firmware/query";
    }

    public class DevParamReq
    {
        /*
         * Ӳ�� id�����豸�����ɣ�����Ϊ32λguid����֤ȫ��Ψһ
         */
        public string hardwareId { get; set; }
        public string secret { get; set; }
    }

    public class DevParamRsp
    {
        public int code { get; set; }
        public IoTDevInfo data { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }

    public class NoticeData
    {
        /*
         *  ��Ϣ����
         *  -1����ʾ��ֻ̨�����˹̼���û�д����豸
         */
        public int code { get; set; }
        /*
         *  ��Ϣ����
         */
        public string message { get; set; }
    }

    public class QrCodeData
    {
        public int portid { get; set; }
        public string qrcodeUrl { get; set; }
    }

    /*
     * ���⽱������ʾ����{"type":56,"ext":"{\\"name\\":\\"���Ƚ�\\",\\"note\\":\\"ȫ�̽�\\"}","seq":1733277248,"num":1,"orderNum":"2024101214554301000001"}
     */
    public class TicketOutData
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        /*
         * �������ͣ�*1.ʵ���,��λ:��*51.��Ʒ,��λ:��*52.��Ʊ,��λ:��*53.����,*56.���⽱��
         */
        public int type { get; set; }

        /*
         * ext: ��չ��Ϣ��json�ַ�������ֵ�򴫿գ�����Ӧ�������ͻ��в�ͬ���壬��typeΪ56����
         * �ϴ����⽱�����ƣ����磺{"name":"һ�Ƚ�","note":"ȫ�̽�"}��name��Ӧ�����̨�齱�����
         * �ƣ�note��Ӧ���ǻ����ϵ��н����ƣ����⽱����Ӧ�ĳ��������У�һ�Ƚ������Ƚ������Ƚ���
         * ʮ�Ƚ�����Ҫ�ڹ����̨->�齱�������Ӧ�Ļ���ƣ���Ӧname��
         */
        public string ext { get; set; }

        /*
         * ��ţ���Ӧһ�������¶�η֣���Ŵ�1��ʼ������Ҳ���Զ���Ϊʱ�������ʱ���ֻ
         * �ܾ�ȷ���룬���ܳ���10λ���������ظ�
         */
        public string seq { get; set; }

        /*
         * Ϊ��������type=56ʱ������һ������Ϊ1����ʾ����һ�����⽱��������Ϊ2�����ʾ�н�2�Σ����ӡ2���н�СƱ
         */
        public int num { get; set; }

        /*
         * �����ţ���ӦͶ�ҵĶ�����
         */
        public string orderNum { get; set; }
    }

    public class CoinData
    {
        /// <summary>
        /// Ͷ������
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string orderNum { get; set; }

        /// <summary>
        /// �û�id
        /// </summary>
        public string memberId { get; set; }
    }

    public class IoTData
    {
        public string ext { get; set; }
        public int type { get; set; }

        public string seq { get; set; }
        //�汾��
        public int version { get; set; }

        //��1���������豸���ƻ��Զ���
        public string hardwareId { get; set; }

        //�豸���뷽ʽ��1�����ں�(Ĭ��Ϊ1)2��С���� 3��Լսƽ̨
        public int accessType { get; set; }

        //�ź�ֵ������0~32֮�䣬32���ź���ã�һ�������Ϊ32�������wifi��������Ĺ���
        //�ź�ֵ13-16Ϊǿ���ź�ֵ9-12Ϊ�У��ź�ֵ8����Ϊ��
        public int signal { get; set; }

        /*
         * һ����data��ߣ���ʾԼ����ֵ��Ŀǰû���ã���Ϊ0
         */
        public int code { get; set; }

        /*
         * һ����data��ߣ���ʾ������״̬��trueΪ�ɹ���falseΪʧ��
         * ��Ϊfalseʱ��һ����Ҫ��¼������ʾmessage���ݣ����ڵ���
         */
        public bool success { get; set; }

        /*
         * ��successΪtrueʱ��Ϊ�������ɹ�����successΪfalseʱ��Ϊʧ�ܵ����� ������ʾ��Ӳ��id��һ��
         */
        public string message { get; set; }

        /*
         * ��Ӧ�豸��ά�룬����豸�ж���˿ڣ��򷵻�ÿ���˿ڶ�Ӧ�Ķ�ά��
         */
        public List<QrCodeData> qrcodeUrls { get; set; }

        /*
        * Ͷ������
        */
        public int num { get; set; }
        /*
         * ������
         */
        public string orderNum { get; set; }

        /*
         * �û�id
         */
        public string memberId { get; set; }
    }

    //ͨ����Ϣ�ṹ�壬���ͺͽ��ն�ʹ�����
    public class IoTMsg
    {
        /*
         *  ��ϢΨһ��ʶ
         *  ����Ϣ���ͷ�����32λguid,��Ϣ�ط�ʱMessageid���ֲ���
         */
        public string messageid { get; set; }

        /*
         *  ��Ϣ����
         *  s2c:������������͵��ն�
         *  c2s:�����ն˷��͵�������
         */
        public string messagetype { get; set; }

        /*
         *  ��Ϣָ��
         *  ����firmware-online�����豸������firmware-online-reply��ʾ�ظ�
         */
        public string messagecmd { get; set; }

        /*
         *  ��Ϣ����
         *  ������Ϣ���ݲ�ͬ,data��  ��Ϊ�գ�����ǻظ���Ϣ��һ �������3��������
                success:�����Ƿ�ɹ�
                message:������Ϣ����
                code:�������뷵�أ�Ŀǰû
                ���õ�����Ϊ0��
         */
        public IoTData data { get; set; }

        /*
         *  ָ���ʱ��ʱ���
         *  ��ȷ���룬�������ڣ�2024-02-2817:18:59��ת��Ϊʱ���Ϊ��1709111939
         */
        public string timestamp { get; set; }

        /*
         *  �豸��Ӧ�˿�
         *  �������˿ڣ���1��2��3���ƣ����ֻ��1���˿ڣ���Ϊ1
         */
        public int portid { get; set; }

    }
}
