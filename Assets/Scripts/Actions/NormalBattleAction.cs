using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일직선으로 Projectile을 발사하는 BattleAction 클래스
/// </summary>
public class NormalBattleAction : ITriggerBattleAction
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

    bool ITriggerBattleAction.IsActive { get; }

    bool ITriggerBattleAction.IsPassive { get; }

    int ITriggerBattleAction.Level { get; }

    bool ITriggerBattleAction.IsAvailable { get; }

    bool ITriggerBattleAction.IsLoaded { get; }

    private float lastFireSuccessTime;

    private string bulletPrefabName = "MultiShotBullet";

    public NormalBattleAction(Character character)
    {
        this.character = character;

        // FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것
        FireRate = 0.2f;
        lastFireSuccessTime = -1;
    }

    void ITriggerBattleAction.Cancel()
    {
        
    }
    void ITriggerBattleAction.Start()
    {
        // FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것
        float speed = 50f;
        float maxDistance = 50f;
        DamageInfo damageInfo;
        damageInfo.Damage = 1;

        Projectile.Create(bulletPrefabName, character.transform.position, character.transform.rotation, speed, maxDistance, damageInfo);

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
