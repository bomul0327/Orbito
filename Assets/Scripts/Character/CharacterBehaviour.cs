using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터의 모든 행동들은 여기에 정의해놓고, 구현할 것
/// 이 클래스를 굳이 상속받을 이유가 있다면 반드시 물어보고 만들 것
/// </summary>
public class CharacterBehaviour
{

    Character character;
    Transform charTransform;
    Vector3 mousePos;

    CharacterBehaviour(Character character)
    {
        this.character = character;
        charTransform = character.transform;
    }


    public void MoveFront()
    {
        charTransform.position += character.MoveSpeed * charTransform.right * Time.deltaTime;
    }

    public void Rotate(Vector3 center, float radius)
    {
        Debug.Log("Rotate");
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

    public void AirBomb(Vector3 dir)
    {
        Debug.Log("Air Bomb");
    }
}



