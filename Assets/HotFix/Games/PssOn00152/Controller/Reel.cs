using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
//using __ConstData = PssOn00152.ConstData;
using _customBB = PssOn00152.CustomBlackboard;
using _reelSetBB = SlotMaker.ReelSettingBlackboard;



namespace PssOn00152
{

    public class Reel : ReelBase
    {

        /// <summary> 已经滚动圈数 </summary>
        private int curRollTime = 0;
        /// <summary> 需要滚动圈数 </summary>
        private int needRollTime = 0;
        /// <summary> 滚轮停止回调 </summary>
        private UnityAction reelStopCallback = null;
        /// <summary> 滚轮显示结果 </summary>
        private List<int> columnResult = new List<int>() { 0, 0, 0 };


        Transform tfmSymbols;
        //Button btnReel;




        const string COR_REEL_TURN = "COR_REEL_TURN";
        const string COR_REEL_TO_STOP = "COR_REEL_TO_STOP";


        /// <summary> 
        /// 被点中中 
        /// </summary>
        /// <remark> 
        /// * 被点中时只能跑一圈.
        /// </remark>
        bool isReelPointering = false;





        /**【decket】
         * cache row (first)
         * deck up rows []
         * deck down rows []
         * cache row (end)
         */
        int deckUpStartIndex => 1;
        int deckUpEndIndex => _customBB.Instance.row;
        int deckDownStartIndex => _customBB.Instance.row + 1;
        int deckDownEndIndex => _customBB.Instance.row + _customBB.Instance.row;




        void Start()
        {

            tfmSymbols = transform.Find("Symbols");
            for (int i = 0; i < tfmSymbols.childCount; i++)
            {
                symbolList.Add(tfmSymbols.GetChild(i).GetComponent<Symbol>());
            }
            for (int i = 0; i < tfmSymbols.childCount; i++)
            {

                int symbolIndex = Random.Range(0, _customBB.Instance.symbolCount);
                symbolList[i].SetSymbolImage(_customBB.Instance.symbolNumber[symbolIndex]);
            }

            tfmSymbols.localPosition = new Vector3(tfmSymbols.localPosition.x, 0, tfmSymbols.localPosition.z);

            /*
            for (int i = 1; i < tfmSymbols.childCount-1; i++)
            {
                ItemList[i].btnBase.onClick.RemoveAllListeners();
                ItemList[i].btnBase.onClick.AddListener(OnSymbolClick);
            }*/

            /*
            btnReel = transform.Find("Button").GetComponent<Button>();
            btnReel.onClick.RemoveAllListeners();
            btnReel.onClick.AddListener(OnSymbolClick);
            */

            //EventCenter.Instance.AddEventListener<EventData>(TouchTrigger.ON_TRIGGER_EVENT, OnTouchReel);

            TouchTrigger touch = transform.Find("Touch").GetComponent<TouchTrigger>();
            touch.onPointerEnter.AddListener(OnSymbolPointerEnter);
            touch.onPointerExit.AddListener(OnSymbolPointerExit);

            isReelPointering = false;
        }



        public override void StartTurn(int targetRollTime, UnityAction reelStopCallback)
        {
            this.needRollTime = isReelPointering? 1: targetRollTime;
            this.curRollTime = 0;
            this.reelStopCallback = reelStopCallback;

            ClearReelTween();
            DoCor(COR_REEL_TURN, _ReelTurn());
        }



        /// <summary> 修改滚轮图标 </summary>
        private void ResetIconData()
        {
            for (int i = deckDownStartIndex; i <= deckDownEndIndex; i++) //for (int i = 4; i <= 6; i++)
            {
                int symbolNumber = symbolList[i - _customBB.Instance.row].GetSymbolNumber(); // int index = ItemList[i - 3].GetIndex();
                symbolList[i].SetSymbolImage(symbolNumber);
                symbolList[i].SetBtnInteractableState(true);
            }
            tfmSymbols.localPosition = new Vector3(tfmSymbols.localPosition.x, _customBB.Instance.reelMaxOffsetY, tfmSymbols.localPosition.z);

            for (int i = deckUpStartIndex; i <= deckUpEndIndex; i++)  //for (int i = 1; i <= 3; i++)
            {
                int symbolNumber = _customBB.Instance.symbolNumber[Random.Range(0, _customBB.Instance.symbolCount)];
                symbolList[i].SetSymbolImage(symbolNumber);
                symbolList[i].SetBtnInteractableState(true);
            }
        }


        public override void SetReelDeck(string reelValue = null) //reelValue = "1,2,3"
        {
            for (int i = deckDownStartIndex; i <= deckDownEndIndex; i++)  //for (int i = 4; i <= 6; i++)
            {
                int symbolNumber = symbolList[i - _customBB.Instance.row].GetSymbolNumber();
                symbolList[i].SetSymbolImage(symbolNumber);
                symbolList[i].SetBtnInteractableState(true);
            }
            //这里开始设置结果
            SetRollEndResult();
            tfmSymbols.localPosition = new Vector3(tfmSymbols.localPosition.x, 0, tfmSymbols.localPosition.z);
        }


