using GameMaker;
using JP;
using System.Collections;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Random = System.Random;

//最多四个彩金
/*
* 3 小彩金
* 2 中彩金
* 1 大彩金
* 0 巨大彩金

namespace JP
{
    
    public class JackpotInfo
    {
        public int machineId;
        public int seatId;
        public int[] lottery = new int[4];   //0:表示没有开出彩金，1:表示已开出彩金
        public double[] jackpotlottery = new double[4]; //开出的彩金,注意:此处的单位是钱的单位，而且是乘以了100的，分机收到这个值要根据分机的分值比来转成成对应的分数，而且还要将此值除以100
        public double[] jackpotout = new double[4];     //彩金显示积累分
        public double[] jackpotold = new double[4];     //开出彩金前的显示积累分
        public double[] ScoreRate = new double[4];                   //分数比例，彩金服务器发给接入服务器的数组，目前默认为1,采用一分几元的模式
        public double[] triggerScore = new double[4]; //触发分数
        public double[] openAcc = new double[4];    //彩金开奖累积分
    }

    public class JackpotBase
    {
        public double[] mfBaseValue = new double[4];        //基本值
        public double[] mfLowValue = new double[4];     //最小触发值
        public double[] mfHighValue = new double[4];        //最大触发值
        public double[] mfMidValue = new double[4];     //平均触发值
        public int[] mBetThreshold = new int[4];             //每个彩金的最低压分值;
        public int[] mBetThresholdMax = new int[4];              //每个彩金的最高压分值;
        public int[][] mJackpotTrigger = new int[4][];
        public int[] mJackpotIdx = new int[4];

        public double[] mfOutPercent = new double[4];       //显示值的百分比   
        public double[] mfInnerPercent = new double[4];     //隐藏值的百分比
        public double[] mfSettingPercent = new double[4];   //设定的彩金百分比

        public int[] mJpWeight = new int[4];     //彩金比重
        public int mJpTotalWeight;             //彩金比重总和
        public double[] mJpTriggerDiff = new double[4];       //触发差值
                                            //2025/01/11增加修改
        public double[] mfRangeValue = new double[4]; //彩金开奖范围
    }

  
    public class JackpotTotal
    {
        //提拔余数
        public double[] mfPullRemainCent = new double[4];     //彩金提拔余数
        public double[] mfOutPullRemainCent = new double[4];  //彩金显示提拔余数
        public double[] mfInnerPullRemainCent = new double[4];  //彩金隐藏提拔余数

        public double[] mfJpCent = new double[4];           //开出的彩金分数
        public double[] mfInnerRemainCent = new double[4];  //内部剩余积分
        public double[] mfInnerClPullCent = new double[4];  //内部彩金提拨分

        public double[] mfTotalCent = new double[4];        //彩金开奖累积分
        public double[] mfOutTotalCent = new double[4];     //彩金显示累积分
        public double[] mfTotalLottery = new double[4];     //开出彩金积累分数

        //彩金显示提拨分
        public double[] mfOutPullCent = new double[4];
        //彩金隐藏提拨分
        public double[] mfInnerPullCent = new double[4];

        //提拨分
        public double[] mfOutTotalPullCent = new double[4];                     //彩金显示累计提拨分
        public double[] mfInnerTotalPullCent = new double[4];                   //彩金隐藏累计提拨分
        public double[] mfTotalPullCent = new double[4];                        //彩金累计提拔分
        public double[] mfJpPullCent = new double[4];                       //4个彩金的彩金提拔分

        //补偿积分
        public double[] mfCompensateCent = new double[4];   //补偿积分
        public double[] mfCompensatePullCent = new double[4];   //补偿彩金提拨分
        public double[] mfCompensatePullRemainCent = new double[4]; //补偿彩金提拨余数
        public double[] mfDiffCent = new double[4]; //差值

        public double mfPullRemainCentTotal;                    //所有的彩金提拨余数
    }

    public class BetInfo
    {
        public int machineId;
        public int seatId;
        public double bet;
        //压分比例
        public double betPercent;
        public double ScoreRate;                   //分机分值比 分数比例,采用一分几元的模式,接入的服务器需要每次上发这个数值
        public double JpPercent;                    //分机彩金百分比
    }

    //2
    public class JackpotData
    {
        public JackpotBase mJackpotBase = new JackpotBase();
        public JackpotTotal mJackpotTotal = new JackpotTotal();
    }
}

*/

public class MySteryJackpot02 : MonoBehaviour //Singleton<MySteryJackpot>
{

    private static MySteryJackpot02 _instance;

