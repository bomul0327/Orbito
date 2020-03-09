using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// 자원의 보유량
    /// </summary>
    public int currentResource = 0;
    private IEnumerator coroutine;
    float waitingTimeForIR = 0.1f;

    public void Awake()
    {
        // 세이브 데이터 등을 통해서 가지고 있는 자원들 받아올 것.
    }

    public void StartIncreaseResource(Character character, IState state)
    {
        coroutine = IncreaseResource(character, state);
        StartCoroutine(coroutine);
    }

    public void StopIncreaseResource()
    {
        StopCoroutine(coroutine);
    }

    public IEnumerator IncreaseResource(Character character, IState state)
    {
        yield return new WaitForSeconds(waitingTimeForIR);

        while (character.CharacterStateMachine.CurrentState.Equals(state))
        {
            currentResource++;
            Debug.Log(currentResource);
            yield return new WaitForSeconds(waitingTimeForIR);
        }
    }
}


