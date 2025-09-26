using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// EventSystem是Unity中用于处理输入事件的系统，它可以跟踪鼠标、触摸和其他输入设备的状态。
/// 可以通过使用EventSystem来获取"OnPointerEnter"事件上的指针
/// 
/// 1. 确保场景中存在一个EventSystem对象。如果没有，请在Hierarchy面板中右键点击并选择"UI" -> "Event System"来创建一个。
/// 2. 在需要获取"OnPointerEnter"事件的对象上添加一个UI组件，例如Button、Image等。
/// 3. 在该UI组件上添加一个脚本，用于处理"OnPointerEnter"事件。
/// 4. 在脚本中，使用Unity的事件系统来监听"OnPointerEnter"事件，并获取指针信息。可以通过以下代码实现：
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Collider2D))]
public class TouchTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public const string ON_TRIGGER_EVENT = "ON_TRIGGER_EVENT";
    public const string PointerEnter = "PointerEnter";
    public const string PointerExit = "PointerExit";


    public bool isPointerEnter = true;
    public bool isPointerExit = true;

    public  GameObject go;


    [FormerlySerializedAs("onPointerEnter")]
    [SerializeField]
    private UnityEvent m_OnPointerEnter = new UnityEvent();

    public UnityEvent onPointerEnter
    {
        get => m_OnPointerEnter;
        set => m_OnPointerEnter = value;
    }

    [FormerlySerializedAs("onPointerExit")]
    [SerializeField]
    private UnityEvent m_OnPointerExit = new UnityEvent();

    public UnityEvent onPointerExit
    {
        get => m_OnPointerExit;
        set => m_OnPointerExit = value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPointerEnter)
            return;

        m_OnPointerEnter.Invoke();

        //DebugUtil.Log($"i am pointer enter {eventData} ");
        EventCenter.Instance.EventTrigger<EventData>(ON_TRIGGER_EVENT, new EventData<object>(PointerEnter, go.transform.GetSiblingIndex()));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPointerExit)
            return;

        m_OnPointerExit.Invoke();

        EventCenter.Instance.EventTrigger<EventData>(ON_TRIGGER_EVENT, new EventData<object>(PointerExit, go.transform.GetSiblingIndex()));
    }
        
}