using System;

public interface IEventListener
{
    /// <summary>
    /// 이벤트를 받았을 때, 호출
    /// </summary>
    void OnEvent (IEvent e);
}
