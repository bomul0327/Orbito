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
    /// <summary>
    /// 만들어진 UnityObjectPool들을 관리하는 Dictionary입니다.
    /// Asset의 이름을 Key로 사용합니다.
    /// </summary>
    private static Dictionary<string, UnityObjectPool> poolDict = new Dictionary<string, UnityObjectPool>();

    //기본 capacity 및 최대 capacity(resizeable할 경우) : 현재 임시로 설정
    public static int defaultPoolCapacity = 10;
    public static int maxPoolCapacity = 15;

    /// <summary>
    /// UnityObjectPool을 만들거나, 이미 존재한다면 그 인스턴스를 가져옵니다.
    /// Default : poolCapacity=10, Resizeable=disable, AutoReturn=disable
    /// </summary>
    /// <param name="assetName"></param>
    public static UnityObjectPool GetOrCreate(string _assetName)
    {
        UnityObjectPool instance;

        if (poolDict.TryGetValue(_assetName, out instance))
        {
            //인스턴스를 가져온다.
        }
        else
        {
            //UnityObjectPool을 만든다
            instance = new GameObject(_assetName + "ObjectPool").AddComponent<UnityObjectPool>();
            instance.assetName = _assetName;
            instance.poolCapacity = defaultPoolCapacity;
            instance.isResizeable = Resizeable.Disable;
            instance.isAutoReturn = AutoReturn.Disable;
            var targetObj = Resources.Load(_assetName, typeof(GameObject)) as GameObject;
            instance.pooledObj = targetObj.AddComponent<PooledUnityObject>();
            instance.Allocate();
            poolDict.Add(_assetName, instance);
        }
        return instance;
    }

    // 이 풀에서 관리하는 asset의 이름
    public string assetName { get; private set; }

    // 이 풀에서 Instantiate를 할 오브젝트
    public PooledUnityObject pooledObj;

    // 풀링이 되고 있는 오브젝트를 관리할 리스트
    private Queue<PooledUnityObject> availablePool = new Queue<PooledUnityObject>();
    private List<PooledUnityObject> activePool = new List<PooledUnityObject>();

    public int poolCapacity { get; private set; }

    public enum Resizeable { Enable, Disable }; //capacity가 늘어 날 수 있는지 여부
    public enum AutoReturn { Enable, Disable }; //일정 시간이 지나면 자동 반환되는지 여부

    public Resizeable isResizeable;
    public AutoReturn isAutoReturn;
    public float autoReturnTime = 10.0f;//자동 반환까지 걸리는 시간(10초로 임시 설정)

    

    //만약에 정해진 pool count를 넘어갈 때는, 새로운 PooledUnityObject를 만들어주고, 그걸 List나 Array 등에 넣어서 추가도 해줘야 합니다.
    // 파라메터들은 추후에도 추가가 될 수 있습니다. 예를 들면 이 오브젝트는 지정한 오브젝트를 따라갈 수 있게 만든다던지, 혹은 일정 시간이 지나면 자동으로 반환이 된다던지
    // Instantiate를 할 때, asset의 이름을 활용해서 만드는 것을 추천합니다. 저희는 Json을 통해서 어떤 Object들이 풀링될 수 있을 것인지를 결정할 것이기 때문에.
    /// <summary>
    /// 풀에 있는 오브젝트를 활성화하고 반환합니다.
    /// </summary>
    /// <param name="pos">Instantiate될 때, 오브젝트의 위치값</param>
    /// <param name="rot">Instantiate될 때, 오브젝트의 회전값</param>
    public PooledUnityObject Instantiate (Vector3 pos, Quaternion rot)
    {
        PooledUnityObject obj;

        int poolCount = activePool.Count;

        if (poolCount < poolCapacity)
        {
            obj = availablePool.Dequeue();
        }
        else if(isResizeable == Resizeable.Enable && poolCapacity < maxPoolCapacity)
        {
            obj = Instantiate<PooledUnityObject>(pooledObj, transform);
            obj.name = assetName + poolCount.ToString();
            poolCapacity++;
        }
        else
        {
            obj = activePool[0]; //제일 오래된 object 반환
            activePool.Remove(obj);
            activePool.Add(obj);
            obj.transform.SetPositionAndRotation(pos, rot);
            if (isAutoReturn == AutoReturn.Enable)
            {
                //기존에 실행되던 타이머를 멈추고 새로 시작
                StopCoroutine(obj.returnTimer);
                obj.returnTimer = ReturnTimer(autoReturnTime, obj);
                StartCoroutine(obj.returnTimer);
            }
            return obj;
        }
        activePool.Add(obj);
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(pos, rot);

        if(isAutoReturn == AutoReturn.Enable)
        {
            obj.returnTimer = ReturnTimer(autoReturnTime, obj);
            StartCoroutine(obj.returnTimer);
        }

        return obj;
    }

    /// <summary>
    /// 일정 시간이 지난 후 자동으로 오브젝트를 풀에 반환합니다.
    /// </summary>
    /// <param name="timeToReturn">반환까지 걸리는 시간</param>
    /// <param name="obj">반환 대상 오브젝트</param>
    /// <returns></returns>
    private IEnumerator ReturnTimer(float timeToReturn, PooledUnityObject obj)
    {
        while(timeToReturn >= 0)
        {
            timeToReturn -= Time.deltaTime;
            yield return null;
        }
        Return(obj);
    }

    private void Allocate()
    {
        for (int i = 0; i < poolCapacity; ++i)
        {
            PooledUnityObject obj = Instantiate<PooledUnityObject>(pooledObj, transform);
            obj.name = assetName + i.ToString();
            
            availablePool.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    
    /// <summary>
    /// 특정 오브젝트를 풀에 반환하고 비활성화 시키는 역할입니다.
    /// </summary>
    /// <param name="obj">반환할 오브젝트</param>
    public void Return (PooledUnityObject obj)
    {
        if(activePool.Contains(obj))
        {
            obj.SetActive(false);
            activePool.Remove(obj);
            availablePool.Enqueue(obj);
            if(obj.returnTimer != null)
            {
                StopCoroutine(obj.returnTimer);
                obj.returnTimer = null;
            }
        }
    }

    /// <summary>
    /// 현재 사용하고 있는 오브젝트들을 전부 반환하고, 비활성화 시키는 역할입니다.
    /// </summary>
    public void ReturnAll ()
    {
        if (activePool == null)
            return;

        for(int i = 0; i< activePool.Count; i++)
        {
            PooledUnityObject activeObj = activePool[i];

            if(activeObj != null)
            {
                Return(activeObj);
            }
        }
    }

    /// <summary>
    /// 모든 오브젝트들을 없애고, 풀 자체를 해제해주는 역할입니다.
    /// 게임이 끝났을 때, 풀을 없애야할 때 불러줍니다.
    /// </summary>
    public void Dispose ()
    {
        if (activePool == null)
            return;

        ReturnAll();
        
        while(availablePool.Count != 0)
        {
            PooledUnityObject obj = availablePool.Dequeue();
            Destroy(obj.gameObject);
        }
        availablePool = null;
        activePool = null;
        Destroy(gameObject);
    }

    
}