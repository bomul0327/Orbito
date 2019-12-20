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

    public CharacterBehaviour(Character character)
    {
        this.character = character;
        charTransform = character.transform;
    }

    /// <summary>
    /// character의 MoveSpeed로, character의 y축으로 이동
    /// </summary>
    public void MoveFront()
    {
        charTransform.Translate(0, character.MoveSpeed * Time.deltaTime, 0);
    }

    /// <summary>
    /// 공전을 시작했을 때, 공전하는 방향을 character가 바라보게 한다.
    /// </summary>
    /// <param name="planetPos"></param>
    public void Rotate(Vector3 planetPos)
    {
        var dir = (planetPos - charTransform.position).normalized;
        var antiClockwiseDir = Vector2.Perpendicular(charTransform.position - charTransform.up).normalized;
        if (Vector2.Dot(antiClockwiseDir, charTransform.position) > 0) dir = -dir;

        float angle = Mathf.Acos(dir.x) * Mathf.Rad2Deg;
        if (dir.y < 0) angle *= -1;
        charTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// planetPos를 중심으로 MoveSpeed를 각속도로 변환한 속도로 공전
    /// </summary>
    /// <param name="planetPos"></param> character가 공전하는 행성의 위치
    public void Revolve(Vector3 planetPos)
    {
        float deg = (character.MoveSpeed / Vector3.Distance(planetPos, charTransform.position)) * Mathf.Rad2Deg * Time.deltaTime;

        var antiClockwiseDir = Vector2.Perpendicular(charTransform.position - planetPos).normalized;
        if (Vector2.Dot(antiClockwiseDir, charTransform.up) > 0) deg = -deg;

        charTransform.RotateAround(planetPos, -Vector3.forward, deg);
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