        IEnumerator _ReelTurn(bool isOnce = false)
        {
            if (needRollTime == 0)
                yield break;

            bool isNext;
            state = ReelState.StartTurn;

            if (_reelSetBB.Instance.GetTimeReboundStart(reelIndex) > 0)
            {
                yield return Rebound(
                    _reelSetBB.Instance.GetOffsetYReboundStart(reelIndex),
                    _reelSetBB.Instance.GetTimeReboundStart(reelIndex)
                );
            }

            while (curRollTime < needRollTime)//0, 1
            {
                ResetIconData();
                if (curRollTime == needRollTime - 1)
                {
                    state = ReelState.StartStop;
                    //这里开始设置结果
                    SetRollEndResult();
                }

                DOTweenUtil.Instance.DOLocalMoveY(tfmSymbols, 0, _reelSetBB.Instance.GetTimeTurnOnce(reelIndex), Ease.Linear, () => { isNext = true; });

                /*
                corReel = AsyncActionUtils.ApplyLocalMovement(
                    this,
                    tfmSymbols,
                    tfmSymbols.localPosition,
                    new Vector3(tfmSymbols.localPosition.x, 0, 0),
                    __ConstData.Instance.GetTimeTurnOnce(reelIndex),
                    TweenUtils.VectorTweenLinear,
                    0,
                    () => { isNext = true; }
                );*/


                isNext = false;
                yield return new WaitUntil(() => isNext);

                if (++curRollTime >= needRollTime)
                {
                    break;
                }
            }

            if (_reelSetBB.Instance.GetTimeReboundEnd(reelIndex) > 0)
            {

                yield return Rebound(
                    _reelSetBB.Instance.GetOffsetYReboundEnd(reelIndex),
                    _reelSetBB.Instance.GetTimeReboundEnd(reelIndex)
                );
            }

            state = ReelState.EndStop;

            reelStopCallback?.Invoke();

        }


        public override void SetReelState(ReelState state = ReelState.Idle)
        {
            this.state = state;
        }

        /// <summary>
        /// 鼠标或手指，点击
        /// </summary>
        void OnSymbolPointerEnter()
        {
            //DebugUtil.Log($"i am reel({reelIndex})");
            isReelPointering = true;
            ReelToStop();
        }
        /// <summary>
        /// 鼠标或手指，不点击
        /// </summary>
        void OnSymbolPointerExit()
        {
            isReelPointering = false;
        }

        /// <summary>
        /// 停止滚动
        /// </summary>
        void ReelToStop()
        {
            if (state == ReelState.StartTurn)
            {
                state = ReelState.StartStop;

                ClearReelTween();
                ClearCor(COR_REEL_TURN);
                DoCor(COR_REEL_TO_STOP, _ReelToStop());
            }
        }


        void ClearReelTween()
        {
            ClearDoTween();
            ClearCorReel();
        }
        void ClearDoTween()
        {
            tfmSymbols.DOKill();
        }
        void ClearCorReel()
        {
            if(corReel != null)
                StopCoroutine(corReel);
            corReel = null;
        }
        Coroutine corReel = null;

        IEnumerator _ReelToStop()
        {
            bool isNext = false;
            state = ReelState.StartStop;
            SetRollEndResult();

            //移动到0位置
            //1.本身就在0位置
            //2.运动中，且已经移动几个格子
            //3.滚动中、正开始滚动、正在停止滚动

            DOTweenUtil.Instance.DOLocalMoveY(tfmSymbols, 0, _reelSetBB.Instance.GetTimeTurnOnce(reelIndex), Ease.Linear, () => { isNext = true; });

            /*corReel = AsyncActionUtils.ApplyLocalMovement(
                this, 
                tfmSymbols, 
                tfmSymbols.localPosition,
                new Vector3(tfmSymbols.localPosition.x,0,0), 
                __ConstData.Instance.GetTimeTurnOnce(reelIndex),
                TweenUtils.VectorTweenLinear,
                0,
                () => { isNext = true; });*/

            isNext = false;
            yield return new WaitUntil(() => isNext);


            if (_reelSetBB.Instance.GetTimeReboundEnd(reelIndex) > 0)
            {
                yield return Rebound(
                    _reelSetBB.Instance.GetOffsetYReboundEnd(reelIndex),
                    _reelSetBB.Instance.GetTimeReboundEnd(reelIndex)
                );
            }

            state = ReelState.EndStop;

            reelStopCallback?.Invoke();
 
        }

        [Button]
        void TestShowReelPos()
        {
            DebugUtils.Log($"tfmSymbols locPos.y = {tfmSymbols.localPosition.y}");
            DebugUtils.Log($"tfmSymbols Pos.y = {tfmSymbols.position.y}");
        }





