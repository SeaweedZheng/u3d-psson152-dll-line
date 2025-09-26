using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMaker
{
    public delegate void EventDelegate(EventData eventData);
    public class MessageDelegates
    {

        private Dictionary<string, EventDelegate> delegates;

        public MessageDelegates(Dictionary<string, EventDelegate> delegates)
        {
            this.delegates = delegates;
        }

        public void Delegate(EventData eventData)
        {
            EventDelegate del;
            if (delegates.TryGetValue(eventData.name, out del))
                del.Invoke(eventData);
        }
    }
}
