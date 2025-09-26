using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMaker;
using GameMaker;
using Sirenix.OdinInspector;
//using __ConstData = PssOn00152.ConstData;
using _customBB = PssOn00152.CustomBlackboard;
using _reelSetBB = SlotMaker.ReelSettingBlackboard;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;
using System.Linq;
namespace PssOn00152
{

    public enum SpinWinEvent
    {
        None,
        TotalWinLine,
        SingleWinLine,
    }

    public partial class SlotMachineController : SlotMachineBaseController
    {

        //const string COR_GAME_START = "COR_GAME_START";

        protected void Awake()
        {
            //SoundManager.Instance.PlayMusic(SoundConstName.mainGameLoopPlaying);

            this.column = _customBB.Instance.column;
            this.row = _customBB.Instance.row;

            for (int i = 0; i < this.column; i++)
            {
                reels[i].reelIndex = i;
            }


            base.Init(_customBB.Instance);
        }


        protected void OnDisable()
        {
            //ClearAllCor();
        }



        #region 免费游戏显示赢线特效


        public bool CheckHasSymbolChange(SymbolWin curSymbolWin)
        {
                List<Cell> cells = curSymbolWin.cells;
                for (int i = 0; i< cells.Count;  i++)
                {
                    Cell cel = cells[i];
                    SymbolBase symble = GetVisibleSymbolFromDeck(cel.column, cel.row);
                    if (symble.number != 0 && symble.number != curSymbolWin.symbolNumber)
                    {
                        return true;
                    }
                }
            return false;
        }


        public bool Check5kind(SymbolWin curSymbolWin)
        {
            List<int> rowIndexLst = new List<int>();
            foreach (Cell cel in curSymbolWin.cells)
            {
                if (!rowIndexLst.Contains(cel.column))
                {
                    rowIndexLst.Add(cel.column);
                }
            }
            return rowIndexLst.Count == 5;
        }

        /*
        IEnumerator ShowSingleWinListOnceInFreeSpin(List<SymbolWin> winList, float waitMS = 3f, float minS = 3f)
        {
            int idx = 0;
            while (idx < winList.Count)
            {
                SymbolWin curSymbolWin = winList[idx];

                //改变图标
                if (CheckHasSymbolChange(curSymbolWin))
                {
                    yield return ShowSymbolChange(curSymbolWin);

                    //五连线

                    ShowSymbolWin(curSymbolWin, true, false);
                }
                else
                {
                    ShowSymbolWin(curSymbolWin, true , true);
                }


                //是否是五连线

                EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                    new EventData<SymbolWin>(SlotMachineEvent.SingleWin, curSymbolWin));

                yield return SlotWaitForSeconds(waitMS, minS);

                ++idx;
            }

            //停止特效显示
            SkipWin();
        }*/



        #endregion


        #region 显示赢线特效

        /*
        /// <summary>
        /// 每条线单独显示一次
        /// </summary>
        /// <param name="winList"></param>
        /// <param name="waitMS"></param>
        /// <param name="minS"></param>
        /// <param name="isAnim"></param>
        /// <returns></returns>
        public IEnumerator ShowSingleWinListOnce(List<SymbolWin> winList, float waitMS = 3f,float minS= 3f , bool isAnim = true)
        {
            int idx = 0;
            while (idx < winList.Count)
            {

                ShowSymbolWin(winList[idx], isAnim);

                EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                    new EventData<SymbolWin>(SlotMachineEvent.SingleWinLine, winList[idx]));

                yield return SlotWaitForSeconds(waitMS);

                ++idx;

            }

            //停止特效显示
            SkipWinLine(false);
        }*/




        //public void ShowSymbolWinAnim(SymbolWin symbolWin) => ShowSymbolWin(symbolWin, true);
        //public void ShowSymbolWinNoAnim(SymbolWin symbolWin) => ShowSymbolWin(symbolWin, false);

        //private void _SymbolWin(SymbolWin symbolWin, bool isAmin = true, int? symbolIndex = null)

