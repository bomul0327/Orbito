using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 예시로 만들어놓은 배틀액션
/// </summary>
public class NormalBattleAction : ITriggerBattleAction
{
    Character character;

    public NormalBattleAction(Character character)
    {
        this.character = character;
    }

    void ITriggerBattleAction.Cancel()
    {
        Debug.Log("Canceled");
    }
    void ITriggerBattleAction.Start()
    {
        Debug.Log("Start");
    }

    bool ITriggerBattleAction.Trigger()
    {
        using (var cmd = CommandFactory.GetOrCreate<TriggerBattleActionCommand>(character, this))
        {
            CommandDispatcher.Publish(cmd);
            return true;
        }
    }
}
