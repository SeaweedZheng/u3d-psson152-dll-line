using GameMaker;
using SlotMaker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

namespace PssOn00152
{

    public class PanelFreeSpinController : MonoBehaviour
    {
        public GameObject goMyCredit, goBet, goWin, goCutdown,
            goRemainFreeSpin, goLastFreeSpin;


        Text txtMyCredit, txtBet, txtWin, txtCutdown;



        int freeSpinTotalTimes;
        int freeSpinPlayTimes;
        MessageDelegates onPropertyChangedEventDelegates;
        MessageDelegates onMetaUIDelegates;
        MessageDelegates onWinEventDelegates;
        void Awake()
        {
            txtMyCredit = goMyCredit.GetComponent<Text>();
            txtBet = goBet.GetComponent<Text>();
            txtWin = goWin.GetComponent<Text>();
            txtCutdown = goCutdown.GetComponent<Text>();

            onPropertyChangedEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                    { "./totalBet", OnPropertyChangeTotalBet },
                    { "./freeSpinTotalTimes",  OnPropertyChangeFreeSpinTotalTimes },
                    { "./freeSpinPlayTimes", OnPropertyChangeFreeSpinPlayTimes},
                    { "./gameState",OnPropertyGameState },
                 }
             );
            onMetaUIDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                { MetaUIEvent.UpdateNaviCredit, OnUpdateNaviCredit },
                }
            );
            onWinEventDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                { SlotMachineEvent.TotalWinCredit, OnTotalWinCredit },
                }
            );
        }
        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(MetaUIEvent.ON_CREDIT_EVENT, onMetaUIDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);

            InitParam();
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(MetaUIEvent.ON_CREDIT_EVENT, onMetaUIDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        }

        void InitParam()
        {
            txtWin.text = "0";

            txtMyCredit.text = _consoleBB.Instance.myCredit.ToString(); // 不要千分号   _consoleBB.Instance.myCredit.ToString("N0");

            goRemainFreeSpin.SetActive(true);
            goLastFreeSpin.SetActive(false);
            OnPropertyChangeTotalBet();
            OnPropertyChangeFreeSpinPlayTimes();
            OnPropertyChangeFreeSpinTotalTimes();
            OnUpdateNaviCredit();
        }
        void OnPropertyChangeTotalBet(EventData receivedEvent = null)
        {
            long totalBet = receivedEvent != null ? (long)receivedEvent.value : -1;
            if (totalBet == -1)
            {
                List<long> betList = _consoleBB.Instance.betList;
                int betIndex = BlackboardUtils.FindVariable<int>("./betIndex").value;
                totalBet = betList[betIndex];
            }

            txtBet.text = totalBet.ToString(); // 不要千分号   totalBet.ToString("N0");
        }


        void OnPropertyChangeFreeSpinTotalTimes(EventData receivedEvent = null)
        {
            freeSpinTotalTimes = receivedEvent != null ? (int)receivedEvent.value : -1;
            if (freeSpinTotalTimes == -1)
                freeSpinTotalTimes = BlackboardUtils.GetValue<int>("./freeSpinTotalTimes");

            OnChangeFreeSpinTime();
        }

        void OnPropertyChangeFreeSpinPlayTimes(EventData receivedEvent = null)
        {
            freeSpinPlayTimes = receivedEvent != null ? (int)receivedEvent.value : -1;
            if (freeSpinPlayTimes == -1)
                freeSpinPlayTimes = BlackboardUtils.GetValue<int>("./freeSpinPlayTimes");

            OnChangeFreeSpinTime();
        }


        void OnChangeFreeSpinTime()
        {
            txtCutdown.text = (freeSpinTotalTimes - freeSpinPlayTimes).ToString();

            if (freeSpinTotalTimes == 0)
            {
                goLastFreeSpin.SetActive(false);
                goRemainFreeSpin.SetActive(false);
                goCutdown.SetActive(false);
            }
            else if (freeSpinTotalTimes - freeSpinPlayTimes == 0)
            {
                goLastFreeSpin.SetActive(true);
                goRemainFreeSpin.SetActive(false);
                goCutdown.SetActive(false);
            }
            else
            {
                goLastFreeSpin.SetActive(false);
                goRemainFreeSpin.SetActive(true);
                goCutdown.SetActive(true);
            }
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

            txtMyCredit.text = toCredit.ToString(); // 不要千分号   toCredit.ToString("N0");
        }


        void OnTotalWinCredit(EventData receivedEvent)
        {
            long totalWinCredit = (long)receivedEvent.value;

            txtWin.text = totalWinCredit.ToString(); // 不要千分号  totalWin.ToString("N0");
        }

        void OnPropertyGameState(EventData receivedEvent = null)
        {
            string gameState = (string)receivedEvent?.value;
            if (gameState == GameState.Spin || gameState == GameState.FreeSpin)
            {
                txtWin.text = "0";
            }
        }

    }
}