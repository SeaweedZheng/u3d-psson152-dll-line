using SlotMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMaker
{
    public class ReelSettingBlackboard : MonoWeakSelectSingleton<ReelSettingBlackboard>
    {

        [Serializable]
        public struct STReelSetting
        {
            public int reelIndex;

            public ReelSetting reelSetting;
        }


        public const string REEL_SETTING_REGULAR = "Reel Setting Regular";
        public const string REEL_SETTING_SLOW_MOTION = "Reel Setting Slow Motion";
        public const string REEL_SETTING_STOP = "Reel Setting Stop Immediately";


        /// <summary>
        /// ���ֵ�Ĭ�ϲ�������
        /// </summary>
        [SerializeField]
        private ReelSetting defaultReelSetting = new ReelSetting()
        {
            timeTurnStartDelay = 0.12f,
            timeTurnOnce = 0.12f,
            timeReboundStart = 0.15f,
            timeReboundEnd = 0.08f,

            offsetYReboundStart = 200,
            offsetYReboundEnd = -20,

            numReelTurn = 7,
            numReelTurnGap = 1,
        };

        /// <summary>
        /// ĳ�й��ֵ������ò���
        /// </summary>
        [SerializeField]
        private List<STReelSetting> eachReelSettings = new List<STReelSetting>();

        /// <summary>
        /// ���붯̬���ù��ֲ�����һ���ò��ϣ�
        /// </summary>
        public Dictionary<int, ReelSetting> dynamicReelSettings = new Dictionary<int, ReelSetting>();




        /// <summary>
        /// ĳ�й��ִ��ڵ�����������
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        bool ContainsEachReelSetting(int reelIndex) 
        {
            for (int i = 0; i < eachReelSettings.Count; i++)
            {
                if (eachReelSettings[i].reelIndex == reelIndex)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ��ȡĳ�й��ֵ�����
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        ReelSetting GetEachReelSettings(int reelIndex) 
        {
            for (int i = 0; i < eachReelSettings.Count; i++)
            {
                if (eachReelSettings[i].reelIndex == reelIndex)
                    return eachReelSettings[i].reelSetting;
            }
            return null;
        }

        /// <summary>
        /// ����ת��һȦ��ʱ��
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public float GetTimeTurnOnce(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].timeTurnOnce != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].timeTurnOnce;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).timeTurnOnce != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).timeTurnOnce;
            }
            return defaultReelSetting.timeTurnOnce;
        }

        /// <summary>
        /// ��ȡ��ת�ص���ʱ��
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public float GetTimeReboundStart(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].timeReboundStart != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].timeReboundStart;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).timeReboundStart != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).timeReboundStart;
            }
            return defaultReelSetting.timeReboundStart;
        }

        /// <summary>
        /// ��ȡ�����ص���ʱ��
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public float GetTimeReboundEnd(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].timeReboundEnd != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].timeReboundEnd;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).timeReboundEnd != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).timeReboundEnd;
            }
            return defaultReelSetting.timeReboundEnd;
        }


        /// <summary>
        /// ����������ת����ʱʱ��
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public float GetTimeTurnStartDelay(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].timeTurnStartDelay != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].timeTurnStartDelay;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).timeTurnStartDelay != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).timeTurnStartDelay;
            }

            return defaultReelSetting.timeTurnStartDelay;
        }


        /// <summary>
        /// ��ȡ��ת�ص���ƫ����
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public float GetOffsetYReboundStart(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].offsetYReboundStart != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].offsetYReboundStart;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).offsetYReboundStart != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).offsetYReboundStart;
            }
            return defaultReelSetting.offsetYReboundStart;
        }

        /// <summary>
        /// ��ȡ�����ص���ƫ����
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public float GetOffsetYReboundEnd(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].offsetYReboundEnd != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].offsetYReboundEnd;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).offsetYReboundEnd != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).offsetYReboundEnd;
            }
            return defaultReelSetting.offsetYReboundEnd;
        }

        /// <summary>
        /// ��ȡ���ֲ���ת��Ȧ��
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public int GetNumReelTurnGap(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].numReelTurnGap != ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].numReelTurnGap;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).numReelTurnGap != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).numReelTurnGap;
            }
            return defaultReelSetting.numReelTurnGap;

        }


        /// <summary>
        /// �������ֳ���Ҫת����Ȧ��
        /// </summary>
        /// <param name="reelIndex"></param>
        /// <returns></returns>
        public int GetNumReelTurn(int reelIndex = -1)
        {
            if (dynamicReelSettings.ContainsKey(reelIndex) && dynamicReelSettings[reelIndex].numReelTurn !=  ReelSetting.NONE)
            {
                return dynamicReelSettings[reelIndex].numReelTurn;
            }
            if (ContainsEachReelSetting(reelIndex) && GetEachReelSettings(reelIndex).numReelTurn != ReelSetting.NONE)
            {
                return GetEachReelSettings(reelIndex).numReelTurn;
            }
            return defaultReelSetting.numReelTurn;
        }








        #region Ӯ�������

        /// <summary> ��5���� </summary>
        //public SymbolLineShowInfo win5Kind;

        /// <summary> ��5���� </summary>
        //public SymbolLineShowInfo win;

        #endregion
    }
}