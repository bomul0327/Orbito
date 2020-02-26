using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character의 무기를 변경하는 Command.
/// </summary>
public class WeaponChangeCommand : ICommand
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
        character.Behaviour.ChangeWeapon(slotIndex);
    }

    public void SetData(params object[] values)
    {
        character = values[0] as Character;
        slotIndex = (int)values[1];
    }
}
    