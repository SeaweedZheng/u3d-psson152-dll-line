using GameMaker;
using SimpleJSON;
using Sirenix.OdinInspector;
using SlotMaker;
using System.Collections.Generic;
using UnityEngine;

//ChinaTown100
namespace PssOn00152
{

    [System.Serializable]
    public enum WinLevelType
    {
        None = 0,
        Big,
        Mega,
        Super,
        Ultra,
        Ultimate,
    }


    [System.Serializable]
    public class CustomDicWinMultiple : UnitySerializedDictionary<WinLevelType, long> { }

    [System.Serializable]
    public class WinMultiple
    {
        public WinLevelType winLevelType;
        public long multiple;
    }

    public partial class ContentBlackboard : MonoWeakSingleton<ContentBlackboard>
    {
        ObservableObject m_Observable;
        public ObservableObject observable
        {
            get
            {
                if (m_Observable == null)
                {
                    string[] classNamePath = this.GetType().ToString().Split('.');
                    m_Observable = new ObservableObject(classNamePath[classNamePath.Length - 1], "./");
                }
                return m_Observable;
            }
        }



        #region 游戏房间


        /*
        {
            "reelStrips": [{
                "strip": [11, 2, 3, 2, 7, 6, 9, 4, 4, 4, 11, 8, 1, 11, 3, 3, 7, 3, 8, 3, 6, 5, 9, 4, 11, 5, 7, 6, 9, 3, 5, 11, 1, 10, 11, 5, 7, 5, 10, 3, 6, 9, 9, 1, 9, 4, 6, 4, 10, 5, 10, 6, 1, 7, 8, 4, 4, 4, 8, 7, 4, 4, 4, 10, 6, 4, 4, 4, 5, 10, 5, 6, 7, 2, 3, 3, 3]
            }, {
                "strip": [11, 7, 3, 10, 0, 9, 5, 5, 8, 7, 1, 8, 7, 6, 9, 5, 11, 6, 7, 1, 4, 8, 2, 0, 9, 4, 5, 3, 10, 4, 11, 0, 6, 7, 1, 8, 11, 5, 8, 0, 6, 5, 11, 2, 10, 0, 5, 8, 10, 1, 8, 2, 9, 11, 3, 9, 0, 4, 9, 4, 10, 3, 4, 4, 0, 5, 5, 10, 7, 4, 5, 5, 5, 6, 4, 4, 4]
            }, {
                "strip": [10, 3, 4, 4, 3, 10, 1, 3, 7, 3, 5, 4, 4, 7, 3, 10, 6, 4, 4, 9, 5, 8, 6, 9, 1, 10, 4, 6, 3, 0, 5, 2, 8, 8, 1, 8, 11, 5, 11, 9, 1, 9, 11, 5, 11, 4, 2, 1, 2, 7, 6, 4, 6, 8, 5, 3, 5, 11, 9, 4, 5, 7, 4, 4, 7, 6, 10, 4, 4, 4, 4, 4, 4]
            }, {
                "strip": [11, 2, 3, 4, 3, 4, 5, 8, 5, 7, 0, 9, 4, 5, 8, 5, 0, 4, 7, 6, 6, 3, 8, 9, 3, 6, 9, 8, 2, 0, 7, 6, 7, 2, 8, 6, 7, 2, 5, 9, 6, 9, 2, 8, 6, 10, 4, 3, 3, 7, 3, 5, 11, 8, 9, 3, 3, 3]
            }, {
                "strip": [10, 3, 4, 2, 8, 3, 9, 8, 5, 2, 5, 10, 4, 4, 4, 3, 11, 2, 7, 2, 3, 11, 9, 8, 3, 8, 10, 7, 11, 2, 5, 10, 3, 7, 4, 10, 6, 10, 6, 4, 11, 11, 2, 7, 11, 4, 4, 4, 8, 5, 9, 5, 6, 4, 4, 4, 7, 6, 9, 5, 4, 4, 5, 3, 3, 3]
            }]
        }*/

        const string strBS = "{\"reelStrips\":[{\"strip\":[11,2,3,2,7,6,9,4,4,4,11,8,1,11,3,3,7,3,8,3,6,5,9,4,11,5,7,6,9,3,5,11,1,10,11,5,7,5,10,3,6,9,9,1,9,4,6,4,10,5,10,6,1,7,8,4,4,4,8,7,4,4,4,10,6,4,4,4,5,10,5,6,7,2,3,3,3]},{\"strip\":[11,7,3,10,0,9,5,5,8,7,1,8,7,6,9,5,11,6,7,1,4,8,2,0,9,4,5,3,10,4,11,0,6,7,1,8,11,5,8,0,6,5,11,2,10,0,5,8,10,1,8,2,9,11,3,9,0,4,9,4,10,3,4,4,0,5,5,10,7,4,5,5,5,6,4,4,4]},{\"strip\":[10,3,4,4,3,10,1,3,7,3,5,4,4,7,3,10,6,4,4,9,5,8,6,9,1,10,4,6,3,0,5,2,8,8,1,8,11,5,11,9,1,9,11,5,11,4,2,1,2,7,6,4,6,8,5,3,5,11,9,4,5,7,4,4,7,6,10,4,4,4,4,4,4]},{\"strip\":[11,2,3,4,3,4,5,8,5,7,0,9,4,5,8,5,0,4,7,6,6,3,8,9,3,6,9,8,2,0,7,6,7,2,8,6,7,2,5,9,6,9,2,8,6,10,4,3,3,7,3,5,11,8,9,3,3,3]},{\"strip\":[10,3,4,2,8,3,9,8,5,2,5,10,4,4,4,3,11,2,7,2,3,11,9,8,3,8,10,7,11,2,5,10,3,7,4,10,6,10,6,4,11,11,2,7,11,4,4,4,8,5,9,5,6,4,4,4,7,6,9,5,4,4,5,3,3,3]}]}";

