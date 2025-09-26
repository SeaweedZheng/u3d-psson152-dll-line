using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace GameMaker
{

    //handle
    //handler
    //EventHandler
    [System.Serializable]
    public class CustomDicMachineButtonKeyHandle : UnitySerializedDictionary<MachineButtonKey, UnityEvent<MachineButtonInfo>> { }


    public class MachineButtonEventDispatcher : CorBehaviour
    {
        public MachineCustomButton machineCustomButton = new MachineCustomButton()
        {
            btnShowLst = new List<MachineButtonKey>(),
            btnType = MachineButtonType.Regular,
        };

        public CustomDicMachineButtonKeyHandle longClickHandler = new CustomDicMachineButtonKeyHandle();

        public CustomDicMachineButtonKeyHandle shortClickHandler = new CustomDicMachineButtonKeyHandle();

        public CustomDicMachineButtonKeyHandle downClickHandler = new CustomDicMachineButtonKeyHandle();

        public CustomDicMachineButtonKeyHandle upClickHandler = new CustomDicMachineButtonKeyHandle();


        public UnityEvent<MachineButtonInfo> machineButtonEventHanler = new UnityEvent<MachineButtonInfo>();


        private void Awake()
        {
            machineCustomButton.mark = $"{transform.name}#{Guid.NewGuid().ToString()}";
        }
        void OnEnable()
        {
             EventCenter.Instance.AddEventListener<EventData>(MachineDeviceController.MACHINE_BUTTON_EVENT, OnEventMachineButton);
        }

        // Update is called once per frame
        void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(MachineDeviceController.MACHINE_BUTTON_EVENT, OnEventMachineButton);
        }


        public void ChangeButtonShow(List<MachineButtonKey> btnShowLst, MachineButtonType btnType = MachineButtonType.Regular, bool isPriority = false)
        {
            machineCustomButton.btnShowLst = btnShowLst;
            machineCustomButton.btnType = btnType;
            machineCustomButton.isPriority = isPriority;
            //DebugUtil.Log($"Get machine button focus : {machineCustomButton.mark}");
            EventCenter.Instance.EventTrigger<EventData>(MachineCustomButton.MACHINE_CUSTOM_BUTTON_FOCUS_EVENT,
                new EventData<MachineCustomButton>(machineCustomButton.mark, machineCustomButton));
        }

        public void OnTop()
        {
            //DebugUtil.Log($"Get machine button focus : {machineCustomButton.mark}");
            EventCenter.Instance.EventTrigger<EventData>(MachineCustomButton.MACHINE_CUSTOM_BUTTON_FOCUS_EVENT,
                new EventData<MachineCustomButton>(machineCustomButton.mark, machineCustomButton));
        }

        private void OnEventMachineButton(EventData evt)
        {
            if (evt.name != machineCustomButton.mark)
                return;

            MachineButtonInfo info = (MachineButtonInfo)evt.value;
            machineButtonEventHanler.Invoke(info);

            if (info != null)
            {
                string keyName = Enum.GetName(typeof(MachineButtonKey), info.btnKey);

                if (!info.isUp)
                {
                    if (downClickHandler.ContainsKey(info.btnKey))
                        downClickHandler[info.btnKey].Invoke(info);

                    if (longClickHandler.ContainsKey(info.btnKey))
                        DoCor(keyName, DoCheckLongClick(info));  
                }
                else
                {
 
                    if (upClickHandler.ContainsKey(info.btnKey))
                        upClickHandler[info.btnKey].Invoke(info);

                    if (IsCor(keyName))
                    {
                        ClearCor(keyName);

                        if (shortClickHandler.ContainsKey(info.btnKey))
                            shortClickHandler[info.btnKey].Invoke(info);
                    }
                }
            }
        }


        IEnumerator DoCheckLongClick(MachineButtonInfo info)
        {
            yield return new WaitForSeconds(0.2f);
            // longClickPreEventHandler

            yield return new WaitForSeconds(0.6f);
            longClickHandler[info.btnKey].Invoke(info);
        }

    }
}