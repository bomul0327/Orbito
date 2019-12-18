using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// capacity가 늘어 날 수 있는지 여부
// Static : 고정 / Limited : 설정된 maxPoolCapacity까지 Capacity 확장 가능 / Unlimited : 제한 없이 확장 가능
public enum PoolScaleType { Static, Limited, Unlimited };

// 일정 시간이 지나면 자동 반환되는지 여부
// Manual : 사용자가 직접 반환 / Auto : 일정 시간 이후 자동 반환(직접 반환 불가)
public enum PoolReturnType { Manual, Auto };

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

    //기본 capacity : 현재 임시로 설정
    public static int DefaultPoolCapacity = 10;

    /// <summary>
    /// UnityObjectPool을 만들거나, 이미 존재한다면 그 인스턴스를 가져옵니다.
    /// Default : poolCapacity=10, PoolScaleType=Static, PoolReturnType=Manual
    /// </summary>
    /// <param name="assetFileName">풀링하고자 하는 파일 이름</param>
    public static UnityObjectPool GetOrCreate(string assetFileName)
    {
        UnityObjectPool instance;

        if (poolDict.TryGetValue(assetFileName, out instance))
        {
            //인스턴스를 가져온다.
        }
        else
        {
            //UnityObjectPool을 만든다
            instance = new GameObject(assetFileName + "ObjectPool").AddComponent<UnityObjectPool>();
            instance.AssetName = assetFileName;
            instance.PoolCapacity = DefaultPoolCapacity;
            instance.myPoolScaleType = PoolScaleType.Static;
            instance.myPoolReturnType = PoolReturnType.Manual;
            var targetObj = Resources.Load(assetFileName, typeof(GameObject)) as GameObject;
            instance.PooledObj = targetObj.AddComponent<PooledUnityObject>();
            instance.Allocate();
            poolDict.Add(assetFileName, instance);
        }
        return instance;
    }

    // 이 풀에서 관리하는 asset의 이름
    public string AssetName { get; private set; }

    // 이 풀에서 Instantiate를 할 오브젝트
    public PooledUnityObject PooledObj;

    // 풀링이 되고 있는 오브젝트를 관리할 리스트
    private Queue<PooledUnityObject> availablePool = new Queue<PooledUnityObject>();
    private List<PooledUnityObject> activePool = new List<PooledUnityObject>();

    public int PoolCapacity { get; private set; }
    public int MaxPoolCapacity = 15; //PoolScaleTypeType이 Limited인 경우 늘어날 수 있는 최대 capacity

    private PoolScaleType myPoolScaleType;
    private PoolReturnType myPoolReturnType;
    public float AutoReturnTime = 10.0f;//자동 반환까지 걸리는 시간(10초로 임시 설정)
    private Queue<IEnumerator> timerQueue = new Queue<IEnumerator>(); // 코루틴 관리를 위한 큐

    /// <summary>
    /// 풀의 설정을 변경합니다.
    /// 활성화 중인 오브젝트가 있을 경우 설정이 적용되지 않고 false를 반환합니다.
    /// </summary>
    /// <param name="cap">PoolScaleType 종류</param>
    /// <param name="ret">PoolReturnType 종류</param>
    /// <returns></returns>
    public bool SetOption(PoolScaleType cap, PoolReturnType ret)
    {
        if(activePool.Count > 0)
        {
            return false;
        }
        myPoolScaleType = cap;
        myPoolReturnType = ret;
        return true;
    }

    //만약에 정해진 pool count를 넘어갈 때는, 새로운 PooledUnityObject를 만들어주고, 그걸 List나 Array 등에 넣어서 추가도 해줘야 합니다.
    // 파라메터들은 추후에도 추가가 될 수 있습니다. 예를 들면 이 오브젝트는 지정한 오브젝트를 따라갈 수 있게 만든다던지, 혹은 일정 시간이 지나면 자동으로 반환이 된다던지
    // Instantiate를 할 때, asset의 이름을 활용해서 만드는 것을 추천합니다. 저희는 Json을 통해서 어떤 Object들이 풀링될 수 있을 것인지를 결정할 것이기 때문에.
    /// <summary>
    /// 풀에 있는 오브젝트를 활성화하고 반환합니다. 더 이상 오브젝트를 활성화할 수 없다면 null값을 반환합니다.
    /// </summary>
    /// <param name="pos">Instantiate될 때, 오브젝트의 위치값</param>
    /// <param name="rot">Instantiate될 때, 오브젝트의 회전값</param>
    public PooledUnityObject Instantiate (Vector3 pos, Quaternion rot)
    {
        PooledUnityObject obj;

        int poolCount = activePool.Count;

        if (poolCount < PoolCapacity)
        {
            obj = availablePool.Dequeue();
        }
        else if((myPoolScaleType == PoolScaleType.Limited && PoolCapacity < MaxPoolCapacity) || myPoolScaleType == PoolScaleType.Unlimited)
        {
            obj = Instantiate<PooledUnityObject>(PooledObj, transform);
            obj.name = AssetName + poolCount.ToString();
            PoolCapacity++;
        }
        else
        {
            return null;
        }
        activePool.Add(obj);
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(pos, rot);

        if(myPoolReturnType == PoolReturnType.Auto)
        {
            IEnumerator timer = ReturnByTime(AutoReturnTime, obj);
            timerQueue.Enqueue(timer);
            StartCoroutine(timer);
        }

        return obj;
    }

    /// <summary>
    /// 일정시간이 지난 뒤 오브젝트를 풀에 자동으로 반환합니다.
    /// </summary>
    /// <param name="timeToReturn">반환까지 걸리는 시간</param>
    /// <param name="obj">반환 대상 오브젝트</param>
    /// <returns></returns>
    private IEnumerator ReturnByTime(float timeToReturn, PooledUnityObject obj)
    {
        yield return new WaitForSeconds(timeToReturn);
        timerQueue.Dequeue();
        Return(obj, true);
    }

    private void Allocate()
    {
        for (int i = 0; i < PoolCapacity; ++i)
        {
            PooledUnityObject obj = Instantiate<PooledUnityObject>(PooledObj, transform);
            obj.name = AssetName + i.ToString();
            
            availablePool.Enqueue(obj);
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 특정 오브젝트를 풀에 반환하고 비활성화 시키는 역할입니다.
    /// PoolReturnTypeType이 Auto일 경우에는 작동되지 않습니다.
    /// </summary>
    /// <param name="obj">반환할 오브젝트</param>
    /// <param name="byCoroutine">시스템에 의한 호출인지 여부</param>
    /// 
    public void Return (PooledUnityObject obj, bool bySystem = false)
    {
        if(myPoolReturnType == PoolReturnType.Auto && !bySystem)
        {
            return;
        }

        if(activePool.Contains(obj))
        {
            obj.SetActive(false);
            activePool.Remove(obj);
            availablePool.Enqueue(obj);
        }
    }

    /// <summary>
    /// 현재 사용하고 있는 오브젝트들을 전부 반환하고, 비활성화 시키는 역할입니다.
    /// </summary>
    public void ReturnAll ()
    {
        if (activePool == null)
            return;

        int activeNum = activePool.Count;
        for(int i = 0; i< activeNum; i++)
        {
            PooledUnityObject activeObj = activePool[0];

            if(activeObj == null)
            {
                return;
            }
            if(myPoolReturnType == PoolReturnType.Auto)
            {
                StopCoroutine(timerQueue.Dequeue());
            }
            Return(activeObj, true);
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