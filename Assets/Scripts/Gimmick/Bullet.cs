using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : FieldObject, IUpdatable
{
    public float moveSpeed = 20.0f;

    public void OnEnable()
    {
        UpdateManager.Instance.AddUpdatable(this);
    }

    private void OnDisable()
    {
        UpdateManager.Instance.RemoveUpdatable(this);
    }


    void IUpdatable.OnUpdate(float dt)
    {
        transform.Translate(0, Time.deltaTime * moveSpeed, 0);

        // TODO : 적에 부딪히면 총알 사라지게

        // 총알이 화면 밖으로 나갔을 때 총알을 제거한다
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position); //월드 좌표를 스크린 좌표로 변형한다.
        if (view.y < -50 || view.y > 400 || view.x < -100 || view.x > 650)
        {
            var bulletObjectPool = UnityObjectPool.GetOrCreate("Bullet");
            bulletObjectPool.Return(this.GetComponent<PooledUnityObject>());
        }
    }

}
