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
        [Tooltip("游戏id")]
        public int gameId = -1;

        [Title("机台参数")]


        #region 彩金设置
        [Title("彩金设置")]

        /// <summary> 彩金巨奖最大值 </summary>
        [Tooltip("彩金巨奖范围最大值")]
        public long jpGrandRangeMax = 80000;

        /// <summary> 彩金巨奖最小值 </summary>
        [Tooltip("彩金巨奖范围最小值")]
        public long jpGrandRangeMin = 50000;

        /// <summary> 彩金巨奖最大值 </summary>
        [Tooltip("彩金巨奖最大值")]
        public long defJpGrandMax = 80000;

        /// <summary> 彩金巨奖最小值 </summary>
        [Tooltip("彩金巨奖最小值")]
        public long defJpGrandMin = 50000;

        /// <summary> 彩金头奖最大值 </summary>
        [Tooltip("彩金头奖范围最大值")]
        public long jpMajorRangeMax = 30000;

        /// <summary> 彩金头奖最小值 </summary>
        [Tooltip("彩金头奖范围最小值")]
        public long jpMajorRangeMin = 10000;

        /// <summary> 彩金头奖最大值 </summary>
        [Tooltip("彩金头奖最大值")]
        public long defJpMajorMax = 30000;

        /// <summary> 彩金头奖最小值 </summary>
        [Tooltip("彩金头奖最小值")]
        public long defJpMajorMin = 10000;


        /// <summary> 彩金大奖最大值 </summary>
        [Tooltip("彩金大奖范围最大值")]
        public long jpMinorRangeMax = 8000;

        /// <summary> 彩金大奖最小值 </summary>
        [Tooltip("彩金大奖范围最小值")]
        public long jpMinorRangeMin = 5000;


        /// <summary> 彩金大奖最大值 </summary>
        [Tooltip("彩金大奖最大值")]
        public long defJpMinorMax = 8000;

        /// <summary> 彩金大奖最小值 </summary>
        [Tooltip("彩金大奖最小值")]
        public long defJpMinorMin = 5000;





        /// <summary> 彩金小奖最大值 </summary>
        [Tooltip("彩金小奖范围最大值")]
        public long jpMiniRangeMax = 3000;

        /// <summary> 彩金小奖最小值 </summary>
        [Tooltip("彩金小奖范围最小值")]
        public long jpMiniRangeMin = 1000;

        /// <summary> 彩金小奖最大值 </summary>
        [Tooltip("彩金小奖最大值")]
        public long defJpMiniMax = 3000;

        /// <summary> 彩金小奖最小值 </summary>
        [Tooltip("彩金小奖最小值")]
        public long defJpMiniMin = 1000;


        #endregion

        #region 用户设置
        [Title("用户设置")]

        [Title("用户密码Admin")]
        public string passwordAdmin = "187653214";
        [Title("用户密码Manager")]
        public string passwordManager = "88888888";
        [Title("用户密码Shift")]
        public string passwordShift = "666666";
        #endregion

        #region 语言
        [Title("语言")]

        public string defLanguage = "cn";
        #endregion

        #region 音乐
        [Title("音效设置")]
        /// <summary> 音效 </summary>
        public float defSound = 0.5f;

        /// <summary> 背景音乐 </summary>
        public float defMusic = 0.5f;
        #endregion

        #region 数据记录设置
        [Title("数据记录设置")]

        [Tooltip("最大游戏次数记录")]
        public int maxMaxGameRecord = 50000;
        [Tooltip("最小游戏次数记录")]
        public int minMaxGameRecord = 100;
        [Tooltip("默认游戏次数记录")]
        public int defMaxGameRecord = 1000;

        [Tooltip("最大投退币次数记录")]
        public int maxMaxCoinInOutRecord = 50000;
        [Tooltip("最少投退币次数记录")]
        public int minMaxCoinInOutRecord = 100;
        [Tooltip("默认投退币次数记录")]
        public int defMaxCoinInOutRecord = 1000;

        [Tooltip("最大彩金次数记录")]
        public int maxMaxJackpotRecord = 50000;
        [Tooltip("最下彩金次数记录")]
        public int minMaxJackpotRecord = 100;
        [Tooltip("默认彩金次数记录")]
        public int defMaxJackpotRecord = 1000;

        [Tooltip("最大报错次数记录")]
        public int maxMaxErrorRecord = 5000;
        [Tooltip("最下报错次数记录")]
        public int minMaxErrorRecord = 100;
        [Tooltip("默认报错次数记录")]
        public int defMaxErrorRecord = 500;


        [Tooltip("最大事件次数记录")]
        public int maxMaxEventRecord = 5000;
        [Tooltip("最下事件次数记录")]
        public int minMaxEventRecord = 100;
        [Tooltip("默认事件次数记录")]
        public int defMaxEventRecord = 500;


        [Tooltip("日营收统计记录最大次数")]
        public int maxMaxBusinessDayRecord = 720;
        [Tooltip("日营收统计记录最小次数")]
        public int minMaxBusinessDayRecord = 1;
        [Tooltip("默认日营收统计记录次数")]
        public int defMaxBusinessDayRecord = 7;


        #endregion

        #region 投退币设置
        [Title("投退币设置")]

        [Tooltip("最大上下分倍率(1脉冲多少分)")]
        public int maxScoreUpDownScale = 10000;
        [Tooltip("最小上下分倍率(1脉冲多少分)")]
        public int minScoreUpDownScale = 100;
        [Tooltip("默认上下分倍率(1脉冲多少分)")]
        public int defScoreUpDownScale = 100;

        
        [Tooltip("最大上分长按倍率(1脉冲多少分)")]
        public int maxScoreUpLongClickScale = 100000;
        [Tooltip("最小上分长按倍率(1脉冲多少分)")]
        public int minScoreUpLongClickScale = 1000;
        [Tooltip("默认上分长按倍率(1脉冲多少分)")]
        public int defScoreUpLongClickScale = 1000;


        /// <summary>1币几分 最大值 </summary>
        [Tooltip("最大投币倍率(1币几分)")]
        public int maxCoinInScale = 200;
        /// <summary>1币几分 最小值 </summary>
        [Tooltip("最小投币倍率(1币几分)")]
        public int minCoinInScale = 10;
        [Tooltip("默认投币倍率(1币几分)")]
        public int defCoinInScale = 10;


        /// <summary> “1票几分”最大值 </summary>
        [Tooltip("最大退票倍率(1票几分)")]
        public readonly int maxCoinOutPerTicket2Credit = 200;
        /// <summary> “1票几分”最小值 </summary>
        [Tooltip("最小退票倍率(1票几分)")]
        public readonly int minCoinOutPerTicket2Credit = 0;
        [Tooltip("默认退票倍率(1票几分)")]
        public readonly int defCoinOutPerTicket2Credit = 200;


        /// <summary>  “1分几票”最大值 </summary>
        [Tooltip("最大退票倍率(1分几票)")]
        public readonly int maxCoinOutPerCredit2Ticket = 50;
        /// <summary>  “1分几票”最小值 </summary>
        [Tooltip("最小退票倍率(1分几票)")]
        public readonly int minCoinOutPerCredit2Ticket = 0;
        [Tooltip("默认退票倍率(1分几票)")]
        public readonly int defCoinOutPerCredit2Ticket = 0;


        /// <summary> “1钞几分”最大值  </summary>
        [Tooltip("最大进钞倍率(1钞几分)")]
        public readonly int maxBillInScale = 100;
        /// <summary> “1钞几分”最小值 </summary>
        [Tooltip("最小进钞倍率(1钞几分)")]
        public readonly int minBillInScale = 1;
        [Tooltip("默认进钞倍率(1钞几分)")]
        public readonly int defBillInScale = 1;


        /// <summary> “打印”最大值  </summary>
        [Tooltip("最大打印倍率(1钞多少分)")]
        public readonly int maxPrintOutScale = 100;
        /// <summary> “打印”最小值 </summary>
        [Tooltip("最小打印倍率(1钞多少分)")]
        public readonly int minPrintOutScale = 10;
        [Tooltip("默认打印倍率(1钞多少分)")]
        public readonly int defPrintOutScale = 10;

        [Tooltip("主货币和游戏分兑换比例(1钞多少分)")]
        public int moneyMeterScale = 100;
        #endregion



        /// <summary> “打印”最大值  </summary>
        [Tooltip("最大打印倍率(1钞多少分)")]
        public readonly int maxJackpotPercent = 100;
        /// <summary> “打印”最小值 </summary>
        [Tooltip("最小打印倍率(1钞多少分)")]
        public readonly int minJackpotPercent = 1;
        [Tooltip("默认打印倍率(1钞多少分)")]
        public readonly int defJackpotPercent = 5;




        /// <summary> 是否显示打印日志 </summary>
        public int isDebug => ApplicationSettings.Instance.isRelease? 0 : 1;

        /// <summary> 是否显示包更新信息 </summary>
        public int isUpdateInfo => ApplicationSettings.Instance.isRelease ? 0 : 1;

        /// <summary> 是否打开联网彩金 </summary>
        public int  isJackpotOnline => 1;

        /// <summary> 使用调试页面 </summary>
        public int enableReporterPage => ApplicationSettings.Instance.isRelease ? 0 : 1;

        /// <summary> 使用测试工具 </summary>
        public int enableTestTool => ApplicationSettings.Instance.isRelease ? 0 : 1;
    }
}