        /*
        /// <summary>
        /// 准备弃用
        /// </summary>
        /// <param name="symbolWin"></param>
        /// <param name="isAmin"></param>
        /// <param name="isUseMySelfSymbolIndex"></param>
        public void ShowSymbolWin(SymbolWin symbolWin, bool isAmin = true, bool isUseMySelfSymbolIndex = true)
        {
            //停止特效显示
            SkipWinLine(false);

            foreach (Cell cel in symbolWin.cells)
            {
                BaseSymbol symble = GetSymbolFromDeck(cel.column, cel.row);

                // 开始播放单中奖线效显示
                //int idx = symbolWin.symbolIndex; //不允许！存在 wild 或 symbol;
                //int idx = symbolIndex != null ? (int)symbolIndex : symble.index;
                int idx = isUseMySelfSymbolIndex ? symble.index : symbolWin.symbolIndex;

                string symbolName = _customBB.Instance.symbolHitEffect[idx];  // wild  or symbol;

                // 图标动画
                GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                symble.AddSymbolEffect(goSymbolEffect, isAmin);

                // 边框
                if (_spinWEBB.Instance.isFrame)
                {
                    string borderEffect = _customBB.Instance.borderEffect;
                    GameObject goBorderEffect = ContentPoolManager.Instance.GetObject(borderEffect);
                    symble.AddBorderEffect(goBorderEffect);
                }


                // 整体变大特效
                if (isAmin && _spinWEBB.Instance.isBigger)
                    symble.ShowBiggerEffect();
                    //symble.transform.Find("Animator").GetComponent<Animator>().Play("Scale");
            }
        }
        */


        /*
        
        /// <summary>
        /// 显示所有赢线的图标，一次
        /// </summary>
        /// <param name="winList"></param>
        public IEnumerator ShowTotalWinListOnce(List<SymbolWin> winList, float waitS, float minS)
        {

            ShowTotalWinListOnce(winList);
            yield return SlotWaitForSeconds(waitS, minS);
            SkipWinLine(false);
        }
        */


        /*
        /// <summary>
        /// 将弃用
        /// </summary>
        /// <param name="winList"></param>
        /// <param name="isSendEvent"></param>
        public void ShowTotalWinListOnce(List<SymbolWin> winList, bool isSendEvent = true)
        {

            List<BaseSymbol> bsLst = new List<BaseSymbol>();

            long earnCredit = 0;
            List<Cell> cells = new List<Cell>();

            foreach (SymbolWin sw in winList)
            {
                foreach (Cell cel in sw.cells)
                {
                    BaseSymbol symble = GetSymbolFromDeck(cel.column, cel.row);
                    if (bsLst.Contains(symble))
                        continue;
                    cells.Add(new Cell(cel.column, cel.row));
                    bsLst.Add(symble);
                }

                earnCredit += sw.earnCredit;
            }

            //停止特效显示
            SkipWinLine(false);

            foreach (BaseSymbol symble in bsLst)
            {
                string symbolName = _customBB.Instance.symbolHitEffect[symble.index];  // wild  or symbol;

                GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                symble.AddSymbolEffect(goSymbolEffect, false);

                if (_spinWEBB.Instance.isFrame)
                {
                    string borderEffect = _customBB.Instance.borderEffect;
                    GameObject goBorderEffect = ContentPoolManager.Instance.GetObject(borderEffect);
                    symble.AddBorderEffect(goBorderEffect);
                }

            }

            if (isSendEvent)
            {
                SymbolWin totalWin = new SymbolWin()
                {
                    earnCredit = earnCredit,
                    cells = cells,
                };

                EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                    new EventData<SymbolWin>(SlotMachineEvent.TotalWinLine, totalWin));
            }

            // 留到上一层添加延时
            // yield return new WaitForSeconds(3);
        }*/

        /*
        /// <summary>
        /// 准备弃用
        /// </summary>
        /// <param name="winList"></param>
        /// <returns></returns>
        public IEnumerator ShowSingleWinListAwayDuringIdle(List<SymbolWin> winList)
        {
            while (winList.Count > 0) //while (idx < winList.Count)
            {
                yield return ShowSingleWinListOnce(winList);
            }
        }*/

