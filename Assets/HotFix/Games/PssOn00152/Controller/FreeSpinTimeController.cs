using GameMaker;
using SlotMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _contentBB = PssOn00152.ContentBlackboard;

namespace PssOn00152
{
    public class FreeSpinTimeController : MonoBehaviour
    {

        public GameObject goRemainFreeSpin, goLastFreeSpin, goCutdown;

        Text txtCutdown;
        int freeSpinTotalTimes;
        int freeSpinPlayTimes;

        MessageDelegates onPropertyChangedEventDelegates;
        MessageDelegates onContentEventDelegates;

        void Awake()
        {
            txtCutdown = goCutdown.GetComponent<Text>();

            onPropertyChangedEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     //{ "./freeSpinTotalTimes",  OnPropertyChangeFreeSpinTotalTimes },
                     //{ "./freeSpinPlayTimes", OnPropertyChangeFreeSpinPlayTimes},

                     //{ "./freeSpinTotalTimes",   OnPropertyChangeFreeSpinTimes},
                     //{ "./freeSpinPlayTimes",   OnPropertyChangeFreeSpinTimes},

                     { "./showFreeSpinRemainTime",OnPropertyChangeShowFreeSpinRemainTime }
                 }
             );


            onContentEventDelegates = new MessageDelegates
            (
                new Dictionary<string, EventDelegate>
                {
                    { SlotMachineEvent.BeginBonus, OnBeginBonus },
                }
            );
        }

        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_CONTENT_EVEN, onContentEventDelegates.Delegate);
            InitParam();
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_CONTENT_EVEN, onContentEventDelegates.Delegate);
        }
        void InitParam()
        {
            goRemainFreeSpin.SetActive(true);
            goLastFreeSpin.SetActive(false);
            //OnPropertyChangeFreeSpinPlayTimes();
            //OnPropertyChangeFreeSpinTotalTimes();
            //OnPropertyChangeFreeSpinTimes();
            OnPropertyChangeShowFreeSpinRemainTime();
        }
        /*void OnPropertyChangeFreeSpinTotalTimes(EventData receivedEvent = null)
        {
            freeSpinTotalTimes = receivedEvent != null ? (int)receivedEvent.value : -1;
            if (freeSpinTotalTimes == -1)
                freeSpinTotalTimes = _contentBB.Instance.freeSpinTotalTimes;

            OnChangeFreeSpinTime();
        }

        void OnPropertyChangeFreeSpinPlayTimes(EventData receivedEvent = null)
        {
            freeSpinPlayTimes = receivedEvent != null ? (int)receivedEvent.value : -1;
            if (freeSpinPlayTimes == -1)
                freeSpinPlayTimes = _contentBB.Instance.freeSpinPlayTimes;

            OnChangeFreeSpinTime();
        }


        void OnBeginBonus(EventData receivedEvent)
        {
            OnPropertyChangeFreeSpinTimes();
        }
*/


        /*void OnPropertyChangeFreeSpinTimes(EventData receivedEvent = null)
        {
            if(receivedEvent == null ) //初始化
            {
                freeSpinTotalTimes = _contentBB.Instance.freeSpinTotalTimes;
                freeSpinPlayTimes = _contentBB.Instance.freeSpinPlayTimes;
            }
            else if (_contentBB.Instance.isFreeSpinAdd) // 免费游戏+7
            {
                //freeSpinTotalTimes = _contentBB.Instance.freeSpinTotalTimes;  //这个不读取
                freeSpinPlayTimes = _contentBB.Instance.freeSpinPlayTimes;
            }
            else //免费游戏正常局
            {
                freeSpinTotalTimes = _contentBB.Instance.freeSpinTotalTimes;
                freeSpinPlayTimes = _contentBB.Instance.freeSpinPlayTimes;
            }


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
        }*/

        void OnBeginBonus(EventData receivedEvent)
        {
            OnPropertyChangeShowFreeSpinRemainTime();
        }

        void OnPropertyChangeShowFreeSpinRemainTime(EventData receivedEvent = null)
        {
            int curShowTime = _contentBB.Instance.showFreeSpinRemainTime;
            int freeSpinTotalTimes = _contentBB.Instance.freeSpinTotalTimes;

            Debug.LogWarning($"curShowTime: {curShowTime}");

            txtCutdown.text = (curShowTime).ToString();


            if (curShowTime != 0)
            {
                goLastFreeSpin.SetActive(false);
                goRemainFreeSpin.SetActive(true);
                goCutdown.SetActive(true);
            }
            else if (freeSpinTotalTimes != 0 && curShowTime == 0)
            {
                goLastFreeSpin.SetActive(true);
                goRemainFreeSpin.SetActive(false);
                goCutdown.SetActive(false);
            }
            else
            {
                goLastFreeSpin.SetActive(false);
                goRemainFreeSpin.SetActive(false);
                goCutdown.SetActive(false);
            }

        }


    }
}