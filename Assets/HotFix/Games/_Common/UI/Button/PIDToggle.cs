using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameMaker
{
    public class PIDToggle : Toggle
    {
        [SerializeField]
        public bool onDisable = true;
        public GameObject disableCover;

        /// <summary> 可交互 </summary>
        public UnityBoolEvent onInteractableChanged;


        public GameObject goOn, goOff;

        private const string kDefaultOnAnimName = "On";
        private const string kDefaultOffAnimName = "Off";
        [SerializeField]
        private string m_OnTrigger = kDefaultOnAnimName;
        [SerializeField]
        private string m_OffTrigger = kDefaultOffAnimName;
        Animator m_Animator;


        protected override void Awake()
        {
            base.Awake();
            m_Animator = GetComponent<Animator>();

            if (disableCover == null)
            {
                Transform t = transform.Find("Anchor/Cover");

                if (t != null)
                    disableCover = t.gameObject;
            }

            onValueChanged.AddListener(OnToggleValueChanged);
        }

        protected override void Start()
        {
            base.Start();
            OnToggleValueChanged(isOn);
        }

        private void OnToggleValueChanged(bool on)
        {
            if (m_Animator != null)
            {
                m_Animator.ResetTrigger(m_OnTrigger);
                m_Animator.ResetTrigger(m_OffTrigger);
                m_Animator.SetTrigger(on ? m_OnTrigger : m_OffTrigger);
            }
            if (goOn != null)
            {
                goOn?.SetActive(on);
            }
            if (goOff != null)
            {
                goOff?.SetActive(!on);
            }

        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {

            if (gameObject.activeInHierarchy)
            {
                if (state == SelectionState.Normal || state == SelectionState.Highlighted)
                {
                    if (disableCover != null) disableCover.SetActive(false);
                    onInteractableChanged.Invoke(true);
                }
                else if (state == SelectionState.Pressed)
                {
                }
                else if (state == SelectionState.Disabled)
                {
                    if (onDisable && (disableCover != null)) disableCover.SetActive(true);
                    onInteractableChanged.Invoke(false);
                }

                //if (state == SelectionState.Normal || state == SelectionState.Selected)  // 这里Selected不准,延时变化
                //OnToggleValueChanged(isOn);               
            }

            base.DoStateTransition(state, instant);


            //if (state == SelectionState.Normal|| state == SelectionState.Selected)  // 这里Selected不准 ,延时变化
                //OnToggleValueChanged(isOn);
        }

    }
}

