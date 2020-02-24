using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerBattleAction
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
    /// 배틀 액션을 발생시키려고 했을 때 호출
    /// </summary>
    /// <returns>실제로 발동될 때, true, 발동이 안 될 때는 false</returns>
    bool Trigger();

    /// <summary>
    /// 실제로 실행하는 처음에 호출
    /// </summary>
    void Start();

    /// <summary>
    /// 배틀 액션이 취소가 되었을 때 호출
    /// </summary>
    void Cancel();

}
