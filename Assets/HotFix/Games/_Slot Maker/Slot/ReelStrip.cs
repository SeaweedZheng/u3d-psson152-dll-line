using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotMaker
{
    [Serializable]
    public class ReelStrip
    {
        //List<SymbolInfo> strip;
        [SerializeField]
        public List<int> strip;
    }

    [Serializable]
    public class ReelStrips{

        /// <summary> ÿ�е���� </summary>
        [SerializeField]
        public List<ReelStrip> reelStrips;
    }
}