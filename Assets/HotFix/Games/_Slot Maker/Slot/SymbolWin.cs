using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMaker
{


    /// <summary> ����Ӯ������ </summary>
    [System.Serializable]
    public class SymbolWin
    {
        public long earnCredit = 0;
        public long multiplier = 1;
        /// <summary> �е����ߺ� </summary>
        public int lineNumber = -1;
        public int symbolNumber = -1;
        public List<Cell> cells = new List<Cell>();
        public string customData = "";
    }


    /// <summary> ������Ӯ������ </summary>
    public class TotalSymbolWin : SymbolWin
    {
        public List<int> lineNumbers;
    }
}