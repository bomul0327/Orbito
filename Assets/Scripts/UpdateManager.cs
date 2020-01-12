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

    // End of Frame에서 삭제될 Updatable 보관하는 큐
    private Queue<IUpdatable> removeUpdatableQueue = new Queue<IUpdatable>();

    private Queue<IFixedUpdatable> removeFixedUpdatableQueue = new Queue<IFixedUpdatable>();

    private Queue<ILateUpdatable> removeLateUpdatableQueue = new Queue<ILateUpdatable>();

    /// <summary>
    /// 제거 프로세스 실행 중인지 체크하는 플래그
    /// </summary>
    private bool isActivatedRemoveProcess = false;

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
            removeUpdatableQueue.Enqueue(updatable);
        }

        if (!isActivatedRemoveProcess)
        {
            StartCoroutine(ActivateRemoveProcess());
        }
    }

    public void RemoveFixedUpdatable(IFixedUpdatable updatable)
    {
        if (fixedList.Contains(updatable))
        {
            removeFixedUpdatableQueue.Enqueue(updatable);
        }

        if (!isActivatedRemoveProcess)
        {
            StartCoroutine(ActivateRemoveProcess());
        }
    }

    public void RemoveLateUpdatable(ILateUpdatable updatable)
    {
        if (lateList.Contains(updatable))
        {
            removeLateUpdatableQueue.Enqueue(updatable);
        }

        if (!isActivatedRemoveProcess)
        {
            StartCoroutine(ActivateRemoveProcess());
        }
    }

    //FIXME: 나중에 코루틴 개조할 때 이거 없앨 것.
    WaitForEndOfFrame endFrame = new WaitForEndOfFrame();

    /// <summary>
    /// 실질적인 제거 작업은 여기서 진행
    /// </summary>
    private IEnumerator ActivateRemoveProcess()
    {
        isActivatedRemoveProcess = true;
        yield return endFrame;

        while (removeUpdatableQueue.Count != 0)
        {
            updatableList.Remove(removeUpdatableQueue.Dequeue());
        }

        while (removeFixedUpdatableQueue.Count != 0)
        {
            fixedList.Remove(removeFixedUpdatableQueue.Dequeue());
        }

        while (removeLateUpdatableQueue.Count != 0)
        {
            lateList.Remove(removeLateUpdatableQueue.Dequeue());
        }

        isActivatedRemoveProcess = false;
    }
}