        /// <summary>
        /// 滚轮滚动至少一次
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>
        /// * 如果滚轮还没滚动，则正常滚动一次。<br/>
        /// * 如果滚轮已经在滚动，则里面停止。<br/>
        /// * 当滚轮已经停止，则直接退出，且不调用回调函数<br/>
        /// </remarks>
        public override void ReelToStopOrTurnOnce(UnityAction action = null)
        {
            if (this.reelStopCallback != null)
                this.reelStopCallback = action;

            if (state == ReelState.StartStop) //开始停止
                return;

            if (state == ReelState.EndStop) //已经停止
            {
                //this.reelStopCallback?.Invoke();
                return;
            }

            if (state == ReelState.Idle) 
            {
                StartTurn(1, action);
            }
            else if (state == ReelState.StartTurn)
            {
                ReelToStop();
            }
        }



    /// <summary>
    /// 回弹效果
    /// </summary>
    /// <param name="yTo"></param>
    /// <param name="durationS"></param>
    /// <returns></returns>
    public IEnumerator Rebound(float yTo = -80, float durationS = 0.05f)
        {
            /*bool isNext = false;
            corReel = AsyncActionUtils.ApplyLocalMovement(
                this,
                tfmSymbols,
                tfmSymbols.localPosition,
                new Vector3(tfmSymbols.localPosition.x, yTo, 0),
                __ConstData.Instance.GetTimeTurnOnce(reelIndex),
                TweenUtils.VectorTweenLinear,
                0,
                () => { isNext = true; }
            );
            yield return new WaitUntil(()=>isNext==true);
            isNext = false;

            corReel = AsyncActionUtils.ApplyLocalMovement(
                this,
                tfmSymbols,
                tfmSymbols.localPosition,
                new Vector3(tfmSymbols.localPosition.x, 0, 0),
                __ConstData.Instance.GetTimeTurnOnce(reelIndex),
                TweenUtils.VectorTweenLinear,
                0,
                () => { isNext = true; }
            );
            yield return new WaitUntil(() => isNext == true);
            isNext = false;
            */

  
            bool isNext = false;
            DOTweenUtil.Instance.DOLocalMoveY(tfmSymbols, yTo, durationS, Ease.Linear, () =>
            {
                DOTweenUtil.Instance.DOLocalMoveY(tfmSymbols, 0, durationS, Ease.Linear, () =>
                {
                    isNext = true;
                });
            });
            yield return new WaitUntil(()=>isNext==true);
            isNext = false;        
        }



        /// <summary> 特殊 Symbol Effect </summary>
        public override void SymbolAppearEffect()
        {

            for (int i = deckUpStartIndex; i <= deckUpEndIndex; i++)  //for (int i = 1; i <= 3; i++)
            {

                SymbolBase symble = symbolList[i];

                string symbolNumber = symble.number.ToString();

                // Keys不能用！！
                /**/
                bool isHashSymbolNumber = false;
                foreach (KeyValuePair<string, string> kv in _customBB.Instance.symbolAppearEffect)
                {
                    if (kv.Key == symbolNumber)
                    {
                        isHashSymbolNumber = true;
                        break;
                    }
                }

                //if (symbolIndex == 0 || symbolIndex == 1)
                //if (_customBB.Instance.specialSymbolEffect.Keys.Contains(symbolIndex))  // Key 、 Value不能用！！
                //if (_customBB.Instance.specialSymbolEffect.ContainsKey(symbolIndex))
                if(isHashSymbolNumber)
                {
                    /*
                    string symbolName = _customBB.Instance.hitSymbolEffect[symbolIndex];
                    GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                    goSymbolEffect.GetComponent<AnimBaseUI>().Play("Idle", true);
                    symble.AddSymbolEffect(goSymbolEffect);
                    */

                    string symbolName = _customBB.Instance.symbolAppearEffect[symbolNumber.ToString()];
                    GameObject goSymbolEffect = ContentPoolManager.Instance.GetObject(symbolName);
                    symble.AddSymbolEffect(goSymbolEffect);
                }
            }
        }


        /// <summary>
        /// 设置停止图标
        /// </summary>
        private void SetRollEndResult()
        {
            for (int i = deckUpStartIndex; i <= deckUpEndIndex; i++)  //for (int i = 1; i <= 3; i++)
            {
                symbolList[i].SetSymbolImage(columnResult[i - 1]);
            }
            //DebugUtil.Log("设置结果");
        }

        /*
        //获取这一列第N个结果
        public int GetColumItemIndex(int dex)
        {
            return ItemList[dex].GetIndex();
        }*/

        /// <summary>
        /// 游戏开始的时候设定最终显示的结果
        /// </summary>
        /// <param name="result"></param>
        public override void SetResult(List<int> result)
        {
            columnResult = result;
        }
    }
}