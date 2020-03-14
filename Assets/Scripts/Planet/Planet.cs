using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행성의 정보와 컴포넌트들을 가지고 있는 클래스
/// </summary>
public class Planet : FieldObject
{
    public float Radius;

    public PlanetBehaviour Behaviour
    {
        get;
        private set;
    }

    public enum Size
    {
        small,
        medium,
        big
    }

    private bool selected;

    public bool Selectable;

    public float MoveSpeed = 10;

    public float GravityScale;

    public class Resources
    {
        public float CurrentResorces;
        public float MaxResources;
    }

    /// <summary>
    /// 부유물
    /// </summary>
    public struct FloatingMatter
    {
        public float InnerRadius;

        public int CurrentHP;

        public int MaxHP;

        public class Resources
        {
            public float CurrentResorces;
            public float MaxResources;
        }

        public float MoveSpeed;
    }

    private void Awake()
    {
        // Get Controller and Behaviour by something.
        // Json 데이터가 준비되면 Json을 통해서 받아올 것
        // Controller = ?;
        // Behaviour = ?;
    }
}