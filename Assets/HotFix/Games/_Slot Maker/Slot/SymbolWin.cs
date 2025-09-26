using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMaker
{


    /// <summary> 单线赢的数据 </summary>
    [System.Serializable]
    public class SymbolWin
    {
        public long earnCredit = 0;
        public long multiplier = 1;
        /// <summary> 中单线线号 </summary>
        public int lineNumber = -1;
        public int symbolNumber = -1;
        public List<Cell> cells = new List<Cell>();
        public string customData = "";
    }


    /// <summary> 所有线赢的数据 </summary>
    public class TotalSymbolWin : SymbolWin
    {
        public List<int> lineNumbers;
    }
}