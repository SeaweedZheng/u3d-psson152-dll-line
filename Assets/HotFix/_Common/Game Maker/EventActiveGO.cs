using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EventActiveGO : MonoBehaviour
{

    [System.Serializable]
    public class EventActiveInfo
    {
        /// <summary> 事件类型 </summary>
        public string eventType;

        /// <summary> 事件名 </summary>
        public string eventName;

        /// <summary> 事件值 </summary>
        public string eventValue = null;

        /// <summary> 关闭兄弟节点 </summary>
        public bool isColseSiblingGo = false;

        /// <summary> 要打开的节点 </summary>
        public GameObject go;
    }

    public List<EventActiveInfo> events;

    private void OnEnable()
    {
        foreach (EventActiveInfo e in events)
        {
            EventCenter.Instance.AddEventListener<EventData>(e.eventType, OnEvent);
        }
    }

    private void OnDisable()
    {
        foreach (EventActiveInfo e in events)
        {
            EventCenter.Instance.RemoveEventListener<EventData>(e.eventType, OnEvent);
        }
    }

   
    public void OnEvent(EventData data)
    {
        string eventName = data.name;

        object eventValue = data.value;

        for (int i=0; i<events.Count; i++)
        {
            EventActiveInfo target = events[i];

            if (eventName == target.eventName 
                && (eventValue == null || (target.eventValue != null && $"{eventValue}" == $"{target.eventValue}"))
                )
            {
                if(target.isColseSiblingGo && target.go.transform.parent != null)
                {
                    foreach (Transform tfm in target.go.transform.parent)
                        tfm.gameObject.SetActive(false);
                }
                target.go.SetActive(true);
            }
        }
    }
}
