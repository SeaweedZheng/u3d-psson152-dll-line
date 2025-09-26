using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMaker
{
    [Serializable]
    public class ReelSetting
    {
        public const int NONE = -99999;
        
        /// <summary>��Ȧ�״�ת����ʱ </summary>
        [Tooltip("��Ȧ�״�ת����ʱ")]
        public float timeTurnStartDelay = NONE;

        /// <summary>��Ȧת��ʱ�� </summary>
        [Tooltip("��Ȧת��ʱ��")]
        public float timeTurnOnce = NONE;

        /// <summary>���й����״�ת��ʱ���ص���ʱ�� </summary>
        [Tooltip("���й����״�ת��ʱ���ص���ʱ��")]
        public float timeReboundStart = NONE;

        /// <summary>���й��ֽ���ת��ʱ���ص���ʱ�� </summary>
        [Tooltip("���й��ֽ���ת��ʱ���ص���ʱ��")]
        public float timeReboundEnd = NONE;

        /// <summary>���й����״�ת��ʱ���ص���ƫ���� </summary>
        [Tooltip("���й����״�ת��ʱ���ص���ƫ����")]
        public float offsetYReboundStart = NONE;

        /// <summary>���й��ֽ���ת��ʱ���ص���ƫ���� </summary>
        [Tooltip("���й��ֽ���ת��ʱ���ص���ƫ����")]
        public float offsetYReboundEnd = NONE;

        /// <summary> ���й���ת����Ȧ�� </summary>
        [Tooltip("���й���ת����Ȧ��")]
        public int numReelTurn = NONE;

        /// <summary> ���й���,��ǰһ�й��ֶ�ת����Ȧ�������ֶ��й��ּ��ת���� </summary>
        [Tooltip("���й��ֶ�ת����Ȧ��")]
        public int numReelTurnGap = NONE;
        // public int numReelTurnGapLow = -1;
        // public int numReelTurnGapMedium = -1;
        // public int numReelTurnGapHigh = -1;
    }

    public enum SymbolLineShowType
    {
        /// <summary> ����ʾ </summary>
        None = 0,

        /// <summary> ��ʾ�н�ͼ�꣬�������� </summary>
        SymbolLine,

        /// <summary> ��ʾ�н�ͼ�꣬������ </summary>
        SymbolLineAnim,

        /// <summary> ���С��н�ͼ����ߡ�ȫ��ʾ���������� </summary>
        AllSymbolLine,

        /// <summary> ���С��н�ͼ����ߡ�ȫ��ʾ�������� </summary>
        AllSymbolLineAnim,

        /// <summary> ���С��н�ͼ����ߡ�������ʾ���������� </summary>
        PerSymbolLine,

        /// <summary> ���С��н�ͼ����ߡ�������ʾ�������� </summary>
        PerSymbolLineAnim,
    }

    [Serializable]
    public class SymbolLineShowInfo
    {
        /// <summary>��ʾ������ </summary>
        [Tooltip("��ʾ������")]
        public SymbolLineShowType showType = SymbolLineShowType.None;

        /// <summary>��ʾʱ�� </summary>
        [Tooltip("��ʾʱ��,��λ��")]
        public float showTimeS = 0f;
    }

}