        public ReelStrips BS = new ReelStrips()
        {
            reelStrips = new List<ReelStrip>
            {
                new ReelStrip{strip = new List<int>(){11, 2, 3, 2, 7, 6, 9, 4, 4, 4, 11, 8, 1, 11, 3, 3, 7, 3, 8, 3, 6, 5, 9, 4, 11, 5, 7, 6, 9, 3, 5, 11, 1, 10, 11, 5, 7, 5, 10, 3, 6, 9, 9, 1, 9, 4, 6, 4, 10, 5, 10, 6, 1, 7, 8, 4, 4, 4, 8, 7, 4, 4, 4, 10, 6, 4, 4, 4, 5, 10, 5, 6, 7, 2, 3, 3, 3}, },
                new ReelStrip{strip = new List<int>(){11, 7, 3, 10, 0, 9, 5, 5, 8, 7, 1, 8, 7, 6, 9, 5, 11, 6, 7, 1, 4, 8, 2, 0, 9, 4, 5, 3, 10, 4, 11, 0, 6, 7, 1, 8, 11, 5, 8, 0, 6, 5, 11, 2, 10, 0, 5, 8, 10, 1, 8, 2, 9, 11, 3, 9, 0, 4, 9, 4, 10, 3, 4, 4, 0, 5, 5, 10, 7, 4, 5, 5, 5, 6, 4, 4, 4},},
                new ReelStrip{strip = new List<int>(){10, 3, 4, 4, 3, 10, 1, 3, 7, 3, 5, 4, 4, 7, 3, 10, 6, 4, 4, 9, 5, 8, 6, 9, 1, 10, 4, 6, 3, 0, 5, 2, 8, 8, 1, 8, 11, 5, 11, 9, 1, 9, 11, 5, 11, 4, 2, 1, 2, 7, 6, 4, 6, 8, 5, 3, 5, 11, 9, 4, 5, 7, 4, 4, 7, 6, 10, 4, 4, 4, 4, 4, 4},},
                new ReelStrip{ strip = new List<int>(){11, 2, 3, 4, 3, 4, 5, 8, 5, 7, 0, 9, 4, 5, 8, 5, 0, 4, 7, 6, 6, 3, 8, 9, 3, 6, 9, 8, 2, 0, 7, 6, 7, 2, 8, 6, 7, 2, 5, 9, 6, 9, 2, 8, 6, 10, 4, 3, 3, 7, 3, 5, 11, 8, 9, 3, 3, 3}, },
                new ReelStrip{ strip = new List<int>(){10, 3, 4, 2, 8, 3, 9, 8, 5, 2, 5, 10, 4, 4, 4, 3, 11, 2, 7, 2, 3, 11, 9, 8, 3, 8, 10, 7, 11, 2, 5, 10, 3, 7, 4, 10, 6, 10, 6, 4, 11, 11, 2, 7, 11, 4, 4, 4, 8, 5, 9, 5, 6, 4, 4, 4, 7, 6, 9, 5, 4, 4, 5, 3, 3, 3}}
            }
        };
        /*
                const string strFS1 = """
        {
            \"reelStrips\": [{
                \"strip\": [11, 1, 3, 5, 10, 11, 6, 9, 9, 3, 11, 11, 10, 1, 4, 9, 9, 9, 7, 5, 2, 11, 7, 8, 6, 10, 11, 10, 6, 10, 4, 8, 5, 8, 6, 7, 11, 10, 8, 3, 7, 10, 7, 7, 7, 3, 8, 9, 9, 9, 1, 11, 11, 11, 2, 3, 5, 10, 10, 9, 9, 7, 5, 1, 7, 7, 6, 10]
            }, {
                \"strip\": [11, 3, 11, 2, 6, 11, 5, 10, 10, 5, 9, 8, 1, 11, 11, 5, 8, 11, 5, 8, 7, 1, 6, 11, 10, 2, 4, 9, 9, 10, 7, 11, 11, 8, 9, 7, 1, 8, 4, 6, 2, 9, 7, 6, 7, 1, 8, 9, 10, 5, 10, 9, 0, 11, 11, 5, 11, 11, 6, 7, 11, 11, 6, 8, 1, 9, 7, 2, 9, 10, 2, 8, 9, 10]
            }, {
                \"strip\": [8, 7, 6, 3, 10, 10, 1, 10, 10, 7, 2, 8, 7, 7, 3, 11, 7, 4, 8, 9, 7, 6, 10, 10, 1, 10, 10, 4, 11, 8, 10, 10, 1, 10, 10, 11, 7, 4, 10, 10, 1, 10, 10, 8, 11, 0, 10, 10, 1, 10, 10, 8, 7, 4, 3, 5, 8, 9, 8, 7, 8, 7, 8, 8, 4, 11, 8, 7, 5, 7, 11, 3, 5, 6]
            }, {
                \"strip\": [11, 10, 4, 9, 9, 9, 7, 10, 8, 10, 8, 2, 4, 5, 8, 8, 9, 2, 8, 7, 10, 6, 10, 8, 2, 9, 7, 4, 8, 7, 10, 5, 11, 8, 9, 7, 2, 9, 7, 8, 6, 10, 11, 3, 9, 7, 11, 3, 8, 9, 5, 11, 10, 8, 6, 9, 7, 9, 11, 11, 5, 6, 10, 8, 0, 11, 11, 3, 10, 10, 11, 9, 9, 11, 10, 11, 8, 11, 10, 6, 3, 4]
            }, {
                \"strip\": [10, 3, 4, 2, 11, 11, 10, 10, 7, 10, 8, 3, 10, 10, 4, 11, 7, 11, 7, 7, 7, 2, 7, 8, 4, 7, 8, 8, 5, 8, 8, 6, 7, 8, 6, 9, 10, 11, 9, 6, 9, 9, 8, 9, 6, 10, 8, 9, 5, 10, 7, 5, 8, 7, 8, 9, 10, 4, 11, 7, 10, 9, 10, 9, 11, 7, 9, 7, 6, 4, 5, 9, 11, 11, 11, 8, 2, 3, 4, 9, 10, 7, 8, 9]
            }]
        }
        """;*/

        const string strFS = "{\"reelStrips\":[{\"strip\":[11,1,3,5,10,11,6,9,9,3,11,11,10,1,4,9,9,9,7,5,2,11,7,8,6,10,11,10,6,10,4,8,5,8,6,7,11,10,8,3,7,10,7,7,7,3,8,9,9,9,1,11,11,11,2,3,5,10,10,9,9,7,5,1,7,7,6,10]},{\"strip\":[11,3,11,2,6,11,5,10,10,5,9,8,1,11,11,5,8,11,5,8,7,1,6,11,10,2,4,9,9,10,7,11,11,8,9,7,1,8,4,6,2,9,7,6,7,1,8,9,10,5,10,9,0,11,11,5,11,11,6,7,11,11,6,8,1,9,7,2,9,10,2,8,9,10]},{\"strip\":[8,7,6,3,10,10,1,10,10,7,2,8,7,7,3,11,7,4,8,9,7,6,10,10,1,10,10,4,11,8,10,10,1,10,10,11,7,4,10,10,1,10,10,8,11,0,10,10,1,10,10,8,7,4,3,5,8,9,8,7,8,7,8,8,4,11,8,7,5,7,11,3,5,6]},{\"strip\":[11,10,4,9,9,9,7,10,8,10,8,2,4,5,8,8,9,2,8,7,10,6,10,8,2,9,7,4,8,7,10,5,11,8,9,7,2,9,7,8,6,10,11,3,9,7,11,3,8,9,5,11,10,8,6,9,7,9,11,11,5,6,10,8,0,11,11,3,10,10,11,9,9,11,10,11,8,11,10,6,3,4]},{\"strip\":[10,3,4,2,11,11,10,10,7,10,8,3,10,10,4,11,7,11,7,7,7,2,7,8,4,7,8,8,5,8,8,6,7,8,6,9,10,11,9,6,9,9,8,9,6,10,8,9,5,10,7,5,8,7,8,9,10,4,11,7,10,9,10,9,11,7,9,7,6,4,5,9,11,11,11,8,2,3,4,9,10,7,8,9]}]}";

