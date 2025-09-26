using GameMaker;
using SlotMaker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

namespace Panel001
{
    /*
    public interface IPointerUpHandler : IEventSystemHandler
    {
        void OnPointerUp(PointerEventData eventData);
    }

    public interface IResponseData 
    {
        //void OnPointerUp(PointerEventData eventData);

        void GetBetList();
    }*/

    public class PanelController : MonoBehaviour // MonoWeakSingleton<PanelManager>
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



        public GameObject goPopChange, goPopHelp, goPopBet, goPopPayTable;
        public GameObject goBetPanel, goPagePanel,goCreditPanel;
        
        CanvasGroup cgCreditPanel,cgBetPanel,cgPagePanel;
        StateButton btnSound,btnSpin,btnPageBack,btnPageNext,bntPageExit;
        GameObject goPops, goPopBG;
        Button btnChange, btnHelp, btnBet;
        Button btnPayTable;

        NoticeUI uiMyCredit, uiWin, uiTotalBet;  //ctrlTotalBet

        // Start is called before the first frame update


        MessageDelegates onPropertyChangedEventDelegates;


        MessageDelegates onWinEventDelegates;

        MessageDelegates onMetaUIDelegates;


        void Awake()
        {
            //goBetPanel = transform.Find("Bottom/Bet Panel").gameObject;
            //goPagePanel = transform.Find("Bottom/Page Panel").gameObject;
            //goCreditPanel = transform.Find("Bottom/Credit Panel").gameObject;

            cgCreditPanel = goCreditPanel.GetComponent<CanvasGroup>(); 
            cgBetPanel = goBetPanel.GetComponent<CanvasGroup>(); 
            cgPagePanel = goPagePanel.GetComponent<CanvasGroup>();

            goPops = transform.Find("Pops").gameObject;
            goPopBG = goPops.transform.Find("BG").gameObject;

            btnChange = transform.Find("Bottom/Anchor/Button Change").GetComponent<Button>();
            btnHelp = goBetPanel.transform.Find("Anchor/Button Help").GetComponent<Button>();
            btnBet = goBetPanel.transform.Find("Anchor/Button Bet").GetComponent<Button>();

            btnChange.onClick.AddListener(OnClickButtonChange);
            btnHelp.onClick.AddListener(OnClickButtonHelp);
            btnBet.onClick.AddListener(OnClickButtonBet);


            uiMyCredit = goCreditPanel.transform.Find("Anchor/My Credit").GetComponent<NoticeUI>();
            uiWin = goCreditPanel.transform.Find("Anchor/Win").GetComponent<NoticeUI>();
            uiTotalBet = goCreditPanel.transform.Find("Anchor/Total Bet").GetComponent<NoticeUI>();
            btnSpin = goCreditPanel.transform.Find("Anchor/Button Spin").GetComponent<StateButton>();

            btnSpin.onLongClick.AddListener(OnLongClickSpinButton);
            btnSpin.onShortClick.AddListener(OnShortClickSpinButton);


            btnPageBack = goPagePanel.transform.Find("Anchor/Button Back").GetComponent<StateButton>();
            btnPageNext = goPagePanel.transform.Find("Anchor/Button Next").GetComponent<StateButton>();
            bntPageExit = goPagePanel.transform.Find("Anchor/Button Exit").GetComponent<StateButton>();

            btnPageBack.onClickDown.AddListener(OnClickButtonPageBack);
            btnPageNext.onClickDown.AddListener(OnClickButtonPageNext);
            bntPageExit.onClickUp.AddListener(OnClickButtonPageExit);



            btnSound = goPopHelp.transform.Find("Anchor/Button Sound").GetComponent<StateButton>();
            btnPayTable = goPopHelp.transform.Find("Anchor/Open Pay Table").GetComponent<Button>();
            btnPayTable.onClick.AddListener(OnClickButtonPayTable);



            onPropertyChangedEventDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                    { "./totalBet", OnPropertyChangeTotalBet },
                    { "./betList",  OnPropertyChangeBetList },
                    { "./btnSpinState", OnPropertyBtnSpinState},
                    { "./gameState",OnPropertyGameState },
                    //{ "ContentBlackboard/totalBet", OnPropertyChangeTotalBet },
                    //{ "ContentBlackboard/betList",  OnPropertyChangeBetList },
                    //{ "MainBlackboard/soundVolume",  OnPropertyChangeBetList }
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

        }


        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            //EventCenter.Instance.AddEventListener<CreditEventData>(PanelEvent.ON_CREDIT_CHANGE_EVENT, OnCreditEvent);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(MetaUIEvent.ON_CREDIT_EVENT, onMetaUIDelegates.Delegate);

        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            //EventCenter.Instance.RemoveEventListener<CreditEventData>(PanelEvent.ON_CREDIT_CHANGE_EVENT, OnCreditEvent);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(MetaUIEvent.ON_CREDIT_EVENT, onMetaUIDelegates.Delegate);            
        }


        void Start() {
            InitParam();

        }




        void InitParam()
        {
            uiWin.SetToCredit(0);
            OnClickButtonPageBack(null);

            OnPropertyChangeBetList();
            OnPropertyChangeTotalBet();
            OnPropertyBtnSpinState();
            OnUpdateNaviCredit();
            //         public List<GameObject> pages = new List<GameObject>();
            //goPopPayTable.Find("Anchor")
        }

        #region 监听值变化

        void OnPropertyChangeTotalBet(EventData receivedEvent = null)
        {
            long totalBet = receivedEvent != null? (long)receivedEvent.value: - 1;
            if (totalBet == -1)
            {
                List<long> betList = _consoleBB.Instance.betList;
                totalBet = betList[0];
            }

            DebugUtils.Log($"OnPropertyChangeTotalBet ：{totalBet}");
            uiTotalBet.SetToCredit(totalBet);
        }




        void OnPropertyChangeBetList(EventData receivedEvent = null)
        {
            List<long> betList = (List<long>)receivedEvent?.value; 

            if (betList == null) 
                betList = _consoleBB.Instance.betList;
            

            Transform tfmBtnParent = goPopBet.transform.Find("Anchor");

            GameObject goClone = tfmBtnParent.GetChild(2).gameObject;
            for (int i= tfmBtnParent.childCount; i< betList.Count + 2;i++)
            {
                GameObject go = Instantiate(goClone);
                go.transform.parent = tfmBtnParent;
            }

            foreach (Transform chd in tfmBtnParent)
            {
                chd.GetComponent<Button>().onClick.RemoveAllListeners();
                chd.gameObject.SetActive(false);
            }

            for (int i = 0;  i < betList.Count + 2; i++)
            {
                Transform tfm = tfmBtnParent.GetChild(i);
                tfm.gameObject.SetActive(true);

                if (i >= 2)
                {
                    int idx = i - 2;
                    tfm.name = $"{betList[idx]}";
                    tfm.Find("Anchor/Text").GetComponent<Text>().text = tfm.name;
                }

                tfm.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickPopBetBtn(tfm.name);
                });
            }
        }


        void OnPropertyBtnSpinState(EventData receivedEvent = null)
        {
            string changeSpinState = (string)receivedEvent?.value;

            if (changeSpinState == null)
                changeSpinState = BlackboardUtils.GetValue<string>(null, "./btnSpinState");

            switch (changeSpinState)
            {
                case "Stop":
                    btnSpin.SetState("Stop");
                    cgBetPanel.interactable = true;
                    break;
                case "Spin":
                    btnSpin.SetState("Spin");
                    cgBetPanel.interactable = false;
                    break;
                case "Auto":
                    btnSpin.SetState("Auto");
                    cgBetPanel.interactable = false;
                    break;
            }
        }


        void OnPropertyGameState(EventData receivedEvent = null)
        {
            string gameState = (string)receivedEvent?.value;
            if (gameState == "Spin")
            {
                uiWin.SetToCredit(0);
            }
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

        void OnClickPopBetBtn(string name)
        {

            DebugUtils.Log($"name = {name}");
         
            switch (name)
            {
                case "Collect":
                    break;
                case "Service":
                    break;
                default:
                    long bet = long.Parse(name);
                    BlackboardUtils.SetValue<long>(null, "./totalBet", bet);
                    break;
            }
        }

        void OnClickButtonChange()
        {
            bool isOpen = ChangePop(PopState.Change, goPopChange);

        }
        void OnClickButtonHelp()
        {
            bool isOpen = ChangePop(PopState.Help, goPopHelp);

            if (isOpen)
            {
                Transform tfmBtnHelp = btnHelp.transform;
                Transform tfmPopHelp = goPopHelp.transform;
                Vector3 fromWorldPos = tfmBtnHelp.parent.TransformPoint(tfmBtnHelp.localPosition);
                Vector3 toLocalPos = tfmPopHelp.parent.InverseTransformPoint(fromWorldPos);
                tfmPopHelp.localPosition = new Vector3(toLocalPos.x, tfmPopHelp.localPosition.y,  tfmPopHelp.localPosition.z);

                //float soundVolume = BlackboardUtils.GetValue<float>(null,"/soundVolume");
                //List<float> soundVolumeLst = BlackboardUtils.GetValue<List<float>>(null, "/soundVolumeLst");

                //int idx = MainBlackboard.Instance.soundVolumeLst.IndexOf(MainBlackboard.Instance.soundVolume);
                int idx = _consoleBB.Instance.soundLevel;

                switch (idx)
                {
                    case 0:
                        btnSound.SetState("Sound1");
                        break;
                    case 1:
                        btnSound.SetState("Sound2");
                        break;
                    case 2:
                        btnSound.SetState("Sound3");
                        break;
                    case 3:
                        btnSound.SetState("Mute");
                        break;
                }
                //btnSound.SetState("Disabled");
            }
        }
        void OnClickButtonBet()
        {
            bool isOpen = ChangePop(PopState.Bet, goPopBet);
        }



        void OnPopOpen(bool isOpen)
        {
            cgCreditPanel.interactable = !isOpen;
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
            OnPopOpen(false);
        }



        void OnClickButtonPayTable()
        {
            CloseAllPop();
            goBetPanel.SetActive(false);
            goPagePanel.SetActive(true);

            bool isOpen = ChangePop(PopState.PayTable, goPopPayTable);

            if (isOpen)
            {

                Transform anchor = goPopPayTable.transform.Find("Anchor");

                /*for (int i = anchor.childCount -1; i>=0; i--)
                {
                    GameObject.Destroy(anchor.GetChild(i));
                }*/
                if (anchor.childCount == 0)
                {
                    List<GameObject> pages = BlackboardUtils.GetValue<List<GameObject>>(null, "@customData/paytable");
                    foreach (GameObject page in pages)
                    {
                        Transform tfm = GameObject.Instantiate(page).transform;
                        tfm.SetParent(anchor);
                        tfm.localScale = Vector3.one;
                        tfm.localPosition = Vector3.zero;

                        float left = 0f;
                        float bottom = 0f;
                        tfm.GetComponent<RectTransform>().offsetMin = new Vector2(left,bottom);

                        float right = 0f;
                        float top = 0f;
                        tfm.GetComponent<RectTransform>().offsetMax = new Vector2(right, top);
                    }
                }

                for(int i=0;i<anchor.childCount;i++)
                {
                    anchor.GetChild(i).gameObject.SetActive(i == 0);
                }
                btnPageBack.SetState("Last");
                btnPageNext.SetState("Normal");
            }
        }

        void OnClickButtonPageBack(string customData)
        {
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
        }

        void OnClickButtonPageNext(string customData)
        {
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
                index = anchor.childCount -1;

            //if (++index >= anchor.childCount) index = 0;


            for (int i= 0; i < anchor.childCount; i++)
            {
                anchor.GetChild(i).gameObject.SetActive(i == index);
            }

            btnPageBack.SetState(index == 0 ? "Last": "Normal");
            btnPageNext.SetState(index == anchor.childCount -1 ? "Last" : "Normal");
        }



        void OnClickButtonPageExit(string customData)
        {
            CloseAllPop();
            goBetPanel.SetActive(true);
            goPagePanel.SetActive(false);
            cgCreditPanel.interactable = true;
        }

        #endregion







        #region Spin按钮

        public void OnLongClickSpinButton(string customDataOrState) => OnClickSpinButton(true);
        public void OnShortClickSpinButton(string customDataOrState) => OnClickSpinButton(false);

        public void OnClickSpinButton(bool isLong)
        {
            EventCenter.Instance.EventTrigger<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT,
               new EventData<bool>(PanelEvent.SpinButtonClick, isLong));
        }


        #endregion





        #region 声音按钮


        public void OnClickSoundButton(string customData)
        {
            DebugUtils.Log($"state = {btnSound.state}");

            int idx = 0;
            switch (btnSound.state)
            {
                case "Normal":
                case "Sound1":
                    btnSound.SetState("Sound2");
                    idx = 1;
                    break;
                case "Sound2":
                    btnSound.SetState("Sound3");
                    idx = 2;
                    break;
                case "Sound3":
                    btnSound.SetState("Mute");
                    idx = 3;
                    break;
                case "Mute":
                    btnSound.SetState("Sound1");
                    idx = 0;
                    break;
                case "Disabled":
                    break;
            }
            _consoleBB.Instance.soundLevel = idx;
            //MainBlackboard.Instance.soundVolume = MainBlackboard.Instance.soundVolumeLst[idx];
        }



        #endregion



        private void SetButtonInteractable()
        {
            GetComponentInChildren<Button>();
        }


    }
}