using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameMaker
{

    public static class DefaultSettingsUtils
    {
        public static DefaultSettingsInfo Config(int gameId = -1)
        {
            if(DefaultSettingsBlackboard.Instance.gameSettings != null)
                for (int i = 0; i< DefaultSettingsBlackboard.Instance.gameSettings.Count; i++)
                {
                    if (DefaultSettingsBlackboard.Instance.gameSettings[i].gameId == gameId)
                        return DefaultSettingsBlackboard.Instance.gameSettings[i];
                }
            return DefaultSettingsBlackboard.Instance.defaultSettings;
        }
    }


    //[Serializable]
    //[CreateAssetMenu(fileName = "DefaultSettings", menuName = "GameMaker/ScriptableObject/DefaultSettings")]
    public class DefaultSettingsBlackboard :MonoSingleton<DefaultSettingsBlackboard>//: MonoSingleton<DefaultSettingsBlackboard> //: ScriptableObjectSingleton<DefaultSettings>
    {
        //[DisplayOnly]
        public readonly DefaultSettingsInfo defaultSettings = new DefaultSettingsInfo();

        public readonly List<DefaultSettingsInfo> gameSettings = new List<DefaultSettingsInfo>();
    }

    [Serializable]
    public class DefaultSettingsInfo
    {
        [Tooltip("��Ϸid")]
        public int gameId = -1;

        [Title("��̨����")]


        #region �ʽ�����
        [Title("�ʽ�����")]

        /// <summary> �ʽ�޽����ֵ </summary>
        [Tooltip("�ʽ�޽���Χ���ֵ")]
        public long jpGrandRangeMax = 80000;

        /// <summary> �ʽ�޽���Сֵ </summary>
        [Tooltip("�ʽ�޽���Χ��Сֵ")]
        public long jpGrandRangeMin = 50000;

        /// <summary> �ʽ�޽����ֵ </summary>
        [Tooltip("�ʽ�޽����ֵ")]
        public long defJpGrandMax = 80000;

        /// <summary> �ʽ�޽���Сֵ </summary>
        [Tooltip("�ʽ�޽���Сֵ")]
        public long defJpGrandMin = 50000;

        /// <summary> �ʽ�ͷ�����ֵ </summary>
        [Tooltip("�ʽ�ͷ����Χ���ֵ")]
        public long jpMajorRangeMax = 30000;

        /// <summary> �ʽ�ͷ����Сֵ </summary>
        [Tooltip("�ʽ�ͷ����Χ��Сֵ")]
        public long jpMajorRangeMin = 10000;

        /// <summary> �ʽ�ͷ�����ֵ </summary>
        [Tooltip("�ʽ�ͷ�����ֵ")]
        public long defJpMajorMax = 30000;

        /// <summary> �ʽ�ͷ����Сֵ </summary>
        [Tooltip("�ʽ�ͷ����Сֵ")]
        public long defJpMajorMin = 10000;


        /// <summary> �ʽ�����ֵ </summary>
        [Tooltip("�ʽ�󽱷�Χ���ֵ")]
        public long jpMinorRangeMax = 8000;

        /// <summary> �ʽ����Сֵ </summary>
        [Tooltip("�ʽ�󽱷�Χ��Сֵ")]
        public long jpMinorRangeMin = 5000;


        /// <summary> �ʽ�����ֵ </summary>
        [Tooltip("�ʽ�����ֵ")]
        public long defJpMinorMax = 8000;

        /// <summary> �ʽ����Сֵ </summary>
        [Tooltip("�ʽ����Сֵ")]
        public long defJpMinorMin = 5000;





        /// <summary> �ʽ�С�����ֵ </summary>
        [Tooltip("�ʽ�С����Χ���ֵ")]
        public long jpMiniRangeMax = 3000;

        /// <summary> �ʽ�С����Сֵ </summary>
        [Tooltip("�ʽ�С����Χ��Сֵ")]
        public long jpMiniRangeMin = 1000;

        /// <summary> �ʽ�С�����ֵ </summary>
        [Tooltip("�ʽ�С�����ֵ")]
        public long defJpMiniMax = 3000;

        /// <summary> �ʽ�С����Сֵ </summary>
        [Tooltip("�ʽ�С����Сֵ")]
        public long defJpMiniMin = 1000;


        #endregion

        #region �û�����
        [Title("�û�����")]

        [Title("�û�����Admin")]
        public string passwordAdmin = "187653214";
        [Title("�û�����Manager")]
        public string passwordManager = "88888888";
        [Title("�û�����Shift")]
        public string passwordShift = "666666";
        #endregion

        #region ����
        [Title("����")]

        public string defLanguage = "cn";
        #endregion

        #region ����
        [Title("��Ч����")]
        /// <summary> ��Ч </summary>
        public float defSound = 0.5f;

        /// <summary> �������� </summary>
        public float defMusic = 0.5f;
        #endregion

        #region ���ݼ�¼����
        [Title("���ݼ�¼����")]

        [Tooltip("�����Ϸ������¼")]
        public int maxMaxGameRecord = 50000;
        [Tooltip("��С��Ϸ������¼")]
        public int minMaxGameRecord = 100;
        [Tooltip("Ĭ����Ϸ������¼")]
        public int defMaxGameRecord = 1000;

        [Tooltip("���Ͷ�˱Ҵ�����¼")]
        public int maxMaxCoinInOutRecord = 50000;
        [Tooltip("����Ͷ�˱Ҵ�����¼")]
        public int minMaxCoinInOutRecord = 100;
        [Tooltip("Ĭ��Ͷ�˱Ҵ�����¼")]
        public int defMaxCoinInOutRecord = 1000;

        [Tooltip("���ʽ������¼")]
        public int maxMaxJackpotRecord = 50000;
        [Tooltip("���²ʽ������¼")]
        public int minMaxJackpotRecord = 100;
        [Tooltip("Ĭ�ϲʽ������¼")]
        public int defMaxJackpotRecord = 1000;

        [Tooltip("��󱨴������¼")]
        public int maxMaxErrorRecord = 5000;
        [Tooltip("���±��������¼")]
        public int minMaxErrorRecord = 100;
        [Tooltip("Ĭ�ϱ��������¼")]
        public int defMaxErrorRecord = 500;


        [Tooltip("����¼�������¼")]
        public int maxMaxEventRecord = 5000;
        [Tooltip("�����¼�������¼")]
        public int minMaxEventRecord = 100;
        [Tooltip("Ĭ���¼�������¼")]
        public int defMaxEventRecord = 500;


        [Tooltip("��Ӫ��ͳ�Ƽ�¼������")]
        public int maxMaxBusinessDayRecord = 720;
        [Tooltip("��Ӫ��ͳ�Ƽ�¼��С����")]
        public int minMaxBusinessDayRecord = 1;
        [Tooltip("Ĭ����Ӫ��ͳ�Ƽ�¼����")]
        public int defMaxBusinessDayRecord = 7;


        #endregion

        #region Ͷ�˱�����
        [Title("Ͷ�˱�����")]

        [Tooltip("������·ֱ���(1������ٷ�)")]
        public int maxScoreUpDownScale = 10000;
        [Tooltip("��С���·ֱ���(1������ٷ�)")]
        public int minScoreUpDownScale = 100;
        [Tooltip("Ĭ�����·ֱ���(1������ٷ�)")]
        public int defScoreUpDownScale = 100;

        
        [Tooltip("����Ϸֳ�������(1������ٷ�)")]
        public int maxScoreUpLongClickScale = 100000;
        [Tooltip("��С�Ϸֳ�������(1������ٷ�)")]
        public int minScoreUpLongClickScale = 1000;
        [Tooltip("Ĭ���Ϸֳ�������(1������ٷ�)")]
        public int defScoreUpLongClickScale = 1000;


        /// <summary>1�Ҽ��� ���ֵ </summary>
        [Tooltip("���Ͷ�ұ���(1�Ҽ���)")]
        public int maxCoinInScale = 200;
        /// <summary>1�Ҽ��� ��Сֵ </summary>
        [Tooltip("��СͶ�ұ���(1�Ҽ���)")]
        public int minCoinInScale = 10;
        [Tooltip("Ĭ��Ͷ�ұ���(1�Ҽ���)")]
        public int defCoinInScale = 10;


        /// <summary> ��1Ʊ���֡����ֵ </summary>
        [Tooltip("�����Ʊ����(1Ʊ����)")]
        public readonly int maxCoinOutPerTicket2Credit = 200;
        /// <summary> ��1Ʊ���֡���Сֵ </summary>
        [Tooltip("��С��Ʊ����(1Ʊ����)")]
        public readonly int minCoinOutPerTicket2Credit = 0;
        [Tooltip("Ĭ����Ʊ����(1Ʊ����)")]
        public readonly int defCoinOutPerTicket2Credit = 200;


        /// <summary>  ��1�ּ�Ʊ�����ֵ </summary>
        [Tooltip("�����Ʊ����(1�ּ�Ʊ)")]
        public readonly int maxCoinOutPerCredit2Ticket = 50;
        /// <summary>  ��1�ּ�Ʊ����Сֵ </summary>
        [Tooltip("��С��Ʊ����(1�ּ�Ʊ)")]
        public readonly int minCoinOutPerCredit2Ticket = 0;
        [Tooltip("Ĭ����Ʊ����(1�ּ�Ʊ)")]
        public readonly int defCoinOutPerCredit2Ticket = 0;


        /// <summary> ��1�����֡����ֵ  </summary>
        [Tooltip("����������(1������)")]
        public readonly int maxBillInScale = 100;
        /// <summary> ��1�����֡���Сֵ </summary>
        [Tooltip("��С��������(1������)")]
        public readonly int minBillInScale = 1;
        [Tooltip("Ĭ�Ͻ�������(1������)")]
        public readonly int defBillInScale = 1;


        /// <summary> ����ӡ�����ֵ  </summary>
        [Tooltip("����ӡ����(1�����ٷ�)")]
        public readonly int maxPrintOutScale = 100;
        /// <summary> ����ӡ����Сֵ </summary>
        [Tooltip("��С��ӡ����(1�����ٷ�)")]
        public readonly int minPrintOutScale = 10;
        [Tooltip("Ĭ�ϴ�ӡ����(1�����ٷ�)")]
        public readonly int defPrintOutScale = 10;

        [Tooltip("�����Һ���Ϸ�ֶһ�����(1�����ٷ�)")]
        public int moneyMeterScale = 100;
        #endregion



        /// <summary> ����ӡ�����ֵ  </summary>
        [Tooltip("����ӡ����(1�����ٷ�)")]
        public readonly int maxJackpotPercent = 100;
        /// <summary> ����ӡ����Сֵ </summary>
        [Tooltip("��С��ӡ����(1�����ٷ�)")]
        public readonly int minJackpotPercent = 1;
        [Tooltip("Ĭ�ϴ�ӡ����(1�����ٷ�)")]
        public readonly int defJackpotPercent = 5;




        /// <summary> �Ƿ���ʾ��ӡ��־ </summary>
        public int isDebug => ApplicationSettings.Instance.isRelease? 0 : 1;

        /// <summary> �Ƿ���ʾ��������Ϣ </summary>
        public int isUpdateInfo => ApplicationSettings.Instance.isRelease ? 0 : 1;

        /// <summary> �Ƿ�������ʽ� </summary>
        public int  isJackpotOnline => 1;

        /// <summary> ʹ�õ���ҳ�� </summary>
        public int enableReporterPage => ApplicationSettings.Instance.isRelease ? 0 : 1;

        /// <summary> ʹ�ò��Թ��� </summary>
        public int enableTestTool => ApplicationSettings.Instance.isRelease ? 0 : 1;
    }
}
