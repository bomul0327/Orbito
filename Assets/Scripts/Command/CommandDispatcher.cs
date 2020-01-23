using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Command를 관리하고, 실행하는 클래스
/// 모든 Command들은 여기를 무조건 통하게 해야 한다.
/// </summary>
public static class CommandDispatcher
{
    /// <summary>
    /// 커맨드를 실행시킵니다.
    /// </summary>
    public static void Publish(ICommand command)
    {
        command.Execute();
    }
}
