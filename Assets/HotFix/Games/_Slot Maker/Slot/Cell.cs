using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotMaker
{
    [Serializable]
    public class Cell
    {
        public int column;
        public int row;
        public Cell()
        {
            column = 0;
            row = 0;
        }
        public Cell(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public override int GetHashCode() { return GetHashCode(column, row); }

        public static int GetHashCode(int column, int row)
        {
            return column * 10000 + row;
        }
    }
}