using GameMaker;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMaker
{
    public partial class TurnBlackboard : MonoWeakSingleton<TurnBlackboard>
    {
        ObservableObject m_Observable;
        public ObservableObject observable
        {
            get
            {
                if (m_Observable == null)
                {
                    string[] classNamePath = this.GetType().ToString().Split('.');
                    m_Observable = new ObservableObject(classNamePath[classNamePath.Length - 1], "@turn/");
                }
                return m_Observable;
            }
        }



        public List<Turn> history = new List<Turn>();

        public Turn turn = null;

        public Spin spin = null;

        public Bonus bonus = null;

        public ContentNode current = null;





        [Button]
        void TestClearTurn()
        {
            TurnBlackboard.Instance.turn = null;
            TurnBlackboard.Instance.spin = null;
            TurnBlackboard.Instance.bonus = null;
            TurnBlackboard.Instance.current = null;
        }


    }
}