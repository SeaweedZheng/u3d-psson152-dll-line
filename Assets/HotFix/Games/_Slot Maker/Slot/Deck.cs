using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlotMaker
{
    [Serializable]
    public class Deck
    {
        public List<int> stripIndices;
        public List<List<SymbolInfo>> deck;
        //public List<List<SymbolInfo>> mask;
        //public List<List<bool>> hitMap;
        public SymbolInfo GetSymbolInfo(int reelIndex, int row)
        {
            return deck[reelIndex][row];
        }
    }
}
