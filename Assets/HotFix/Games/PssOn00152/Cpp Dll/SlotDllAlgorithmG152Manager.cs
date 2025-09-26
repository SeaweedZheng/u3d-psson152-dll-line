using GameMaker;
using System.Text;
using System;
using PssOn00152;
using Newtonsoft.Json;
using UnityEngine;
using Spine.Unity;
using JP;
using System.Collections.Generic;

namespace SlotDllAlgorithmG152
{
    public class SlotDllAlgorithmG152Manager : MonoSingleton<SlotDllAlgorithmG152Manager>
    {
        string prefix => "【Dll Algorithm】";



        const string CACHE_ALGORITHM_G152_PULL_DATA = "CACHE_ALGORITHM_G152_PULL_DATA";
        const string CACHE_ALGORITHM_G152_SLOT_DATA = "CACHE_ALGORITHM_G152_SLOT_DATA";
        const string CACHE_ALGORITHM_G152_LINK_INFO = "CACHE_ALGORITHM_G152_LINK_INFO";
        const string CACHE_ALGORITHM_G152_SUMMARY = "CACHE_ALGORITHM_G152_SUMMARY";


        const string CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL0 = "CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL0";
        const string CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL1 = "CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL1";
        const string CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL2 = "CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL2";
        const string CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL3 = "CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL3";

        const string CACHR_ALGORITHM_G152_WAVE_SCORE = "CACHR_ALGORITHM_G152_WAVE_SCORE";  //波动分数 funded



        string curWaveScore
        {
            get
            {
                if (m_CurWaveScore == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHR_ALGORITHM_G152_WAVE_SCORE, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurWaveScore = str;
                }
                return m_CurWaveScore;
            }
            set
            {
                m_CurWaveScore = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHR_ALGORITHM_G152_WAVE_SCORE, m_CurWaveScore);
            }
        }
        string m_CurWaveScore = null;



