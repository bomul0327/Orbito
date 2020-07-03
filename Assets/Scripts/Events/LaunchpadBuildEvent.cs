/// <summary>
/// 발사대가 건설되었을 때 발행되는 이벤트.
/// </summary>
struct LaunchpadBuildEvent:IEvent
{
    /// <summary>
    /// 건설된 발사대.
    /// </summary>
    public Launchpad launchpad;
}
