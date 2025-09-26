using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameMaker
{
    /// <summary> ���ֲʽ���Ϣ </summary>
    [Serializable]
    public class JackpotInfo
    {
        public string name;

        public int id;
        /// <summary> ui��ǰ��ʾ�Ĳʽ�ֵ�������ʽ�ֵ�� </summary>
        public float nowCredit;
        /// <summary> ��ǰ�����Ĳʽ�ֵ </summary>
        public float curCredit;
        public float maxCredit;
        public float minCredit;


    }


    /// <summary> �ʽ��н���Ϣ </summary>
    [System.Serializable]
    public class JackpotWinInfo
    {
        public string name;
        public int id;
        /// <summary> �н�ʱ���õ��Ĳʽ�ֵ </summary>
        public float winCredit;
        /// <summary> �н�ʱ�����Ĳʽ�ֵ </summary>
        public float whenCredit;
        /// <summary> ��ǰ�����Ĳʽ�ֵ </summary>
        public float curCredit;

        public long creditBefore = -1;

        public long creditAfter = -1;
    }

    /// <summary> ���ֲʽ���Ϣ </summary>
    public class JackpotRes
    {

        public float curJackpotGrand;
        public float curJackpotMajor;
        public float curJackpotMinior;
        public float curJackpotMini;

        public List<JackpotWinInfo> jpWinLst = new List<JackpotWinInfo>();
    }

}
