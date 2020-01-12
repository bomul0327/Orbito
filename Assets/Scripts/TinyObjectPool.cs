using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 작은 사이즈의 클래스들을 풀링
/// </summary>
/// <typeparam name="T"></typeparam>
public class TinyObjectPool<T> : IPool where T : class, IDisposable, new()
{
    private Queue<T> availableObjects = new Queue<T>();
    private List<T> inUseObjects = new List<T>();

    /// <summary>
    /// 풀에 사용 가능한 객체가 있으면 반환하고 없으면 생성 후 반환합니다
    /// </summary>
    public T GetOrCreate()
    {
        T obj;
        if (availableObjects.Count != 0)
        {
            obj = availableObjects.Dequeue();
        }
        else
        {
            obj = new T();
        }
        inUseObjects.Add(obj);
        return obj;
    }

    /// <summary>
    /// 사용한 객체를 다시 풀에 반환하여 비활성화 합니다
    /// </summary>
    /// <param name="obj">반환할 객체</param>
    public void Return(object obj)
    {
        if (inUseObjects.Contains((T)obj))
        {
            inUseObjects.Remove((T)obj);
            availableObjects.Enqueue((T)obj);
        }
    }

    /// <summary>
    /// 사용 중인 모든 객체들을 풀에 반환합니다
    /// </summary>
    public void ReturnAll()
    {
        if (inUseObjects.Count == 0) return;

        for (int i = inUseObjects.Count - 1; i >= 0; --i)
        {
            Return(inUseObjects[i]);
        }
    }

    /// <summary>
    /// 풀을 해제합니다
    /// </summary>
    public void Dispose()
    {
        ReturnAll();

        while (availableObjects.Count != 0)
        {
            var obj = availableObjects.Dequeue();
            obj.Dispose();
            obj = null;
        }
        inUseObjects = null;
        availableObjects = null;
    }

}
