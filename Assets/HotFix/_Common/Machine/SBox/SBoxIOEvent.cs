/**
 * @file    
 * @author  Huang Wen <Email:ww1383@163.com, QQ:214890094, WeChat:w18926268887>
 * @version 1.0
 *
 * @section LICENSE
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * @section DESCRIPTION
 *
 * This file is ...
 */
using System.Collections.Generic;

namespace Hal
{
    public static class SBoxIOEvent
    {
        public delegate void EventCallback(SBoxPacket packet);
        static Dictionary<int, EventCallback> EventDictionary = new Dictionary<int, EventCallback>();

        /**
          *  @brief          �����¼��ص���ֻ�ܼ�һ��EventId�Ļص����ظ��ļӲ���ȥ
          *  @param[in]      EventId �¼�ID
          *  @param[in]      Callback �¼��ص�
          *  @return         ��
          *  @details        
          */
        public static void AddListener(int EventId, EventCallback Callback)
        {
            if (EventDictionary.ContainsKey(EventId))
            {

            }
            else
            {
                EventDictionary.Add(EventId, Callback);
            }
        }

        /**
          *  @brief          ɾ���¼��ص�
          *  @param[in]      EventId �¼�ID
          *  @return         ��
          *  @details        
          */
        public static void RemoveListener(int EventId)
        {
            if (EventDictionary.ContainsKey(EventId))
            {
                EventDictionary.Remove(EventId);
            }
        }

        /**
          *  @brief          �����¼�
          *  @param[in]      EventId �¼�ID
          *  @param[in]      packet �¼��ص��Ĳ���
          *  @return         ��
          *  @details        
          */
        public static void SendEvent(int EventId, SBoxPacket packet)
        {
            EventCallback Callback;

            if (EventDictionary.TryGetValue(EventId, out Callback))
            {
                Callback(packet);
            }
        }
    }
}

