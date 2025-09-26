using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GameMaker
{
    public class PIDButtonX : PIDButton, IPointerDownHandler, IPointerUpHandler
    {

        [FormerlySerializedAs("onClickDown")]
        [SerializeField]
        private UnityEvent m_OnClickDown = new UnityEvent();
        public UnityEvent onClickDown
        {
            get => m_OnClickDown;
            set => m_OnClickDown = value;
        }

        [FormerlySerializedAs("onClickUp")]
        [SerializeField]
        private UnityEvent m_OnClickUp = new UnityEvent();
        public UnityEvent onClickUp
        {
            get => m_OnClickUp;
            set => m_OnClickUp = value;
        }


        [FormerlySerializedAs("onLongClick")]
        [SerializeField]
        private UnityEvent m_OnLongClick = new UnityEvent();

        public UnityEvent onLongClick
        {
            get => m_OnLongClick;
            set => m_OnLongClick = value;
        }

        [FormerlySerializedAs("onShortClick")]
        [SerializeField]
        private UnityEvent m_OnShortClick = new UnityEvent();

        public UnityEvent onShortClick
        {
            get => m_OnShortClick;
            set => m_OnShortClick = value;
        }

        public float longPressThreshold = 0.8f;
        private float timePressed = 0.0f;



        [SerializeField]
        private bool m_IsEnableDoubleClick = false;



        [FormerlySerializedAs("onDoubleClick")]
        [SerializeField]
        private UnityEvent m_OnDoubleClick = new UnityEvent();

        public UnityEvent onDoubleClick
        {
            get => m_OnDoubleClick;
       
            set =>  m_OnDoubleClick = value;
      
        }



        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (!interactable)
                return;

            timePressed = Time.unscaledTime;
            m_OnClickDown.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (!interactable)
                return;
            m_OnClickUp.Invoke();


            if (Time.unscaledTime - timePressed >= longPressThreshold)
            {
                m_OnLongClick.Invoke();
                //DebugUtil.Log("==@ ³¤°´");
            }
            else
            {
                if (!m_IsEnableDoubleClick)
                {
                    m_OnShortClick.Invoke();
                }
                else
                {
                    //DebugUtil.Log("==@ i am here");
                    if (_corShortClick != null)
                    {
                        ClearCorShortClick();
                        //DebugUtil.Log("==@ Ë«»÷");
                        m_OnDoubleClick.Invoke();
                    }
                    else
                    {
                        DoCorShortClick();
                    }
                }
            }

        }

        Coroutine _corShortClick;
        void ClearCorShortClick()
        {
            if (_corShortClick != null)
                StopCoroutine(_corShortClick);
            _corShortClick = null;
        }

        void DoCorShortClick()
        {
            ClearCorShortClick();
            if(gameObject.active)
                _corShortClick = StartCoroutine(DoShortClick());
        }

        IEnumerator DoShortClick()
        {
            yield return new WaitForSeconds(0.8f);
            //DebugUtil.Log("==@ µ¥»÷");
            m_OnShortClick.Invoke();
        }

    }

}
