using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameMaker
{

    public partial class StateButton : CorBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        private CanvasGroup parentCanvasGroup = null;
        private void Start()
        {
            if (scaleTarget == null)
            {
                scaleTarget = transform;
            }
            if (cg == null)
            {
                cg = gameObject.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = gameObject.AddComponent<CanvasGroup>();
                }
            }
            // �ܸ���CanvasGroup�������
            if (parentCanvasGroup == null)
            {
                Transform parent = transform.parent;
                int j = 10;
                do
                {
                    if (parent == null || parentCanvasGroup != null)
                    {
                        break;
                    }
                    parentCanvasGroup = parent.GetComponent<CanvasGroup>();
                    parent = parent.parent;
                } while (--j > 0);
            }

            SetState(m_State);
        }

        protected void OnDisable()
        {
            ClearAllCor();
            transform.localScale = Vector3.one;
        }

        #region ����������� CanvasGroup
        private bool oldParentInteractable = true;
        private bool? oldInteractable = null;
        protected void Update()
        {
            if (parentCanvasGroup != null 
                && oldParentInteractable != parentCanvasGroup.interactable)
            {
                oldParentInteractable = parentCanvasGroup.interactable;
                if (parentCanvasGroup.interactable == false)
                {
                    if (oldInteractable == null)
                        oldInteractable = interactable;
                    interactable = false;
                }
                else
                {
                    interactable = oldInteractable ?? interactable;
                    oldInteractable = null;
                }
            }
        }
        #endregion






        [SerializeField]
        private bool m_IsEnableDoubleClick = false;

        #region ��ť�������ť����

        public float longPressThreshold = 0.8f;
        private float timePressed = 0.0f;


        [FormerlySerializedAs("onDoubleClick")]
        [SerializeField]
        private UnityStringEvent m_OnDoubleClick = new UnityStringEvent();

        public UnityStringEvent onDoubleClick
        {
            get
            {
                return m_OnDoubleClick;
            }
            set
            {
                m_OnDoubleClick = value;
            }
        }




        [FormerlySerializedAs("onLongClick")]
        [SerializeField]
        private UnityStringEvent m_OnLongClick = new UnityStringEvent();

        public UnityStringEvent onLongClick
        {
            get
            {
                return m_OnLongClick;
            }
            set
            {
                m_OnLongClick = value;
            }
        }

        [FormerlySerializedAs("onShortClick")]
        [SerializeField]
        private UnityStringEvent m_OnShortClick = new UnityStringEvent();

        public UnityStringEvent onShortClick
        {
            get
            {
                return m_OnShortClick;
            }
            set
            {
                m_OnShortClick = value;
            }
        }
        

        [FormerlySerializedAs("onClickUp")]
        [SerializeField]
        private UnityStringEvent m_OnClickUp = new UnityStringEvent();

        public UnityStringEvent onClickUp
        {
            get
            {
                return m_OnClickUp;
            }
            set
            {
                m_OnClickUp = value;
            }
        }


        [FormerlySerializedAs("onClickDown")]
        [SerializeField]
        private UnityStringEvent m_OnClickDown = new UnityStringEvent();

        public UnityStringEvent onClickDown
        {
            get
            {
                return m_OnClickDown;
            }
            set
            {
                m_OnClickDown = value;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
                return;

            float toFactor = scaleFactor;
            ScaleButton(toFactor, false);

            timePressed = Time.unscaledTime;
            m_OnClickDown.Invoke(m_State);
        }


       
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable)
                return;

            ScaleButton(1f, false);

            m_OnClickUp.Invoke(m_State);

            //PrepareOnLongClick : ���� ClickUp �� ClickDown ��ʱ�� �� ��ť״̬�� �ж��Ƿ���Ҫ��Ӷ���
            if (Time.unscaledTime - timePressed >= longPressThreshold)
            {
                m_OnLongClick.Invoke(m_State);
                //DebugUtil.Log("==@ ����");
            }
            else
            {
                if (!m_IsEnableDoubleClick)
                {
                    m_OnShortClick.Invoke(m_State);
                }
                else
                {
                    //DebugUtil.Log("==@ i am here");
                    if (IsCor(COR_DO_SHORT_CLICK))
                    {
                        ClearCor(COR_DO_SHORT_CLICK);
                        //DebugUtil.Log("==@ ˫��");
                        m_OnDoubleClick.Invoke(m_State);
                    }
                    else
                    {
                        DoCor(COR_DO_SHORT_CLICK, DoShortClick());
                    }
                }
            }

        }
        const string COR_DO_SHORT_CLICK = "COR_DO_SHORT_CLICK";
        const string COR_CHANGE_SCALE = "COR_CHANGE_SCALE";
        IEnumerator DoShortClick()
        {
            yield return new WaitForSeconds(0.8f);
            //DebugUtil.Log("==@ ����");
            m_OnShortClick.Invoke(m_State);
        }

        #endregion




        #region  ��ť�������

        public Transform scaleTarget;
        public float scaleFactor = 0.8f;
        public float frequency = 10f;
        public float damping = 1f;
        private Vector3 toScale = Vector3.one;
        private Vector3 scaleVelocity = Vector3.one;

        private void ScaleButton(float toFactor, bool instant)
        {
            ClearCor(COR_CHANGE_SCALE);
            //StopCoroutine("ScaleCoroutine");
            toScale = new Vector3(toFactor, toFactor, 1f);
            if (instant)
            {
                scaleTarget.localScale = toScale;
            }
            else
            {
                //StartCoroutine("ScaleCoroutine");
                DoCor(COR_CHANGE_SCALE, ScaleCoroutine()); 
            }
        }

        private IEnumerator ScaleCoroutine()
        {
            while (!Mathf.Approximately(toScale.x, scaleTarget.localScale.x))
            {
                Vector3 delta = PIDUtils.CalcDisplacement(scaleTarget.localScale, toScale, ref scaleVelocity, Vector3.zero, frequency, damping, Time.deltaTime);
                scaleTarget.localScale += delta;
                yield return null;
            }
        }

        #endregion


        #region  ���ò�ͬ״̬�ı���

        [Serializable]
        public class ButtonState
        {
            [SerializeField]
            public string state;

            [SerializeField]
            public GameObject goStateTarget;

            public static readonly string Normal = "Normal";
            public static readonly string Highlighted = "Highlighted";
            public static readonly string Pressed = "Pressed";
            public static readonly string Selected = "Selected";
            public static readonly string Disabled = "Disabled";
        }

        public List<ButtonState> buttonStateLst = new List<ButtonState>()
        {
            new ButtonState() { state=ButtonState.Normal},
            new ButtonState() { state=ButtonState.Disabled},
            //new ButtonState() { state=ButtonState.Highlighted},
            //new ButtonState() { state=ButtonState.Pressed},
            //new ButtonState() { state=ButtonState.Selected},
        };


        private string m_State = ButtonState.Normal;

        public string state
        {
            get { return m_State; }
        }
        public virtual void SetState(string sta)
        {
            m_State = sta;

            if (ButtonState.Disabled == sta && null == GetConfigButtonState(ButtonState.Disabled))
            {
                if (cg != null)
                    cg.alpha = 0.8f;//�������ɫ

                return;
            }

            if (cg != null && cg.alpha != 1)
                cg.alpha = 1;


            foreach (ButtonState bs in buttonStateLst)
            {
                bs.goStateTarget.SetActive(false);
            }
            for (int i = 0; i < buttonStateLst.Count; i++)
            {

                ButtonState bs = buttonStateLst[i];
                if (bs.state == sta)
                {
                    bs.goStateTarget.SetActive(true);
                    break;
                }
            }
        }

        private ButtonState GetConfigButtonState(string state)
        {
            for (int i = 0; i < buttonStateLst.Count; i++)
            {
                if (buttonStateLst[i].state == state)
                {
                    return buttonStateLst[i];
                }
            }
            return null;
        }

        #endregion


        #region interactable ����

        public CanvasGroup cg;


        [Tooltip("Can the Selectable be interacted with?")]
        [SerializeField]
        private bool m_Interactable = true;

        public bool interactable
        {
            get
            {
                return m_Interactable;
            }
            set
            {
                if (m_Interactable && !value)
                {
                    oldState = m_State;
                }

                m_Interactable = value;

                SetState(value ? oldState : ButtonState.Disabled);
                //SetState(value ? ButtonState.Normal : ButtonState.Disabled);
            }
        }
        private string oldState = ButtonState.Normal;


        //ԭ������ ButtonState.Disabled ��ԭ���Ͳ��� Disable
        //

        [Button] //ʹ��StateButton1Editor�󣬴˹����޷�ʹ�ã���
        void SetInteractable()
        {
            interactable = !m_Interactable;
        }
        #endregion

    }

    public partial class StateButton : CorBehaviour
    {
#if UNITY_EDITOR
        [CustomEditor(typeof(StateButton))]
        public class StateButton1Editor : Editor
        {
            private bool m_Interactable_old;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                StateButton script = target as StateButton; // ���ƹ�����

                if (m_Interactable_old != script.m_Interactable)
                {
                    script.interactable = script.m_Interactable;
                    m_Interactable_old = script.m_Interactable;
                    DebugUtils.Log("���Ա�ˢ�� ��interactable");
                }

                /*if (GUILayout.Button("���԰�ťA�����������"))
                {
                    DebugUtil.Log("���԰�ťA �����");
                }*/
            }
        }
#endif
    }
}