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
    public void LookPerpendicular(Vector3 planetPos, bool isClockwise)
    {
        Vector3 direction = Circle.Perpendicular(planetPos, charTransform.position, isClockwise);
        LookDirection(direction);
    }


    /// <summary>
    /// planetPos를 중심으로 MoveSpeed를 각속도로 변환한 속도로 공전.
    /// </summary>
    /// <param name="center"></param> character가 공전하는 행성의 위치
    /// <param name="isClockwise"></param> character의 공전 방향
    public void Revolve(Vector3 center, bool isClockwise)
    {
        Vector3 position = charTransform.position;

        float radius = Vector2.Distance(center, position);
        float deltaAngle = Circle.LinearToAngleSpeed(character.MoveSpeed, radius) * Time.deltaTime;
        deltaAngle = isClockwise ? -deltaAngle : deltaAngle;

        charTransform.RotateAround(center, -Vector3.forward, deltaAngle);
    }


    /// <summary>
    /// center에 대한 Local 좌표계에서의 Revolve를 수행합니다.
    /// <param name="center">공전 궤도 중심.</param>
    /// <param name="radius">공전 궤도의 반지름.</param>
    /// <param name="normal">공전 중심에서 공전하는 물체까지의 unit 방향 벡터.</param>
    /// <param name="isClockwise"></param>
    public void LocalRevolve(Vector3 center, float radius, ref Vector3 normal, bool isClockwise)
    {
        float angleSpeed = Circle.LinearToAngleSpeed(character.MoveSpeed, radius) * Time.deltaTime;
        angleSpeed = isClockwise ? -angleSpeed : angleSpeed;

        Quaternion revolveRotation = Quaternion.Euler(0, 0, angleSpeed);
        normal = revolveRotation * normal;

        charTransform.position = center + normal * radius;
    }

    /// <summary>
    /// Character가 특정 방향을 바라보도록 설정.
    /// </summary>
    /// <param name="direction">Character가 바라볼 방향.</param>
    public void LookDirection(Vector3 direction)
    {
        charTransform.rotation = Quaternion.LookRotation(-Vector3.forward, direction);
    }
    

    /// <summary>
    /// 무기 장비 슬롯에서 무기를 선택합니다.
    /// </summary>
    /// <param name="slotIndex">선택할 무기의 슬롯 번호(zero-based).</param>
    public void SelectWeapon(int slotIndex)
    {
        var newSelectedWeapon = character.WeaponSlots[slotIndex];

        // 슬롯이 비어있거나 이미 선택된 무기라면 아무것도 하지 않는다.
        if (newSelectedWeapon == null || newSelectedWeapon == character.SelectedWeapon) return;

        //현재 선택 중인 무기를 선택 해제한다.
        UnselectWeapon();

        character.SelectedWeapon = newSelectedWeapon;
        character.SelectedBattleAction = newSelectedWeapon.BattleAction;
    }

    /// <summary>
    /// 현재 선택 중인 무기를 선택 해제합니다.
    /// </summary>
    public void UnselectWeapon()
    {
        if (character.SelectedWeapon == null) return;

        character.SelectedWeapon = null;
        character.SelectedBattleAction = null;
    }

    /// <summary>
    /// 장비를 장비 슬롯에 장착합니다.
    /// </summary>
    /// <param name="newEquipment">슬롯에 새로 장착할 무기</param>
    /// <param name="slotIndex">무기를 장착할 슬롯의 번호(zero-based).</param>
    public void Equip(Equipment newEquipment, int slotIndex)
    {
        var equipmentSlot = GetEquipmentSlot(newEquipment.EquipmentType);

        //이전에 장착되어 있는 장비를 탈착한다.
        Unequip(slotIndex, newEquipment.EquipmentType);

        // 장착한 장비의 스탯 적용.
        character.Stats += newEquipment.Stats;

        equipmentSlot[slotIndex] = newEquipment;

    }

    /// <summary>
    /// 장비 슬롯에서 장비를 탈착합니다.
    /// </summary>
    /// <param name="slotIndex">탈착할 무기가 있는 슬롯의 번호(zero-based).</param>
    /// <param name="equipmentType">탈착할 장비의 타입.</param>
    public void Unequip(int slotIndex, EquipmentType equipmentType)
    {
        var equipmentSlot = GetEquipmentSlot(equipmentType);

        var lastEquipment = equipmentSlot[slotIndex];

        // 슬롯이 비어있다면 아무 것도 하지 않는다.
        if (lastEquipment == null) return;

        // 탈착한 장비의 스탯 해제.
        character.Stats -= lastEquipment.Stats;

        //장착 해제 한 장비가 현재 선택 중인 무기라면, 선택을 해제해야 한다.
        if (lastEquipment == character.SelectedWeapon)
        {
            UnselectWeapon();
        }


        equipmentSlot[slotIndex] = null;
    }

    /// <summary>
    /// Character의 장비 슬롯을 가져옵니다.
    /// </summary>
    /// <param name="equipmentSlotType">가져올 장비 슬롯의 타입.</param>
    private Equipment[] GetEquipmentSlot(EquipmentType equipmentSlotType)
    {
        if (equipmentSlotType == EquipmentType.Weapon)
            return character.WeaponSlots;
        else
            return character.NonweaponSlots;
    }


    public void Attack()
    {
        Debug.Log("Attack");
    }


}