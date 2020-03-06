/// <summary>
/// Character의 장비 술롯에서 장비를 탈착하는 Command.
/// </summary>
public class UnequipCommand : ICommand
{
    Character character;
    /// <summary>
    /// 탈착할 장비의 타입.
    /// </summary>
    Equipment.EquipmentType equipmentType;

    /// <summary>
    /// 탈착할 장비의 index(zero-based).
    /// </summary>
    int slotIndex;

    public void Dispose()
    {
        character = null;
        slotIndex = -1;
    }

    public void Execute()
    {
        if (equipmentType == Equipment.EquipmentType.Weapon)
            character.Behaviour.UnequipWeapon(slotIndex);
        else if (equipmentType == Equipment.EquipmentType.NonWeapon)
            character.Behaviour.UnequipNonWeapon(slotIndex);
    }

    public void SetData(params object[] values)
    {
        character = values[0] as Character;
        equipmentType = (Equipment.EquipmentType)values[1];
        slotIndex = (int)values[2];
    }
}