        public ReelStrips FS = new ReelStrips()
        {
            reelStrips = new List<ReelStrip>
            {
            new ReelStrip{strip = new List<int>(){11, 1, 3, 5, 10, 11, 6, 9, 9, 3, 11, 11, 10, 1, 4, 9, 9, 9, 7, 5, 2, 11, 7, 8, 6, 10, 11, 10, 6, 10, 4, 8, 5, 8, 6, 7, 11, 10, 8, 3, 7, 10, 7, 7, 7, 3, 8, 9, 9, 9, 1, 11, 11, 11, 2, 3, 5, 10, 10, 9, 9, 7, 5, 1, 7, 7, 6, 10}},
            new ReelStrip{strip = new List<int>(){11, 3, 11, 2, 6, 11, 5, 10, 10, 5, 9, 8, 1, 11, 11, 5, 8, 11, 5, 8, 7, 1, 6, 11, 10, 2, 4, 9, 9, 10, 7, 11, 11, 8, 9, 7, 1, 8, 4, 6, 2, 9, 7, 6, 7, 1, 8, 9, 10, 5, 10, 9, 0, 11, 11, 5, 11, 11, 6, 7, 11, 11, 6, 8, 1, 9, 7, 2, 9, 10, 2, 8, 9, 10}},
            new ReelStrip{strip = new List<int>(){8, 7, 6, 3, 10, 10, 1, 10, 10, 7, 2, 8, 7, 7, 3, 11, 7, 4, 8, 9, 7, 6, 10, 10, 1, 10, 10, 4, 11, 8, 10, 10, 1, 10, 10, 11, 7, 4, 10, 10, 1, 10, 10, 8, 11, 0, 10, 10, 1, 10, 10, 8, 7, 4, 3, 5, 8, 9, 8, 7, 8, 7, 8, 8, 4, 11, 8, 7, 5, 7, 11, 3, 5, 6}},
            new ReelStrip{strip = new List<int>(){11, 10, 4, 9, 9, 9, 7, 10, 8, 10, 8, 2, 4, 5, 8, 8, 9, 2, 8, 7, 10, 6, 10, 8, 2, 9, 7, 4, 8, 7, 10, 5, 11, 8, 9, 7, 2, 9, 7, 8, 6, 10, 11, 3, 9, 7, 11, 3, 8, 9, 5, 11, 10, 8, 6, 9, 7, 9, 11, 11, 5, 6, 10, 8, 0, 11, 11, 3, 10, 10, 11, 9, 9, 11, 10, 11, 8, 11, 10, 6, 3, 4}},
            new ReelStrip{strip = new List<int>(){10, 3, 4, 2, 11, 11, 10, 10, 7, 10, 8, 3, 10, 10, 4, 11, 7, 11, 7, 7, 7, 2, 7, 8, 4, 7, 8, 8, 5, 8, 8, 6, 7, 8, 6, 9, 10, 11, 9, 6, 9, 9, 8, 9, 6, 10, 8, 9, 5, 10, 7, 5, 8, 7, 8, 9, 10, 4, 11, 7, 10, 9, 10, 9, 11, 7, 9, 7, 6, 4, 5, 9, 11, 11, 11, 8, 2, 3, 4, 9, 10, 7, 8, 9}}
            }
        };



        const string strBFS = "{\"reelStrips\":[{\"strip\":[11,1,3,5,10,11,6,9,9,3,11,11,10,1,4,9,9,9,7,5,2,11,7,8,6,10,11,10,6,10,4,8,5,8,6,7,11,10,8,3,7,10,7,7,7,3,8,9,9,9,1,11,11,11,2,3,5,10,10,9,9,7,5,1,7,7,6,10]},{\"strip\":[11,3,11,2,6,11,5,10,10,5,9,8,1,11,11,5,8,11,5,8,7,1,6,11,10,2,4,9,9,10,7,11,11,8,9,7,1,8,4,6,2,9,7,6,7,1,8,9,10,5,10,9,0,11,11,5,11,11,6,7,11,11,6,8,1,9,7,2,9,10,2,8,9,10]},{\"strip\":[8,7,6,3,10,10,1,10,10,7,2,8,7,7,3,11,7,4,8,9,7,6,10,10,1,10,10,4,11,8,10,10,1,10,10,11,7,4,10,10,1,10,10,8,11,0,10,10,1,10,10,8,7,4,3,5,8,9,8,7,8,7,8,8,4,11,8,7,5,7,11,3,5,6]},{\"strip\":[11,10,4,9,9,9,7,10,8,10,8,2,4,5,8,8,9,2,8,7,10,6,10,8,2,9,7,4,8,7,10,5,11,8,9,7,2,9,7,8,6,10,11,3,9,7,11,3,8,9,5,11,10,8,6,9,7,9,11,11,5,6,10,8,0,11,11,3,10,10,11,9,9,11,10,11,8,11,10,6,3,4]},{\"strip\":[10,3,4,2,11,11,10,10,7,10,8,3,10,10,4,11,7,11,7,7,7,2,7,8,4,7,8,8,5,8,8,6,7,8,6,9,10,11,9,6,9,9,8,9,6,10,8,9,5,10,7,5,8,7,8,9,10,4,11,7,10,9,10,9,11,7,9,7,6,4,5,9,11,11,11,8,2,3,4,9,10,7,8,9]}]}";

        public ReelStrips BFS = JsonUtility.FromJson<ReelStrips>(strBFS);


        #endregion


