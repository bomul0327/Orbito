using System.Collections.Generic;
using System;
using UnityEngine;

namespace OrbitoEvent
{
    /// <summary>
    /// 이벤트의 수신과 발송을 관리하는 Dispatcher 클래스입니다.
    /// </summary>
    public static class EventDispatcher
    {
        private static List<IEventListener> listenerList = new List<IEventListener>();

        /// <summary>
        /// IEventListener를 EventDispatcher에 등록합니다. 등록된 IEventListener는 이벤트를 수신합니다.
        /// </summary>
        public static void AddListener(IEventListener listener)
        {
            if (!listenerList.Contains(listener))
            {
                listenerList.Add(listener);
            }
            else
            {
                Debug.LogError($"This IEventListener already registered for this event! (Duplicate registeration is not allowed)");
            }
        }

        /// <summary>
        /// IEventListener를 EventDispatcher에서 삭제합니다. 삭제된 IEventListener는 더 이상 이벤트를 수신하지 않습니다.
        /// </summary>
        public static void RemoveListener(IEventListener listener)
        {
            if (!listenerList.Remove(listener))
            {
                Debug.LogError($"Such EventListener({listener}) was not found in dispatcher. Are you sure you added this listener?");
            }
        }

        /// <summary>
        /// 등록된 모든 EventListener들에게 이벤트를 발송합니다. 이벤트를 수신한 IEventListener는 OnEvent() 함수를 호출됩니다.
        /// </summary>
        public static void Notify(IEvent evt)
        {
            for (var i = 0; i < listenerList.Count; i++)
            {
                listenerList[i].OnEvent(evt);
            }
        }
    }
}