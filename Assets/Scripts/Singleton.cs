﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 싱글턴 클래스를 위한 포맷. 새로 싱글턴 만들어야 하는 이유 없으면 이거 가져다가 쓰세요.
/// </summary>
/// <example>
/// public class AAA: Singleton<AAA>
/// {
///     ~~~
/// }
/// </example>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    Debug.LogError("There's no active " + typeof(T) + " in this scene");
                }
            }

            return _instance;
        }
    }
}