        [Button]
        void SetBF()
        {
            /*BS = new ReelStrips()
            {
                reelStrips = new List<ReelStrip>
                {
                    new ReelStrip{strip = new List<int>(){11, 2, 3, 2, 7, 6, 9, 4, 4, 4, 11, 8, 1, 11, 3, 3, 7, 3, 8, 3, 6, 5, 9, 4, 11, 5, 7, 6, 9, 3, 5, 11, 1, 10, 11, 5, 7, 5, 10, 3, 6, 9, 9, 1, 9, 4, 6, 4, 10, 5, 10, 6, 1, 7, 8, 4, 4, 4, 8, 7, 4, 4, 4, 10, 6, 4, 4, 4, 5, 10, 5, 6, 7, 2, 3, 3, 3}, },
                    new ReelStrip{strip = new List<int>(){11, 7, 3, 10, 0, 9, 5, 5, 8, 7, 1, 8, 7, 6, 9, 5, 11, 6, 7, 1, 4, 8, 2, 0, 9, 4, 5, 3, 10, 4, 11, 0, 6, 7, 1, 8, 11, 5, 8, 0, 6, 5, 11, 2, 10, 0, 5, 8, 10, 1, 8, 2, 9, 11, 3, 9, 0, 4, 9, 4, 10, 3, 4, 4, 0, 5, 5, 10, 7, 4, 5, 5, 5, 6, 4, 4, 4},},
                    new ReelStrip{strip = new List<int>(){10, 3, 4, 4, 3, 10, 1, 3, 7, 3, 5, 4, 4, 7, 3, 10, 6, 4, 4, 9, 5, 8, 6, 9, 1, 10, 4, 6, 3, 0, 5, 2, 8, 8, 1, 8, 11, 5, 11, 9, 1, 9, 11, 5, 11, 4, 2, 1, 2, 7, 6, 4, 6, 8, 5, 3, 5, 11, 9, 4, 5, 7, 4, 4, 7, 6, 10, 4, 4, 4, 4, 4, 4},},
                    new ReelStrip{ strip = new List<int>(){11, 2, 3, 4, 3, 4, 5, 8, 5, 7, 0, 9, 4, 5, 8, 5, 0, 4, 7, 6, 6, 3, 8, 9, 3, 6, 9, 8, 2, 0, 7, 6, 7, 2, 8, 6, 7, 2, 5, 9, 6, 9, 2, 8, 6, 10, 4, 3, 3, 7, 3, 5, 11, 8, 9, 3, 3, 3}, },
                    new ReelStrip{ strip = new List<int>(){10, 3, 4, 2, 8, 3, 9, 8, 5, 2, 5, 10, 4, 4, 4, 3, 11, 2, 7, 2, 3, 11, 9, 8, 3, 8, 10, 7, 11, 2, 5, 10, 3, 7, 4, 10, 6, 10, 6, 4, 11, 11, 2, 7, 11, 4, 4, 4, 8, 5, 9, 5, 6, 4, 4, 4, 7, 6, 9, 5, 4, 4, 5, 3, 3, 3}}
                }
            };*/
            BS = JsonUtility.FromJson<ReelStrips>(strBS);

            FS = new ReelStrips()
            {
                reelStrips = new List<ReelStrip>
                {
                new ReelStrip{strip = new List<int>(){11, 1, 3, 5, 10, 11, 6, 9, 9, 3, 11, 11, 10, 1, 4, 9, 9, 9, 7, 5, 2, 11, 7, 8, 6, 10, 11, 10, 6, 10, 4, 8, 5, 8, 6, 7, 11, 10, 8, 3, 7, 10, 7, 7, 7, 3, 8, 9, 9, 9, 1, 11, 11, 11, 2, 3, 5, 10, 10, 9, 9, 7, 5, 1, 7, 7, 6, 10}},
                new ReelStrip{strip = new List<int>(){11, 3, 11, 2, 6, 11, 5, 10, 10, 5, 9, 8, 1, 11, 11, 5, 8, 11, 5, 8, 7, 1, 6, 11, 10, 2, 4, 9, 9, 10, 7, 11, 11, 8, 9, 7, 1, 8, 4, 6, 2, 9, 7, 6, 7, 1, 8, 9, 10, 5, 10, 9, 0, 11, 11, 5, 11, 11, 6, 7, 11, 11, 6, 8, 1, 9, 7, 2, 9, 10, 2, 8, 9, 10}},
                new ReelStrip{strip = new List<int>(){8, 7, 6, 3, 10, 10, 1, 10, 10, 7, 2, 8, 7, 7, 3, 11, 7, 4, 8, 9, 7, 6, 10, 10, 1, 10, 10, 4, 11, 8, 10, 10, 1, 10, 10, 11, 7, 4, 10, 10, 1, 10, 10, 8, 11, 0, 10, 10, 1, 10, 10, 8, 7, 4, 3, 5, 8, 9, 8, 7, 8, 7, 8, 8, 4, 11, 8, 7, 5, 7, 11, 3, 5, 6}},
                new ReelStrip{strip = new List<int>(){11, 10, 4, 9, 9, 9, 7, 10, 8, 10, 8, 2, 4, 5, 8, 8, 9, 2, 8, 7, 10, 6, 10, 8, 2, 9, 7, 4, 8, 7, 10, 5, 11, 8, 9, 7, 2, 9, 7, 8, 6, 10, 11, 3, 9, 7, 11, 3, 8, 9, 5, 11, 10, 8, 6, 9, 7, 9, 11, 11, 5, 6, 10, 8, 0, 11, 11, 3, 10, 10, 11, 9, 9, 11, 10, 11, 8, 11, 10, 6, 3, 4}},
                new ReelStrip{strip = new List<int>(){10, 3, 4, 2, 11, 11, 10, 10, 7, 10, 8, 3, 10, 10, 4, 11, 7, 11, 7, 7, 7, 2, 7, 8, 4, 7, 8, 8, 5, 8, 8, 6, 7, 8, 6, 9, 10, 11, 9, 6, 9, 9, 8, 9, 6, 10, 8, 9, 5, 10, 7, 5, 8, 7, 8, 9, 10, 4, 11, 7, 10, 9, 10, 9, 11, 7, 9, 7, 6, 4, 5, 9, 11, 11, 11, 8, 2, 3, 4, 9, 10, 7, 8, 9}}
                }
            };
            BFS = JsonUtility.FromJson<ReelStrips>(strBFS);
        }


