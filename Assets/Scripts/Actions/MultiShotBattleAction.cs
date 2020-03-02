using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 부채꼴 형태로 Projectile을 발사하는 BattleAction 클래스
/// </summary>
public class MultiShotBattleAction : ITriggerBattleAction
{
    Character character;

    /// <summary>
    /// 한 번에 발사하는 총탄의 갯수.
    /// </summary>
    public int BulletCountPerShot
    {
        get;
        set;
    }

    /// <summary>
    /// 총탄이 발사되는 영역의 각도.
    /// </summary>
    public float Range
    {
        get;
        set;
    }

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

    public MultiShotBattleAction(Character character)
    {
        this.character = character;

        //FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것.
        //지금은 테스트를 위해 여기에서 적당한 값을 설정함.
        BulletCountPerShot = 6;
        Range = 90;
        FireRate = 0.2f;

        lastFireSuccessTime = -1;
    }

    void ITriggerBattleAction.Cancel()
    {

    }

    void ITriggerBattleAction.Start()
    {
        float angleDelta = Range / BulletCountPerShot;
        float initAngle = (BulletCountPerShot - 1) * 0.5f * angleDelta;

        var basePosition = character.transform.position;
        var baseRotation = character.transform.rotation;

        for (int i = 0; i < BulletCountPerShot; i++)
        {
            var bulletInitPosition = basePosition + new Vector3(0, 0, 0);
            var bulletInitRotation = baseRotation * Quaternion.Euler(0, 0, initAngle - angleDelta * i);

            // FIXME: JSON 시스템이 준비되면 JSON 데이터에서 받아올 것.
            // 지금은 테스트를 위해 여기에서 Projectile의 변수를 바로 설정함.

            // FIXME2: Projectile 컴포넌트의 변수값을 설정해주기 위해 모든 총탄 객체에 대해서
            // GetComponent를 사용하는 것이 썩 좋아보이지는 않음.
            // 총탄별로 다른 변수값을 가진다면 모르지만, 그것도 아님.
            // 미리 값이 설정된 Prefab을 사용하는 방법도 있겠지만,
            // 자세한 내용은 TD님과 상의해 볼 것.

            float speed = 50f;
            float maxDistance = 50f;

            Projectile.Create(bulletPrefabName, bulletInitPosition, bulletInitRotation, speed, maxDistance);

        }

    }

    bool ITriggerBattleAction.Trigger()
    {
        float elapsedTime = Time.time - lastFireSuccessTime;

        //발사주기보다 더 많은 시간이 지나야 다시 발사할 수 있음.
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
