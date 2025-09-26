using GameMaker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMaker;
using SoundKey = GameMaker.SoundKey;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using UnityEngine.EventSystems;
using Game;
using System;


namespace Panel004
{
    public class PanelController : CorBehaviour // MonoWeakSingleton<PanelManager>
    {
        PopState popState = PopState.None;
        enum PopState
        {
            None,
            Change,
            Help,
            Bet,
            PayTable,
        }



        public GameObject goPops, goPopChange, goPopHelp, goPopPayTable;// goPopBet,
        public GameObject goLeftPanel, goMiddlePanel, goRightPanel,
            goBetPanel, goPagePanel, goPageSelectPanel, goPageIndexPanel, goHelpPanel, goHelpSettingPanel;

        CanvasGroup cgRightPanel, cgLeftPanel, cgPagePanel;
        StateButton btnSound, btnSpin, btnPageBack, btnPageNext;
        GameObject goPopBG;
        Button btnHelp, btnBetUp, btnBetDown, bntPageExit, btnHelpExit; //btnChange, 
        Button btnPayTable, btnRedeem;

        NoticeUI uiMyCredit, uiWin, uiTotalBet;  //ctrlTotalBet

        // Start is called before the first frame update


        MessageDelegates onPropertyChangedEventDelegates;


        MessageDelegates onWinEventDelegates;

        MessageDelegates onMetaUIDelegates;

        SoundHelper soundHelper;


        void Awake()
        {
            //goBetPanel = transform.Find("Bottom/Bet Panel").gameObject;
            //goPagePanel = transform.Find("Bottom/Page Panel").gameObject;
            //goCreditPanel = transform.Find("Bottom/Credit Panel").gameObject;

            cgRightPanel = goRightPanel.GetComponent<CanvasGroup>();
            cgLeftPanel = goLeftPanel.GetComponent<CanvasGroup>();
            cgPagePanel = goPagePanel.GetComponent<CanvasGroup>();

            goPopBG = goPops.transform.Find("BG").gameObject;

            //btnChange = transform.Find("Bottom/Anchor/Button Change").GetComponent<Button>();
            btnHelp = goLeftPanel.transform.Find("Anchor/Button Help").GetComponent<Button>();
            btnBetUp = goBetPanel.transform.Find("Anchor/Button Up").GetComponent<Button>();
            btnBetDown = goBetPanel.transform.Find("Anchor/Button Down").GetComponent<Button>();

            //btnChange.onClick.AddListener(OnClickButtonChange);
            btnHelp.onClick.AddListener(OnClickButtonHelp);
            btnBetUp.onClick.AddListener(OnClickButtonBetUp);
            btnBetDown.onClick.AddListener(OnClickButtonBetDown);



            uiMyCredit = goRightPanel.transform.Find("Anchor/My Credit").GetComponent<NoticeUI>();
            uiWin = goMiddlePanel.transform.Find("Anchor/Win").GetComponent<NoticeUI>();
            uiTotalBet = goBetPanel.transform.Find("Anchor/Total Bet").GetComponent<NoticeUI>();
            btnSpin = goRightPanel.transform.Find("Anchor/Spin/Anchor/Spin Button").GetComponent<StateButton>();

            btnSpin.onLongClick.AddListener(OnLongClickSpinButton);
            btnSpin.onShortClick.AddListener(OnShortClickSpinButton);


            bntPageExit = goPagePanel.transform.Find("Anchor/Button Exit").GetComponent<Button>();
            btnHelpExit = goHelpPanel.transform.Find("Anchor/Button Exit").GetComponent<Button>();

            btnPageBack = goPageSelectPanel.transform.Find("Anchor/Button Back").GetComponent<StateButton>();
            btnPageNext = goPageSelectPanel.transform.Find("Anchor/Button Next").GetComponent<StateButton>();

            bntPageExit.onClick.AddListener(OnClickButtonPageExit);
            btnHelpExit.onClick.AddListener(OnClickButtonHelpExit);

            btnPageBack.onClickDown.AddListener(OnClickButtonPageBack);
            btnPageNext.onClickDown.AddListener(OnClickButtonPageNext);

            btnSound = goHelpSettingPanel.transform.Find("Anchor/Button Sound").GetComponent<StateButton>();
            btnSound.onClickDown.RemoveAllListeners();
            btnSound.onClickUp.RemoveAllListeners();
            btnSound.onClickUp.AddListener(OnClickButtonSound);

            btnPayTable = goHelpSettingPanel.transform.Find("Anchor/Button Pay Table").GetComponent<Button>();
            btnPayTable.onClick.AddListener(OnClickButtonPayTable);



            btnRedeem = goHelpSettingPanel.transform.Find("Anchor/Button Redeem").GetComponent<Button>();
            btnRedeem.onClick.AddListener(OnClickButtonRedeem);



            soundHelper = new SoundHelper();


            onPropertyChangedEventDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                    { "./totalBet", OnPropertyChangeTotalBet },
                    { "@console/betList",  OnPropertyChangeBetList },
                    { "./btnSpinState", OnPropertyChangeBtnSpinState},
                    { "./gameState",OnPropertyGameState },
                    { "@console/isConnectMoneyBox",OnPropertyIsConnectMoneyBox}
                    //{ "./isMaxBetCtedit", OnPropertyBtnSpinState},
                   // { "./isMinBetCtedit",OnPropertyGameState },
                }
            );

            onWinEventDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                    { SlotMachineEvent.TotalWinCredit, OnTotalWinCredit },
                }
            );

            onMetaUIDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                    { MetaUIEvent.UpdateNaviCredit, OnUpdateNaviCredit },
                }
            );


            //DebugUtil.Log($" @@@ root name =  {transform.root.name} "); //Page Game Main(Clone)
            transform.root.GetComponent<MachineButtonEventDispatcher>().machineButtonEventHanler.AddListener(OnClickMachineButton);

        }

        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            //EventCenter.Instance.AddEventListener<CreditEventData>(PanelEvent.ON_CREDIT_CHANGE_EVENT, OnCreditEvent);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(MetaUIEvent.ON_CREDIT_EVENT, onMetaUIDelegates.Delegate);

            AddNetButtonHandle();

            InitParam();
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            //EventCenter.Instance.RemoveEventListener<CreditEventData>(PanelEvent.ON_CREDIT_CHANGE_EVENT, OnCreditEvent);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(MetaUIEvent.ON_CREDIT_EVENT, onMetaUIDelegates.Delegate);

            RemoveNetButtonHandle();
        }


        void InitParam()
        {
            uiWin.SetToCredit(0);

            OnPropertyChangeBetList();
            OnPropertyChangeTotalBet();
            OnPropertyChangeBtnSpinState();
            OnPropertyChangeBetIndex();
            OnUpdateNaviCredit();
            OnPropertyIsConnectMoneyBox();
            //public List<GameObject> pages = new List<GameObject>();
            //goPopPayTable.Find("Anchor")
        }


        bool isClickDown = false;
        public void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                if (!info.isUp)
                {
                    isClickDown = true;
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            btnSpin.OnPointerDown(null);
                            break;
                            //case MachineButtonKey.BtnHelp: 帮助按钮
                    }
                }
                else if (isClickDown)
                {
                    isClickDown = false;
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            btnSpin.OnPointerUp(null);
                            break;
                            //case MachineButtonKey.BtnHelp: 帮助按钮
                    }
                }
            }
        }

        #region 监听值变化

        void OnPropertyChangeTotalBet(EventData receivedEvent = null)
        {
            long totalBet = receivedEvent != null ? (long)receivedEvent.value : -1;
            if (totalBet == -1)
            {
                List<long> betList = _consoleBB.Instance.betList;
                int betIndex = BlackboardUtils.FindVariable<int>("./betIndex").value;
                totalBet = betList[betIndex];
            }

            DebugUtils.Log($"OnPropertyChangeTotalBet ：{totalBet}");
            uiTotalBet.SetToCredit(totalBet);

            SetBtnDownUpInteractable();
        }

        void SetBtnDownUpInteractable()
        {
            List<long> betList = betList = _consoleBB.Instance.betList;
            int betIndex = BlackboardUtils.GetValue<int>("./betIndex");
            ChangeBetButtonInteractable(betIndex, betList.Count);
        }

        void OnPropertyChangeBetList(EventData receivedEvent = null)
        {
            List<long> betList = (List<long>)receivedEvent?.value;

            if (betList == null)
                betList = _consoleBB.Instance.betList;


            DebugUtils.LogWarning($"@[Check] 22 console/betList = {JSONNodeUtil.ObjectToJsonStr(betList)} ");


            int betIndex = BlackboardUtils.GetValue<int>("./betIndex");

            DebugUtils.LogWarning($"@[Check] i am OnPropertyChangeBetList");
            try
            {
                if (betIndex >= betList.Count)
                {
                    betIndex = 0;
                    BlackboardUtils.SetValue<int>("./betIndex", betIndex);
                }

                BlackboardUtils.SetValue<long>("./totalBet", betList[betIndex]);
            }
            catch (System.Exception e)
            {
                DebugUtils.LogWarning($"@[Check] betIndex = {betIndex} betList = {JSONNodeUtil.ObjectToJsonStr(betList)}");
                DebugUtils.LogException(e);
            }

            ChangeBetButtonInteractable(betIndex, betList.Count);
        }


        void OnPropertyChangeBetIndex(EventData receivedEvent = null)
        {
            int betIndex = receivedEvent != null ? (int)receivedEvent.value : -1;
            if (betIndex == -1)
                betIndex = BlackboardUtils.GetValue<int>("./betIndex");

            List<long> betList = _consoleBB.Instance.betList;

            ChangeBetButtonInteractable(betIndex, betList.Count);
        }


        void OnPropertyChangeBtnSpinState(EventData receivedEvent = null)
        {
            string changeSpinState = (string)receivedEvent?.value;

            if (changeSpinState == null)
                changeSpinState = BlackboardUtils.GetValue<string>("./btnSpinState");

            switch (changeSpinState)
            {
                case "Stop":
                    {
                        btnSpin.SetState("Stop");
                        //ChangeBetButtonInteractable();
                        cgLeftPanel.interactable = true;                    
                    }
                    break;
                case "Spin":
                    {
                        btnSpin.SetState("Spin");
                        if (popState != PopState.None)
                            CloseAllPop();
                        cgLeftPanel.interactable = false;
                    }
                    break;
                case "Auto":
                    {
                        btnSpin.SetState("Auto");
                        if(popState != PopState.None)
                            CloseAllPop();
                        cgLeftPanel.interactable = false;
                    }
                    break;
            }
        }


        void OnPropertyGameState(EventData receivedEvent = null)
        {
            string gameState = (string)receivedEvent?.value;
            if (gameState == GameState.Spin || gameState == GameState.FreeSpin)
            {
                uiWin.SetToCredit(0);
            }
        }



        void OnPropertyIsConnectMoneyBox(EventData receivedEvent = null)
        {
            bool isConnectMoneyBox = receivedEvent == null? _consoleBB.Instance.isConnectMoneyBox
                : (bool)receivedEvent?.value;

            btnRedeem.gameObject.SetActive(isConnectMoneyBox);
        }
        #endregion



        #region 事件
        /*void OnCreditEvent(CreditEventData receivedEvent = null)
        {
            if (receivedEvent.isAnim) {

                long nowCredit = receivedEvent.nowCredit >= 0 ? receivedEvent.nowCredit : uiMyCredit.nowCredit;
                uiMyCredit.MoveToCredit(nowCredit, receivedEvent.toCredit);
            }
            else
            {
                uiMyCredit.SetToCredit(receivedEvent.toCredit);
            }
        }*/

        void OnTotalWinCredit(EventData receivedEvent)
        {
            long totalWinCredit = (long)receivedEvent.value;
            uiWin.SetToCredit(totalWinCredit);
        }

        void OnUpdateNaviCredit(EventData receivedEvent = null)
        {

            bool isAmin = false;
            long fromCredit = 0;
            long toCredit = 0;
            if (receivedEvent == null || receivedEvent.value == null)
            {
                isAmin = false;
                toCredit = MainBlackboardController.Instance.myTempCredit;
            }
            else
            {
                UpdateNaviCredit data = (UpdateNaviCredit)receivedEvent.value;

                isAmin = data.isAnim;
                fromCredit = data.fromCredit;
                toCredit = data.toCredit;
            }


            if (!isAmin)
            {
                uiMyCredit.SetToCredit(toCredit);
            }
            else
            {
                //uiMyCredit.MoveToCredit(myCredit);
                uiMyCredit.MoveToCredit(fromCredit, toCredit);
            }
        }
        #endregion



        #region 按钮点击


        /*void OnClickButtonChange()
        {
            bool isOpen = ChangePop(PopState.Change, goPopChange);

        }*/
        void OnClickButtonHelp()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            bool isOpen = ChangePop(PopState.Help, goPopHelp);

            if (isOpen)
            {
                goHelpPanel.SetActive(true);
                /*
                Transform tfmBtnHelp = btnHelp.transform;
                Transform tfmPopHelp = goPopHelp.transform;
                Vector3 fromWorldPos = tfmBtnHelp.parent.TransformPoint(tfmBtnHelp.localPosition);
                Vector3 toLocalPos = tfmPopHelp.parent.InverseTransformPoint(fromWorldPos);
                tfmPopHelp.localPosition = new Vector3(toLocalPos.x, tfmPopHelp.localPosition.y, tfmPopHelp.localPosition.z);
                */

                int idx = _consoleBB.Instance.soundLevel;

                switch (idx)
                {
                    case 0:
                        btnSound.SetState("Mute");
                        break;
                    case 1:
                        btnSound.SetState("Sound1");
                        break;
                    case 2:
                        btnSound.SetState("Sound2");
                        break;
                    case 3:
                        btnSound.SetState("Sound3");
                        break;

                }
                //btnSound.SetState("Disabled");
            }
        }



        void OnClickButtonBetUp()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.BetUp);

            List<long> betList = _consoleBB.Instance.betList;
            Variable<int> vBetIndex = BlackboardUtils.FindVariable<int>("./betIndex");

            int betIndex = vBetIndex.value;
            if (++betIndex >= betList.Count)
            {
                betIndex = betList.Count - 1;
            }
            vBetIndex.value = betIndex;

            BlackboardUtils.SetValue<long>("./totalBet", betList[betIndex]);

            ChangeBetButtonInteractable(betIndex, betList.Count);
        }

    #region 同步押注按钮
        int curBetIndex = 0;
        int curBetListCount = 1;
        void ChangeBetButtonInteractable(int? betIndex01 = null, int? betListCount01 = null)
        {
            if (betIndex01 != null && betListCount01 != null)
            {
                curBetIndex = (int)betIndex01;
                curBetListCount = (int)betListCount01;
            }
            btnBetDown.interactable = curBetIndex > 0;
            btnBetUp.interactable = curBetIndex < curBetListCount - 1;

            /*
            if(btnBetDown.interactable)
                btnBetDown.transform.GetComponent<Image>().color  = new Color(1f, 1f, 1f, 1f);

            if (btnBetUp.interactable)
                btnBetUp.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            */
            //Debug.LogError($" btnBetDown.interactable:{btnBetDown.interactable}, btnBetUp.interactable:{btnBetUp.interactable} curBetIndex:{curBetIndex} curBetListCount:{curBetListCount} ");
        }
    #endregion

        void OnClickButtonBetDown()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.BetDown);

            List<long> betList = _consoleBB.Instance.betList;
            Variable<int> vBetIndex = BlackboardUtils.FindVariable<int>("./betIndex");

            int betIndex = vBetIndex.value;
            if (--betIndex < 0)
            {
                betIndex = 0;
            }

            vBetIndex.value = betIndex;

            BlackboardUtils.SetValue<long>("./totalBet", betList[betIndex]);

            ChangeBetButtonInteractable(betIndex, betList.Count);
        }

        void OnPopAllCloase()
        {
            goHelpPanel.SetActive(false);
            goPagePanel.SetActive(false);
        }

        void OnPopOpen(bool isOpen)
        {
            cgRightPanel.interactable = !isOpen;
            cgLeftPanel.interactable = !isOpen;
        }

        bool ChangePop(PopState toState, GameObject pop)
        {
            if (popState == toState)
            {
                popState = PopState.None;
                goPopBG.SetActive(false);
                pop?.SetActive(false);
                OnPopOpen(false);
                return false;
            }
            if (popState != PopState.None)
                return false;
            popState = toState;
            goPopBG.SetActive(true);
            pop?.SetActive(true);
            OnPopOpen(true);
            return true;
        }

        void CloseAllPop()
        {
            if (popState != PopState.None)
            {
                popState = PopState.None;
                foreach (Transform tfmChd in goPops.transform)
                {
                    tfmChd.gameObject.SetActive(false);
                }
            }
            //goLeftPanel.SetActive(false);

            OnPopAllCloase();

            OnPopOpen(false);
        }



        void OnClickButtonPayTable()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.Tab);

            CloseAllPop();
            goPagePanel.SetActive(true);

            bool isOpen = ChangePop(PopState.PayTable, goPopPayTable);

            if (isOpen)
            {

                cgLeftPanel.interactable = true; // 说明页要允许押注按钮可以用


                Transform anchorPages = goPopPayTable.transform.Find("Anchor");

                /*for (int i = anchor.childCount -1; i>=0; i--)
                {
                    GameObject.Destroy(anchor.GetChild(i));
                }*/



                if (anchorPages.childCount == 0)
                {
                    List<GameObject> pages = BlackboardUtils.GetValue<List<GameObject>>(null, "@customData/paytable");

                    foreach (GameObject page in pages)
                    {
                        Transform tfm = GameObject.Instantiate(page).transform;
                        tfm.SetParent(anchorPages);
                        tfm.localScale = Vector3.one;
                        tfm.localPosition = Vector3.zero;

                        float left = 0f;
                        float bottom = 0f;
                        tfm.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom);

                        float right = 0f;
                        float top = 0f;
                        tfm.GetComponent<RectTransform>().offsetMax = new Vector2(right, top);
                    }



                    #region 亮灯

                    Transform tfmBtnParent = goPageIndexPanel.transform.Find("Anchor");

                    GameObject goClone = tfmBtnParent.GetChild(0).gameObject;

                    for (int i = tfmBtnParent.childCount; i < pages.Count; i++)
                    {
                        GameObject go = Instantiate(goClone);
                        go.transform.parent = tfmBtnParent;
                    }

                    foreach (Transform chd in tfmBtnParent)
                    {
                        chd.gameObject.SetActive(false);
                    }

                    for (int i = 0; i < pages.Count; i++)
                    {
                        Transform tfm = tfmBtnParent.GetChild(i);
                        tfm.gameObject.SetActive(true);
                    }
                    #endregion

                }

                for (int i = 0; i < anchorPages.childCount; i++)
                {
                    anchorPages.GetChild(i).gameObject.SetActive(i == 0);
                }

                ShowPageIndex(0);

                btnPageBack.SetState("Last");
                btnPageNext.SetState("Normal");


            }
        }



        void OnClickButtonRedeem()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            CloseAllPop();

            //发送事件

            EventCenter.Instance.EventTrigger<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT,
                new EventData(PanelEvent.RedeemButtonClick));
        }



        void ShowPageIndex(int index)
        {
            Transform tfmPageIndexAnchor = goPageIndexPanel.transform.Find("Anchor");
            foreach (Transform tfmChd in tfmPageIndexAnchor)
            {
                tfmChd.GetComponent<StateButton>().SetState(StateButton.ButtonState.Normal);
            }
            tfmPageIndexAnchor.GetChild(index)
                .GetComponent<StateButton>().SetState(StateButton.ButtonState.Selected);
        }


        void OnClickButtonPageBack(string customData)
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            Transform anchor = goPopPayTable.transform.Find("Anchor");

            int index = 0;
            for (; index < anchor.childCount; index++)
            {
                if (anchor.GetChild(index).gameObject.active)
                {
                    break;
                }
            }

            if (index == 0)
                return;

            if (--index < 0)
                index = 0;

            //if (--index <0) index = anchor.childCount - 1;

            for (int i = 0; i < anchor.childCount; i++)
            {
                anchor.GetChild(i).gameObject.SetActive(i == index);
            }

            btnPageBack.SetState(index == 0 ? "Last" : "Normal");
            btnPageNext.SetState(index == anchor.childCount - 1 ? "Last" : "Normal");


            ShowPageIndex(index);
        }



        void OnClickButtonPageNext(string customData)
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            Transform anchor = goPopPayTable.transform.Find("Anchor");

            int index = 0;
            for (; index < anchor.childCount; index++)
            {
                if (anchor.GetChild(index).gameObject.active)
                {
                    break;
                }
            }

            if (index == anchor.childCount - 1)
                return;

            if (++index >= anchor.childCount)
                index = anchor.childCount - 1;

            //if (++index >= anchor.childCount) index = 0;


            for (int i = 0; i < anchor.childCount; i++)
            {
                anchor.GetChild(i).gameObject.SetActive(i == index);
            }

            btnPageBack.SetState(index == 0 ? "Last" : "Normal");
            btnPageNext.SetState(index == anchor.childCount - 1 ? "Last" : "Normal");


            ShowPageIndex(index);
        }



        void OnClickButtonPageExit()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            CloseAllPop();
            //goLeftPanel.SetActive(true);
            //goPagePanel.SetActive(false);
            //cgRightPanel.interactable = true;
        }

        void OnClickButtonHelpExit()
        {
            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            CloseAllPop();
        }


        #endregion



        #region Spin按钮

        public void OnLongClickSpinButton(string customDataOrState) => OnClickSpinButton(true);
        public void OnShortClickSpinButton(string customDataOrState) => OnClickSpinButton(false);


        public void OnClickSpinButton(bool isLong)
        {
            if (!isLong)
                soundHelper.PlaySoundEff(SoundKey.SpinClick);

            EventCenter.Instance.EventTrigger<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT,
               new EventData<bool>(PanelEvent.SpinButtonClick, isLong));
        }

        #endregion





        #region 声音按钮


        public void OnClickButtonSound(string customData)
        {
            DebugUtils.Log($"state = {btnSound.state}");

            int idx = 0;
            switch (btnSound.state)
            {
                case "Normal":
                case "Sound1":
                    btnSound.SetState("Sound2");
                    idx = 2;
                    break;
                case "Sound2":
                    btnSound.SetState("Sound3");
                    idx = 3;
                    break;
                case "Sound3":
                    btnSound.SetState("Mute");
                    idx = 0;
                    break;
                case "Mute":
                    btnSound.SetState("Sound1");
                    idx = 1;
                    break;
                case "Disabled":
                    break;
            }
            _consoleBB.Instance.soundLevel = idx;

            SoundManager.Instance.SetBGMVolumScale(_consoleBB.Instance.music);
            SoundManager.Instance.SetEFFVolumScale(_consoleBB.Instance.sound);

            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);

            //MainBlackboard.Instance.soundVolume = MainBlackboard.Instance.soundVolumeLst[idx];
        }



        #endregion



        private void SetButtonInteractable()
        {
            GetComponentInChildren<Button>();
        }















        #region  网络按钮

        const string MARK_NET_BTN_PANLE = "MARK_NET_BTN_PANLE";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_TABLE,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnTable,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_PREV,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnPrev,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_NEXT,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnNext,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_BET_UP,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnBetUp,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_BET_DOWN,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnBetDown,
            });

            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_BET_MAX,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnBetMax,
            });

            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_EXIT,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnExit,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnSpin,
            });

           /* NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_STOP,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnStop,
            });*/

            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_AUTO,
                mark = MARK_NET_BTN_PANLE,
                onClick = OnNetBtnAuto,
            });

        }

        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_PANLE);


        void OnNetBtnTable(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0)
            {
                Debug.Log($"Game Main: {PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080)}");
                return;
            }

            if ((popState == PopState.PayTable) 
                || BlackboardUtils.GetValue<bool>("./isSpin"))
            {
                info.onCallback?.Invoke(false);
                return;
            }

            //NetButtonManager.Instance.ShowUIAminButtonClick(btnPayTable, MARK_NET_BTN_PANLE, NetButtonManager.TABLE);
            OnClickButtonPayTable();

            info.onCallback?.Invoke(true);
        }
        void OnNetBtnPrev(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

            //if (BlackboardUtils.GetValue<bool>("./isSpin")) return;

            if (popState != PopState.PayTable  || !btnPageBack.interactable)
            {

                info.onCallback?.Invoke(false);
                return;
            }

            NetButtonManager.Instance.ShowUIAminButtonClick(() =>
            {
                btnPageBack.OnPointerDown(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            },()=> {
                btnPageBack.OnPointerUp(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
                //btnPageBack.onShortClick.Invoke();
            }, MARK_NET_BTN_PANLE, NetButtonManager.BTN_PREV);

            info.onCallback?.Invoke(true);
        }

        void OnNetBtnNext(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

            //if (BlackboardUtils.GetValue<bool>("./isSpin")) return;

            if (!btnPageNext.interactable || popState != PopState.PayTable)
            {
                info.onCallback?.Invoke(false);
                return;
            }

            NetButtonManager.Instance.ShowUIAminButtonClick(() =>
            {
                btnPageNext.OnPointerDown(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }, () => {
                btnPageNext.OnPointerUp(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }, MARK_NET_BTN_PANLE, NetButtonManager.BTN_NEXT);

            info.onCallback?.Invoke(true);
        }
        void OnNetBtnBetMax(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

            if (BlackboardUtils.GetValue<bool>("./isSpin"))
            {
                info.onCallback?.Invoke(false);
                return;
            }

            List<long> betList = _consoleBB.Instance.betList;
            Variable<int> vBetIndex = BlackboardUtils.FindVariable<int>("./betIndex");
            int betIndex = vBetIndex.value;

            if (betIndex != betList.Count - 1)
            {
                soundHelper.PlaySoundEff(GameMaker.SoundKey.BetUp);

                betIndex = betList.Count - 1;
                vBetIndex.value = betIndex;

                BlackboardUtils.SetValue<long>("./totalBet", betList[betIndex]);
                ChangeBetButtonInteractable(betIndex, betList.Count);
            }
            info.onCallback?.Invoke(true);
        }

        void OnNetBtnBetUp(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;


            if (!btnBetUp.interactable || BlackboardUtils.GetValue<bool>("./isSpin"))
            {
                info.onCallback?.Invoke(false);
                return;
            }

            NetButtonManager.Instance.ShowUIAminButtonClick(btnBetUp, MARK_NET_BTN_PANLE, NetButtonManager.BTN_BET_UP);

            info.onCallback?.Invoke(true);
        }

        void OnNetBtnBetDown(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

            if (!btnBetDown.interactable || BlackboardUtils.GetValue<bool>("./isSpin"))
            {
                info.onCallback?.Invoke(false);
                return;
            }

            NetButtonManager.Instance.ShowUIAminButtonClick(btnBetDown, MARK_NET_BTN_PANLE, NetButtonManager.BTN_BET_DOWN);

            info.onCallback?.Invoke(true);
        }

        void OnNetBtnExit(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;
            if (popState == PopState.None)
            {
                info.onCallback?.Invoke(false);
                return;
            }

            soundHelper.PlaySoundEff(GameMaker.SoundKey.NormalClick);
            CloseAllPop();

            info.onCallback?.Invoke(true);
        }


        void OnNetBtnSpin(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

            if(popState != PopState.None)
            {
                info.onCallback?.Invoke(false);
                return;
            }

            NetButtonManager.Instance.ShowUIAminButtonClick(() =>
            {
                btnSpin.OnPointerDown(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }, () => {
                btnSpin.OnPointerUp(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }, MARK_NET_BTN_PANLE, NetButtonManager.BTN_SPIN);

            info.onCallback?.Invoke(true);

            /*
            if (isOk)
                Loom.QueueOnMainThread((res) => {

                    bool isAuto = BlackboardUtils.GetValue<bool>("./isSpin");

                    if (info != null)
                        info.onCallback?.Invoke(isAuto);

                }, null, 0.8f);
            else
            {
                if (info != null)
                    info.onCallback?.Invoke(false);
            }*/
        }

        /*void OnNetBtnStop(NetButtonInfo info)
        {
            bool isOk = false;

            if (info != null && info.dataType == NetButtonManager.DATA_MQTT_REMOTE_CONTROL)
            {
                isOk = PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) == 0 && popState == PopState.None;
            }
            else
            {
                isOk = PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) == 0 && popState == PopState.None;
            }

            if (isOk)
            {
                if (BlackboardUtils.GetValue<bool>("./isSpin"))
                {
                    NetButtonManager.Instance.ShowUIAminButtonClick(() =>
                    {
                        btnSpin.OnPointerDown(new PointerEventData(null)
                        {
                            button = PointerEventData.InputButton.Left,
                        });
                    }, () => {
                        btnSpin.OnPointerUp(new PointerEventData(null)
                        {
                            button = PointerEventData.InputButton.Left,
                        });
                    }, MARK_NET_BTN_PANLE, NetButtonManager.BTN_TABLE);
                }
            }

            if (info.dataType == NetButtonManager.DATA_MQTT_REMOTE_CONTROL 
                && info.toDo=="StopAuto")
            {
                Loom.QueueOnMainThread((res) => {

                    bool isAuto = BlackboardUtils.GetValue<bool>("./isAuto");

                    if (info != null)
                        info.onCallback?.Invoke(!isAuto);

                }, null, 0.8f);
            }
            else
            {
                if (info != null)
                    info.onCallback?.Invoke(isOk);
            }

        }*/

        void OnNetBtnAuto(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != 0) return;

            if (popState != PopState.None)
            {
                info.onCallback?.Invoke(false);
                return;
            }

            NetButtonManager.Instance.ShowUIAminButtonLongClick(() =>
            {
                btnSpin.OnPointerDown(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }, () => {
                btnSpin.OnPointerUp(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }, MARK_NET_BTN_PANLE, NetButtonManager.BTN_AUTO);


            Loom.QueueOnMainThread((res) => {

                bool isAuto = BlackboardUtils.GetValue<bool>("./isAuto");

                if (info != null)
                    info.onCallback?.Invoke(isAuto);

            }, null, 1.2f);

        }



        #endregion

    }


}
