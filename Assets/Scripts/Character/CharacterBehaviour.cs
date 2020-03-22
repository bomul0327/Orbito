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
    /// 무기 장비 슬롯에서 무기를 선택합니다.
    /// </summary>
    /// <param name="slotIndex">선택할 무기의 슬롯 번호(zero-based).</param>
    public void SelectWeapon(int slotIndex)
    {
        var newSelectedWeapon = character.weaponSlot[slotIndex];

        // 슬롯이 비어있거나 이미 선택된 무기라면 아무것도 하지 않는다.
        if (newSelectedWeapon == null || newSelectedWeapon == character.selectedWeapon) return;

        //현재 선택 중인 무기를 선택 해제한다.
        UnselectWeapon();

        character.selectedWeapon = newSelectedWeapon;
        character.selectedBattleAction = newSelectedWeapon.BattleAction;
    }

    /// <summary>
    /// 현재 선택 중인 무기를 선택 해제합니다.
    /// </summary>
    public void UnselectWeapon()
    {
        if (character.selectedWeapon == null) return;

        character.selectedWeapon = null;
        character.selectedBattleAction = null;
    }

    /// <summary>
    /// 장비를 장비 슬롯에 장착합니다.
    /// </summary>
    /// <param name="newEquipment">슬롯에 새로 장착할 무기</param>
    /// <param name="slotIndex">무기를 장착할 슬롯의 번호(zero-based).</param>
    public void Equip(Equipment newEquipment, int slotIndex)
    {
        var equipmentSlot = GetEquipmentSlot(newEquipment.equipmentType);
        
        //이전에 장착되어 있는 장비를 탈착한다.
        Unequip(slotIndex, newEquipment.equipmentType);

        // 탈착한 장비의 스탯 적용.
        character.stat += newEquipment.stats;

        equipmentSlot[slotIndex] = newEquipment;

    }

    /// <summary>
    /// 장비 슬롯에서 장비를 탈착합니다.
    /// </summary>
    /// <param name="slotIndex">탈착할 무기가 있는 슬롯의 번호(zero-based).</param>
    /// <param name="equipmentType">탈착할 장비의 타입.</param>
    public void Unequip(int slotIndex, Equipment.EquipmentType equipmentType)
    {
        var equipmentSlot = GetEquipmentSlot(equipmentType);

        var lastEquipment = equipmentSlot[slotIndex];

        // 슬롯이 비어있다면 아무 것도 하지 않는다.
        if (lastEquipment == null) return;

        // 탈착한 장비의 스탯 해제.
        character.stat -= lastEquipment.stats;

        //장착 해제 한 장비가 현재 선택 중인 무기라면, 선택을 해제해야 한다.
        if (lastEquipment == character.selectedWeapon)
        {
            UnselectWeapon();
        }


        equipmentSlot[slotIndex] = null;
    }

    /// <summary>
    /// Character의 장비 슬롯을 가져옵니다.
    /// </summary>
    /// <param name="equipmentSlotType">가져올 장비 슬롯의 타입.</param>
    private Equipment[] GetEquipmentSlot(Equipment.EquipmentType equipmentSlotType)
    {
        if (equipmentSlotType == Equipment.EquipmentType.Weapon)
            return character.weaponSlot;
        else
            return character.nonWeaponSlot;
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