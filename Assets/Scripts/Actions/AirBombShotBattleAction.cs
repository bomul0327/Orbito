using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마우스 방향으로 유탄을 발사하는 액션
/// </summary>
public class AirbombShotBattleAction : ITriggerBattleAction
{
    Character character;

    /// <summary>
    /// 발사 주기.
    /// </summary>
    public float FireRate
    {
        get;
        set;
    }

    public float Distance
    {
        get;
        set;
    }

    bool ITriggerBattleAction.IsActive { get; }

    bool ITriggerBattleAction.IsPassive { get; }

    int ITriggerBattleAction.Level { get; }

    bool ITriggerBattleAction.IsAvailable { get; }

    bool ITriggerBattleAction.IsLoaded { get; }

    private float lastFireSuccessTime;

    string AirbombPrefabName = "AirbombExplosionEffect";

    public AirbombShotBattleAction(Character character)
    {
        this.character = character;

        // FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것
        FireRate = 3f;
        lastFireSuccessTime = -1;
        Distance = 5f;
    }

    void ITriggerBattleAction.Cancel()
    {
        
    }
    void ITriggerBattleAction.Start()
    {
        Vector3 targetUnitVector = (Input.mousePosition - Camera.main.WorldToScreenPoint(character.transform.position)).normalized;

        var airBombPool = UnityObjectPool.GetOrCreate(AirbombPrefabName);

        airBombPool.Instantiate(character.transform.position + targetUnitVector * Distance, new Quaternion(0, 0, 0, 0));

        // character.Behaviour.LookDirection(character.transform.position - targetUnitVector);
        var dir = Vector2.Perpendicular(targetUnitVector);        
        float angle = Mathf.Acos(dir.x) * Mathf.Rad2Deg;
        if (dir.y < 0) angle *= -1;
        character.transform.localRotation = Quaternion.Euler(0,0, angle);
    }

    bool ITriggerBattleAction.Trigger()
    {
        float elapsedTime = Time.time - lastFireSuccessTime;

        // 발사주기보다 더 많은 시간이 지나야 다시 발사 가능
        if (elapsedTime < FireRate)
        {
            return false;
        }

        lastFireSuccessTime = Time.time;

        using (var cmd = CommandFactory.GetOrCreate<TriggerBattleActionCommand>(character, this))
        {
            CommandDispatcher.Publish(cmd);
            return true;
        }
    }
}