        /*
                   JsonUtility.FromJson<ReelStrips>($@"
       {"reelStrips":[
       [11, 2, 3, 2, 7, 6, 9, 4, 4, 4, 11, 8, 1, 11, 3, 3, 7, 3, 8, 3, 6, 5, 9, 4, 11, 5, 7, 6, 9, 3, 5, 11, 1, 10, 11, 5, 7, 5, 10, 3, 6, 9, 9, 1, 9, 4, 6, 4, 10, 5, 10, 6, 1, 7, 8, 4, 4, 4, 8, 7, 4, 4, 4, 10, 6, 4, 4, 4, 5, 10, 5, 6, 7, 2, 3, 3, 3], 
       [11, 7, 3, 10, 0, 9, 5, 5, 8, 7, 1, 8, 7, 6, 9, 5, 11, 6, 7, 1, 4, 8, 2, 0, 9, 4, 5, 3, 10, 4, 11, 0, 6, 7, 1, 8, 11, 5, 8, 0, 6, 5, 11, 2, 10, 0, 5, 8, 10, 1, 8, 2, 9, 11, 3, 9, 0, 4, 9, 4, 10, 3, 4, 4, 0, 5, 5, 10, 7, 4, 5, 5, 5, 6, 4, 4, 4], 
       [10, 3, 4, 4, 3, 10, 1, 3, 7, 3, 5, 4, 4, 7, 3, 10, 6, 4, 4, 9, 5, 8, 6, 9, 1, 10, 4, 6, 3, 0, 5, 2, 8, 8, 1, 8, 11, 5, 11, 9, 1, 9, 11, 5, 11, 4, 2, 1, 2, 7, 6, 4, 6, 8, 5, 3, 5, 11, 9, 4, 5, 7, 4, 4, 7, 6, 10, 4, 4, 4, 4, 4, 4], 
       [11, 2, 3, 4, 3, 4, 5, 8, 5, 7, 0, 9, 4, 5, 8, 5, 0, 4, 7, 6, 6, 3, 8, 9, 3, 6, 9, 8, 2, 0, 7, 6, 7, 2, 8, 6, 7, 2, 5, 9, 6, 9, 2, 8, 6, 10, 4, 3, 3, 7, 3, 5, 11, 8, 9, 3, 3, 3], 
       [10, 3, 4, 2, 8, 3, 9, 8, 5, 2, 5, 10, 4, 4, 4, 3, 11, 2, 7, 2, 3, 11, 9, 8, 3, 8, 10, 7, 11, 2, 5, 10, 3, 7, 4, 10, 6, 10, 6, 4, 11, 11, 2, 7, 11, 4, 4, 4, 8, 5, 9, 5, 6, 4, 4, 4, 7, 6, 9, 5, 4, 4, 5, 3, 3, 3]
       ]}");*/


        /*
        public ReelStrips FS = JsonUtility.FromJson<ReelStrips>("""
        {
            "reelStrips": [{
                "strip": [11, 2, 3, 2, 7, 6, 9, 4, 4, 4, 11, 8, 1, 11, 3, 3, 7, 3, 8, 3, 6, 5, 9, 4, 11, 5, 7, 6, 9, 3, 5, 11, 1, 10, 11, 5, 7, 5, 10, 3, 6, 9, 9, 1, 9, 4, 6, 4, 10, 5, 10, 6, 1, 7, 8, 4, 4, 4, 8, 7, 4, 4, 4, 10, 6, 4, 4, 4, 5, 10, 5, 6, 7, 2, 3, 3, 3]
            }, {
                "strip": [11, 7, 3, 10, 0, 9, 5, 5, 8, 7, 1, 8, 7, 6, 9, 5, 11, 6, 7, 1, 4, 8, 2, 0, 9, 4, 5, 3, 10, 4, 11, 0, 6, 7, 1, 8, 11, 5, 8, 0, 6, 5, 11, 2, 10, 0, 5, 8, 10, 1, 8, 2, 9, 11, 3, 9, 0, 4, 9, 4, 10, 3, 4, 4, 0, 5, 5, 10, 7, 4, 5, 5, 5, 6, 4, 4, 4]
            }, {
                "strip": [10, 3, 4, 4, 3, 10, 1, 3, 7, 3, 5, 4, 4, 7, 3, 10, 6, 4, 4, 9, 5, 8, 6, 9, 1, 10, 4, 6, 3, 0, 5, 2, 8, 8, 1, 8, 11, 5, 11, 9, 1, 9, 11, 5, 11, 4, 2, 1, 2, 7, 6, 4, 6, 8, 5, 3, 5, 11, 9, 4, 5, 7, 4, 4, 7, 6, 10, 4, 4, 4, 4, 4, 4]
            }, {
                "strip": [11, 2, 3, 4, 3, 4, 5, 8, 5, 7, 0, 9, 4, 5, 8, 5, 0, 4, 7, 6, 6, 3, 8, 9, 3, 6, 9, 8, 2, 0, 7, 6, 7, 2, 8, 6, 7, 2, 5, 9, 6, 9, 2, 8, 6, 10, 4, 3, 3, 7, 3, 5, 11, 8, 9, 3, 3, 3]
            }, {
                "strip": [10, 3, 4, 2, 8, 3, 9, 8, 5, 2, 5, 10, 4, 4, 4, 3, 11, 2, 7, 2, 3, 11, 9, 8, 3, 8, 10, 7, 11, 2, 5, 10, 3, 7, 4, 10, 6, 10, 6, 4, 11, 11, 2, 7, 11, 4, 4, 4, 8, 5, 9, 5, 6, 4, 4, 4, 7, 6, 9, 5, 4, 4, 5, 3, 3, 3]
            }]
        }
        """
            );*/


        /*
                public string jsonString1 =
                $@"
                {
                    "reelStrips": [{
                        "strip": [11, 2, 3, 2, 7, 6, 9, 4, 4, 4, 11, 8, 1, 11, 3, 3, 7, 3, 8, 3, 6, 5, 9, 4, 11, 5, 7, 6, 9, 3, 5, 11, 1, 10, 11, 5, 7, 5, 10, 3, 6, 9, 9, 1, 9, 4, 6, 4, 10, 5, 10, 6, 1, 7, 8, 4, 4, 4, 8, 7, 4, 4, 4, 10, 6, 4, 4, 4, 5, 10, 5, 6, 7, 2, 3, 3, 3]
                    }, {
                        "strip": [11, 7, 3, 10, 0, 9, 5, 5, 8, 7, 1, 8, 7, 6, 9, 5, 11, 6, 7, 1, 4, 8, 2, 0, 9, 4, 5, 3, 10, 4, 11, 0, 6, 7, 1, 8, 11, 5, 8, 0, 6, 5, 11, 2, 10, 0, 5, 8, 10, 1, 8, 2, 9, 11, 3, 9, 0, 4, 9, 4, 10, 3, 4, 4, 0, 5, 5, 10, 7, 4, 5, 5, 5, 6, 4, 4, 4]
                    }, {
                        "strip": [10, 3, 4, 4, 3, 10, 1, 3, 7, 3, 5, 4, 4, 7, 3, 10, 6, 4, 4, 9, 5, 8, 6, 9, 1, 10, 4, 6, 3, 0, 5, 2, 8, 8, 1, 8, 11, 5, 11, 9, 1, 9, 11, 5, 11, 4, 2, 1, 2, 7, 6, 4, 6, 8, 5, 3, 5, 11, 9, 4, 5, 7, 4, 4, 7, 6, 10, 4, 4, 4, 4, 4, 4]
                    }, {
                        "strip": [11, 2, 3, 4, 3, 4, 5, 8, 5, 7, 0, 9, 4, 5, 8, 5, 0, 4, 7, 6, 6, 3, 8, 9, 3, 6, 9, 8, 2, 0, 7, 6, 7, 2, 8, 6, 7, 2, 5, 9, 6, 9, 2, 8, 6, 10, 4, 3, 3, 7, 3, 5, 11, 8, 9, 3, 3, 3]
                    }, {
                        "strip": [10, 3, 4, 2, 8, 3, 9, 8, 5, 2, 5, 10, 4, 4, 4, 3, 11, 2, 7, 2, 3, 11, 9, 8, 3, 8, 10, 7, 11, 2, 5, 10, 3, 7, 4, 10, 6, 10, 6, 4, 11, 11, 2, 7, 11, 4, 4, 4, 8, 5, 9, 5, 6, 4, 4, 4, 7, 6, 9, 5, 4, 4, 5, 3, 3, 3]
                    }]
                }
                ";
                */