    public static MySteryJackpot02 Instance
    {
        get
        {
            if (_instance == null)
            {
                var founds = FindObjectsOfType(typeof(MySteryJackpot02));
                if (founds.Length > 0)
                {
                    _instance = (MySteryJackpot02)founds[0];
                    if (_instance.transform.parent == null) // 判断是否是根节点，
                        DontDestroyOnLoad(_instance.gameObject);
                }
                else
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<MySteryJackpot02>();
                    singleton.name = "MySteryJackpot02";    //"(Singleton)" + typeof(MoneyBoxManager).ToString();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    Coroutine corSaveJPGame;
    void DoTaskSaveJPGame(Action func,float timeS)
    {
        if(corSaveJPGame != null) 
            StopCoroutine(corSaveJPGame);

        corSaveJPGame = StartCoroutine(DoTask(() =>
        {
            func?.Invoke();
            corSaveJPGame = null;
        }, timeS));
    }
    IEnumerator DoTask(Action cb, float timeS)
    {
        yield return new WaitForSecondsRealtime(timeS);
        cb?.Invoke();
    }











    static int JACKPOT_MAX = 4;
 //   int mJackpotModel;
	double mFactor;			//当前默认为100
    double mScoreRate;                  //分值比
    double mJpPercent;					//彩金百分比
    public JP.JackpotInfo mJackpotInfo;

    public JP.JackpotData _mJackpotData;
    public JP.JackpotData mJackpotData
    {
        get
        {

            if (string.IsNullOrEmpty(curJPGameDataCache))
            {
                //Debug.LogError("curJPGameDataCache is null when get mJackpotData");
                throw new Exception("curJPGameDataCache is null when get mJackpotData");
            }

            if (_mJackpotData == null)
            {
                string cache = SQLitePlayerPrefs03.Instance.GetString(curJPGameDataCache, "");
                if (!string.IsNullOrEmpty(cache))
                {
                    _mJackpotData = JsonConvert.DeserializeObject<JackpotData>(cache);
                }
                else
                {
                    _mJackpotData = new JP.JackpotData();
                    for (int i = 0; i < 4; i++)
                    {
                        _mJackpotData.mJackpotBase.mfBaseValue[i] = 0;
                        _mJackpotData.mJackpotBase.mfLowValue[i] = 0;
                        _mJackpotData.mJackpotBase.mfHighValue[i] = 0;
                        _mJackpotData.mJackpotBase.mfMidValue[i] = 0;

                        _mJackpotData.mJackpotBase.mfOutPercent[i] = 0;
                        _mJackpotData.mJackpotBase.mfInnerPercent[i] = 0;
                        _mJackpotData.mJackpotBase.mfSettingPercent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfPullRemainCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfJpCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfInnerRemainCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfTotalCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfOutTotalCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfOutTotalPullCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfInnerTotalPullCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfOutPullRemainCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfTotalLottery[i] = 0;

                        _mJackpotData.mJackpotTotal.mfOutPullCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfInnerPullCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfTotalPullCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfInnerClPullCent[i] = 0;

                        _mJackpotData.mJackpotTotal.mfCompensateCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] = 0;
                        _mJackpotData.mJackpotTotal.mfDiffCent[i] = 0;

                        _mJackpotData.mJackpotBase.mJpWeight[i] = 0;

                        _mJackpotData.mJackpotBase.mJpTriggerDiff[i] = 0;

                        _mJackpotData.mJackpotTotal.mfJpPullCent[i] = 0;

                        _mJackpotData.mJackpotBase.mJackpotIdx[i] = 0;
                        _mJackpotData.mJackpotBase.mBetThreshold[i] = 0;
                        _mJackpotData.mJackpotBase.mBetThresholdMax[i] = 0;
                        _mJackpotData.mJackpotBase.mfRangeValue[i] = 0;
                    }

                    _mJackpotData.mJackpotBase.mJpWeight[0] = 100;// 2;
                    _mJackpotData.mJackpotBase.mJpWeight[1] = 100;// 2;
                    _mJackpotData.mJackpotBase.mJpWeight[2] = 200;// 1;
                    _mJackpotData.mJackpotBase.mJpWeight[3] = 200;// 1;

                    _mJackpotData.mJackpotBase.mJpTotalWeight = 600;

                    _mJackpotData.mJackpotTotal.mfPullRemainCentTotal = 0;

                    ResetConfig();  //这里会循环嵌套调用？


                    string curData = JsonConvert.SerializeObject(_mJackpotData);
                    SQLitePlayerPrefs03.Instance.SetString(curJPGameDataCache, curData);
                }
                return _mJackpotData;
            }

            if (!isClearDirty)
            {
                isClearDirty = true;
                string lastData = JsonConvert.SerializeObject(_mJackpotData);
                /* Loom.QueueOnMainThread((data) =>
                 {
                     string curData = JsonConvert.SerializeObject(_mJackpotData);

                     if(curData != lastData)
                         SQLitePlayerPrefs02.Instance.SetString(JP_CAME_DATA_CACH,curData);

                     isDirty = false;
                 }, lastData, 0.5f);*/

                DoTaskSaveJPGame(() => {
                    string curData = JsonConvert.SerializeObject(_mJackpotData);

                    if (curData != lastData)
                        SQLitePlayerPrefs03.Instance.SetString(curJPGameDataCache, curData);

                    isClearDirty = false;

                },0.5f);
            }

           return _mJackpotData;
        }
        set
        {
            if (string.IsNullOrEmpty(curJPGameDataCache))
            {
                //Debug.LogError("curJPGameDataCache is null when get mJackpotData");
                throw new Exception("curJPGameDataCache is null when set mJackpotData");
            }

            if (value == null)
                SQLitePlayerPrefs03.Instance.SetString(curJPGameDataCache, "");
            else
            {
                string curData = JsonConvert.SerializeObject(value);
                SQLitePlayerPrefs03.Instance.SetString(curJPGameDataCache, curData);
            }
            _mJackpotData = value;
        }
    }


    Action saveCacheFunc;
    bool isClearDirty = false;

    public void Init(int credit)
    {

        this.mFactor = 100;
        this.mScoreRate = 1;
        mJpPercent = 0.01;

        SetJackpotPool(credit);

        //JackpotBase temp = mJackpotData.mJackpotBase;  //初始化一下
        // SetupJackpotData();
    }

    public void ShuffleExt(int jpType)
    {
        int[] leftValue = new int[5] { 0, 0, 0, 0, 0 };
        int[] rightValue = new int[5] { 0, 0, 0, 0, 0 };
        int[] totalValue = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //TODO
        Random random = new Random();
        for (int i = 0; i < 5; i++)
        {
            int randValue = random.Next(0, (int)(mJackpotData.mJackpotBase.mfRangeValue[jpType] + 1)); // max是排除的上限
            leftValue[i] = randValue + (int)mJackpotData.mJackpotBase.mfLowValue[jpType] + (int)mJackpotData.mJackpotBase.mfRangeValue[jpType] * i;
            rightValue[i] = (int)mJackpotData.mJackpotBase.mfMidValue[jpType] * 2 - leftValue[i];
        }
        for (int i = 0; i < 5; i++)
        {
            totalValue[i] = leftValue[i];
        }
        for (int i = 5; i < 10; i++)
        {
            totalValue[i] = rightValue[i - 5];
        }

        Shuffle(totalValue, 10);

        if (mJackpotData.mJackpotBase.mJackpotTrigger[jpType] == null)
        {
            mJackpotData.mJackpotBase.mJackpotTrigger[jpType] = new int[10];
        }
        for (int i = 0; i < 10; i++)
        {
            mJackpotData.mJackpotBase.mJackpotTrigger[jpType][i] = totalValue[i];
        }
        mJackpotData.mJackpotBase.mJackpotIdx[jpType] = 0;
    }

    public void Shuffle(int[] data,int len)
    {
        Random rand = new Random();
        for (int i = 0; i < len; i++)
        {
            int t = rand.Next(len);
            int temp = data[t];
            data[t] = data[i];
            data[i] = temp;
        }
    }

   

    public void ResetJackopt(int jpType,bool bSetZ)
    {
       
        if (bSetZ)
        {
            mJackpotData.mJackpotTotal.mfOutTotalCent[jpType] = 0;
        }
        else
        {
            mJackpotData.mJackpotTotal.mfOutTotalCent[jpType] = mJackpotData.mJackpotBase.mfBaseValue[jpType];
        }
        mJackpotData.mJackpotTotal.mfTotalCent[jpType] = mJackpotData.mJackpotBase.mfBaseValue[jpType];

    }

    public bool CheckHasOne(int[] data,int len)
    {
        for (int i = 0; i < len; i++)
        {
            if (data[i] == 1) return true;
        }
        return false;
    }

    /*
     * 50 - 100 2倍     0.10/2    保留3位 
     * 150 - 200 4倍    0.10/4    保留3位 
     * 250 - 300 6倍    0.10/6    保留3位  0.001
     * 500 10倍
     * 600 12倍
     */


    const string COR_SAVE_JP_GAME_DATA = "COR_SAVE_JP_GAME_DATA";

    const string JP_GAME_DATA_CACHE0 = "JP_GAME_DATA_CACHE0";
    const string JP_GAME_DATA_CACHE1 = "JP_GAME_DATA_CACHE1";
    const string JP_GAME_DATA_CACHE2 = "JP_GAME_DATA_CACHE2";
    const string JP_GAME_DATA_CACHE3 = "JP_GAME_DATA_CACHE3";
    const string JP_GAME_DATA_CACHE4 = "JP_GAME_DATA_CACHE4";


    string curJPGameDataCache = "";

    float multipleA = 0;

    void SetJackpotPool(int credit)
    {
        string _curJPGameDataCache = "";
        switch (credit)
        {
            case 50:
            case 100:
                {
                    _curJPGameDataCache = JP_GAME_DATA_CACHE0;
                    multipleA = 2;
                }
                break;
            case 150:
            case 200:
                {
                    _curJPGameDataCache = JP_GAME_DATA_CACHE1;
                    multipleA = 4;
                }
                break;
            case 250:
            case 300:
                {
                    _curJPGameDataCache = JP_GAME_DATA_CACHE2;
                    multipleA = 6;
                }
                break;
            case 500:
                {
                    _curJPGameDataCache = JP_GAME_DATA_CACHE3;
                    multipleA = 10;
                }
                break;
            case 600:
                {
                    _curJPGameDataCache = JP_GAME_DATA_CACHE4;
                    multipleA = 12;
                }
                break;
            default:
                Debug.LogError($"游戏彩金配置报错！{credit}");
                throw new Exception("游戏彩金配置报错！");
        }

        if (_curJPGameDataCache != curJPGameDataCache)
        {
            if(corSaveJPGame != null)
                StopCoroutine(corSaveJPGame);
            corSaveJPGame = null;

            if (_mJackpotData != null && !string.IsNullOrEmpty(curJPGameDataCache))
            {
                string curData = JsonConvert.SerializeObject(_mJackpotData);
                SQLitePlayerPrefs03.Instance.SetString(curJPGameDataCache, curData);
            }
            
            _mJackpotData = null;
            curJPGameDataCache = _curJPGameDataCache;
            isClearDirty = false;

            JackpotBase temp = mJackpotData.mJackpotBase;  //初始化一下
        }
    }

    public void ResetConfig()
    {
        mJackpotData.mJackpotBase.mBetThreshold[3] = 100;
        mJackpotData.mJackpotBase.mBetThreshold[2] = 200;
        mJackpotData.mJackpotBase.mBetThreshold[1] = 500;
        mJackpotData.mJackpotBase.mBetThreshold[0] = 1000;

        mJackpotData.mJackpotBase.mBetThresholdMax[3] = 500;
        mJackpotData.mJackpotBase.mBetThresholdMax[2] = 1000;
        mJackpotData.mJackpotBase.mBetThresholdMax[1] = 2500;
        mJackpotData.mJackpotBase.mBetThresholdMax[0] = 5000;

        mJackpotData.mJackpotBase.mJpWeight[0] = 100;// 2;
        mJackpotData.mJackpotBase.mJpWeight[1] = 100;// 2;
        mJackpotData.mJackpotBase.mJpWeight[2] = 200;// 1;
        mJackpotData.mJackpotBase.mJpWeight[3] = 200;// 1;
        mJackpotData.mJackpotBase.mJpTotalWeight = 600;

        /* 旧版本
        SetBaseValue(3, 0.005, 1000, 1200, 2000);
        SetBaseValue(2, 0.005, 2000, 2500, 5000);
        SetBaseValue(1, 0.005, 20000, 25000, 50000);
        SetBaseValue(0, 0.005, 50000, 60000, 100000);
         */

        /*
         * A = 2
         *  SetBaseValue(3, 0.005, 1000 * A, 1200* A, 2000* A);
            SetBaseValue(2, 0.005, 2000* A, 2500* A, 5000* A);
            SetBaseValue(1, 0.005, 20000* A, 25000* A, 50000* A);
            SetBaseValue(0, 0.005, 50000* A, 60000* A, 100000* A);
         */

        SetBaseValue(3, 0.005, 1000 * multipleA, 1200 * multipleA, 2000 * multipleA);
        SetBaseValue(2, 0.005, 2000 * multipleA, 2500 * multipleA, 5000 * multipleA);
        SetBaseValue(1, 0.005, 20000 * multipleA, 25000 * multipleA, 50000 * multipleA);
        SetBaseValue(0, 0.005, 50000 * multipleA, 60000 * multipleA, 100000 * multipleA);

        for (int i = 0; i < 4; i++)
        {
            ShuffleExt( i);
            ResetJackopt(i, false);
        }
    }


    public JP.JackpotInfo JackpotFlexiblePercentExtModify(BetInfo betInfo)
    {
        SetJackpotPool((int)betInfo.bet);
        // betInfo.JpPercent = betInfo.JpPercent / multipleA;  
        // betInfo.JpPercent = Math.Round(betInfo.JpPercent, 3);//保留小数后3为

      //  betInfo.bet *= 100;

        JP.JackpotInfo jInfo = new JP.JackpotInfo();
      
        jInfo.machineId = betInfo.machineId;
        jInfo.seatId = betInfo.seatId;
        for(int i = 0;i < 4; i++)
        {
            mJackpotData.mJackpotTotal.mfJpPullCent[i] = 0;
        }


        //fprintf(mfile, "    总压分1 : %f\n", totalBet);
        //所有彩金的提拨分
        double jpCent = betInfo.bet * betInfo.ScoreRate * betInfo.JpPercent * mFactor + mJackpotData.mJackpotTotal.mfPullRemainCentTotal;
        int nCent = (int)jpCent;
        //所有彩金的提拔余数
        //mJackpotData.mJackpotTotal.mfPullRemainCentTotal += jpCent - nCent;
        mJackpotData.mJackpotTotal.mfPullRemainCentTotal = jpCent - nCent;

        //彩金提拨分
        //jp1 彩金提拨分
        mJackpotData.mJackpotTotal.mfJpPullCent[0] = nCent * ((double)mJackpotData.mJackpotBase.mJpWeight[0] / (double)mJackpotData.mJackpotBase.mJpTotalWeight);

        //jp2 彩金提拨分
        mJackpotData.mJackpotTotal.mfJpPullCent[1] = nCent * ((double)mJackpotData.mJackpotBase.mJpWeight[1] / (double)mJackpotData.mJackpotBase.mJpTotalWeight);

        //jp3 彩金提拨分
        mJackpotData.mJackpotTotal.mfJpPullCent[2] = nCent * ((double)mJackpotData.mJackpotBase.mJpWeight[2] / (double)mJackpotData.mJackpotBase.mJpTotalWeight);

        //jp4 彩金提拨分
        mJackpotData.mJackpotTotal.mfJpPullCent[3] = nCent - (mJackpotData.mJackpotTotal.mfJpPullCent[0] + mJackpotData.mJackpotTotal.mfJpPullCent[1] + mJackpotData.mJackpotTotal.mfJpPullCent[2]);

        for (int i = 0; i < JACKPOT_MAX; i++)
        {
            //1 计算当前提波分余数
            double jpPullCent = mJackpotData.mJackpotTotal.mfJpPullCent[i] + mJackpotData.mJackpotTotal.mfPullRemainCent[i];
            int nJpPullCent = (int)jpPullCent;
            //mJackpotData.mJackpotTotal.mfPullRemainCent[i] += jpPullCent - nJpPullCent;
            mJackpotData.mJackpotTotal.mfPullRemainCent[i] = jpPullCent - nJpPullCent;

            //彩金累计提拨分
            mJackpotData.mJackpotTotal.mfTotalPullCent[i] += nJpPullCent;

            //for test
            double InnerTotalPullCent = nJpPullCent * mJackpotData.mJackpotBase.mfInnerPercent[i] + mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i];
            int nInnerTotalPullCent = (int)InnerTotalPullCent;

            //彩金隐藏提拨余数
            mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i] = InnerTotalPullCent - nInnerTotalPullCent;

            //彩金隐藏提拨分
            mJackpotData.mJackpotTotal.mfInnerPullCent[i] = nInnerTotalPullCent;


            //4 彩金显示提拨分
            mJackpotData.mJackpotTotal.mfOutPullCent[i] = nJpPullCent - nInnerTotalPullCent;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //5 计算彩金显示累计提拨分
            mJackpotData.mJackpotTotal.mfOutTotalPullCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i];

            //6 计算彩金隐藏累计提拨分
            mJackpotData.mJackpotTotal.mfInnerTotalPullCent[i] += mJackpotData.mJackpotTotal.mfInnerPullCent[i];

            //7 计算彩金开奖累积分  彩金开奖累积分=彩金开奖累积分+彩金显示提拨分
            mJackpotData.mJackpotTotal.mfTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i];

