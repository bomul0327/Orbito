using System;
using UnityEngine;

/// <summary>
/// 배틀 액션을 실행하기 위한 커맨드
/// </summary>
public class TriggerBattleActionCommand : ICommand
{
    Character character;

    ITriggerBattleAction battleAction;

    void ICommand.Execute()
    {
        battleAction.Start();
    }

    void ICommand.SetData(params object[] values)
    {
        character = (Character)values[0];
        battleAction = (ITriggerBattleAction)values[1];
    }

    void IDisposable.Dispose()
    {
        character = null;
    }
}
