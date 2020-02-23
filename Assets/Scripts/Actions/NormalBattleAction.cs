using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일직선으로 총알이 나가는 공격
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
        if (character.gameObject.activeSelf == false)
        {
            return;
        }

        var bulletObjectPool = UnityObjectPool.GetOrCreate("Bullet");
        bulletObjectPool.SetOption(PoolScaleType.Unlimited, PoolReturnType.Manual);
        bulletObjectPool.Instantiate(character.transform.position, character.transform.rotation);
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
