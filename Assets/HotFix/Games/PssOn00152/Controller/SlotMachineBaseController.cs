using GameMaker;
using PssOn00152;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _reelSetBB = SlotMaker.ReelSettingBlackboard;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;

namespace SlotMaker
{
    public class SlotMachineBaseController : CorBehaviour
    {
        protected ICustomBlackboard customModel;

        /// <summary> 遮罩 </summary>
        public GameObject goSlotCover;

        /// <summary> 线 </summary>
        public Transform tfmPayLines;

        /// <summary>
        /// 所有滚轮
        /// </summary>
        public List<ReelBase> reels;

        /// <summary>
        /// 特殊图标播放动画
        /// </summary>
        protected bool isSymbolAppearEffectWhenReelStop;


        /// <summary> 立马停止 </summary>
        public bool isStopImmediately = false;


        /// <summary> 滚轮静止时顶部预留图标个数 </summary>
        public int bufferTop = 1;

        /// <summary> 滚轮静止时底部预留图标个数 </summary>
        public int bufferButton = 4;


        public int column;

        public int row;


        public void Init(ICustomBlackboard customModel)
        {
            this.customModel = customModel; 
        }

        #region 图标操作

        /// <summary>
        /// 获取甲板可见区域的第row行，第col列的图标对象
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public SymbolBase GetVisibleSymbolFromDeck(int col, int row)
        {
            int _row = row + bufferTop;

            try
            {
                ReelBase reel = reels[col];
                SymbolBase symbol = reel.symbolList[_row];
                return symbol;
            }
            catch (Exception e)
            {
                DebugUtils.LogError($"col = {col} row = {_row}");
                DebugUtils.LogException(e);
            }
            return null;
        }


        /// <summary>
        /// 改变SymbolWin所描述的中奖图标对象的图标图片
        /// </summary>
        /// <param name="symbolWin"></param>
        public void ChangeSymbol(SymbolWin symbolWin)
        {
            foreach (Cell cel in symbolWin.cells)
            {
                SymbolBase symble = GetVisibleSymbolFromDeck(cel.column, cel.row);
                symble.SetSymbolImage(symbolWin.symbolNumber);
            }
        }

        #endregion

        /*
        public IEnumerator SlotWaitForSeconds(float waitS, float minS = 0f)
        {
            if (minS < 0f)
                DebugUtil.LogError("【SlotMachine】: minS less than 0");
            else if (waitS < 0f)
                DebugUtil.LogError("【SlotMachine】: waitS less than 0");
            else if (waitS < minS)
                DebugUtil.LogError("【SlotMachine】: waitS less than minS");


            if (waitS <= 0f)
                yield break;

            if (minS > 0f)
                yield return new WaitForSeconds(minS);

            if (isStopImmediately)
                yield break;

            float targetTimeS = Time.time + (waitS - minS);
            while (targetTimeS > Time.time)
            {
                if (isStopImmediately)
                    break;
                //yield return new WaitForSeconds(0.01f);
                yield return null;
            }
        }*/

        public IEnumerator SlotWaitForSeconds(float waitS)
        {
            if (waitS <= 0f)
                yield break;

            float targetTimeS = Time.time + waitS;
            while (targetTimeS > Time.time)
            {
                yield return null;
            }
        }

        /// <summary>
        /// 某列滚轮显示特殊图标﻿的动画
        /// </summary>
        /// <param name="colIndex"></param>
        protected void ShowReelSymbolAppearEffect(int colIndex)
        {
            ReelBase reel = reels[colIndex];
            reel.SymbolAppearEffect();
        }


        public void ShowSymbolAppearEffectAfterReelStop(bool isEnable = true)
        {
            isSymbolAppearEffectWhenReelStop = isEnable;
        }



        #region 甲板设置


        [Button]
        public void SetReelsDeck(string strDeckRowCol = "1,1,1,1,1#2,2,6,2,2#3,3,3,3,3")
        {
            //停止特效显示
            SkipWinLine(false);

            int[] deckColRow = SlotTool.GetDeckColRow(strDeckRowCol).ToArray();
            List<List<int>> colrowLsts = SlotTool.GetDeckColRow(deckColRow,
                this.column,
                this.row);

            List<int>[] colrow = colrowLsts.ToArray();

            //这个还要判断特殊图标 如果有还需要改变滚轮滚的次数 还有特殊表现效果
            //模拟图标
            for (int i = 0; i < this.column; i++)
            {
                reels[i].SetResult(colrow[i]);
                reels[i].SetReelDeck();
            }
        }

