using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMaker
{
    public static partial class GlobalEvent
    {


        #region �����¼�
        ///<summary> �����¼� </summary>
        public const string ON_TOOL_EVENT = "ON_TOOL_EVENT";

        ///<summary> �����ҳ�水ť��</summary>
        public const string PageButton = "PageButton";

        ///<summary> �������ӡ����ť��</summary>
        public const string CustomButtonDivicePrenter = "CustomButtonDivicePrenter";
        ///<summary> �������������롱</summary>
        public const string CustomButtonClearCode = "CustomButtonClearCode";
        ///<summary> ������Ϸ֡�</summary>
        public const string CustomButtonCreditUp = "CustomButtonCreditUp";
        ///<summary> ������·֡�</summary>
        public const string CustomButtonCreditDown = "CustomButtonCreditDown";

        /// <summary> ��ȡsbox����˺���Ϣ </summary>
        public const string CustomButtonSboxGetAccount = "CustomButtonSboxGetAccount";

        ///<summary> �����Ͷ�ҡ�</summary>
        public const string CustomButtonCoinIn = "CustomButtonCoinIn";

        ///<summary> �������Ʊ��</summary>
        public const string CustomButtonTicketOut = "CustomButtonTicketOut";

        /// <summary> ��ʾ��Ϣ </summary>
        public const string TipPopupMsg = "TipPopupMsg";

        /// <summary> �ÿ��ά��Ͷ�� </summary>
        public const string IOTCoinIn = "IOTCoinIn";

        /// <summary> �ÿ��ά����Ʊ </summary>
        //public const string IOTTicketOut = "IOTTicketOut";

        /// <summary> ��ʾ���� </summary>
        public const string ShowCode = "ShowCode";

        /// <summary> Aes ���� </summary>
        public const string AesTest = "AesTest";


        /// <summary> ������ </summary>
        public const string DeviceCounterClear = "DeviceCounterClear";

        /// <summary> Ͷ������1 </summary>
        public const string DeviceCounterAddCoinIn = "DeviceCounterAddCoinIn";

        /// <summary> �˱�����1 </summary>
        public const string DeviceCounterAddCoinOut = "DeviceCounterAddCoinOut";
        /// <summary> Ͷ������100 </summary>
        public const string DeviceCounterAddCoinIn100 = "DeviceCounterAddCoinIn100";

        /// <summary> ��ʾ������n������ </summary>
        public const string ShowTableLastData = "ShowTableLastData";


        /// <summary> ���Դ�ӡ��ά�� </summary>
        public const string DeviceTestPrintQRCode = "DeviceTestPrintQRCode";

        public const string DeviceTestPrintTicket = "DeviceTestPrintTicket";


        public const string DeviceTestPrintJCM950 = "DeviceTestPrintJCM950";
        public const string DeviceTestPrintTRANSACT950 = "DeviceTestPrintTRANSACT950";
        #endregion




        #region GM�¼�

        ///<summary> �����¼� </summary>
        public const string ON_GM_EVENT = "ON_GM_EVENT";

        public const string GMFreeSpin = "GMFreeSpin";
        public const string GMBigWin = "GMBigWin";
        public const string GMJp1 = "GMJp1";
        public const string GMJp2 = "GMJp2";
        public const string GMJp3 = "GMJp3";
        public const string GMJp4 = "GMJp4";
        public const string GMJpOnline = "GMJpOnline";
        public const string GMBonus1 = "GMBonus1";
        public const string GMBonus2 = "GMBonus2";
        public const string GMBonus3 = "GMBonus3";
        #endregion


        #region ��ʼ��ϵͳ�����¼�
        /// <summary>  ������ʼ���¼�  </summary>
        public const string ON_INIT_SETTINGS_EVENT = "ON_INIT_SETTINGS_EVENT";
        /// <summary> ��Ӳ�����ʼ������-value:int</summary>
        public const string AddSettingsCount = "AddSettingsCount";
        /// <summary> ��Ӳ�����ʼ������-value:string</summary>
        public const string InitSettings = "InitSettings";
        /// <summary> ˢ�¼���ҳ��������ʾ����Ϣ-value:string</summary>
        public const string RefreshProgressMsg = "RefreshProgressMsg";


        /// <summary> ������ʼ��ϵͳ���������¼��¼�</summary>
        public const string ON_INIT_SETTINGS_FINISH_EVENT = "ON_INIT_SETTINGS_FINISH_EVENT";
        #endregion



        #region MOCK�¼�
        ///<summary> MOCK�¼� </summary>
        public const string ON_MOCK_EVENT = "ON_MOCK_EVENT";
        ///<summary> �Զ�������򱻵�� </summary>
        public const string PlayerAccountChange = "PlayerAccountChange";
        #endregion


        #region UI�¼�
        ///<summary> UI�¼� </summary>
        public const string ON_UI_INPUT_EVENT = "ON_UI_INPUT_EVENT";
        ///<summary> �Զ�������򱻵�� </summary>
        public const string CustomInputClick = "CustomInputClick";
        #endregion


        #region ҳ���¼�
        ///<summary> ��Ϸ�¼� </summary>
        public const string ON_PAGE_EVENT = "ON_PAGE_EVENT";

        ///<summary> �ö�ҳ�淢���仯 </summary>
        public const string PageOnTopChange = "PageOnTopChange";

        #endregion



        #region ��Ϸ�¼�
        ///<summary> ��Ϸ�¼� </summary>
        public const string ON_GAME_EVENT = "ON_GAME_EVENT";

        //public const string GameStart = "GameStart";
        //public const string GameIdle = "GameIdle";
        #endregion




        #region Ͷ�˱��¼�
        ///<summary> Ͷ�˱��¼� </summary>
        public const string ON_COIN_IN_OUT_EVENT = "ON_COIN_IN_OUT_EVENT";

        ///<summary> ��Ʊ��� </summary>
        public const string CoinOutSuccess = "CoinOutSuccess";

        ///<summary> ��Ʊ���� </summary>
        public const string CoinOutError = "CoinOutError";

        ///<summary> ������б��ض������� </summary>
        public const string ClearAllOrderCache = "ClearAllOrderCache";






        ///<summary> Ͷ����� </summary>
        public const string CoinInCompleted = "CoinInCompleted";

        ///<summary> ��Ʊ��� </summary>
        public const string CoinOutCompleted = "CoinOutCompleted";


        ///<summary> ��ά��Ͷ����� </summary>
        public const string IOTCoinInCompleted = "IOTCoinInCompleted";
        ///<summary> ��ά����Ʊ��� </summary>
        public const string IOTCoinOutCompleted = "IOTCoinOutCompleted";
        #endregion


        #region Ӳ���¼�
        ///<summary> �����¼� </summary>
        public const string ON_DEVICE_EVENT = "ON_DEVICE_EVENT";
        ///<summary> �Զ�������򱻵�� </summary>
        public const string ScanQRCode = "ScanQRCode";
        ///<summary> ������� </summary>
        public const string CodeCompleted = "CodeCompleted";
        #endregion



        #region Ǯ���¼�
        ///<summary> Ǯ���¼� </summary>
        public const string ON_MONEY_BOX_EVENT = "ON_MONEY_BOX_EVENT";
        ///<summary> Ǯ�������̨�Ϸ� </summary>
        public const string MoneyBoxRequestMachineQRCodeUp = "MoneyBoxRequestMachineQRCodeUp";
        #endregion


        #region ���簴ť����
        public const string ON_MQTT_REMOTE_CONTROL_EVENT = "ON_MQTT_REMOTE_CONTROL_EVENT";
        #endregion
    }

    /*
    public static partial class GlobalEvent
    {
        ///<summary> ��Ϸ��Ч </summary>
        public const string ON_GAME_SOUND_EVENT = "ON_GAME_SOUND_EVENT";
        ///<summary> ��Ϸ�������� </summary>
        public const string ON_GAME_MUSIC_EVENT = "ON_GAME_MUSIC_EVENT";
    }
    */


}