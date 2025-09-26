using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// EventSystem��Unity�����ڴ��������¼���ϵͳ�������Ը�����ꡢ���������������豸��״̬��
/// ����ͨ��ʹ��EventSystem����ȡ"OnPointerEnter"�¼��ϵ�ָ��
/// 
/// 1. ȷ�������д���һ��EventSystem�������û�У�����Hierarchy������Ҽ������ѡ��"UI" -> "Event System"������һ����
/// 2. ����Ҫ��ȡ"OnPointerEnter"�¼��Ķ��������һ��UI���������Button��Image�ȡ�
/// 3. �ڸ�UI��������һ���ű������ڴ���"OnPointerEnter"�¼���
/// 4. �ڽű��У�ʹ��Unity���¼�ϵͳ������"OnPointerEnter"�¼�������ȡָ����Ϣ������ͨ�����´���ʵ�֣�
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