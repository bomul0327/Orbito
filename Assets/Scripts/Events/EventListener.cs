using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : Singleton<EventListener>,IEventListener
{
    void IEventListener.OnEvent(IEvent e)
    {
        if(e is DestroyEvent)
        {
            // TODO : 파괴 관련 처리 (이펙트, 아이템 및 자원 드랍 등)
            
        }
    }
}
