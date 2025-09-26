using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(MachineButtonEventDispatcher))]
    public class PageMachineButtonBase : PageBase
    {

        #region ��̨��ť�¼�
        MachineButtonEventDispatcher _machineButtonEventDispatcher;
        protected MachineButtonEventDispatcher machineButtonEventDispatcher
        {
            get
            {
                if (_machineButtonEventDispatcher == null)
                {
                    _machineButtonEventDispatcher = transform.GetComponent<MachineButtonEventDispatcher>();
                    if (_machineButtonEventDispatcher != null)
                        _machineButtonEventDispatcher.machineButtonEventHanler.AddListener(OnClickMachineButton);
                }
                return _machineButtonEventDispatcher;
            }
        }
        public override void OnTop()
        {
            /*
            //�����ť�¼�
            EventCenter.Instance.EventTrigger<EventData>(MachineCustomButton.MACHINE_CUSTOM_BUTTON_EVENT,
            new EventData<MachineCustomButton>("NULL", null));
            */

            machineButtonEventDispatcher?.OnTop();
        }
        public virtual void OnClickMachineButton(MachineButtonInfo info)
        {
            /* if (info != null)
             {
                 if (info.isDown)
                 {
                     switch (info.btnKey)
                     {
                         case MachineButtonKey.BtnSpin:
                             break;
                     }
                 }
                 else
                 {
                     switch (info.btnKey)
                     {
                         case MachineButtonKey.BtnSpin:
                             break;
                     }
                 }
             }*/
        }
        #endregion






        #region ��̨��ť�¼�-����UI��ť���µĶ���
        Coroutine _corBtn = null;
        IEnumerator BtnDelayEvent(Button btn)
        {
            yield return new WaitForSecondsRealtime(0.15f);      
            btn.onClick.Invoke();//�� btn.OnSubmit(null); 
            _corBtn = null;
        }
        protected void ShowUIAminButtonClick(Button btn, MachineButtonInfo info)
        {
            if (!info.isUp && btn.interactable)
            {
                //ֻ�а��¶������������¼�
                btn.OnPointerDown(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });
            }
            else if (info.isUp && btn.interactable)
            {
                //ֻ�е��𶯻����������¼�
                btn.OnPointerUp(new PointerEventData(null)
                {
                    button = PointerEventData.InputButton.Left,
                });

                if(_corBtn != null)
                    StopCoroutine(_corBtn);
                _corBtn = StartCoroutine(BtnDelayEvent(btn));
            }
        }
        #endregion
    }
}
