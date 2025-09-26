using GameMaker;
using SlotMaker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PssOn00152
{

    public class MultiplierAloneController : MonoBehaviour
    {
        public GameObject goMultiplierAlone;

        MessageDelegates onPropertyChangedEventDelegates;

        void Awake()
        {
            onPropertyChangedEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                    { "./multiplierAlone", OnPropertyChangeMultiplierAlone },

                 }
             );
        }

        private void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

            InitParam();
        }

        private void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        }
        void InitParam()
        {
            OnPropertyChangeMultiplierAlone();
        }

        void OnPropertyChangeMultiplierAlone(EventData receivedEvent = null)
        {
            int multiplierAlone = receivedEvent != null ? (int)receivedEvent.value : -1;
            if (multiplierAlone == -1)
                multiplierAlone = BlackboardUtils.GetValue<int>("./multiplierAlone");
            

            if (multiplierAlone > 1)
            {
                //goMultiplierAlone.GetComponent<Text>().text = $"x{multiplierAlone}";
                goMultiplierAlone.GetComponent<Text>().text = $"X{multiplierAlone}";
                goMultiplierAlone.gameObject.SetActive(true);
            }
            else
            {
                goMultiplierAlone.gameObject.SetActive(false);
            }
        }


    }
}
