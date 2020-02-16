using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerBattleAction
{
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
