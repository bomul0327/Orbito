using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모노비헤이비어가 아닌 상황 및 최대한 유니티 업데이트를 호출하지 않기 위해서 하나의 업데이트에서만 처리
/// </summary>
public class UpdateManager : Singleton<UpdateManager>
{
    private List<IUpdatable> updatableList = new List<IUpdatable>();

    private List<IFixedUpdatable> fixedList = new List<IFixedUpdatable>();

    private List<ILateUpdatable> lateList = new List<ILateUpdatable>();

    private void Awake()
    {

    }

    private void Update()
    {
        foreach (var item in updatableList)
        {
            item.OnUpdate(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        foreach (var item in fixedList)
        {
            item.OnFixedUpdate(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        foreach (var item in lateList)
        {
            item.OnLateUpdate(Time.deltaTime);
        }
    }

    public void AddUpdatable(IUpdatable updatable)
    {
        if (!updatableList.Contains(updatable))
        {
            updatableList.Add(updatable);
        }
    }

    public void AddFixedUpdatable(IFixedUpdatable updatable)
    {
        if (!fixedList.Contains(updatable))
        {
            fixedList.Add(updatable);
        }
    }

    public void AddLateUpdatable(ILateUpdatable updatable)
    {
        if (!lateList.Contains(updatable))
        {
            lateList.Add(updatable);
        }
    }

    public void RemoveUpdatable(IUpdatable updatable)
    {
        if (updatableList.Contains(updatable))
        {
            updatableList.Remove(updatable);
        }
    }

    public void RemoveFixedUpdatable(IFixedUpdatable updatable)
    {
        if (fixedList.Contains(updatable))
        {
            fixedList.Remove(updatable);
        }
    }

    public void RemoveLateUpdatable(ILateUpdatable updatable)
    {
        if (lateList.Contains(updatable))
        {
            lateList.Remove(updatable);
        }
    }
}
