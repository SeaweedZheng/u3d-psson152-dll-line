using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IOT
{
    public class IOTModel : BaseManager<IOTModel>
    {
        /// <summary>
        /// ��������
        /// </summary>
        public enum TicketOutType
        {
            /// <summary>
            /// ʵ���
            /// </summary>
            Coin = 1,
            /// <summary>
            /// ��Ʒ
            /// </summary>
            Gift = 51,
            /// <summary>
            /// ��Ʊ
            /// </summary>
            Ticket = 52,
            /// <summary>
            /// ����
            /// </summary>
            Integral = 53,
            /// <summary>
            /// ���⽱��
            /// </summary>
            Special = 56
        }

        public int PortId
        {
            get
            {
                return qrCodeDatas[0].portid;
            }
        }


        private bool _linkIOT;
        /// <summary>
        /// �Ƿ��¼�ÿ��ȡ����ά��
        /// </summary>
        public bool LinkIOT
        {
            get
            {
                return _linkIOT;
            }
            set
            {
                _linkIOT = value;
                if (_linkIOT)
                    EventCenter.Instance.EventTrigger(EventHandle.REFRESH_QRCORD);
            }
        }

        private string _linkId;


        /// <summary>
        /// �Ƿ����΢�ź�
        /// </summary>
        public string LinkId
        {
            get
            { 
                _linkId = PlayerPrefs.GetString("linkId", null);
                return _linkId;
            }
            set
            {
                _linkId = value;
                PlayerPrefs.SetString("linkId", _linkId);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public TicketOutType ticketOutType = TicketOutType.Ticket;
        /// <summary>
        /// δ������ʻ���
        /// </summary>
        public int ticketOutFrame;
        /// <summary>
        /// ��ά������(ֻ��ע��IOT�ɹ����з���)
        /// </summary>
        public List<QrCodeData> qrCodeDatas;
        /// <summary>
        /// δ��ɳ�������(����IOT����, ��δ�ӵ�����)
        /// </summary>
        public List<TicketOutData> unfinishTicketOutDatas = new List<TicketOutData>();

        public Texture GetQRTexture(Color color, UnityAction errorAction = null)
        {
            if (qrCodeDatas == null || qrCodeDatas.Count == 0)
            {
                errorAction?.Invoke();
                return null;
            }
            return Utils.GenerateQRImageWithColor(qrCodeDatas[0].qrcodeUrl, color);
        }
    }
}

