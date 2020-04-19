using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class Utils
{
    /// <summary>
    /// 딕셔너리에서 반환된 객체의 정수형 변수 중 특정 이름을 가진 변수 값을 반환합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict">딕셔너리 객체</param>
    /// <param name="key">딕셔너리에 쓸 키값</param>
    /// <param name="varName">키값으로 반환될 객체 내의 변수 이름</param>
    /// <param name="defaultVal">반환될 객체가 없거나 해당 이름을 갖는 변수가 없는 경우 반환할 정수값</param>
    /// <returns></returns>
    public static int GetIntFromDictionary<T>(Dictionary<string, T> dict, string key, string varName, int defaultVal)
    {
        if (dict.ContainsKey(key))
        {
            FieldInfo info = typeof(T).GetField(varName, BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.Instance | BindingFlags.Static);
            object target = info.GetValue(dict[key]);
            if (target is int)
                return (int)target;
        }
        return defaultVal;
    }

    /// <summary>
    /// 딕셔너리에서 반환된 객체의 float형 변수 중 특정 이름을 가진 변수 값을 반환합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict">딕셔너리 객체</param>
    /// <param name="key">딕셔너리에 쓸 키값</param>
    /// <param name="varName">키값으로 반환될 객체 내의 변수 이름</param>
    /// <param name="defaultVal">반환될 객체가 없거나 해당 이름을 갖는 변수가 없는 경우 반환할 float값</param>
    /// <returns></returns>
    public static float GetFloatFromDictionary<T>(Dictionary<string, T> dict, string key, string varName, float defaultVal)
    {
        if (dict.ContainsKey(key))
        {
            FieldInfo info = typeof(T).GetField(varName, BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.Instance | BindingFlags.Static);
            object target = info.GetValue(dict[key]);
            if (target is float)
                return (float)target;
        }
        return defaultVal;
    }
}