            //8 如果补偿积分等于0
            if (mJackpotData.mJackpotTotal.mfCompensateCent[i] == 0) //mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = 0;
            {
                mJackpotData.mJackpotTotal.mfOutTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i];
            }
            else if (mJackpotData.mJackpotTotal.mfCompensateCent[i] < 0)
            {   //9 如果补偿积分小于0

                //double scale = mJackpotData.mJackpotTotal.mfDiffCent[i] / (mJackpotData.mJackpotTotal.mfDiffCent[i] + ((double)mJackpotData.mJackpotBase.mfBaseValue[i] - (double)mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]]));
                //mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = -(mJackpotData.mJackpotTotal.mfOutPullCent[i] * scale) + mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i];

                //补偿彩金提拨分
                //int nXValue = (int)mJackpotData.mJackpotTotal.mfCompensatePullCent[i];
                double scale = mJackpotData.mJackpotTotal.mfDiffCent[i] / (double)(mJackpotData.mJackpotTotal.mfDiffCent[i] - mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]]);

                mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = -(mJackpotData.mJackpotTotal.mfJpPullCent[i] * scale) + mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i];

                int nXValue = (int)mJackpotData.mJackpotTotal.mfCompensatePullCent[i];

                //mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] += mJackpotData.mJackpotTotal.mfCompensatePullCent[i] - nXValue;
                mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] = mJackpotData.mJackpotTotal.mfCompensatePullCent[i] - nXValue;
                mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = nXValue;

                if ((mJackpotData.mJackpotTotal.mfCompensateCent[i] - mJackpotData.mJackpotTotal.mfCompensatePullCent[i]) < 0)
                {
                    mJackpotData.mJackpotTotal.mfCompensateCent[i] -= mJackpotData.mJackpotTotal.mfCompensatePullCent[i];
                }
                else
                {
                    mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = mJackpotData.mJackpotTotal.mfCompensateCent[i];
                    mJackpotData.mJackpotTotal.mfCompensateCent[i] = 0;
                }

                mJackpotData.mJackpotTotal.mfOutTotalCent[i] += mJackpotData.mJackpotTotal.mfJpPullCent[i] + mJackpotData.mJackpotTotal.mfCompensatePullCent[i];
            }
            else if (mJackpotData.mJackpotTotal.mfCompensateCent[i] > 0)
            {   //11 如果补偿积分大于0
                double scale = mJackpotData.mJackpotTotal.mfDiffCent[i] / (mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]] - mJackpotData.mJackpotBase.mfBaseValue[i]);
                mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = mJackpotData.mJackpotTotal.mfOutPullCent[i] * scale + mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i];

                //补偿彩金提拨分
                int nXValue = (int)mJackpotData.mJackpotTotal.mfCompensatePullCent[i];

                //mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] += mJackpotData.mJackpotTotal.mfCompensatePullCent[i] - nXValue;
                mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] = mJackpotData.mJackpotTotal.mfCompensatePullCent[i] - nXValue;
                mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = nXValue;

                if ((mJackpotData.mJackpotTotal.mfCompensateCent[i] - mJackpotData.mJackpotTotal.mfCompensatePullCent[i]) > 0)
                {
                    mJackpotData.mJackpotTotal.mfCompensateCent[i] -= mJackpotData.mJackpotTotal.mfCompensatePullCent[i];
                }
                else
                {
                    mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = mJackpotData.mJackpotTotal.mfCompensateCent[i];
                    mJackpotData.mJackpotTotal.mfCompensateCent[i] = 0;
                }

                mJackpotData.mJackpotTotal.mfOutTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i] + mJackpotData.mJackpotTotal.mfCompensatePullCent[i];
            }

            //12 计算彩金显示累积分   彩金显示累计分=彩金显示累计分+彩金显示提拨分+补偿彩金提拨分
            //mJackpotData.mJackpotTotal.mfOutTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i] + mJackpotData.mJackpotTotal.mfCompensatePullCent[i];

            //xtotal += mfCompensatePullCent[i];

            jInfo.triggerScore[i] = (double)mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]];
            jInfo.openAcc[i] = mJackpotData.mJackpotTotal.mfTotalCent[i];
            //7 计算是否开出彩金
            bool value1 = mJackpotData.mJackpotTotal.mfTotalCent[i] >= jInfo.triggerScore[i];// (mJackpotTrigger[i][mJackpotIdx[i]] * mFactor);
                                                                                                                  //	bool value2 = (betInfo.bet * betInfo.ScoreRate * mFactor) >= mJackpotData.mJackpotBase.mBetThreshold[i];
            bool value3 = mJackpotData.mJackpotTotal.mfOutTotalCent[i] >= jInfo.triggerScore[i];// (mJackpotTrigger[i][mJackpotIdx[i]] * mFactor);
            if (value1 && value3)
            {
                if (!CheckHasOne(jInfo.lottery, JACKPOT_MAX))
                {
                    double zjbl = 1;
                    //if (mJackpotData.mJackpotBase.mBetThresholdMax[i] == 0) {
                    //	zjbl = 1;
                    //}
                    //else {

                    //	double xf = (betInfo.bet * betInfo.ScoreRate * mFactor) / mJackpotData.mJackpotBase.mBetThresholdMax[i];
                    //	//zjbl = min((betInfo.bet * betInfo.ScoreRate * mFactor) / mBetThresholdMax[i], 1.0);// 1.5;// 
                    //	double zjbl = xf < 1 ? xf : 1;
                    //}

                    mJackpotData.mJackpotTotal.mfJpCent[i] = (int)(((double)mJackpotData.mJackpotTotal.mfOutTotalCent[i] * zjbl) / mFactor) * mFactor;

                    mJackpotData.mJackpotTotal.mfDiffCent[i] = (double)mJackpotData.mJackpotTotal.mfOutTotalCent[i] - (double)mJackpotData.mJackpotTotal.mfJpCent[i];

                    mJackpotData.mJackpotTotal.mfTotalLottery[i] += mJackpotData.mJackpotTotal.mfJpCent[i];
                    mJackpotData.mJackpotTotal.mfCompensateCent[i] += mJackpotData.mJackpotTotal.mfDiffCent[i];
                    int lastJpTrigger = mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]];

                    mJackpotData.mJackpotBase.mJackpotIdx[i]++;
                    if (mJackpotData.mJackpotBase.mJackpotIdx[i] > 9)
                    {       //若是10个彩金触发值用完了，则需要重新洗出新的触发值
                        ShuffleExt(i);
                    }

                    int nextJpTrigger = mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]];

                    if (mJackpotData.mJackpotTotal.mfTotalCent[i] >= mJackpotData.mJackpotTotal.mfOutTotalCent[i])
                    {
                        mJackpotData.mJackpotBase.mJpTriggerDiff[i] += mJackpotData.mJackpotTotal.mfOutTotalCent[i] - lastJpTrigger;
                    }
                    else
                    {
                        mJackpotData.mJackpotBase.mJpTriggerDiff[i] += mJackpotData.mJackpotTotal.mfTotalCent[i] - lastJpTrigger;
                    }

                    if ((nextJpTrigger - mJackpotData.mJackpotBase.mJpTriggerDiff[i]) >= (mJackpotData.mJackpotBase.mfLowValue[i]))
                    {
                        mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]] -= (int)mJackpotData.mJackpotBase.mJpTriggerDiff[i];
                        mJackpotData.mJackpotBase.mJpTriggerDiff[i] = 0;
                    }
                    else
                    {
                        mJackpotData.mJackpotBase.mJpTriggerDiff[i] -= mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]] - mJackpotData.mJackpotBase.mfLowValue[i];
                        mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]] = (int)mJackpotData.mJackpotBase.mfLowValue[i];
                    }

                    jInfo.jackpotlottery[i] = mJackpotData.mJackpotTotal.mfJpCent[i];// *(1 / (betInfo.ScoreRate == 0 ? 1 : betInfo.ScoreRate));
                    jInfo.jackpotold[i] = mJackpotData.mJackpotTotal.mfOutTotalCent[i];
                    jInfo.lottery[i] = 1;

                    ResetJackopt(i, mJackpotData.mJackpotTotal.mfCompensateCent[i] < 0);
                    jInfo.jackpotout[i] = mJackpotData.mJackpotTotal.mfOutTotalCent[i];
                }
                else
                {
                    jInfo.jackpotout[i] = mJackpotData.mJackpotTotal.mfOutTotalCent[i];
                }

            }
            else
            {
                jInfo.jackpotout[i] = mJackpotData.mJackpotTotal.mfOutTotalCent[i];
            }
        }
        //SaveDataToDB(t);
        return jInfo;
    }
    public JP.JackpotInfo JackpotInitData(int credit)
    {
        SetJackpotPool(credit);

        JP.JackpotInfo jInfo =  new JP.JackpotInfo();
        for (int i = 0; i < 4; i++)
        {
            jInfo.jackpotout[i] = mJackpotData.mJackpotTotal.mfOutTotalCent[i];
        }
        return jInfo;
    }

    public void SetBaseValue(int jpType,double percent, double bvalue, double lvalue, double hvalue)
    {
        mJackpotData.mJackpotBase.mfBaseValue[jpType] = bvalue * mFactor;
        mJackpotData.mJackpotBase.mfLowValue[jpType] = lvalue * mFactor;
        mJackpotData.mJackpotBase.mfHighValue[jpType] = hvalue * mFactor;
        mJackpotData.mJackpotBase.mfMidValue[jpType] = (mJackpotData.mJackpotBase.mfLowValue[jpType] + mJackpotData.mJackpotBase.mfHighValue[jpType]) / 2;

        //2025/01/11增加修改
        mJackpotData.mJackpotBase.mfRangeValue[jpType] = (mJackpotData.mJackpotBase.mfHighValue[jpType] - mJackpotData.mJackpotBase.mfLowValue[jpType]) / 10;

        mJackpotData.mJackpotBase.mfSettingPercent[jpType] = percent;

        //显示值的百分比 = (平均触发值-基本值)/平均触发值*彩金百分比
        mJackpotData.mJackpotBase.mfOutPercent[jpType] = (mJackpotData.mJackpotBase.mfMidValue[jpType] - mJackpotData.mJackpotBase.mfBaseValue[jpType]) / mJackpotData.mJackpotBase.mfMidValue[jpType]; //* mfSettingPercent[0][jpType];

        //隐藏值的百分比 = 基本值/平均触发值*彩金百分比
        mJackpotData.mJackpotBase.mfInnerPercent[jpType] = mJackpotData.mJackpotBase.mfBaseValue[jpType] / mJackpotData.mJackpotBase.mfMidValue[jpType];// *mfSettingPercent[0][jpType];
    }

    public void SetupJackpotData()
    {
        /*mJackpotData = new JP.JackpotData() ;
        for (int i = 0; i < 4; i++)
        {
            mJackpotData.mJackpotBase.mfBaseValue[i] = 0;
            mJackpotData.mJackpotBase.mfLowValue[i] = 0;
            mJackpotData.mJackpotBase.mfHighValue[i] = 0;
            mJackpotData.mJackpotBase.mfMidValue[i] = 0;

            mJackpotData.mJackpotBase.mfOutPercent[i] = 0;
            mJackpotData.mJackpotBase.mfInnerPercent[i] = 0;
            mJackpotData.mJackpotBase.mfSettingPercent[i] = 0;

            mJackpotData.mJackpotTotal.mfPullRemainCent[i] = 0;

            mJackpotData.mJackpotTotal.mfJpCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerRemainCent[i] = 0;

            mJackpotData.mJackpotTotal.mfTotalCent[i] = 0;
            mJackpotData.mJackpotTotal.mfOutTotalCent[i] = 0;

            mJackpotData.mJackpotTotal.mfOutTotalPullCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerTotalPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfOutPullRemainCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i] = 0;

            mJackpotData.mJackpotTotal.mfTotalLottery[i] = 0;

            mJackpotData.mJackpotTotal.mfOutPullCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfTotalPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfInnerClPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfCompensateCent[i] = 0;
            mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = 0;
            mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] = 0;
            mJackpotData.mJackpotTotal.mfDiffCent[i] = 0;

            mJackpotData.mJackpotBase.mJpWeight[i] = 0;

            mJackpotData.mJackpotBase.mJpTriggerDiff[i] = 0;

            mJackpotData.mJackpotTotal.mfJpPullCent[i] = 0;

            mJackpotData.mJackpotBase.mJackpotIdx[i] = 0;
            mJackpotData.mJackpotBase.mBetThreshold[i] = 0;
            mJackpotData.mJackpotBase.mBetThresholdMax[i] = 0;
            mJackpotData.mJackpotBase.mfRangeValue[i] = 0;
        }

        mJackpotData.mJackpotBase.mJpWeight[0] = 100;// 2;
        mJackpotData.mJackpotBase.mJpWeight[1] = 100;// 2;
        mJackpotData.mJackpotBase.mJpWeight[2] = 200;// 1;
        mJackpotData.mJackpotBase.mJpWeight[3] = 200;// 1;

        mJackpotData.mJackpotBase.mJpTotalWeight = 600;

        mJackpotData.mJackpotTotal.mfPullRemainCentTotal = 0;*/
        ResetConfig();
    }

    public void NeedCode()
    {

    }


    public void Reset()
    {
        mFactor = 100;
        mScoreRate = 1;
        mJpPercent = 0.01;
        SQLitePlayerPrefs03.Instance.DeleteKey(JP_GAME_DATA_CACHE0);
        SQLitePlayerPrefs03.Instance.DeleteKey(JP_GAME_DATA_CACHE1);
        SQLitePlayerPrefs03.Instance.DeleteKey(JP_GAME_DATA_CACHE2);
        SQLitePlayerPrefs03.Instance.DeleteKey(JP_GAME_DATA_CACHE3);
        SQLitePlayerPrefs03.Instance.DeleteKey(JP_GAME_DATA_CACHE4);
        _mJackpotData = null;

        JackpotBase temp = mJackpotData.mJackpotBase;  //初始化一下
    }
    public void ResetOld()
    {
        for (int i = 0; i < 4; i++)
        {
            mJackpotData.mJackpotBase.mfBaseValue[i] = 0;
           mJackpotData.mJackpotBase.mfLowValue[i] = 0;
            mJackpotData.mJackpotBase.mfHighValue[i] = 0;
            mJackpotData.mJackpotBase.mfMidValue[i] = 0;

            mJackpotData.mJackpotBase.mfOutPercent[i] = 0;
            mJackpotData.mJackpotBase.mfInnerPercent[i] = 0;
            mJackpotData.mJackpotBase.mfSettingPercent[i] = 0;

            mJackpotData.mJackpotTotal.mfPullRemainCent[i] = 0;

            mJackpotData.mJackpotTotal.mfJpCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerRemainCent[i] = 0;

            mJackpotData.mJackpotTotal.mfTotalCent[i] = 0;
            mJackpotData.mJackpotTotal.mfOutTotalCent[i] = 0;

            mJackpotData.mJackpotTotal.mfOutTotalPullCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerTotalPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfOutPullRemainCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i] = 0;

            mJackpotData.mJackpotTotal.mfTotalLottery[i] = 0;

            mJackpotData.mJackpotTotal.mfOutPullCent[i] = 0;
            mJackpotData.mJackpotTotal.mfInnerPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfTotalPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfInnerClPullCent[i] = 0;

            mJackpotData.mJackpotTotal.mfCompensateCent[i] = 0;
            mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = 0;
            mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i] = 0;
            mJackpotData.mJackpotTotal.mfDiffCent[i] = 0;

            mJackpotData.mJackpotBase.mJpWeight[i] = 0;

            mJackpotData.mJackpotBase.mJpTriggerDiff[i] = 0;

            mJackpotData.mJackpotTotal.mfJpPullCent[i] = 0;


        }
       
        mJackpotData.mJackpotTotal.mfPullRemainCentTotal = 0;

        mFactor = 100;
        mScoreRate = 1;
        mJpPercent = 0.01;

        ResetConfig();
    }

}
