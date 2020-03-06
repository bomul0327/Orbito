using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주어진 장비를 Character의 장비 슬롯에 장착하는 Command.
/// </summary>
public class EquipCommand : ICommand
{
    Character character;
    /// <summary>
    /// 장착할 장비.
    /// </summary>
    Equipment equipment;
    /// <summary>
    /// 장착할 슬롯의 index()
    /// </summary>
    int slotIndex;

    public void Dispose()
    {
        character = null;
        equipment = null;
        slotIndex = -1;
    }

    public void Execute()
    {
        if (equipment.equipmentType == Equipment.EquipmentType.Weapon)
            character.Behaviour.EquipWeapon(equipment, slotIndex);
        else if (equipment.equipmentType == Equipment.EquipmentType.NonWeapon)
            character.Behaviour.EquipNonWeapon(equipment, slotIndex);
    }

    public void SetData(params object[] values)
    {
        character = values[0] as Character;
        equipment = values[1] as Equipment;
        slotIndex = (int)values[2];
    }
}
