using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandFactory
{
    // 각 커맨드들을 관리하는 딕셔너리
    public static Dictionary<string, ICommand> CommandDict = new Dictionary<string, ICommand>();
    
    /// <summary>
    /// 원하는 타입의 커맨드를 반환합니다
    /// </summary>
    /// <typeparam name="T">커맨드 타입</typeparam>
    /// <returns>커맨드 객체</returns>
    public static T GetOrCreate<T>(params object[] values) where T : class, ICommand, new()
    {
        ICommand resCommand;
        if(CommandDict.TryGetValue(typeof(T).Name, out resCommand))
        {
            
        }
        else
        {
            resCommand = new T();
            CommandDict.Add(typeof(T).Name, resCommand);
        }

        resCommand.SetData(values);
        return (T)resCommand;
    }
}