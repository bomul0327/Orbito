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
    /// 현재 선택중인 무기(selectedWeapon)를 slotIndex에 위치한 무기로 변경.
    /// </summary>
    /// <param name="slotIndex">변경 대상 무기의 슬롯 번호(zero-based).</param>
    public void SelectWeapon(int slotIndex)
    {
        var newSelectedWeapon = character.weaponSlot[slotIndex];
        if (newSelectedWeapon == null || newSelectedWeapon == character.selectedWeapon) return;

        character.selectedWeapon = newSelectedWeapon;
    }

    /// <summary>
    /// Weapon 장비를 장착한다.
    /// </summary>
    /// <param name="newWeapon"></param>
    /// <param name="slotIndex"></param>
    public void EquipWeapon(Equipment newWeapon, int slotIndex)
    {
        Equipment oldWeapon;

        oldWeapon = character.weaponSlot[slotIndex];
        character.weaponSlot[slotIndex] = newWeapon;

        UpdateStatModfication();
    }

    /// <summary>
    /// Weapon 타입 장비를 탈착함.
    /// </summary>
    /// <param name="slotIndex"></param>
    public void UnequipWeapon(int slotIndex)
    {
        Equipment oldWeapon;

        oldWeapon = character.weaponSlot[slotIndex];
        character.weaponSlot[slotIndex] = null;

        if (oldWeapon == character.selectedWeapon)
            character.selectedWeapon = null;

        UpdateStatModfication();
    }

    /// <summary>
    /// NonWeapon 타입 장비를 장착한다.
    /// </summary>
    /// <param name="newNonWeapon"></param>
    /// <param name="slotIndex"></param>
    public void EquipNonWeapon(Equipment newNonWeapon, int slotIndex)
    {
        Equipment oldNonWeapon;

        oldNonWeapon = character.nonWeaponSlot[slotIndex];
        character.nonWeaponSlot[slotIndex] = newNonWeapon;

        UpdateStatModfication();
    }


    /// <summary>
    /// NonWeapon 타입 장비를 탈착한다. 
    /// </summary>
    /// <param name="slotIndex"></param>
    public void UnequipNonWeapon(int slotIndex)
    {
        Equipment oldNonWeapon;

        oldNonWeapon = character.nonWeaponSlot[slotIndex];
        character.nonWeaponSlot[slotIndex] = null;

        UpdateStatModfication();
    }

    /// <summary>
    /// 슬롯에 있는 모든 장비의 StatModifier를 주어진 스탯에 적용한다.
    /// FIXME: 스탯 적용 방식은 아직 합의된 내용이 없으며, 임의로 적용함.
    /// 실제 적용 방식은 이후 크게 수정될 수 있음.
    /// </summary>
    private void UpdateStatModfication()
    {

        //모든 장비의 StatModifier를 찾음.
        var statModifiers = new List<StatModifier>();

        foreach (var equipment in character.weaponSlot)
        {
            if (equipment == null) continue;
            statModifiers.Add(equipment.statModifier);
        }

        foreach (var equipment in character.nonWeaponSlot)
        {
            if (equipment == null) continue;
            statModifiers.Add(equipment.statModifier);
        }

        int rateSum = 0;
        int fixedSum = 0;
        int origin = character.MaxHP;

        foreach (var statModifier in statModifiers)
        {
            switch (statModifier.modifyMethod)
            {
                case StatModifier.ModifyMethod.Rate:
                    rateSum += statModifier.amount;
                    break;
                case StatModifier.ModifyMethod.Fixed:
                    fixedSum += statModifier.amount;
                    break;
            }
        }
        //비율 증가 방식에 의한 스탯 증가량 계산.
        rateSum = (int)Mathf.Ceil(origin * rateSum * 0.01f);

        int finalValue = origin + rateSum + fixedSum;
        character.FinalHP = finalValue;
    }


    /// <summary>
    /// planetPos를 중심으로 MoveSpeed를 각속도로 변환한 속도로 공전
    /// </summary>
    /// <param name="planetPos"></param> character가 공전하는 행성의 위치
    /// <param name="isClockwise"></param> character의 공전 방향
    public void Revolve(Vector3 planetPos, bool isClockwise)
    {
        // player가 바라보는 방향 설정
        var dir = Vector2.Perpendicular(planetPos - charTransform.position).normalized;
        if (!isClockwise) dir = -dir;

        float angle = Mathf.Acos(dir.y) * Mathf.Rad2Deg;
        if (dir.x > 0) angle *= -1;
        charTransform.localRotation = Quaternion.Euler(0, 0, angle);

        // 공전
        float radius = Vector3.Distance(planetPos, charTransform.position);
        float deltaAngle = (character.MoveSpeed / radius) * Time.deltaTime;
        float rotAngle = Mathf.Acos((charTransform.position - planetPos).normalized.x);

        if (charTransform.position.y < planetPos.y) rotAngle = -rotAngle;
        if (isClockwise) deltaAngle = -deltaAngle;

        rotAngle += deltaAngle;
        float cos = Mathf.Cos(rotAngle);
        float sin = Mathf.Sin(rotAngle);
        Vector3 direction = new Vector2(cos, sin);
        Vector3 position = direction * radius;
        charTransform.position = position + planetPos;
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