        /*   
        public string jsonString = """  
        {  
          "Name": "John Doe",  
          "Age": 30,  
          "Address": "123 Main St, Anytown, USA"  
        }  
        """;

           string jsonString1 = $@"  
        {
        \""Name\"": \""John Doe\"",  
        \""Age\"": 30,  
        \""Address\"": \""123 Main St, Anytown, USA\""  
        }  
        ";*/


        // "./payTableSymbolInfos/__0/x3"
        // "./payTableSymbolInfos/__0/x4"
        // "./payTableSymbolInfos/__0/x5"
        public List<PayTableSymbolInfo> m_PayTableSymbolWin = new List<PayTableSymbolInfo>()
        {
            new PayTableSymbolInfo()
            {
                symbol = 0,
                x5 = 0,
                x4 = 0,
                x3 = 0,
            },
            new PayTableSymbolInfo()
            {
                symbol = 1,
                x5 = 0,
                x4 = 0,
                x3 = 2,
            },
            new PayTableSymbolInfo()
            {
                symbol = 2,
                x5 = 10,
                x4 = 4,
                x3 = 1,
            },
            new PayTableSymbolInfo()
            {
                symbol = 3,
                x5 = 6,
                x4 = 2,
                x3 = 0.6,
            },
            new PayTableSymbolInfo()
            {
                symbol = 4,
                x5 = 2,
                x4 = 1,
                x3 = 0.5,
            },
            new PayTableSymbolInfo()
            {
                symbol = 5,
                x5 = 1.6,
                x4 = 0.6,
                x3 = 0.3,
            },
            new PayTableSymbolInfo()
            {
                symbol = 6,
                x5 = 1,
                x4 = 0.3,
                x3 = 0.2,
            },
            new PayTableSymbolInfo()
            {
                symbol = 7,
                x5 = 0.6,
                x4 = 0.2,
                x3 = 0.16,
            },
            new PayTableSymbolInfo()
            {
                symbol = 8,
                x5 = 0.6,
                x4 = 0.2,
                x3 = 0.16,
            },
            new PayTableSymbolInfo()
            {
                symbol = 9,
                x5 = 0.4,
                x4 = 0.2,
                x3 = 0.1,
            },
            new PayTableSymbolInfo()
            {
                symbol = 10,
                x5 = 0.4,
                x4 = 0.2,
                x3 = 0.1,
            },
            new PayTableSymbolInfo()
            {
                symbol = 11,
                x5 = 0.4,
                x4 = 0.2,
                x3 = 0.1,
            },
        };

        public List<PayTableSymbolInfo> payTableSymbolWin
        {
            get => m_PayTableSymbolWin;
            set => observable.SetProperty(ref m_PayTableSymbolWin, value);
        }





        [SerializeField]
        public List<PayTableSymbolInfo> m_PayTableSymbolWinMultiple = new List<PayTableSymbolInfo>()
        {
            new PayTableSymbolInfo()
            {
                symbol = 0,
                x5 = 0,
                x4 = 0,
                x3 = 0,
            },
            new PayTableSymbolInfo()
            {
                symbol = 1,
                x5 = 0,
                x4 = 0,
                x3 = 2,
            },
            new PayTableSymbolInfo()
            {
                symbol = 2,
                x5 = 10,
                x4 = 4,
                x3 = 1,
            },
            new PayTableSymbolInfo()
            {
                symbol = 3,
                x5 = 6,
                x4 = 2,
                x3 = 0.6,
            },
            new PayTableSymbolInfo()
            {
                symbol = 4,
                x5 = 2,
                x4 = 1,
                x3 = 0.5,
            },
            new PayTableSymbolInfo()
            {
                symbol = 5,
                x5 = 1.6,
                x4 = 0.6,
                x3 = 0.3,
            },
            new PayTableSymbolInfo()
            {
                symbol = 6,
                x5 = 1,
                x4 = 0.3,
                x3 = 0.2,
            },
            new PayTableSymbolInfo()
            {
                symbol = 7,
                x5 = 0.6,
                x4 = 0.2,
                x3 = 0.16,
            },
            new PayTableSymbolInfo()
            {
                symbol = 8,
                x5 = 0.6,
                x4 = 0.2,
                x3 = 0.16,
            },
            new PayTableSymbolInfo()
            {
                symbol = 9,
                x5 = 0.4,
                x4 = 0.2,
                x3 = 0.1,
            },
            new PayTableSymbolInfo()
            {
                symbol = 10,
                x5 = 0.4,
                x4 = 0.2,
                x3 = 0.1,
            },
            new PayTableSymbolInfo()
            {
                symbol = 11,
                x5 = 0.4,
                x4 = 0.2,
                x3 = 0.1,
            },
        };

        /// <summary> 单组多次出现的最大赢分 </summary>
        public List<PayTableSymbolInfo> payTableSymbolWinMultiple
        {
            get => m_PayTableSymbolWinMultiple;
            set => observable.SetProperty(ref m_PayTableSymbolWinMultiple, value);
        }


        /// <summary> 线的位置 </summary>
        /*public List<int[]> payLines   //public List<List<int>> payLines;
        {
            get => m_payLines;
            set => m_payLines = value;
        }
        List<int[]> m_payLines;*/
        public List<List<int>> payLines;


        #region Panel

        public int betIndex = 0;


        /*[SerializeField]
        private bool m_IsMaxBetCtedit = false;
        public bool isMaxBetCtedit
        {
            get => m_IsMaxBetCtedit;
            set => observable.SetProperty(ref m_IsMaxBetCtedit, value);
        }

        [SerializeField]
        private bool m_IsMinBetCtedit = true;
        public bool isMinBetCtedit
        {
            get => m_IsMinBetCtedit;
            set => observable.SetProperty(ref m_IsMinBetCtedit, value);
        }*/


