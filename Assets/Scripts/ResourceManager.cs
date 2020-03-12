using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// 자원의 보유량
    /// </summary>
    public int currentResource = 0;
    float waitingTimeForIR = 0.1f;

    public void Awake()
    {
        // 세이브 데이터 등을 통해서 가지고 있는 자원들 받아올 것.
    }

    public IEnumerator IncreaseResource(Character character)
    {
        while (character.CharacterStateMachine.CurrentState is RevolveState)
        {
            yield return new WaitForSeconds(waitingTimeForIR);

            if (character.CharacterStateMachine.CurrentState is RevolveState)
            {
                currentResource++;
                Debug.Log(Time.time + " - " + currentResource);
            }
        }
    }
}


