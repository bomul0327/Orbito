using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character가 slot에 있는 무기를 선택하는 Command.
/// </summary>
public class WeaponSelectCommand : ICommand
{
    Character character;
    int slotIndex;

    public void Dispose()
    {
        character = null;
        slotIndex = -1;
    }

    public void Execute()
    {
        character.Behaviour.SelectWeapon(slotIndex);
    }

    public void SetData(params object[] values)
    {
        character = values[0] as Character;
        slotIndex = (int)values[1];
    }
}
    