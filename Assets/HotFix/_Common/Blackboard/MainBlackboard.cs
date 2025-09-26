using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMaker { 
    public class MainBlackboard : MonoSingleton<MainBlackboard>
    {
        ObservableObject m_Observable;
        public ObservableObject observable
        {
            get
            {
                if (m_Observable == null)
                {
                    string[] classNamePath = this.GetType().ToString().Split('.');
                    m_Observable = new ObservableObject(classNamePath[classNamePath.Length - 1] ,  "/");
                }
                return m_Observable;
            }
        }



        [Tooltip("玩家游戏分")]
        [SerializeField]
        private long m_MyCredit = 0;
        /// <summary>
        /// 用于备份玩家金币，并在播放玩家金币动画中，作为临时计算存储对象。
        /// </summary>
        public long myCredit
        {
            get => m_MyCredit;
            set => observable.SetProperty(ref m_MyCredit, value);
        }



        [SerializeField]
        private int m_GameID = 152; //-1

        public int gameID
        {
            get => m_GameID;
            set => observable.SetProperty(ref m_GameID, value);
        }


        /*public List<float> soundVolumeLst = new List<float>(){ 0.25f, 0.5f, 1f, 0f,};



        [Tooltip("音量")]
        [SerializeField]
        private float m_SoundVolume = 0;
        public float soundVolume
        {
            get => m_SoundVolume;
            set => observable.SetProperty(ref m_SoundVolume, value);
        }*/


        /*[SerializeField]
        private TableMachineItem m_TableMachine;

        public TableMachineItem tableMachine
        {
            get => observable.GetProperty(() => m_TableMachine);
            set => observable.SetProperty(ref m_TableMachine, value);
            //set => m_TableMachine = value;
        }*/




        /// <summary>
        /// 数据上报编号
        /// </summary>
        public int reportId
        {
            get
            {
                if (reportIdNode == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(PARAM_REPORT_ID, "{}");
                    reportIdNode = JSONNode.Parse(str);
                }
                string key = $"{gameID}";
                if (!reportIdNode.HasKey(key))
                    reportIdNode[key] = 0;

                return (int)reportIdNode[key];
            }
            set
            {
                string key = $"{gameID}";
                if (!reportIdNode.HasKey(key))
                    reportIdNode[key] = 0;

                if ((int)reportIdNode[key] != value)
                {
                    reportIdNode[key] = value;
                    SQLitePlayerPrefs03.Instance.SetString(PARAM_REPORT_ID, reportIdNode.ToString());
                }
            }
        }
        public JSONNode reportIdNode = null;
        public const string PARAM_REPORT_ID = "PARAM_REPORT_ID";



        /// <summary>
        /// 本局游戏编号
        /// </summary>
        public int gameNumber
        {
            get
            {
                if (gameNumberNode == null)
                {
                    string str = SQLitePlayerPrefs03.Instance.GetString(PARAM_GAME_NUMBER, "{}");
                    gameNumberNode = JSONNode.Parse(str);
                }
                string key = $"{gameID}";
                if (!gameNumberNode.HasKey(key))
                    gameNumberNode[key] = 0;

                return (int)gameNumberNode[key];
            }
            set
            {
                string key = $"{gameID}";
                if (!gameNumberNode.HasKey(key))
                    gameNumberNode[key] = 0;

                if ((int)gameNumberNode[key] != value)
                {
                    gameNumberNode[key] = value;
                    SQLitePlayerPrefs03.Instance.SetString(PARAM_GAME_NUMBER, gameNumberNode.ToString());
                }
            }
        }
        public JSONNode gameNumberNode = null;
        public const string PARAM_GAME_NUMBER = "PARAM_GAME_NUMBER";


    }
}
