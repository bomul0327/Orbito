using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : FieldObject, IUpdatable
{
    /// <summary>
    /// 탄환이 소멸되기 전까지 최대 이동 가능한 거리.
    /// </summary>
    public float MaxDistance
    {
        get;
        set;
    }

    /// <summary>
    /// 초당 최대 이동 속도.
    /// </summary>
    public float Speed
    {
        get;
        set;
    }

    public DamageInfo damageInfo;

    /// <summary>
    /// 현재까지 탄환이 이동한 거리. MaxDistance 이상 이동할 수 없음.
    /// </summary>
    private float currentDistance;

    private void OnEnable()
    {
        UpdateManager.Instance.AddUpdatable(this);
        currentDistance = 0;
        damageInfo.Damage = 1;
    }

    private void OnDisable()
    {
        UpdateManager.Instance?.RemoveUpdatable(this);
    }

    /// <summary>
    /// projectile 생성
    /// </summary>
    /// <param name="bulletPrefabName">발사할 총알 프리팹</param>
    /// <param name="position">총알의 초기 위치</param>
    /// <param name="rotation">총알의 발사 각도</param>
    /// <param name="speed">총알의 속도</param>
    /// <param name="maxDistance">총알이 이동 가능한 최대 거리</param>
    public static void Create(string bulletPrefabName, Vector3 position, Quaternion rotation, float speed, float maxDistance)
    {
        var bulletObjectPool = UnityObjectPool.GetOrCreate(bulletPrefabName);
        bulletObjectPool.SetOption(PoolScaleType.Unlimited, PoolReturnType.Manual);

        var bulletObject = bulletObjectPool.Instantiate(position, rotation).gameObject;

        var bulletComponent = bulletObject.GetComponent<Projectile>();
        bulletComponent.Speed = speed;
        bulletComponent.MaxDistance = maxDistance;
    }

    public void OnUpdate(float dt)
    {
        float deltaSpeed = Speed * dt;

        MoveForward(deltaSpeed);
        
        // Projectile의 이동거리가 MaxDistance를 초과했을 경우, 탄환을 소멸시킨다.
        // 지금은 이동 거리에 따라서만 탄환 소멸 여부를 판단하지만, 
        // 안전을 위해 최대 생존 시간을 설정하는 것도 고려해야 할 수 있음.
        if (currentDistance > MaxDistance)
        {
            OnBulletDispose();
        }
    }

    /// <summary>
    /// 전면 방향으로 직선 이동.
    /// </summary>
    /// <param name="deltaSpeed">프레임 당 이동 속도</param>
    public void MoveForward(float deltaSpeed)
    {
        transform.position += transform.up * deltaSpeed;
        currentDistance += deltaSpeed;
    }

    /// <summary>
    /// Projectile이 일정 거리를 이동하는 동안 충돌하지 못하는 경우에 실행.
    /// </summary>
    public void OnBulletDispose()
    {
        //여기서 소멸과 관련된 작업을 처리(예: 소멸 이펙트 등).

        //소멸 작업이 끝나면 이 객체를 Object Pool에 반환.
        UnityObjectPool.GetOrCreate("MultiShotBullet").Return(GetComponent<PooledUnityObject>());
    }

    /// <summary>
    /// Projectile이 다른 오브젝트와 충돌하는 경우에 실행.
    /// </summary>
    public void OnBulletHit(Collider2D other)
    {
        //여기서 탄환 충돌과 관련된 작업을 처리(예: Damage관련 커맨드, 충돌 이펙트 등).

        // Damage 주기
        FieldObject fieldObject = other.gameObject.GetComponentInParent<FieldObject>();
        
        using (var cmd = CommandFactory.GetOrCreate<DamageCommand>(fieldObject, damageInfo))
        {
            CommandDispatcher.Publish(cmd);
        }

        //지금은 빠른 테스트를 위해 여기서 바로 피격 Effect를 생성하지만, 
        //나중에는 다른 방식으로 변경할 수 있음.
        var hitParticlePool = UnityObjectPool.GetOrCreate("ProjectileExplosionEffect");
        hitParticlePool.SetOption(PoolScaleType.Unlimited, PoolReturnType.Auto);
        hitParticlePool.AutoReturnTime = 1.5f;
        hitParticlePool.Instantiate(transform.position, transform.rotation);

        //피격 작업이 끝나면 이 객체를 ObjectPool에 반환.
        UnityObjectPool.GetOrCreate("MultiShotBullet").Return(GetComponent<PooledUnityObject>());

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        OnBulletHit(other);
    }
}
