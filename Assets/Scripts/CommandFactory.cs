using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFactory : MonoBehaviour
{
    public static TOPDictionary CommandPoolDict = new TOPDictionary();
    
    /// <summary>
    /// 원하는 타입의 커맨드를 반환합니다
    /// </summary>
    /// <typeparam name="T">커맨드 타입</typeparam>
    /// <returns>커맨드 객체</returns>
    public static T GetOrCreate<T>() where T : class, ICommand, new()
    {
        TinyObjectPool<T> commandPool;
        if(CommandPoolDict.TryGetValue<T>(typeof(T).Name, out commandPool))
        {
            
        }
        else
        {
            commandPool = new TinyObjectPool<T>();
            CommandPoolDict.Add<T>(typeof(T).Name, commandPool);
        }

        return commandPool.GetOrCreate();
    }
}