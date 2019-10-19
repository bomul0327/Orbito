using System;

public interface IBattleAction
{
    /// <summary>
    /// 이 액션이 활성화 되었는지를 체크
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// 이 액션이 패시브로 작동하는지를 체크
    /// </summary>
    bool IsPassive { get; }

    /// <summary>
    /// 이 액션의 레벨
    /// </summary>
    int Level { get; }

    /// <summary>
    /// 이 액션의 실행 가능 여부를 체크
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// 이 풀을 사용하는 리소스들이 로드가 되었는지? 준비가 되었는지를 체크
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// 이 액션이 시작할 때, 한 번 호출됨
    /// </summary>
    void Start();

    /// <summary>
    /// 이 액션을 시도했을 때, 한 번 호출됨
    /// true면 Start로 이어질 수 있고, false면 액션이 실행되지 않음.
    /// </summary>
    /// <returns>실행 가능 여부</returns>
    bool Trigger();

    /// <summary>
    /// 이 액션이 취소될 때, 동작
    /// </summary>
    void Cancel();
}
