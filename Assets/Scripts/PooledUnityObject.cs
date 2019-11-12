using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티의 오브젝트가 아닌 것들도 Pooling이 될 수 있기 때문에 이런 식으로 이름을 변경합니다.
/// <summary>
/// 풀링이 되는 오브젝트 자체에 달리는 스크립트입니다.
/// </summary>
public class PooledUnityObject : MonoBehaviour
{
    // 이 스크립트에는 UnityObjectPool에 영향을 줄 수 있는 것들은 구현하지 않는게 좋습니다.
    // 만약 그런 식의 구현이 필요하다면, 본인이 생각하고 있는 방법이 맞는건지 생각해봅시다.
    // 이 스크립트에서는 오브젝트 하나하나에만 적용될 수 있다고 생각하시면 됩니다.

    // 추후에는 특정 오브젝트를 따라다니거나, 하는 것들을 추가로 만들어야할 수도 있습니다.
    

    /// <summary>
    /// Pool에서 이 오브젝트를 활성화 혹은 비활성화할 때 사용할 메쏘드입니다.
    /// Parameter들은 본인의 입맛에 추가하세요.
    /// 추천드리는 것은 Instantiate를 할 때, 필요한 것들입니다.
    /// </summary>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
