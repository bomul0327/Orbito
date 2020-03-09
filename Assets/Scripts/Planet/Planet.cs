using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행성의 정보와 컴포넌트들을 가지고 있는 클래스
/// </summary>

public class Planet : FieldObject
{
    public float radius;

    public PlanetBehaviour Behaviour
    {
        get;
        private set;
    }

    public PlanetController ConTroller
    {
        get;
        private set;
    }

    public enum size
    {
        small,
        medium,
        big
    }

    private bool m_isSelected;

    public bool selectable;

    public float MoveSpeed = 10;

    public float gravityScale;

    public class resources
    {
    public float currentResorces;
    public float maxResources;
    }
    
    /// <summary>
    /// 부유물
    /// </summary>
    public struct FloatingMatter
        {
        public float innerRadius;

        public int currentHP;

        public int maxHP;

        public float defense;

        public float moveSpeed = 10;

           
        }

    
    /// <summary>
    /// 캐릭터가 행성을 선택하였을 때 호출되는 함수
    /// </summary>
    public void Select()
    {
        if (m_isSelected) return;

        Debug.Log("Planet selected!");
        PlayerInput.Select(this);
        m_isSelected = true;
    }

    /// <summary>
    /// 캐릭터가 선택한 행성을 벗어날 때 호출되는 함수
    /// </summary>
    public void Deselect()
    {
        if (!m_isSelected) return;
        Debug.Log("Planet deselected!");
        m_isSelected = false;
    }

    /// <summary>
    /// Currentresource를 리턴하는 함수
    /// </summary>
    public void giveResource()
    {
        return currentResorces;
        Debug.Log("giveResource");
    }

    private void Awake()
    {
        // Get Controller and Behaviour by something.
        // Json 데이터가 준비되면 Json을 통해서 받아올 것
        // Controller = ?;
        // Behaviour = ?;
    }
}
