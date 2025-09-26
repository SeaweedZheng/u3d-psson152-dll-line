using Game;
using GameMaker;
using SlotMaker;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using _contentBB = PssOn00152.ContentBlackboard;

namespace PssOn00152
{

    /// <summary>
    /// 中奖弹窗
    /// </summary>
    /// <remarks>
    ///  新版本中奖弹窗（1080*1920）<br/>
    ///  从小将滚动到最终将<br/>
    /// </remarks>
    public partial class PopupBigWin01 : PageCorBase
    {
        public GameObject goTotalWin, goTips;

        public Animator atorBG;

        public I18nGameObject i18Icon;

        private Text txtTotalWin => goTotalWin.GetComponent<Text>();
        
        GameObject goBG;
        private Button btnBG => goBG.GetComponent<Button>();


        protected void Awake()
        {
            //goTotalWin = transform.Find("Anchor/Base/Anchor/Text").gameObject;

            goBG = transform.Find("Midground/BG").gameObject;
            btnBG.onClick.RemoveAllListeners();
            btnBG.onClick.AddListener(OnClickBG);
        }

        private void OnEnable()
        {
            AddNetButtonHandle();
        }
        private void OnDisable()
        {
            ClearAllCor();
            RemoveNetButtonHandle();
        }

        const string COR_MOVE_TOTAL_CREDIT = "COR_MOVE_TOTAL_CREDIT";
        const string COR_SHOW_TOTAL_CREDIT = "COR_SHOW_TOTAL_CREDIT";
        const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";
        double _toCredit;
        List<WinMultiple> lst;

        int iconIdx = 0;
        long totalBet = 0;

        int curJpIndex = -1;
        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            foreach (Transform tfm in goTips.transform)
            {
                tfm.gameObject.SetActive(false);
            }

            curJpIndex = -1;
            double fromCredit = 0;
            double toCredit = 10010.1f;
            totalBet = 50;

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                toCredit = (long)argDic["totalEarnCredit"];
                totalBet = (long)argDic["totalBet"];
            }

            lst = new List< WinMultiple >();
           
            foreach (WinMultiple item  in _contentBB.Instance.winMultipleList)
            {
                if (totalBet * item.multiple <= toCredit)
                {
                    int i = 0;
                    for (; i < lst.Count; i++)
                    {
                        if (lst[i].multiple > item.multiple)
                            break;
                    }
                    lst.Insert(i, new WinMultiple()
                    {
                        winLevelType = item.winLevelType,
                        multiple = item.multiple,
                    });
                }
            }

            fromCredit = (lst[0].multiple * totalBet) * 0.6;

            DoCor(COR_MOVE_TOTAL_CREDIT, MoveCredit(fromCredit, toCredit));


            GameSoundHelper.Instance.PlaySound(SoundKey.PopupWinBG);
            //GameSoundHelper.Instance.PlayMusicSingle(SoundKey.PopupWinBG);