        /*
        /// <summary>
        /// 播放“图标改变特效”，并改变图标的图标号
        /// </summary>
        /// <param name="symbolWin"></param>
        /// <param name="minS"></param>
        /// <returns></returns>
        public IEnumerator ShowSymbolChange(SymbolWin symbolWin, float minS = 0.5f)
        {

            foreach (Cell cel in symbolWin.cells)
            {

                BaseSymbol symble = GetSymbolFromDeck(cel.column, cel.row);

                string symbolName = _customBB.Instance.symbolHitEffect[symbolWin.symbolIndex];

                //string symbolName = __ConstData.Instance.symbolEffect[symble.index];  // wild  or symbol;

                GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject("Symbol Change");
                symble.AddSymbolEffect(goSymbolEffect);

                // 整体变大特效
                //symble.transform.Find("Animator").GetComponent<Animator>().Play("Scale");

                if (_spinWEBB.Instance.isBigger)
                    symble.ShowBiggerEffect();
            }

            GameSoundHelper.Instance.PlaySound(SoundKey.FreeSpinChangeSymbol);

            yield return SlotWaitForSeconds(0.5f);

            ChangeSymbol(symbolWin);

            yield return SlotWaitForSeconds(0.4f);
        }
        */
        #endregion



        #region 开奖动画

        public IEnumerator ShowSymbolWinBySetting(SymbolWin symbolWin, bool isUseMySelfSymbolNumber, SpinWinEvent eventType)
        {

            //停止特效显示
            SkipWinLine(false);

            // 立马停止时，不播放赢分环节？
            if (isStopImmediately && _spinWEBB.Instance.isSkipAtStopImmediately)
                yield break;

            //显示遮罩
            goSlotCover?.SetActive(_spinWEBB.Instance.isShowCover);

            foreach (Cell cel in symbolWin.cells)
            {
                SymbolBase symble = GetVisibleSymbolFromDeck(cel.column, cel.row);

                // 开始播放单中奖线效显示
                //int idx = symbolWin.symbolIndex; //不允许！存在 wild 或 symbol;
                //int idx = symbolIndex != null ? (int)symbolIndex : symble.index;
                int symbolNumber = -99;
                string symbolName = "";
                try
                {
                    symbolNumber = isUseMySelfSymbolNumber ? symble.number : symbolWin.symbolNumber;

                    symbolName = _customBB.Instance.symbolHitEffect[symbolNumber.ToString()];  // wild  or symbol;

                    // 图标动画
                    GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                    symble.AddSymbolEffect(goSymbolEffect, _spinWEBB.Instance.isSymbolAnim);

                }
                catch (Exception ex)
                {
                    Debug.LogError($" 报错 symbolNumber: {symbolNumber}  symbolName: {symbolName}");
                }

                // 边框
                if (_spinWEBB.Instance.isFrame)
                {
                    string borderEffect = _customBB.Instance.borderEffect;
                    GameObject goBorderEffect = ContentPoolManager.Instance.GetObject(borderEffect);
                    symble.AddBorderEffect(goBorderEffect);
                }

                // 整体变大特效
                if (_spinWEBB.Instance.isBigger)
                    symble.ShowBiggerEffect();

            }


            // 是否显示线
            if (_spinWEBB.Instance.isShowLine)
            {
                if(symbolWin is TotalSymbolWin)
                {
                    TotalSymbolWin totalSymbolWin = symbolWin as TotalSymbolWin;

                    foreach(int lineNumber in totalSymbolWin.lineNumbers)
                    {
                        int lineIndex = GetPayLineIndex(lineNumber);
                        if (lineIndex >= 0 && lineIndex < tfmPayLines.childCount)
                        {
                            tfmPayLines.GetChild(lineIndex).gameObject.SetActive(true);
                        }
                    }             
                }
                else
                {
                    int lineIndex = GetPayLineIndex(symbolWin.lineNumber);
                    if (lineIndex >= 0 && lineIndex < tfmPayLines.childCount )
                    {
                        tfmPayLines.GetChild(lineIndex).gameObject.SetActive(true);
                    }
                }
            }


            // 事件
            if (eventType == SpinWinEvent.TotalWinLine)
            {
                EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                    new EventData<SymbolWin>(SlotMachineEvent.TotalWinLine, symbolWin));
            }
            else if(eventType ==SpinWinEvent.SingleWinLine)
            {
                EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                    new EventData<SymbolWin>(SlotMachineEvent.SingleWinLine, symbolWin));
            }

