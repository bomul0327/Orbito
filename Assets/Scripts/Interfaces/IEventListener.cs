using System;
/// <summary>
/// EventDispatcher 로부터 이벤트를 수신받을 수 있는 interface 입니다.
/// 이벤트를 수신받기 위해서는, EventDispatcher에 먼저 등록해야 합니다.
/// 이벤트 수신을 중단하는 경우, EventDispatcher에서 반드시 삭제해야 합니다.
/// </summary>
public interface IEventListener
{
    /// <summary>
    /// 이벤트를 수신할 때 호출되는 함수입니다. 
    /// </summary>
    void OnEvent (IEvent e);
}
