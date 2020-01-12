using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOPDictionary
{
    private Dictionary<string, object> dict = new Dictionary<string, object>();

    public void Add<T>(string key, TinyObjectPool<T> tobjPool) where T : class, IDisposable, new()
    {
        dict.Add(key, tobjPool);
    }
    public bool TryGetValue(string key, out IPool tobjPool)
    {
        object result;
        if(dict.TryGetValue(key, out result))
        {
            tobjPool = (IPool)result;
            return true;
        }
        else
        {
            tobjPool = null;
            return false;
        }
    }
    public bool TryGetValue<T>(string key, out TinyObjectPool<T> tobjPool) where T : class, IDisposable, new()
    {
        object result;
        if(dict.TryGetValue(key, out result))
        {
            tobjPool = (TinyObjectPool<T>)result;
            return true;
        }
        else
        {
            tobjPool = null;
            return false;
        }
    }
}