        [Tooltip("Spin Button State")]
        [SerializeField]
        private string m_BtnSpinState = "Stop";
        public string btnSpinState
        {
            get => m_BtnSpinState;
            set => observable.SetProperty(ref m_BtnSpinState, value);
        }



        [Tooltip("总押注")]
        [SerializeField]
        private long m_TotalBet = 0;
        public long totalBet
        {
            get => m_TotalBet;
            set => observable.SetProperty(ref m_TotalBet, value);
        }


        //long beginCrediy = 0;


        [Tooltip("选择的线数")]
        [SerializeField]
        private int m_SelectLine;
        public int selectLine
        {
            get => m_SelectLine;
            set => observable.SetProperty(ref m_SelectLine, value);
        }


        [Tooltip("单线押注分数")]
        [SerializeField]
        private long m_ApostarCredit;

        public long apostarCredit
        {
            get => m_ApostarCredit;
            set => observable.SetProperty(ref m_ApostarCredit, value);
        }


        // 【Bug】:这个在[Inspector窗口]显示不了！
        //public CustomDicWinMultiple winMultipleDic;

        /*
        public List<WinMultiple> winMultipleList = new List<WinMultiple>()
        {
            new WinMultiple(){
                winLevelType = WinLevelType.Big,
                multiple = 15,
            },
            new WinMultiple(){
                winLevelType = WinLevelType.Mega,
                multiple = 30,
            },
            new WinMultiple(){
                winLevelType = WinLevelType.Super,
                multiple = 50,
            },
            new WinMultiple(){
                winLevelType = WinLevelType.Ultra,
                multiple = 100,
            },
            new WinMultiple(){
                winLevelType = WinLevelType.Ultimate,
                multiple = 200,
            },
        };*/
        public List<WinMultiple> winMultipleList = new List<WinMultiple>();



        #endregion





        #region Game State

        [Tooltip("开始玩")]
        [SerializeField]
        private bool m_IsSpin = false;
        public bool isSpin
        {
            get => m_IsSpin;
            set => observable.SetProperty(ref m_IsSpin, value);
        }

        [Tooltip("自动玩")]
        [SerializeField]
        private bool m_IsAuto = false;
        public bool isAuto
        {
            get => m_IsAuto;
            set => observable.SetProperty(ref m_IsAuto, value);
        }


        /// <summary> 请求停止游戏 </summary>
        public bool isRequestToStop = false;


        /// <summary> 请求同步到真实金额 </summary>
        /// <remarks>
        /// * 当游戏中，进行投币，或上分
        /// </remarks>
        public bool isRequestToRealCreditWhenStop = false;



        [Tooltip("游戏状态")]
        [SerializeField]
        private string m_GameState = "Idle";
        public string gameState
        {
            get => m_GameState;
            set => observable.SetProperty(ref m_GameState, value);
        }


        public bool isFreeSpin
        {
            get => curReelStripsIndex == "FS";
        }

        /// <summary> 增局数 </summary>
        public int freeSpinAddNum;

        /// <summary> 额外添加免费游戏 </summary>
        public bool isFreeSpinAdd;
        //public bool isFreeSpinAdd {  get => freeSpinAddNum != 0; }



        /// <summary> 已经是最大免费游戏次数 </summary>
        public bool isFreeSpinMax
        {
            get => freeSpinTotalTimes == 70;
        }



        #endregion








        #region Response Data

        public List<int> middleIndexList;


        /// <summary>
        /// ???
        /// </summary>
        public List<List<int>> shufflingList;


        public string curReelStripsIndex = "BS";

        public string nextReelStripsIndex = "BS";


        public string strDeckRowCol;

        /// <summary>
        /// 这个已经改为：基本游戏+彩金了
        /// </summary>
        public long totalEarnCredit;

        /// <summary> 基础游戏赢分 </summary>
        public long baseGameWinCredit;

        /// <summary> 游戏彩金赢分 </summary>
        public long jackpotGameCredit;

        //public long totalEarnCredit;


        public string response;


        #endregion



        #region response 数据解析



        [SerializeField]
        private List<SymbolWin> m_WinList;
        public List<SymbolWin> winList
        {
            get => observable.GetProperty(() => m_WinList);
            set => observable.SetProperty(ref m_WinList, value);
        }


        /// <summary> 长滚动 </summary>
        public bool isReelsSlowMotion;


        /// <summary> 触发免费游戏的线-（备份 winList 的数据） </summary>
        public SymbolWin winFreeSpinTriggerOrAddCopy;


        /// <summary> 5连线，赢线数据 </summary>
       // public SymbolWin win5Kind;

        /// <summary> 中5连线 </summary>
        //public bool isWin5Kind;
        /// <summary> 免费游戏开始 </summary>
        public bool isFreeSpinTrigger;
        /// <summary> 免费游戏结束 </summary>
        public bool isFreeSpinResult;

        // public string customDataName;


        [SerializeField]
        private int m_MultiplierAlone = 0;
        public int multiplierAlone
        {
            get => m_MultiplierAlone;
            set => observable.SetProperty(ref m_MultiplierAlone, value);
        }


        [Tooltip("免费游戏总赢分")]
        [SerializeField]
        private long m_FreeSpinTotalWinCredit = 0;
        public long freeSpinTotalWinCredit
        {
            get => m_FreeSpinTotalWinCredit;
            set => observable.SetProperty(ref m_FreeSpinTotalWinCredit, value);
        }



        [Tooltip("免费游戏总轮数")]
        [SerializeField]
        private int m_FreeSpinTotalTimes = 0;
        public int freeSpinTotalTimes
        {
            get => m_FreeSpinTotalTimes;
            set => observable.SetProperty(ref m_FreeSpinTotalTimes, value);
        }


        [Tooltip("当前免费游戏轮数")]
        [SerializeField]
        private int m_FreeSpinPlayTimes = 0;
        /// <summary> 当前免费游戏轮数  </summary>
        public int freeSpinPlayTimes
        {
            get => m_FreeSpinPlayTimes;
            set => observable.SetProperty(ref m_FreeSpinPlayTimes, value);
        }

        /// <summary>
        /// 免费游戏显示剩余多少次
        /// </summary>
        public int showFreeSpinRemainTime
        {
            get => m_showFreeSpinRemainTime;
            set => observable.SetProperty(ref m_showFreeSpinRemainTime, value);
        }
        public int m_showFreeSpinRemainTime = 0;

        #endregion






        #region Bonus

