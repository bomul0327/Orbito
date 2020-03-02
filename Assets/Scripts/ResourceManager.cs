using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// 자원의 보유량
    /// </summary>
    public int currentResource = 0;
    Character character;

    public void Awake()
    {
        // 세이브 데이터 등을 통해서 가지고 있는 자원들 받아올 것.
    }

    public void StartIncreasing(Character character)
    {
        this.character = character;
        StartCoroutine("IncreaseResource");
    }

    IEnumerator IncreaseResource()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            character.gimmickBehaviour.IncreaseResource();
            yield return new WaitForSeconds(0.1f);
        }
    }
}