        string curJackpotDataSel0
        {
            get
            {
                if (m_CurJackpotDataSel0 == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL0, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurJackpotDataSel0 = str;
                }
                return m_CurJackpotDataSel0;
            }
            set
            {
                m_CurJackpotDataSel0 = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL0, m_CurJackpotDataSel0);
            }
        }
        string m_CurJackpotDataSel0 = null;



        string curJackpotDataSel1
        {
            get
            {
                if (m_CurJackpotDataSel1 == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL1, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurJackpotDataSel1 = str;
                }
                return m_CurJackpotDataSel1;
            }
            set
            {
                m_CurJackpotDataSel1 = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL1, m_CurJackpotDataSel1);
            }
        }
        string m_CurJackpotDataSel1 = null;


        string curJackpotDataSel2
        {
            get
            {
                if (m_CurJackpotDataSel2 == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL2, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurJackpotDataSel2 = str;
                }
                return m_CurJackpotDataSel2;
            }
            set
            {
                m_CurJackpotDataSel2 = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL2, m_CurJackpotDataSel2);
            }
        }
        string m_CurJackpotDataSel2 = null;


        string curJackpotDataSel3
        {
            get
            {
                if (m_CurJackpotDataSel3 == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL3, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurJackpotDataSel3 = str;
                }
                return m_CurJackpotDataSel3;
            }
            set
            {
                m_CurJackpotDataSel3 = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_JACKPOT_DATA_SEL3, m_CurJackpotDataSel3);
            }
        }
        string m_CurJackpotDataSel3 = null;


        string curSummary
        {
            get
            {
                if (m_CurSummary == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_SUMMARY, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurSummary = str;
                }
                return m_CurSummary;
            }
            set
            {
                m_CurSummary = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_SUMMARY, m_CurSummary);
            }
        }
        string m_CurSummary = null;
        string curLinkInfo
        {
            get
            {
                if (m_CurLinkInfo == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_LINK_INFO, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurLinkInfo = str;
                }
                return m_CurLinkInfo;
            }
            set
            {
                m_CurLinkInfo = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_LINK_INFO, m_CurLinkInfo);
            }
        }
        string m_CurLinkInfo = null;

        string curPullData
        {
            get
            {
                if (m_CurPullData == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_PULL_DATA, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurPullData = str;
                }
                return m_CurPullData;
            }
            set
            {
                m_CurPullData = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_PULL_DATA, m_CurPullData);
            }
        }
        string m_CurPullData = null;


        public string curSlotData
        {
            get
            {
                if (m_CurSlotData == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(CACHE_ALGORITHM_G152_SLOT_DATA, "");
                    if (!string.IsNullOrEmpty(str))
                        m_CurSlotData = str;
                }
                return m_CurSlotData;
            }
            set
            {
                m_CurSlotData = value;
                SQLitePlayerPrefs03.Instance.SetString(CACHE_ALGORITHM_G152_SLOT_DATA, m_CurSlotData);
            }
        }
        string m_CurSlotData = null;



        public void SetWaveScore(int waveScore)
        {
            curWaveScore = waveScore.ToString();
            SlotDllInterfaceG152.DevSetWaveScore(waveScore);
        }

        /*
        * 获取波动分数
        */
        public int GetWaveScore()
        {
            return SlotDllInterfaceG152.DevGetWaveScore();
        }

        /*
         * 获取波动分数列表
         */
        public int[] waveScoreLst => new int[]
        {
            1000,2000,5000,10000,20000,50000
        };




        public void Init(int totalBet)
        {
            int div = ConsoleBlackboard02.Instance.dllLevelIndex;

            if (!string.IsNullOrEmpty(curWaveScore))
            {
                int funded = int.Parse(curWaveScore);
                
                SlotDllInterfaceG152.DevInit((int)SboxIdeaGameState.NormalSpin, div, funded);
                //Debug.LogError($"返还数据1：GameState: {(int)SboxIdeaGameState.NormalSpin}  div: {div}  funded: {funded} ");
            }
            else
            {
                int funded = GetWaveScore();
                SlotDllInterfaceG152.DevInit((int)SboxIdeaGameState.NormalSpin, div, funded);
                //Debug.LogError($"返还数据2：GameState: {(int)SboxIdeaGameState.NormalSpin}  div: {div}  funded: {funded} ");
            }


            if (!string.IsNullOrEmpty(curPullData)
                && !string.IsNullOrEmpty(curSlotData)
                && !string.IsNullOrEmpty(curLinkInfo))
            {
                PullData pPullData = JsonConvert.DeserializeObject<PullData>(curPullData);
                SlotData pSlotData = JsonConvert.DeserializeObject<SlotData>(curSlotData);
                LinkInfo pLinkInfo = JsonConvert.DeserializeObject<LinkInfo>(curLinkInfo);

                SlotDllInterfaceG152.DevSetData(DoTotalToSel(totalBet), ref pPullData, ref pSlotData, ref pLinkInfo);
                //Debug.LogError($"返还数据：PullData: {curPullData}  SlotData: {curSlotData}  LinkInfo: {curLinkInfo} ");
            }

            if (!string.IsNullOrEmpty(curJackpotDataSel0))
            {
                JackpotData pJackpotData = JsonConvert.DeserializeObject<JackpotData>(curJackpotDataSel0);

                SlotDllInterfaceG152.DevSetJackpotData(0, ref pJackpotData);
                //Debug.LogError($"返还数据：JackpotData0: {curJackpotDataSel0} ");
            }

            if (!string.IsNullOrEmpty(curJackpotDataSel1))
            {
                JackpotData pJackpotData = JsonConvert.DeserializeObject<JackpotData>(curJackpotDataSel1);

                SlotDllInterfaceG152.DevSetJackpotData(1, ref pJackpotData);
                //Debug.LogError($"返还数据：JackpotData1: {curJackpotDataSel1} ");
            }

            if (!string.IsNullOrEmpty(curJackpotDataSel2))
            {
                JackpotData pJackpotData = JsonConvert.DeserializeObject<JackpotData>(curJackpotDataSel2);

                SlotDllInterfaceG152.DevSetJackpotData(2, ref pJackpotData);
                //Debug.LogError($"返还数据：JackpotData2: {curJackpotDataSel2} ");
            }


            if (!string.IsNullOrEmpty(curJackpotDataSel3))
            {
                JackpotData pJackpotData = JsonConvert.DeserializeObject<JackpotData>(curJackpotDataSel3);

                SlotDllInterfaceG152.DevSetJackpotData(3, ref pJackpotData);
                //Debug.LogError($"返还数据：JackpotData3: {curJackpotDataSel3} ");
            }


            if (!string.IsNullOrEmpty(curSummary))
            {
                Summary pSummary = JsonConvert.DeserializeObject<Summary>(curSummary);

                SlotDllInterfaceG152.DevSetSummary(ref pSummary);
                //Debug.LogError($"返还数据：Summary: {curSummary} ");
            }



        }



        /// <summary>
        /// 获取彩金档位
        /// </summary>
        /// <returns></returns>
        int DoTotalToSel(long totalBet)
        {
            if (totalBet <= 100)
                return 0;
            else if (totalBet <= 200)
                return 1;
            else if (totalBet <= 300)
                return 2;
            else
                return 3;
        }

        public string GetVersion()
        {
            OnDebug(true, "GetVersion", "");

            byte[] byteArray = new byte[512];
            SlotDllInterfaceG152.DevVersion(ref byteArray[0]);
            // 找到第一个 \0 的位置
            int nullTerminatorIndex = Array.IndexOf(byteArray, (byte)0);
            // 如果没有 \0，则使用整个数组；否则截取到 \0 之前
            int length = (nullTerminatorIndex >= 0) ? nullTerminatorIndex : byteArray.Length;
            // 转换并去除多余的 \0
            string resultStr = Encoding.Default.GetString(byteArray, 0, length);

            OnDebug(false, "GetVersion", $"ver: {resultStr}");

            return resultStr;
        }

        /// <summary>
        /// 获取游戏彩金值
        /// </summary>
        /// <param name="totalBet"></param>
        /// <returns></returns>
        public JackpotInitInfo GetJackpotGame(long totalBet)
        {
            OnDebug(true, "GetJackpotGame", $"totalBet: {totalBet} sel: {DoTotalToSel(totalBet)}");

            JackpotInitInfo pJackpotInitInfo = new JackpotInitInfo();
            SlotDllInterfaceG152.DevGetJackpot(DoTotalToSel(totalBet), ref pJackpotInitInfo);

            OnDebug(false, "GetJackpotGame", $"res: {JsonConvert.SerializeObject(pJackpotInitInfo)}");
            return pJackpotInitInfo;
        }

        void OnDebug(bool isRpcUp, string rpCName,string data)
        {
            if(isRpcUp)
                DebugUtils.LogWarning($"==@ {prefix}<color=green>rpc up</color>: {rpCName}  {data}");
            else
                DebugUtils.LogWarning($"==@ {prefix}<color=yellow>rpc down</color>: {rpCName}  {data}");
        }

        public LinkInfo GetSlotSpin(long totalBet)
        {
            int pid = ConsoleBlackboard02.Instance.pid;

            OnDebug(true, "GetSlotSpin", $"pid:{pid}  totalBet:{totalBet}");

            PullData pPullData = new PullData();  // 这个是机台专用数据！

            SlotData pSlotData = new SlotData();  // 这个是机台专用数据！
            //pSlotData.fgInfo = new FgInfos();
            pSlotData.lines = new byte[11];
            pSlotData.links = new byte[9];
            pSlotData.rewards = new byte[10];
            pSlotData.frees = new byte[6];
            pSlotData.mWaveRaid = new sbyte[32];
            pSlotData.mWaveUp = new ColorInfos[2]
            {
                new ColorInfos(),
                new ColorInfos(),
            };
            pSlotData.mWaveDown = new ColorInfos[2]
            {
                new ColorInfos(),
                new ColorInfos(),
            };

            LinkInfo pLinkInfo = new LinkInfo();  // 普通游戏
            pLinkInfo.lottery = new int[4];
            pLinkInfo.jackpotlottery = new double[4];
            pLinkInfo.jackpotout = new double[4];
            pLinkInfo.jackpotold = new double[4];
            pLinkInfo.linkData = new LinkData[3]
            {
                new LinkData(),
                new LinkData(),
                new LinkData()
            };


            // 获取slot spin 数据
            int gameState = SlotDllInterfaceG152.DevSpin(pid, (int)totalBet, ref pPullData, ref pSlotData, ref pLinkInfo);

            //Debug.LogError("pPullData" + JsonConvert.SerializeObject(pPullData));
            //Debug.LogError("pSlotData" + JsonConvert.SerializeObject(pSlotData));
            //Debug.LogError("pLinkInfo" + JsonConvert.SerializeObject(pLinkInfo) +  $"  gameState: {gameState}");

            curPullData = JsonConvert.SerializeObject(pPullData);
            curSlotData = JsonConvert.SerializeObject(pSlotData);
            curLinkInfo = JsonConvert.SerializeObject(pLinkInfo);
            pLinkInfo.gameState = (byte)gameState;  // pLinkInfo.gameState 固定是0；需要动态修改。

            // 保存4档彩金参数
            for (int i = 0; i < 4; i++)
            {
                JackpotData pJackpotData = new JackpotData();

                pJackpotData.mJackpotTotal = new JackpotTotal();
                pJackpotData.mJackpotTotal.mfPullRemainCent = new double[4];
                pJackpotData.mJackpotTotal.mfOutPullRemainCent = new double[4];
                pJackpotData.mJackpotTotal.mfInnerPullRemainCent = new double[4];
                pJackpotData.mJackpotTotal.mfJpCent = new double[4];
                pJackpotData.mJackpotTotal.mfInnerRemainCent = new double[4];
                pJackpotData.mJackpotTotal.mfInnerClPullCent = new double[4]; // 已初始化
                pJackpotData.mJackpotTotal.mfTotalCent = new double[4];
                pJackpotData.mJackpotTotal.mfOutTotalCent = new double[4];
                pJackpotData.mJackpotTotal.mfTotalLottery = new double[4];
                pJackpotData.mJackpotTotal.mfOutPullCent = new double[4];
                pJackpotData.mJackpotTotal.mfInnerPullCent = new double[4];
                pJackpotData.mJackpotTotal.mfOutTotalPullCent = new double[4];
                pJackpotData.mJackpotTotal.mfInnerTotalPullCent = new double[4];
                pJackpotData.mJackpotTotal.mfTotalPullCent = new double[4];
                pJackpotData.mJackpotTotal.mfJpPullCent = new double[4];
                pJackpotData.mJackpotTotal.mfCompensateCent = new double[4];
                pJackpotData.mJackpotTotal.mfCompensatePullCent = new double[4];
                pJackpotData.mJackpotTotal.mfCompensatePullRemainCent = new double[4];
                pJackpotData.mJackpotTotal.mfDiffCent = new double[4];

                pJackpotData.mJackpotBase = new JackpotBase();
                pJackpotData.mJackpotBase.mfBaseValue = new double[4];           // 基本值
                pJackpotData.mJackpotBase.mfLowValue = new double[4];            // 最小触发值
                pJackpotData.mJackpotBase.mfHighValue = new double[4];           // 最大触发值
                pJackpotData.mJackpotBase.mfMidValue = new double[4];            // 平均触发值
                pJackpotData.mJackpotBase.mBetThreshold = new int[4];             // 最低压分值
                pJackpotData.mJackpotBase.mBetThresholdMax = new int[4];         // 最高压分值
                pJackpotData.mJackpotBase.mJackpotTrigger = new int[40];         // 二维展平数组 [4][10]
                pJackpotData.mJackpotBase.mJackpotIdx = new int[4];              // 彩金索引
                pJackpotData.mJackpotBase.mfOutPercent = new double[4];          // 显示百分比
                pJackpotData.mJackpotBase.mfInnerPercent = new double[4];       // 隐藏百分比
                pJackpotData.mJackpotBase.mJpWeight = new int[4];                // 彩金比重
                pJackpotData.mJackpotBase.mJpTriggerDiff = new double[4];        // 触发差值
                pJackpotData.mJackpotBase.mfRangeValue = new double[4];         // 开奖范围


                SlotDllInterfaceG152.DevGetJackpotData(i, ref pJackpotData);
                switch (i)
                {
                    case 0:
                        curJackpotDataSel0 = JsonConvert.SerializeObject(pJackpotData);
                        break;
                    case 1:
                        curJackpotDataSel1 = JsonConvert.SerializeObject(pJackpotData);
                        break;
                    case 2:
                        curJackpotDataSel2 = JsonConvert.SerializeObject(pJackpotData);
                        break;
                    case 3:
                        curJackpotDataSel3 = JsonConvert.SerializeObject(pJackpotData);
                        break;
                }

            }

            //数据上报
            Summary pSummary = new Summary();
            pSummary.freeGameSummary = new FreeGameSummary
            {
                round = new int[3],
                counter = new int[3],
                score = new int[3]
            };
            pSummary.iconSummary = new IconSummary
            {
                counter = new int[30],
                score = new int[30]
            };
            pSummary.jpSummary = new JpSummary
            {
                counter = new int[16],
                score = new int[16]
            };
            pSummary.storeSummary = new StoreSummary
            {
                reelTotalPull = new double[4],     // 转轮累积提拨库
                linkTotalPull = new double[4],     // 连线累积提拨库
                linkPull = new double[4],          // 连线提拨库
                linkTotalScore = new double[4],    // 连线总得分数
                freeTotalPull = new double[4],     // 免费累积提拨库
                freePull = new double[4],          // 免费提拨库
                freeTotalScore = new double[4]     // 免费游戏得分数
            };


            SlotDllInterfaceG152.DevGetSummary(ref pSummary);
            curSummary = JsonConvert.SerializeObject(pSummary);


            // 中了彩金时，调用
           /* for (int i = 0; i < 4; i++)
            {
                if (pLinkInfo.lottery[i] == 1)
                {
                    SlotDllInterfaceG152.DevJpEnd();
                    break;
                }
            }*/

            SboxIdeaGameState state = (SboxIdeaGameState)gameState;


            if (state == SboxIdeaGameState.JackpotGame)
            {
                SlotDllInterfaceG152.DevJpEnd();
            }
            if (state == SboxIdeaGameState.FreeSpinStart)
            {
                SlotDllInterfaceG152.DevFreeStart();
            }
            if (state == SboxIdeaGameState.FreeSpinEnd)
            {
                SlotDllInterfaceG152.DevFreeEnd();
            }


            OnDebug(false, "GetSlotSpin", $"res: {JsonConvert.SerializeObject(pLinkInfo)}  gameState: {gameState}");
            return pLinkInfo;
        }



        public void SetLevel(int level)
        {
            SlotDllInterfaceG152.DevSetLevel(level);
        }


        public int GetLevel()
        {
            return SlotDllInterfaceG152.DevGetLevel();
        }

        /* public string[] levelLst => new string[]
         {
             "1.050","1.000","0.995","0.990","0.985","0.980","0.975","0.970","0.965","0.960"
         };
        */
        public string[] levelLst => new string[]
        {
            "1050","1000","995","990","985","980","975","970","965","960"
        };


        public void Clear()
        {
            SlotDllInterfaceG152.DevClear();
        }

    }


}