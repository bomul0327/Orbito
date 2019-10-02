using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //싱글톤
    public static ObjectPool instance = null;
    //오브젝트풀
    public List<PooledObject> objectPool = new List<PooledObject>();

    void Awake()
    {
        //ObjectPool 클래스를 인스턴스에 대입
        instance = this;

        //오브젝트 풀 초기화
        for (int ix = 0; ix < objectPool.Count; ++ix)
        {
            objectPool[ix].Initialize(transform);
        }
    }

    public bool PushToPool(string itemName, GameObject item, Transform parent = null)
    {
        //사용한 객체를 ObjectPool에 반환
        PooledObject pool = GetPoolItem(itemName);
        if(pool == null)
        {
            return false;
        }
        pool.PushToPool(item, parent == null ? transform : parent);
        return true;
        
    }

    public GameObject PoPFromPool(string itemName, Transform parent = null)
    {
        //필요한 객체를 오브젝트 풀에 요청할 때 사용할 함수
        PooledObject pool = GetPoolItem(itemName);
        if(pool == null)
        {
            return null;
        }
        return pool.PopFromPool(parent);

    }

    PooledObject GetPoolItem(string itemName)
    {
        //전달된 itemName파라미터와 같은 이름을 가진 오브젝트 풀을 검색하고
        //검색에 성공하면 그 결과를 리턴, 실패하면 null 리턴
        for(int ix = 0; ix < objectPool.Count; ++ix)
        {
            if (objectPool[ix].poolItemName.Equals(itemName))
            {
                return objectPool[ix];
            }
        }
        return null;
    }
    

    //비활성화
    public void ClearItem()
    {
        if (objectPool == null)
            return;

        int count = objectPool.Count;

        for(int i = 0; i < count; i++)
        {
            PooledObject pooledObject = objectPool[i];
            if(pooledObject != null && pooledObject.active)
            {
                pooledObject.active = false;
                pooledObject.gameObject.SetActive(false);
                //★pooledObject의 active 값을 조정하는 부분을 어떻게 넣어야 할 지..
            }
        }
    }

    //해제
    public void Dispose()
    {
        if (objectPool == null)
            return;

        int count = objectPool.Count;

        for(int i = 0; i < count; i++)
        {
            PooledObject obj = objectPool[i];
            GameObject.Destroy(obj.gameObject);
        }
        objectPool = null;
    }


}