        /// <summary> bonus数据 </summary>
        public Dictionary<int, JSONNode> bonusResult = new Dictionary<int, JSONNode>();

        #endregion




        #region Jackpot

        [Tooltip("grand jackpot")]
        [SerializeField]
        JackpotInfo m_UIGrandJP = new JackpotInfo()
        {
            name = "JPGrand",
            id = 0,
            nowCredit = 69000,
            curCredit = 69204,
            maxCredit = 11100000,
            minCredit = 0,
        };
        public JackpotInfo uiGrandJP
        {
            get => m_UIGrandJP;
            set => m_UIGrandJP = value;

            //get => observable.GetProperty(() => m_GrandJP);
            //set => observable.SetProperty(ref m_GrandJP, value);
        }


        [Tooltip("major jackpot")]
        [SerializeField]
        JackpotInfo m_UIMajorJP = new JackpotInfo()
        {
            name = "JPMajor",
            id = 1,
            nowCredit = 15000,
            curCredit = 15134,
            maxCredit = 2500000,
            minCredit = 0,
        };
        public JackpotInfo uiMajorJP
        {
            get => m_UIMajorJP;
            set => m_UIMajorJP = value;
            //get => observable.GetProperty(() => m_MajorJP);
            //set => observable.SetProperty(ref m_MajorJP, value);
        }



        [Tooltip("minor jackpot")]
        [SerializeField]
        JackpotInfo m_UIMinorJP = new JackpotInfo()
        {
            name = "JPMinor",
            id = 2,
            nowCredit = 240000,
            curCredit = 244073,
            maxCredit = 300000,
            minCredit = 0,
        };
        public JackpotInfo uiMinorJP
        {
            get => m_UIMinorJP;
            set => m_UIMinorJP = value;
            //get => observable.GetProperty(() => m_MinorJP);
            //set => observable.SetProperty(ref m_MinorJP, value);
        }



        [Tooltip("mini jackpot")]
        [SerializeField]
        JackpotInfo m_UIMiniJP = new JackpotInfo()
        {
            name = "JPMini",
            id = 3,
            nowCredit = 10000,
            curCredit = 10581,
            maxCredit = 30000,
            minCredit = 0,
        };
        public JackpotInfo uiMiniJP
        {
            get => m_UIMiniJP;
            set => m_UIMiniJP = value;
            //get => observable.GetProperty(() => m_MiniJP);
            //set => observable.SetProperty(ref m_MiniJP, value);
        }



        /// <summary> 游戏彩金数据 </summary>
        public List<JackpotWinInfo> jpWinLst => jpGameRes.jpWinLst;

        /// <summary> 每局的游戏彩金数据 </summary>
        public JackpotRes jpGameRes;


        /// <summary> 中奖前的彩金值 </summary>
        public List<float> jpGameWhenCreditLst
        {
            get
            {
                List<float> jps = new List<float>()
                {
                    jpGameRes.curJackpotGrand,
                    jpGameRes.curJackpotMajor,
                    jpGameRes.curJackpotMinior,
                    jpGameRes.curJackpotMini,
                };
                foreach (JackpotWinInfo item in jpGameRes.jpWinLst)
                {
                    jps[item.id] = item.whenCredit;
                }
                return jps;
            }
        }



        #endregion






        /// <summary> 游戏前 </summary>
        public long creditBefore;

        /// <summary> 游戏后 </summary>
        public long creditAfter;

        /// <summary> 当前本轮游戏开始时间 </summary>
        public long curGameCreatTimeMS;


        /// <summary> 当前本轮游戏guid </summary>
        public string curGameGuid;


        /// <summary> 当前本轮游戏编号 </summary>
        public long curGameNumber;


        /// <summary>  触发免费游戏的编号 </summary>
        public int gameNumberFreeSpinTrigger;



    }



    public partial class ContentBlackboard : MonoWeakSingleton<ContentBlackboard>
    {

        // 这个测试代码（[Button]）会和ContentBlackboardEditor 冲突！！


        [SerializeField]
        private TestBB01 m_TestBB01;
        public TestBB01 testBB01
        {
            get => m_TestBB01;
            set => observable.SetProperty(ref m_TestBB01, value);
        }



        [Button]
        void Test_Object()
        {
            uiMiniJP.minCredit = 999;
        }


        List<JackpotInfo> m_MiniJPLst = new List<JackpotInfo>()
        {
            new JackpotInfo(){
                name = "JP4",
                id = 0,
                nowCredit = 10000,
                curCredit = 10581,
                maxCredit = 30000,
                minCredit = 0,
            },
            new JackpotInfo(){
                name = "JP3",
                id = 1,
                nowCredit = 10000,
                curCredit = 10581,
                maxCredit = 30000,
                minCredit = 0,
            },
            new JackpotInfo(){
                name = "JP2",
                id = 2,
                nowCredit = 10000,
                curCredit = 10581,
                maxCredit = 30000,
                minCredit = 0,
            },
        };

        public List<JackpotInfo> miniJPLst
        {
            get => observable.GetProperty(() => m_MiniJPLst);
            set => observable.SetProperty(ref m_MiniJPLst, value);
        }


        Dictionary<string, JackpotInfo> m_MiniJPDic = new Dictionary<string, JackpotInfo>()
        {
            ["A"] = new JackpotInfo()
            {
                name = "JP4",
                id = 0,
                nowCredit = 10000,
                curCredit = 10581,
                maxCredit = 30000,
                minCredit = 0,
            },
            ["B"] = new JackpotInfo()
            {
                name = "JP3",
                id = 1,
                nowCredit = 10000,
                curCredit = 10581,
                maxCredit = 30000,
                minCredit = 0,
            },
            ["C"] = new JackpotInfo()
            {
                name = "JP2",
                id = 2,
                nowCredit = 10000,
                curCredit = 10581,
                maxCredit = 30000,
                minCredit = 0,
            },
        };

        public Dictionary<string, JackpotInfo> miniJPDic
        {
            get => observable.GetProperty(() => m_MiniJPDic);
            set => observable.SetProperty(ref m_MiniJPDic, value);
        }




        /// <summary>
        /// 大厅彩金数据
        /// </summary>
        public List<WinJackpotInfo> dataJackpotHall = new List<WinJackpotInfo>();



        /**/
        [Button]
        void Test_ObjectDic()
        {
            miniJPDic["C"].minCredit = 999;
        }

        [Button]
        void Test_ObjectLst()
        {
            miniJPLst[2].minCredit = 999;
        }
    }

}





namespace GameMaker
{
    [System.Serializable]
    public class TestBB01
    {

        public string name = "1234";
        public List<long> betList = new List<long>()
            {
                100,
                200,
                300,
                500
            };
    }
}