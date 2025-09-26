using UnityEngine;
using _customBB = PssOn00152.CustomBlackboard;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;
using System;

namespace PssOn00152
{
    public class Symbol : SymbolBase
    {

        protected override void Awake()
        {
            base.Awake();
            //imgIcon = gameObject.GetComponent<Image>();
        }
        // Start is called before the first frame update
        void Start()
        {
            //btn = gameObject.GetComponent<Button>();
            //btnBase.onClick.AddListener(OnClickSymbol);
        }


        #region 读取图标

        Sprite LoadIcon(int indexSymbol, int symbolNumber)
        {
            try
            {
                if(indexSymbol > _customBB.Instance.symbolIcon.Length)
                {
                    DebugUtils.LogError($"@2 找不到此ICON NAME = index({indexSymbol})");
                    return null;
                }
                Sprite spr = _customBB.Instance.symbolIcon[indexSymbol];
                if (spr != null)
                    return spr;

                DebugUtils.LogError($"@3 找不到此ICON NAME = index({indexSymbol})");
            }
            catch (Exception e)
            {
                DebugUtils.LogError($"@4 找不到此ICON NAME = index({indexSymbol})  -- {symbolNumber}");
            }

            return null;
        }
        #endregion


        public override void SetSymbolImage(int symbolNumber, bool needNativeSize = false)
        {
            this.number = symbolNumber;
            int symbolIndex =  _customBB.Instance.symbolNumber.IndexOf(symbolNumber);
            Sprite spr = LoadIcon(symbolIndex, symbolNumber);
            SetSymbolSprite(spr);
        }

        public override int GetSymbolNumber()
        {
            return number;
        }

        public int GetNumber()
        {
            return _customBB.Instance.symbolNumber[number]; // == return index;
        }

        public override void SetBtnInteractableState(bool state)
        {
            return;
            btnBase.interactable = state;
        }


        /*
        public void SetImageActive(bool active)
        {
            imgIcon.enabled = active;
        }*/
        //点击ICON展示动画

        private void OnClickSymbol()
        {
            DebugUtils.Log("i am here 123456");
            /*
            if (aniGameObj != null)
            {
                GameObject.Destroy(aniGameObj);
                aniGameObj = null;
            }
            aniGameObj = __ResMgr.Instance.Load<GameObject>("Games/Icon" + index);
            aniGameObj.transform.SetParent(transform);
            aniGameObj.transform.localPosition = new Vector3(0, 0, 0);
            aniGameObj.transform.localScale = new Vector3(1, 1, 1);
            SetSymbolActive(false);
            */
        }


        /// <summary>
        /// 是否是特殊图标
        /// </summary>
        /// <returns></returns>
        public override bool IsSpecailHitSymbol()
        {
            return _customBB.Instance.specialHitSymbols.Contains(number);
            //return index <= 6; //__ConstData.Instance.IsSpecailIcon(index);
        }

        void OnDestroy()
        {
            //EventCenter.Instance.RemoveEventListener<float>(EventHandle.PLAY_AMOUNT_CHANGE, PlayAmountChangeOnEvent);
        }

        /// <summary>
        /// 添加边款特效
        /// </summary>
        /// <param name="borderEffect"></param>
        /// <returns></returns>
        public override Transform AddBorderEffect(GameObject borderEffect)
        {
            Transform tfmBorder = base.AddBorderEffect(borderEffect);

            if (IsSpecailHitSymbol()) 
            {
                tfmBorder.Find("Animator").GetComponent<Animator>().Play("Flow");
            }

            return tfmBorder;
            // 播放动画
        }



        
        public override void ShowBiggerEffect()
        {
            transform.Find("Animator").GetComponent<Animator>().Play("Scale");
        }

    }
}