using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;
using GameMaker;

public partial class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float longPressThreshold = 1.0f;
    private float timePressed = 0.0f;
    private bool isLongPress = false;

    /* 
     public GameObject goTarget;
     private void Start()
     {
         if (goTarget == null)
         {
             goTarget = gameObject;
         }
     }
     public enum Transition
     {
         None,
         //ColorTint,
         //SpriteSwap,
         //Animation
         Scale,
     }
     [FormerlySerializedAs("transition")]
     [SerializeField]
     private Transition m_Transition = Transition.ColorTint;   
     */


    [Serializable]
    public class ButtonClickedEvent : UnityEvent
    {
    }

    [FormerlySerializedAs("onLongClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnLongClick = new ButtonClickedEvent();

    public ButtonClickedEvent onLongClick
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

    [FormerlySerializedAs("onClickUp")]
    [SerializeField]
    private ButtonClickedEvent m_OnClickUp = new ButtonClickedEvent();

    public ButtonClickedEvent onClickUp
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
    private ButtonClickedEvent m_OnClickDown = new ButtonClickedEvent();

    public ButtonClickedEvent onClickDown
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
        DebugUtils.Log("OnPointerDown");
        float toFactor = scaleFactor;
        ScaleButton(toFactor, false);

        timePressed = Time.unscaledTime;
        m_OnClickDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DebugUtils.Log("OnPointerUp");
        ScaleButton(1f, false);

        m_OnClickUp.Invoke();
        if (Time.unscaledTime - timePressed >= longPressThreshold)
        {
            isLongPress = true;
            m_OnLongClick.Invoke();
        }
        else
        {
            isLongPress = false;
        }
    }


}

public partial class LongPressButton { 

    public Transform scaleTarget;
    public float scaleFactor = 0.8f;
    public float frequency = 10f;
    public float damping = 1f;
    private Vector3 toScale = Vector3.one;
    private Vector3 scaleVelocity = Vector3.one;
    private void ScaleButton(float toFactor, bool instant)
    {
        StopCoroutine("ScaleCoroutine");
        toScale = new Vector3(toFactor, toFactor, 1f);
        if (instant)
        {
            scaleTarget.localScale = toScale;
        }
        else
        {
            StartCoroutine("ScaleCoroutine");
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
    private void Start()
    {
        if (scaleTarget == null)
        {
            scaleTarget = transform;
        }
    }
}
