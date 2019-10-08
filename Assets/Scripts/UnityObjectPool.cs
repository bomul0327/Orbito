using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기존의 싱글턴 구조를 변경합니다.
// Pool들을 Singleton 클래스에서 관리하던 것을 버리고, static한 Dictionary로 관리를 하게 만듭시다.
// 유니티의 오브젝트가 아닌 것들도 Pooling이 될 수 있기 때문에 이런 식으로 이름을 변경합니다.
// Json을 통해서 필요한 정보를 받아오고, 제작할 것이기 때문에, 유니티 인스펙터에서 무엇인가를 꼭 바꾸게 해야 하는 것은 피하는게 좋습니다.
// 다만 아직 Json 시스템을 완전히 구축하지 않았기 때문에 지금은 유니티 인스펙터를 통해서 관리를 해도 괜찮습니다.
public class UnityObjectPool : MonoBehaviour
{
    // 모든 초기화를 이제 풀에서 가져가는 것이 아니라, 게임을 관리하는 클래스가 초기화할 때, 필요한 것들을 불러내는 구조로 변경합니다.
    // 후에 오브젝트 풀이 얼마나 커질 지를 결정하거나, 몇 초 있다가 오브젝트 풀이 자동으로 Return을 하게 될 것인지도 추가가 될 수 있습니다. 그건 나중으로

    // 필요할만한 변수
    // 이 풀에서 관리하는 asset의 이름
    public string assetName;
    // 이 풀에서 Instantiate를 할 오브젝트
    public PooledUnityObject pooledPrefab;
    public GameObject prefab;

    // 풀링이 되고 있는 오브젝트를 관리할 리스트 혹은 배열 (배열로 한다면, Resize를 할 수 있도록 고려를 해야합니다.)
    public List<PooledUnityObject> objectPool = new List<PooledUnityObject>();
    public int poolCount = 0;

    // 추후에 필요할만한 변수
    // 이 풀이 max count를 넘으면 확장될 것인지를 결정하는 enum
    // 이 풀의 오브젝트들이 Return을 할 때, 프로그래머가 직접 호출할 것인가? 아니면 일정 시간이 지나면 자동으로 없앨 것인가를 정하게 될 enum 및 그에 따른 기타 필요한 변수들

    /// <summary>
    /// 만들어진 UnityObjectPool들을 관리하는 Dictionary입니다.
    /// Asset의 이름을 Key로 사용합니다.
    /// </summary>
    private static Dictionary<string, UnityObjectPool> poolDict = new Dictionary<string, UnityObjectPool>();

    /// <summary>
    /// 풀에 있는 오브젝트를 활성화 시키는 역할입니다.
    /// 만약에 정해진 pool count를 넘어갈 때는, 새로운 PooledUnityObject를 만들어주고, 그걸 List나 Array 등에 넣어서 추가도 해줘야 합니다.
    /// 파라메터들은 추후에도 추가가 될 수 있습니다. 예를 들면 이 오브젝트는 지정한 오브젝트를 따라갈 수 있게 만든다던지, 혹은 일정 시간이 지나면 자동으로 반환이 된다던지
    /// Instantiate를 할 때, asset의 이름을 활용해서 만드는 것을 추천합니다. 저희는 Json을 통해서 어떤 Object들이 풀링될 수 있을 것인지를 결정할 것이기 때문에.
    /// </summary>
    /// <param name="pos">Instantiate될 때, 오브젝트의 위치값</param>
    /// <param name="rot">Instantiate될 때, 오브젝트의 회전값</param>
    public PooledUnityObject Instantiate (Vector3 pos, Quaternion rot)
    {
        for (int i = 0; i < poolCount; i++)
        {
            PooledUnityObject obj = Instantiate<PooledUnityObject>(pooledPrefab, pos, rot, transform);
            objectPool.Add(obj);
            obj.SetActive(assetName, prefab, false);
        }
        //★poolCount 초과 시 새로 만들고 List에 추가하는 내용
        //서치를 해보니 Allocate 함수와 Pop 함수를 구분해서
        //Allocate가 Instantiate에 가까워 보이며
        //Pop에서 objectPool.count가 0보다 작을 때 Allocate(필요한 수)를 하는 방식이던데
        //그런식으로 구현을 해야 하나요?
        //참고) https://youngchangoon.tistory.com/21

        return null; 
        //★return값 null이 맞나요? 뭘 넣어도 에러가 나서 일단 null로 해놨습니다.
    }

    /// <summary>
    /// 특정 오브젝트를 풀에 반환하고 비활성화 시키는 역할입니다.
    /// </summary>
    /// <param name="obj">반환할 오브젝트</param>
    public void Return (PooledUnityObject obj)
    {
        obj.SetActive(assetName, prefab, false);
        objectPool.Add(obj);
    }

    /// <summary>
    /// 현재 사용하고 있는 오브젝트들을 전부 반환하고, 비활성화 시키는 역할입니다.
    /// </summary>
    public void ReturnAll ()
    {
        if (objectPool == null)
            return;

        for(int i = 0; i< objectPool.Count; i++)
        {
            PooledUnityObject pooledObj = objectPool[i];

            if(pooledObj != null)
            {
                Return(pooledObj);
            }
            
        }
    }

    /// <summary>
    /// 모든 오브젝트들을 없애고, 풀 자체를 해제해주는 역할입니다.
    /// 게임이 끝났을 때, 풀을 없애야할 때 불러줍니다.
    /// </summary>
    public void Dispose ()
    {
        if (objectPool == null)
            return;

        for(int i = 0; i< objectPool.Count; i++)
        {
            PooledUnityObject obj = objectPool[i];
            GameObject.Destroy(obj.gameObject);
        }
        objectPool = null;

    }

    /// <summary>
    /// UnityObjectPool을 만들거나, 이미 존재한다면 그 인스턴스를 가져옵니다.
    /// </summary>
    /// <param name="assetName"></param>
    public static UnityObjectPool GetOrCreate (string assetName)
    {
        //★poolDict.TryGetKey 써보려고 했는데 에러가 납니다.
        //System.Collections.Immutable?? 이걸 using해야되는 건가요? (using하면 에러남) 
        //https://docs.microsoft.com/ko-kr/dotnet/api/system.collections.immutable.iimmutabledictionary-2.trygetkey?view=netcore-3.0
        
        if (poolDict.ContainsKey(assetName))
        {
            //인스턴스를 가져온다.
            UnityObjectPool unityObjectPool = new UnityObjectPool();
            unityObjectPool = FindObjectOfType<UnityObjectPool>();
        }
        else
        {
            //UnityObjectPool을 만든다
            UnityObjectPool unityObjectPool = new UnityObjectPool();
            GameObject container = new GameObject(assetName + "_objectPool");
            container.AddComponent<UnityObjectPool>();

            poolDict.Add(assetName, unityObjectPool);
        }
        
        return null; //★여기도 return값 뭘로 설정해야 하나요?
    }
}