        #endregion


        #region 滚轮滚动接口



        protected IEnumerator StartTurnReels()
        {

            int reelsCount = this.column;

            bool isNext = false;

            for (int reelIdx = 0; reelIdx < this.column; reelIdx++)
            {
                if (_reelSetBB.Instance.GetTimeTurnStartDelay(reelIdx) > 0)
                {
                    yield return new WaitForSeconds(_reelSetBB.Instance.GetTimeTurnStartDelay(reelIdx));
                }

                //ColumnList[reelIdx].StartRoll(rollTimes + reelIdx * __ConstData.Instance.GetNumReelTurnGap(reelIdx), RollFin);

                int _reelIdx = reelIdx;

                reels[reelIdx].StartTurn(
                    _reelSetBB.Instance.GetNumReelTurn(reelIdx) + reelIdx * _reelSetBB.Instance.GetNumReelTurnGap(reelIdx),
                    () =>
                    {

                        EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT,
                            new EventData<int>(SlotMachineEvent.PrepareStoppedReel, _reelIdx));

                        if (isSymbolAppearEffectWhenReelStop)
                            ShowReelSymbolAppearEffect(_reelIdx);

                        if (--reelsCount <= 0)
                        {
                            isNext = true;
                        }

                    }
                );
            }

            yield return new WaitUntil(() => isNext == true);
            isNext = false;

