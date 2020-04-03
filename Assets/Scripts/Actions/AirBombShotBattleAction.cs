using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마우스 방향으로 유탄을 발사하는 액션
/// </summary>
public class AirBombShotBattleAction : ITriggerBattleAction
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

    public float ExplosionEffectDistance
    {
        get;
        set;
    }

    /// <summary>
    /// 발사 방향
    /// </summary>
    public Vector3 dir
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

    private string AirBombPrefabName = "AirBombExplosionEffect";

    public AirBombShotBattleAction(Character character)
    {
        this.character = character;
        this.dir = dir;

        // FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것
        FireRate = 3f;
        lastFireSuccessTime = -1;
        ExplosionEffectDistance = 5f;
    }

    void ITriggerBattleAction.Cancel()
    {
        
    }
    void ITriggerBattleAction.Start()
    {
        Vector3 targetPositionDeltaUnit = (Input.mousePosition - Camera.main.WorldToScreenPoint(character.transform.position)).normalized;

        var airBombPool = UnityObjectPool.GetOrCreate(AirBombPrefabName);
        airBombPool.SetOption(PoolScaleType.Limited, PoolReturnType.Auto);
        airBombPool.MaxPoolCapacity = 1;

        // FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것
        airBombPool.AutoReturnTime = 2f;

        airBombPool.Instantiate(character.transform.position + targetPositionDeltaUnit * ExplosionEffectDistance, new Quaternion(0, 0, 0, 0));

        var dir = Vector2.Perpendicular(targetPositionDeltaUnit);        
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
