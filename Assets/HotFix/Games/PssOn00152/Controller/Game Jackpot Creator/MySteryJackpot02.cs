using GameMaker;
using JP;
using System.Collections;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Random = System.Random;

//����ĸ��ʽ�
/*
* 3 С�ʽ�
* 2 �вʽ�
* 1 ��ʽ�
* 0 �޴�ʽ�

namespace JP
{
    
    public class JackpotInfo
    {
        public int machineId;
        public int seatId;
        public int[] lottery = new int[4];   //0:��ʾû�п����ʽ�1:��ʾ�ѿ����ʽ�
        public double[] jackpotlottery = new double[4]; //�����Ĳʽ�,ע��:�˴��ĵ�λ��Ǯ�ĵ�λ�������ǳ�����100�ģ��ֻ��յ����ֵҪ���ݷֻ��ķ�ֵ����ת�ɳɶ�Ӧ�ķ��������һ�Ҫ����ֵ����100
        public double[] jackpotout = new double[4];     //�ʽ���ʾ���۷�
        public double[] jackpotold = new double[4];     //�����ʽ�ǰ����ʾ���۷�
        public double[] ScoreRate = new double[4];                   //�����������ʽ������������������������飬ĿǰĬ��Ϊ1,����һ�ּ�Ԫ��ģʽ
        public double[] triggerScore = new double[4]; //��������
        public double[] openAcc = new double[4];    //�ʽ𿪽��ۻ���
    }

    public class JackpotBase
    {
        public double[] mfBaseValue = new double[4];        //����ֵ
        public double[] mfLowValue = new double[4];     //��С����ֵ
        public double[] mfHighValue = new double[4];        //��󴥷�ֵ
        public double[] mfMidValue = new double[4];     //ƽ������ֵ
        public int[] mBetThreshold = new int[4];             //ÿ���ʽ�����ѹ��ֵ;
        public int[] mBetThresholdMax = new int[4];              //ÿ���ʽ�����ѹ��ֵ;
        public int[][] mJackpotTrigger = new int[4][];
        public int[] mJackpotIdx = new int[4];

        public double[] mfOutPercent = new double[4];       //��ʾֵ�İٷֱ�   
        public double[] mfInnerPercent = new double[4];     //����ֵ�İٷֱ�
        public double[] mfSettingPercent = new double[4];   //�趨�Ĳʽ�ٷֱ�

        public int[] mJpWeight = new int[4];     //�ʽ����
        public int mJpTotalWeight;             //�ʽ�����ܺ�
        public double[] mJpTriggerDiff = new double[4];       //������ֵ
                                            //2025/01/11�����޸�
        public double[] mfRangeValue = new double[4]; //�ʽ𿪽���Χ
    }

  
    public class JackpotTotal
    {
        //�������
        public double[] mfPullRemainCent = new double[4];     //�ʽ��������
        public double[] mfOutPullRemainCent = new double[4];  //�ʽ���ʾ�������
        public double[] mfInnerPullRemainCent = new double[4];  //�ʽ������������

        public double[] mfJpCent = new double[4];           //�����Ĳʽ����
        public double[] mfInnerRemainCent = new double[4];  //�ڲ�ʣ�����
        public double[] mfInnerClPullCent = new double[4];  //�ڲ��ʽ��Ღ��

        public double[] mfTotalCent = new double[4];        //�ʽ𿪽��ۻ���
        public double[] mfOutTotalCent = new double[4];     //�ʽ���ʾ�ۻ���
        public double[] mfTotalLottery = new double[4];     //�����ʽ���۷���

        //�ʽ���ʾ�Ღ��
        public double[] mfOutPullCent = new double[4];
        //�ʽ������Ღ��
        public double[] mfInnerPullCent = new double[4];

        //�Ღ��
        public double[] mfOutTotalPullCent = new double[4];                     //�ʽ���ʾ�ۼ��Ღ��
        public double[] mfInnerTotalPullCent = new double[4];                   //�ʽ������ۼ��Ღ��
        public double[] mfTotalPullCent = new double[4];                        //�ʽ��ۼ���η�
        public double[] mfJpPullCent = new double[4];                       //4���ʽ�Ĳʽ���η�

        //��������
        public double[] mfCompensateCent = new double[4];   //��������
        public double[] mfCompensatePullCent = new double[4];   //�����ʽ��Ღ��
        public double[] mfCompensatePullRemainCent = new double[4]; //�����ʽ��Ღ����
        public double[] mfDiffCent = new double[4]; //��ֵ

        public double mfPullRemainCentTotal;                    //���еĲʽ��Ღ����
    }

    public class BetInfo
    {
        public int machineId;
        public int seatId;
        public double bet;
        //ѹ�ֱ���
        public double betPercent;
        public double ScoreRate;                   //�ֻ���ֵ�� ��������,����һ�ּ�Ԫ��ģʽ,����ķ�������Ҫÿ���Ϸ������ֵ
        public double JpPercent;                    //�ֻ��ʽ�ٷֱ�
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
                    if (_instance.transform.parent == null) // �ж��Ƿ��Ǹ��ڵ㣬
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
	double mFactor;			//��ǰĬ��Ϊ100
    double mScoreRate;                  //��ֵ��
    double mJpPercent;					//�ʽ�ٷֱ�
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

                    ResetConfig();  //�����ѭ��Ƕ�׵��ã�


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

        //JackpotBase temp = mJackpotData.mJackpotBase;  //��ʼ��һ��
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
            int randValue = random.Next(0, (int)(mJackpotData.mJackpotBase.mfRangeValue[jpType] + 1)); // max���ų�������
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
     * 50 - 100 2��     0.10/2    ����3λ 
     * 150 - 200 4��    0.10/4    ����3λ 
     * 250 - 300 6��    0.10/6    ����3λ  0.001
     * 500 10��
     * 600 12��
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
                Debug.LogError($"��Ϸ�ʽ����ñ���{credit}");
                throw new Exception("��Ϸ�ʽ����ñ���");
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

            JackpotBase temp = mJackpotData.mJackpotBase;  //��ʼ��һ��
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

        /* �ɰ汾
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
        // betInfo.JpPercent = Math.Round(betInfo.JpPercent, 3);//����С����3Ϊ

      //  betInfo.bet *= 100;

        JP.JackpotInfo jInfo = new JP.JackpotInfo();
      
        jInfo.machineId = betInfo.machineId;
        jInfo.seatId = betInfo.seatId;
        for(int i = 0;i < 4; i++)
        {
            mJackpotData.mJackpotTotal.mfJpPullCent[i] = 0;
        }


        //fprintf(mfile, "    ��ѹ��1 : %f\n", totalBet);
        //���вʽ���Ღ��
        double jpCent = betInfo.bet * betInfo.ScoreRate * betInfo.JpPercent * mFactor + mJackpotData.mJackpotTotal.mfPullRemainCentTotal;
        int nCent = (int)jpCent;
        //���вʽ���������
        //mJackpotData.mJackpotTotal.mfPullRemainCentTotal += jpCent - nCent;
        mJackpotData.mJackpotTotal.mfPullRemainCentTotal = jpCent - nCent;

        //�ʽ��Ღ��
        //jp1 �ʽ��Ღ��
        mJackpotData.mJackpotTotal.mfJpPullCent[0] = nCent * ((double)mJackpotData.mJackpotBase.mJpWeight[0] / (double)mJackpotData.mJackpotBase.mJpTotalWeight);

        //jp2 �ʽ��Ღ��
        mJackpotData.mJackpotTotal.mfJpPullCent[1] = nCent * ((double)mJackpotData.mJackpotBase.mJpWeight[1] / (double)mJackpotData.mJackpotBase.mJpTotalWeight);

        //jp3 �ʽ��Ღ��
        mJackpotData.mJackpotTotal.mfJpPullCent[2] = nCent * ((double)mJackpotData.mJackpotBase.mJpWeight[2] / (double)mJackpotData.mJackpotBase.mJpTotalWeight);

        //jp4 �ʽ��Ღ��
        mJackpotData.mJackpotTotal.mfJpPullCent[3] = nCent - (mJackpotData.mJackpotTotal.mfJpPullCent[0] + mJackpotData.mJackpotTotal.mfJpPullCent[1] + mJackpotData.mJackpotTotal.mfJpPullCent[2]);

        for (int i = 0; i < JACKPOT_MAX; i++)
        {
            //1 ���㵱ǰ�Შ������
            double jpPullCent = mJackpotData.mJackpotTotal.mfJpPullCent[i] + mJackpotData.mJackpotTotal.mfPullRemainCent[i];
            int nJpPullCent = (int)jpPullCent;
            //mJackpotData.mJackpotTotal.mfPullRemainCent[i] += jpPullCent - nJpPullCent;
            mJackpotData.mJackpotTotal.mfPullRemainCent[i] = jpPullCent - nJpPullCent;

            //�ʽ��ۼ��Ღ��
            mJackpotData.mJackpotTotal.mfTotalPullCent[i] += nJpPullCent;

            //for test
            double InnerTotalPullCent = nJpPullCent * mJackpotData.mJackpotBase.mfInnerPercent[i] + mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i];
            int nInnerTotalPullCent = (int)InnerTotalPullCent;

            //�ʽ������Ღ����
            mJackpotData.mJackpotTotal.mfInnerPullRemainCent[i] = InnerTotalPullCent - nInnerTotalPullCent;

            //�ʽ������Ღ��
            mJackpotData.mJackpotTotal.mfInnerPullCent[i] = nInnerTotalPullCent;


            //4 �ʽ���ʾ�Ღ��
            mJackpotData.mJackpotTotal.mfOutPullCent[i] = nJpPullCent - nInnerTotalPullCent;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //5 ����ʽ���ʾ�ۼ��Ღ��
            mJackpotData.mJackpotTotal.mfOutTotalPullCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i];

            //6 ����ʽ������ۼ��Ღ��
            mJackpotData.mJackpotTotal.mfInnerTotalPullCent[i] += mJackpotData.mJackpotTotal.mfInnerPullCent[i];

            //7 ����ʽ𿪽��ۻ���  �ʽ𿪽��ۻ���=�ʽ𿪽��ۻ���+�ʽ���ʾ�Ღ��
            mJackpotData.mJackpotTotal.mfTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i];

            //8 ����������ֵ���0
            if (mJackpotData.mJackpotTotal.mfCompensateCent[i] == 0) //mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = 0;
            {
                mJackpotData.mJackpotTotal.mfOutTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i];
            }
            else if (mJackpotData.mJackpotTotal.mfCompensateCent[i] < 0)
            {   //9 �����������С��0

                //double scale = mJackpotData.mJackpotTotal.mfDiffCent[i] / (mJackpotData.mJackpotTotal.mfDiffCent[i] + ((double)mJackpotData.mJackpotBase.mfBaseValue[i] - (double)mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]]));
                //mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = -(mJackpotData.mJackpotTotal.mfOutPullCent[i] * scale) + mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i];

                //�����ʽ��Ღ��
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
            {   //11 ����������ִ���0
                double scale = mJackpotData.mJackpotTotal.mfDiffCent[i] / (mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]] - mJackpotData.mJackpotBase.mfBaseValue[i]);
                mJackpotData.mJackpotTotal.mfCompensatePullCent[i] = mJackpotData.mJackpotTotal.mfOutPullCent[i] * scale + mJackpotData.mJackpotTotal.mfCompensatePullRemainCent[i];

                //�����ʽ��Ღ��
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

            //12 ����ʽ���ʾ�ۻ���   �ʽ���ʾ�ۼƷ�=�ʽ���ʾ�ۼƷ�+�ʽ���ʾ�Ღ��+�����ʽ��Ღ��
            //mJackpotData.mJackpotTotal.mfOutTotalCent[i] += mJackpotData.mJackpotTotal.mfOutPullCent[i] + mJackpotData.mJackpotTotal.mfCompensatePullCent[i];

            //xtotal += mfCompensatePullCent[i];

            jInfo.triggerScore[i] = (double)mJackpotData.mJackpotBase.mJackpotTrigger[i][mJackpotData.mJackpotBase.mJackpotIdx[i]];
            jInfo.openAcc[i] = mJackpotData.mJackpotTotal.mfTotalCent[i];
            //7 �����Ƿ񿪳��ʽ�
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
                    {       //����10���ʽ𴥷�ֵ�����ˣ�����Ҫ����ϴ���µĴ���ֵ
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

        //2025/01/11�����޸�
        mJackpotData.mJackpotBase.mfRangeValue[jpType] = (mJackpotData.mJackpotBase.mfHighValue[jpType] - mJackpotData.mJackpotBase.mfLowValue[jpType]) / 10;

        mJackpotData.mJackpotBase.mfSettingPercent[jpType] = percent;

        //��ʾֵ�İٷֱ� = (ƽ������ֵ-����ֵ)/ƽ������ֵ*�ʽ�ٷֱ�
        mJackpotData.mJackpotBase.mfOutPercent[jpType] = (mJackpotData.mJackpotBase.mfMidValue[jpType] - mJackpotData.mJackpotBase.mfBaseValue[jpType]) / mJackpotData.mJackpotBase.mfMidValue[jpType]; //* mfSettingPercent[0][jpType];

        //����ֵ�İٷֱ� = ����ֵ/ƽ������ֵ*�ʽ�ٷֱ�
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

        JackpotBase temp = mJackpotData.mJackpotBase;  //��ʼ��һ��
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