            foreach (ReelBase reel in reels)
            {
                reel.SetReelState(ReelState.Idle);
            }

            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_SLOT_EVENT,
                new EventData(SlotMachineEvent.StoppedSlotMachine));

        }


        /// <summary>
        /// 滚轮正常滚动
        /// </summary>
        /// <param name="strDeckRowCol"></param>
        /// <param name="finishCallback"></param>
        /// <returns></returns>
        public IEnumerator TurnReelsNormal(string strDeckRowCol = "1,1,1,1,1#2,2,6,2,2#3,3,3,3,3", Action finishCallback = null)
        {
            //停止特效显示
            SkipWinLine(false);

            int[] deckColRow = SlotTool.GetDeckColRow(strDeckRowCol).ToArray();
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

            //gameState = GameState.GAME;
            //PlayerData.Instance.SetPlayerCredit(credit);
            yield return StartTurnReels();
            // 算分

            finishCallback?.Invoke();
        }



        /// <summary>
        /// 滚轮滚动单次
        /// </summary>
        /// <param name="strDeckRowCol"></param>
        /// <param name="finishCallback"></param>
        /// <returns></returns>
        public IEnumerator TurnReelsOnce(string strDeckRowCol = "1,1,1,1,1#2,2,6,2,2#3,3,3,3,3", Action finishCallback = null)
        {

            SkipWinLine(false);

            int[] deckColRow = SlotTool.GetDeckColRow(strDeckRowCol).ToArray();
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

            yield return ReelsToStopOrTurnOnce(null);
            // 算分

            finishCallback?.Invoke();
        }


        /// <summary>
        /// 已滚动的滚轮立马停止、未滚动的滚轮滚动一次
        /// </summary>
        /// <param name="finishCallback"></param>
        /// <returns></returns>
        public IEnumerator ReelsToStopOrTurnOnce(Action finishCallback)
        {

            // EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_SLOT_EVENT,
            //    new EventData(SlotMachineEvent.SpinSlotMachine));

            int reelsCount = this.column;

            bool isNext = false;

            for (int reelIdx = 0; reelIdx < this.column; reelIdx++)
            {
                if (reels[reelIdx].state == ReelState.EndStop)
                {
                    reelsCount--;
                    continue;
                }

                if (reels[reelIdx].state == ReelState.Idle)
                {
                    if (_reelSetBB.Instance.GetTimeTurnStartDelay(reelIdx) > 0)
                    {
                        yield return new WaitForSeconds(_reelSetBB.Instance.GetTimeTurnStartDelay(reelIdx));
                    }
                }

                int _reelIdx = reelIdx;

                reels[reelIdx].ReelToStopOrTurnOnce(
                    () =>
                    {
                        EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT,
                            new EventData<int>(SlotMachineEvent.PrepareStoppedReel, _reelIdx));

                        if (isSymbolAppearEffectWhenReelStop)
                            ShowReelSymbolAppearEffect(_reelIdx);

                        if (--reelsCount <= 0)
                        {
                            isNext = true;
                        }

                    }
                );
            }

            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            foreach (ReelBase reel in reels)
            {
                reel.SetReelState(ReelState.Idle);
            }


            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_SLOT_EVENT,
                new EventData(SlotMachineEvent.StoppedSlotMachine));

            finishCallback?.Invoke();
        }


        #endregion



        #region 滚轮各阶段的事件


        /// <summary> 停止特效显示 </summary>
        public void SkipWinLine(bool isIncludeTag)
        {
            // 打开基础图标
            if (_spinWEBB.Instance.isHideBaseIcon)
            {
                Symbol[] symbols = transform.GetComponentsInChildren<Symbol>();
                foreach (Symbol sb in symbols)
                {
                    sb.HideBaseSymbolIcon(false);
                }
            }

            foreach (ReelBase reel in reels)
            {
                PooledObject[] comps = reel.transform.GetComponentsInChildren<PooledObject>();
                for (int i = 0; i < comps.Length; i++)
                {
                    if(isIncludeTag)
                        comps[i].ReturnToPool();
                    else
                    {
                        SkipWinLineIgnoreTag tg1 = comps[i].transform.GetComponent<SkipWinLineIgnoreTag>();
                        if (tg1 == null)
                            comps[i].ReturnToPool();
                    }
                }
            }

            // 关掉所有线
            foreach (Transform tfmLine in tfmPayLines)
            {
                tfmLine.gameObject.SetActive(false);
            }

            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                new EventData(SlotMachineEvent.SkipWinLine));
        }


        public long GetTotalWinCredit(List<SymbolWin> winList)
        {
            long totalWinCredit = 0;
            foreach (SymbolWin win in winList)
            {
                totalWinCredit += win.earnCredit;
            }
            return totalWinCredit;
        }

        public void SendPrepareTotalWinCreditEvent(long totalWinCredit , bool isAddToCredit = false)
        {
            PrepareTotalWinCredit res = new PrepareTotalWinCredit()
            {
                totalWinCredit = totalWinCredit,
                isAddToCredit = isAddToCredit,
            };

            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                new EventData<PrepareTotalWinCredit>(SlotMachineEvent.PrepareTotalWinCredit, res));
        }

        /// <summary>
        /// 显示总赢(给控制面板)
        /// </summary>
        /// <param name="winList"></param>
        public void SendTotalWinCreditEvent(List<SymbolWin> winList)
        {
            long totalWinCredit = 0;
            foreach (SymbolWin win in winList)
            {
                totalWinCredit += win.earnCredit;
            }
            SendTotalWinCreditEvent(totalWinCredit);
        }
        public void SendTotalWinCreditEvent(long totalWinCredit)
        {
            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                new EventData<long>(SlotMachineEvent.TotalWinCredit, totalWinCredit));
        }



        /// <summary>
        /// 开始Spin
        /// </summary>
        /// <remarks>
        /// * Turn开始事件<br/>
        /// * 显示玩家金币（显示玩家上把金额）<br/>
        /// </remarks>
        public void BeginTurn()
        {
            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_CONTENT_EVEN,
                        new EventData(SlotMachineEvent.BeginTurn));

            //显示上把游戏最后的金额
            //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
            //new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));

            //同步到游戏最后的金额
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);
        }

        /// <summary>
        /// 开始Spin
        /// </summary>
        /// <remarks>
        /// * SlotMachine开始转动事件<br/>
        /// * Spin开始事件<br/>
        /// * 显示玩家金币（减去压注金额）<br/>
        /// </remarks>
        public void BeginSpin()
        {

            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_SLOT_EVENT,
                new EventData(SlotMachineEvent.SpinSlotMachine));


            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_CONTENT_EVEN,
                        new EventData(SlotMachineEvent.BeginSpin));

            /*显示玩家金币（减去压注）
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));*/

            //显示玩家金币（减去压注）
            MainBlackboardController.Instance.SyncMyUICreditToTemp();
        }


        public void BeginBonus(string name)
        {
            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_CONTENT_EVEN,
                new EventData<string>(SlotMachineEvent.BeginBonus, name));
            //{\"name\":\"freeSpin\",\"guid\":1,\"data\":\"{}\"}
        }

        public void EndBonus(string name)
        {

            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_CONTENT_EVEN,
                new EventData<string>(SlotMachineEvent.EndBonus, name));
        }


        public void BeginBonusFreeSpin() => BeginBonus("FreeSpin");
        public void EndBonusFreeSpin() => EndBonus("FreeSpin");

        
        public void BeginBonusFreeSpinAdd() => BeginBonus("FreeSpinAdd");
        public void EndBonusFreeSpinAdd() => EndBonus("FreeSpinAdd");

        public void BeginBonusJackpot(string name) => BeginBonus($"JP{name}");
        public void EndBonusJackpot(string name) => EndBonus($"JP{name}");

        public void BeginBonusMiniGame(string name) => BeginBonus($"MiniGame{name}");
        public void EndBonusMiniGame(string name) => EndBonus($"MiniGame{name}");

        #endregion



        protected virtual int GetPayLineIndex(int payLineNumber) => payLineNumber - 1;









        #region 遮罩层

        /// <summary> 关闭遮罩层 </summary>
        public void CloseSlotCover() => SetSlotCover(false);

        /// <summary> 打开遮罩层 </summary>
        public void OpenSlotCover() => SetSlotCover(true);

        public virtual void SetSlotCover(bool isShow)
        {
            if (goSlotCover != null)
                goSlotCover.SetActive(isShow);
        }

        #endregion




        #region 【新增方法】显示图标特效

        public List<SymbolBase> GetSymbol(List<int> symbolNumbers)
        {
            List<SymbolBase> symbols = new List<SymbolBase>();
            for (int r = bufferTop; r < row + bufferTop; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    ReelBase reel = reels[c];
                    SymbolBase symbol = reel.symbolList[r];
                    if (symbolNumbers.Contains(symbol.number))
                        symbols.Add(symbol);
                }
            }
            return symbols;
        }
        public List<SymbolBase> GetSymbol(SymbolWin symbolWin)
        {
            List<SymbolBase> symbols = new List<SymbolBase>();
            foreach (Cell cel in symbolWin.cells)
            {
                symbols.Add(GetVisibleSymbolFromDeck(cel.column, cel.row));
            }
            return symbols;
        }

        public List<SymbolBase> GetSymbol(List<Cell> cells)
        {
            List<SymbolBase> symbols = new List<SymbolBase>();
            foreach (Cell cel in cells)
            {
                symbols.Add(GetVisibleSymbolFromDeck(cel.column, cel.row));
            }
            return symbols;
        }

        public void ShowSymbolEffect(SymbolEffectType tp, List<int> symbolNumbers, bool isAmin, int symbolNumber, bool isUseMySelfSymbolNumber)
            => ShowSymbolEffect(tp, GetSymbol(symbolNumbers), isAmin, symbolNumber, isUseMySelfSymbolNumber);

        public void ShowSymbolEffect(SymbolEffectType tp, SymbolWin symbolWin, bool isAmin, int symbolNumber, bool isUseMySelfSymbolNumber)
            => ShowSymbolEffect(tp, GetSymbol(symbolWin), isAmin, symbolWin.symbolNumber, isUseMySelfSymbolNumber);

        public void ShowSymbolEffect(SymbolEffectType tp, List<Cell> cells, bool isAmin, int symbolNumber, bool isUseMySelfSymbolNumber)
            => ShowSymbolEffect(tp, GetSymbol(cells), isAmin, symbolNumber, isUseMySelfSymbolNumber);

        public virtual void ShowSymbolEffect(SymbolEffectType tp, List<SymbolBase> symbols, bool isAmin, int symbolNumber, bool isUseMySelfSymbolNumber)
        {

            foreach (SymbolBase symbol in symbols)
            {

                int symNumber = isUseMySelfSymbolNumber ? symbol.number : symbolNumber;

                if (tp == SymbolEffectType.Hit)
                {
                    string symbolName = customModel.symbolHitEffect[$"{symNumber}"];

                    GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                    symbol.AddSymbolEffect(goSymbolEffect, isAmin);

                }
                else if (tp == SymbolEffectType.Appear)
                {
                    string symbolName = customModel.symbolAppearEffect[$"{symNumber}"];

                    GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                    symbol.AddSymbolEffect(goSymbolEffect, isAmin);
                }
            }
        }

        #endregion







    }
}