            int indx = Random.Range(1, 3);
            GameSoundHelper.Instance.PlaySound(indx == 1 ? SoundKey.PopupWinCongratulate01 : SoundKey.PopupWinCongratulate02);
        }

        void ClosePage()
        {
            GameSoundHelper.Instance.StopSound(SoundKey.PopupWinBG);
            //GameSoundHelper.Instance.StopMusic();
            PageManager.Instance.ClosePage(this, new EventData<string>("PopupClose", ""));
        }
        IEnumerator AutoClose(float timeS = 15f)
        {
            yield return new WaitForSeconds(timeS);
            ClosePage();
        }


        void SetIcon(int index)
        {
            if (curJpIndex == index)
                return;
            curJpIndex = index;

            string skinName = "";

            WinLevelType tp = lst[index].winLevelType;

            switch (tp)
            {
                case WinLevelType.Big:
                    skinName = "bigwin";
                    atorBG.Play("Big Win Begin");
                    GameSoundHelper.Instance.PlaySound(SoundKey.BigWin);
                    break;
                case WinLevelType.Mega:
                    skinName = "megawin";
                    atorBG.Play("Mega Win Begin");
                    GameSoundHelper.Instance.PlaySound(SoundKey.MegaWin);
                    break;
                case WinLevelType.Super:
                    skinName = "superwin";
                    atorBG.Play("Super Win Begin");
                    GameSoundHelper.Instance.PlaySound(SoundKey.SuperWin);
                    break;
                case WinLevelType.Ultra:
                    skinName = "ultrawin";
                    atorBG.Play("Super Win Begin");
                    GameSoundHelper.Instance.PlaySound(SoundKey.SuperWin);
                    break; 
                case WinLevelType.Ultimate:
                    skinName = "ultimatewin";
                    atorBG.Play("Super Win Begin");
                    GameSoundHelper.Instance.PlaySound(SoundKey.SuperWin);
                    break;
            }
            SkeletonMecanim sm = i18Icon.goNow.GetComponent<SkeletonMecanim>();
            sm.initialSkinName = skinName;
            sm.Initialize(true); //重新初始化

            i18Icon.goNow.GetComponent<Animator>().Play("Begin");


            foreach (Transform tfm in goTips.transform)
                tfm.gameObject.SetActive(false);
            
            if (tp != WinLevelType.Big)
            {
                int idx = UnityEngine.Random.Range(0, goTips.transform.childCount);
                goTips.transform.GetChild(idx).gameObject.SetActive(true);
            }
            StartCoroutine(TipScale());
        }

        IEnumerator TipScale()
        {
            goTotalWin.SetActive(false);

            float scale = 0.5f;
            goTips.transform.localScale = new Vector3(scale, scale, 1);
            while (scale < 1)
            {
                scale += 0.05f;
                goTips.transform.localScale = new Vector3(scale, scale, 1);
                yield return new WaitForSeconds(0.02f);
            }

            goTotalWin.SetActive(true);
        }

        IEnumerator MoveCredit(double fromCredit, double toCredit)
        {

            int iconIdexOld = 0;
            iconIdx = 0;
            SetIcon(iconIdx);
    

            _toCredit = toCredit;



            goTotalWin.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            goTotalWin.SetActive(true);

            float scale = 1f;
            goTotalWin.transform.localScale = new Vector3(scale, scale, 1);

            double credit = fromCredit;

            float startTimeS = Time.unscaledTime;
            do
            {
                credit += 12;

                if (iconIdx + 1 < lst.Count && credit >= lst[iconIdx + 1].multiple * totalBet)
                {
                    iconIdx++;
                    if (iconIdx != iconIdexOld)
                    {
                        iconIdexOld = iconIdx;
                        SetIcon(iconIdx);
                    }
                }


                if (credit > toCredit)
                    credit = toCredit;

                txtTotalWin.text = ((long)credit).ToString(); // 不要千分号   credit.ToString("N0");
                                                              //Debug.Log("-@2-" + txtTotalWin.text);

                //Debug.Log($"-@1- {((long)credit)} false");
                EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
                new EventData<PrepareTotalWinCredit02>(
                    SlotMachineEvent.PrepareTotalWinCredit02,
                    new PrepareTotalWinCredit02()
                    {
                        isEndToCredit = false,
                        credit = ((long)credit),
                    })
                );

                //yield return new WaitForSeconds(0.08f);
                yield return new WaitForSeconds(0.01f);

            } while (credit < toCredit && Time.unscaledTime - startTimeS < 4f); // 4秒内必须加完，加不完直接跳到最终结果。

            
            txtTotalWin.text = ((long)toCredit).ToString();
            SetIcon(lst.Count-1);
            


            //Debug.Log($"-@2- {((long)credit)} true");
            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
            new EventData<PrepareTotalWinCredit02>(
                SlotMachineEvent.PrepareTotalWinCredit02,
                new PrepareTotalWinCredit02()
                {
                    isEndToCredit = true,
                    credit = ((long)toCredit),
                })
            );
            


            while (scale < 1.5)
            {
                scale += 0.05f;
                goTotalWin.transform.localScale = new Vector3(scale, scale, 1);
                yield return new WaitForSeconds(0.02f);
            }
            scale = 1.5f;
            while (scale > 1)
            {
                scale -= 0.05f;
                goTotalWin.transform.localScale = new Vector3(scale, scale, 1);
                yield return new WaitForSeconds(0.02f);
            }
            scale = 1f;
            goTotalWin.transform.localScale = new Vector3(scale, scale, 1);

            yield return new WaitForSeconds(1f);

            i18Icon.goNow.GetComponent<Animator>().Play("Result");


            GameSoundHelper.Instance.StopSound(SoundKey.PopupWinBG);
            GameSoundHelper.Instance.PlaySound(SoundKey.PopupWinBeforeClose);

            DoCor(COR_AUTO_CLOSE, AutoClose(4));
        }


        IEnumerator ShowCredit(double credit)
        {
            ClearCor(COR_MOVE_TOTAL_CREDIT);
            goTotalWin.transform.localScale = new Vector3(1, 1, 1);
            txtTotalWin.text = ((long)credit).ToString(); // 不要千分号   credit.ToString("N0");

            //Debug.Log($"-@3- {((long)credit)} true");
            EventCenter.Instance.EventTrigger<EventData>(SlotMachineEvent.ON_WIN_EVENT,
            new EventData<PrepareTotalWinCredit02>(
                SlotMachineEvent.PrepareTotalWinCredit02,
                new PrepareTotalWinCredit02()
                {
                    isEndToCredit = true,
                    credit = ((long)credit),
                })
            );



            GameSoundHelper.Instance.StopSound(SoundKey.PopupWinBG);

            SetIcon(lst.Count - 1);

            //goTotalWin.SetActive(false);
            //yield return new WaitForSeconds(0.8f);
            //goTotalWin.SetActive(true);

            yield return new WaitForSeconds(1.8f);

            // 显示结果特效：变大
            i18Icon.goNow.GetComponent<Animator>().Play("Result");
            GameSoundHelper.Instance.PlaySound(SoundKey.PopupWinBeforeClose);

            DoCor(COR_AUTO_CLOSE, AutoClose(4.5f));
        }


        #region 机台按钮事件
        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:
                        if (info.isUp)
                            OnClickBG();
                        break;
                }
            }
        }
        #endregion


        void OnClickBG()
        {
            if (IsCor(COR_MOVE_TOTAL_CREDIT))
            {
                StartCoroutine(ShowCredit(_toCredit));
            }
            else
            {
                ClosePage();
                //PageManager.Instance.ClosePage(this);
            }
        }









        #region 网络远程按钮
        const string MARK_NET_BTN_POP_BIG_WIN = "MARK_NET_BTN_POP_BIG_WIN";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_POP_BIG_WIN,
                onClick = OnNetBtnSpin,
            });

        }

        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_POP_BIG_WIN);


        void OnNetBtnSpin(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(this) != 0) return;

            OnClickBG();

            info.onCallback?.Invoke(true);
        }


        #endregion


    }


}