            yield return SlotWaitForSeconds(_spinWEBB.Instance.timeS);
        }



        public SymbolWin GetTotalSymbolWin(List<SymbolWin> winList)
        {
            List<SymbolBase> bsLst = new List<SymbolBase>();

            long earnCredit = 0;
            List<Cell> cells = new List<Cell>();

            List<int> lineIndexs = new List<int>();

            foreach (SymbolWin sw in winList)
            {
                foreach (Cell cel in sw.cells)
                {
                    SymbolBase symble = GetVisibleSymbolFromDeck(cel.column, cel.row);
                    if (bsLst.Contains(symble))
                        continue;
                    cells.Add(new Cell(cel.column, cel.row));
                    bsLst.Add(symble);
                }

                // 获得所有赢线的线号
                if (!lineIndexs.Contains(sw.lineNumber))
                    lineIndexs.Add(sw.lineNumber);

                earnCredit += sw.earnCredit;
            }

            TotalSymbolWin totalWin = new TotalSymbolWin()
            {
                lineNumbers = lineIndexs,
                earnCredit = earnCredit,
                cells = cells,
            };

            return totalWin;
        }



        /// <summary>
        /// 显示所有赢线的图标，一次
        /// </summary>
        /// <param name="winList"></param>
        public IEnumerator ShowWinListBySetting(List<SymbolWin> winList)
        {

            // 立马停止时，不播放赢分环节？
            if (isStopImmediately && _spinWEBB.Instance.isSkipAtStopImmediately)
                yield break;

            if (_spinWEBB.Instance.isTotalWin)
            {
                yield return ShowSymbolWinBySetting(GetTotalSymbolWin(winList), true, SpinWinEvent.TotalWinLine);
            }
            else
            {
                //显示遮罩
                //goSlotCover?.SetActive(_spinWEBB.Instance.isShowCover);

                int idx = 0;
                while (idx < winList.Count)
                {
                    yield return ShowSymbolWinBySetting(winList[idx], true , SpinWinEvent.SingleWinLine);

                    ++idx;

                    // 立马停止时，不播放赢分环节？
                    if (isStopImmediately && _spinWEBB.Instance.isSkipAtStopImmediately)
                        break;
                }
            }

            //关闭遮罩
            CloseSlotCover();

            //停止特效显示
            SkipWinLine(false);
        }



        public IEnumerator ShowWinListAwayDuringIdle009(List<SymbolWin> winList)
        {
            while (winList.Count > 0) //while (idx < winList.Count)
            {
                yield return ShowWinListBySetting(winList);
            }
        }



        /// <summary>
        /// 播放“图标改变特效”，并改变图标的图标号
        /// </summary>
        /// <param name="symbolWin"></param>
        /// <param name="minS"></param>
        /// <returns></returns>
        public IEnumerator ShowSymbolChangeBySetting(SymbolWin symbolWin, string symbolEffectName) //"Symbol Change"
        {

            //停止特效显示
            SkipWinLine(false);

            // 立马停止时，不播放赢分环节？
            if (isStopImmediately && _spinWEBB.Instance.isSkipAtStopImmediately)
                yield break;

            //显示遮罩
            goSlotCover?.SetActive(_spinWEBB.Instance.isShowCover);

            foreach (Cell cel in symbolWin.cells)
            {

                SymbolBase symble = GetVisibleSymbolFromDeck(cel.column, cel.row);

                string symbolName = _customBB.Instance.symbolHitEffect[symbolWin.symbolNumber.ToString()];

                GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolEffectName);
                symble.AddSymbolEffect(goSymbolEffect);

                // 边框
                if (_spinWEBB.Instance.isFrame)
                {
                    string borderEffect = _customBB.Instance.borderEffect;
                    GameObject goBorderEffect = ContentPoolManager.Instance.GetObject(borderEffect);
                    symble.AddBorderEffect(goBorderEffect);
                }

                if (_spinWEBB.Instance.isBigger)
                    symble.ShowBiggerEffect();
            }

            //GameSoundHelper.Instance.PlaySound(SoundKey.FreeSpinChangeSymbol);
            yield return SlotWaitForSeconds(_spinWEBB.Instance.timeS);

            ChangeSymbol(symbolWin);

        }


        public void ShowSymbolWinDeck(SymbolWin symbolWin, bool isUseMySelfSymbolNumber)
        {
            //停止特效显示
            SkipWinLine(false);

            //显示遮罩
            goSlotCover?.SetActive(_spinWEBB.Instance.isShowCover);

            foreach (Cell cel in symbolWin.cells)
            {
                SymbolBase symbol = GetVisibleSymbolFromDeck(cel.column, cel.row);

                int symbolNumber = isUseMySelfSymbolNumber ? symbol.number : symbolWin.symbolNumber;

                string symbolName = _customBB.Instance.symbolHitEffect[symbolNumber.ToString()];  // wild  or symbol;

                // 图标动画
                GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                symbol.AddSymbolEffect(goSymbolEffect, _spinWEBB.Instance.isSymbolAnim);

                // 边框
                if (_spinWEBB.Instance.isFrame)
                {
                    string borderEffect = _customBB.Instance.borderEffect;
                    GameObject goBorderEffect = ContentPoolManager.Instance.GetObject(borderEffect);
                    symbol.AddBorderEffect(goBorderEffect);
                }

                // 整体变大特效
                if (_spinWEBB.Instance.isBigger)
                    symbol.ShowBiggerEffect();
            }

            // 是否显示线
            if (_spinWEBB.Instance.isShowLine)
            {
                if (symbolWin is TotalSymbolWin)
                {
                    TotalSymbolWin totalSymbolWin = symbolWin as TotalSymbolWin;

                    foreach (int payLineNumber in totalSymbolWin.lineNumbers)
                    {
                        int lineIndex = GetPayLineIndex(payLineNumber);
                        if (lineIndex >= 0 && lineIndex < tfmPayLines.childCount)
                        {
                            tfmPayLines.GetChild(lineIndex).gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    int lineIndex = GetPayLineIndex(symbolWin.lineNumber);
                    if (lineIndex >= 0 && lineIndex < tfmPayLines.childCount)
                    {
                        tfmPayLines.GetChild(lineIndex).gameObject.SetActive(true);
                    }
                }
            }

        }


        #endregion






        /*
        public void RunGame(string strDeckRowCol = "1,1,1,1,1#2,2,6,2,2#3,3,3,3,3")
        {
            List<int> deckColRow = SlotTool.GetDeckColRow(strDeckRowCol);

            RunGame(deckColRow.ToArray(), 0);

        }

        public void RunGame(int[] deckColRow, int rollTimes, bool showBigWinEff = false)
        {

            List<List<int>> colrowLsts = SlotTool.GetDeckColRow(deckColRow,
                this.column,
                this.row);

            List<int>[] colrow = colrowLsts.ToArray();

            //这个还要判断特殊图标 如果有还需要改变滚轮滚的次数 还有特殊表现效果
            //模拟图标
            for (int i = 0; i < this.column; i++)
            {
                reels[i].SetResult(colrow[i]);
            }

            DoCor(COR_GAME_START, StartTurnReels());

        }*/





        /*
        [Button]
        public void TestTurnReels(string strDeckRowCol = "1,1,1,1,1#2,2,6,2,2#3,3,3,3,3")
        {
            RunGame(strDeckRowCol);
        }



        [Button]
        public void TestGetSymbolInfo(int col, int row)
        {
            int _row = row + bufferTop;

            try
            {
                BaseReel reel = reels[col];
                int length = reel.ItemList.Count;

                if (reel.ItemList.Count <= _row)
                {
                    DebugUtil.Log("超出范围！！！");
                }
                else
                {
                    BaseSymbol symbol = reel.ItemList[_row];
                    DebugUtil.Log($"symbol name = {symbol.name}");
                }
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"col = {col} row = {_row}");
                DebugUtil.LogException(e);
            }
        }*/


        [Button]
        public new void BeginBonusFreeSpinAdd() => BeginBonus("FreeSpinAdd");
        public new void EndBonusFreeSpinAdd() => EndBonus("FreeSpinAdd");
    }
}