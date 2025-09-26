using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EventActiveGO : MonoBehaviour
{

    [System.Serializable]
    public class EventActiveInfo
    {
        /// <summary> �¼����� </summary>
        public string eventType;

        /// <summary> �¼��� </summary>
        public string eventName;

        /// <summary> �¼�ֵ </summary>
        public string eventValue = null;

        /// <summary> �ر��ֵܽڵ� </summary>
        public bool isColseSiblingGo = false;

        /// <summary> Ҫ�򿪵Ľڵ� </summary>
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
