using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public string poolItemName = string.Empty;
    public GameObject prefab = null;
    public int poolCount = 0;
    public bool active;
    [SerializeField]
    private List<GameObject> poolList = new List<GameObject>();
    
    public void Initialize(Transform parent = null)
    {
        //PooledObject 객체를 초기화 할 때 처음 한 번만 호출
        //poolCount에 지정한 수 만큼 객체를 생성해서 poolList 리스트에 추가
        for(int ix = 0; ix < poolCount; ++ix)
        {
            poolList.Add(CreateItem(parent));
        }
    }

    public void PushToPool(GameObject item, Transform parent = null)
    {
        //사용한 객체를 다시 오브젝트 풀에 반환
        item.transform.SetParent(parent);
        item.SetActive(false);
        poolList.Add(item);

    }

    public GameObject PopFromPool(Transform parent = null)
    {
        //객체가 필요할 때 오브젝트 풀에 요청하는 용도로 사용할 함수
        if(poolList.Count == 0)
        {
            poolList.Add(CreateItem(parent));
        }
        GameObject item = poolList[0];
        poolList.RemoveAt(0);
        return item;
        
    }
    
     //★객체가 poolCount보다 많을 때 poolList 리스트를 확장시키는 함수

    private GameObject CreateItem(Transform parent = null)
    {
        //prefab 변수에 지정된 게임 오브젝트를 생성하는 역할
        GameObject item = Object.Instantiate(prefab) as GameObject;
        item.name = poolItemName;
        item.transform.SetParent(parent);
        item.SetActive(false);
        return item;
    }
}
