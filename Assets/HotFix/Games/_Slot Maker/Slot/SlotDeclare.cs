using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlotMaker
{
    public static class GameState
    {
        public const string Idle = "Idle";
        public const string Spin = "Spin";
        public const string FreeSpin = "FreeSpin";
        public const string MiniGame = "MiniGame";
    }


    public static class SpinButtonState
    {
        /// <summary> ��Ϸֹͣ </summary>
        public const string Stop = "Stop";
        /// <summary> ��Ϸ������ </summary>
        public const string Spin = "Spin";
        /// <summary> ��Ϸ�Զ������� </summary>
        public const string Auto = "Auto";
    }
}