using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 천체 종류
public enum PlanetType
{
    FloatingMatter,
    Small,
    Medium,
    Big
}

///<summary>
/// 자원등급
/// D(0~20), C(15~70), B(50~200), A(100~500)
///</summary>
public enum ResourceGrade
{
    A,
    B,
    C,
    D
}

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

    private bool selected;

    public bool Selectable;

    public float MoveSpeed = 10;

    public float GravityScale;

    public int Resources;

    public PlanetType Type;

    private ResourceGrade grade;

    private void Awake()
    {
        // Get Controller and Behaviour by something.
        // Json 데이터가 준비되면 Json을 통해서 받아올 것
        // Controller = ?;
        Behaviour = new PlanetBehaviour(this);

        Type = PlanetType.FloatingMatter;
        grade = ResourceGrade.A;
        HP = 10;

        // 자원 등급에 맞게 자원 초기화
        if (grade == ResourceGrade.A)
        {
            Resources = Random.Range(100, 500);
        }
        else if (grade == ResourceGrade.B)
        {
            Resources = Random.Range(50, 200);
        }
        else if (grade == ResourceGrade.C)
        {
            Resources = Random.Range(15, 70);
        }
        else if (grade == ResourceGrade.D)
        {
            Resources = Random.Range(0, 20);
        }
    }
}