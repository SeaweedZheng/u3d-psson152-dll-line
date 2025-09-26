using GameMaker;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Collider2DTrigger : MonoBehaviour
{

    public const string ON_COLLIDER2D_TRIGGER_EVENT = "ON_COLLIDER2D_TRIGGER_EVENT";
    public const string TriggerEnter2D = "TriggerEnter2D";
    public const string TriggerStay2D = "TriggerStay2D";
    public const string TriggerExit2D = "TriggerExit2D";


    public bool isTriggerEnter2D = true;
    public bool isTriggerStay2D = true;
    public bool isTriggerExit2D = true;


    [FormerlySerializedAs("onTriggerEnter2D")]
    [SerializeField]
    private UnityEvent m_OnTriggerEnter2D = new UnityEvent();
    public UnityEvent onTriggerEnter2D
    {
        get => m_OnTriggerEnter2D;
        set => m_OnTriggerEnter2D = value;
    }

    [FormerlySerializedAs("onTriggerStay2D")]
    [SerializeField]
    private UnityEvent m_OnTriggerStay2D = new UnityEvent();
    public UnityEvent onTriggerStay2D
    {
        get => m_OnTriggerStay2D;
        set => m_OnTriggerStay2D = value;
    }

    [FormerlySerializedAs("onTriggerExit2D")]
    [SerializeField]
    private UnityEvent m_OnTriggerExit2D = new UnityEvent();
    public UnityEvent onTriggerExit2D
    {
        get => m_OnTriggerExit2D;
        set => m_OnTriggerExit2D = value;
    }


    // 当进入触发器
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggerEnter2D)
            return;

        onTriggerEnter2D.Invoke();

        EventCenter.Instance.EventTrigger<EventData>(ON_COLLIDER2D_TRIGGER_EVENT, new EventData<object>(TriggerEnter2D, other));
    }

    // 当停留在触发器内
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggerStay2D)
            return;

        onTriggerStay2D.Invoke();

        EventCenter.Instance.EventTrigger<EventData>(ON_COLLIDER2D_TRIGGER_EVENT, new EventData<object>(TriggerStay2D, other));
    }

    // 当离开触发器
    void OnTriggerExit2D(Collider2D other)
    {
        if (!isTriggerExit2D)
            return;

        onTriggerExit2D.Invoke();

        EventCenter.Instance.EventTrigger<EventData>(ON_COLLIDER2D_TRIGGER_EVENT, new EventData<object>(TriggerExit2D, other));
    }
}
