/// <summary>
/// Character의 장비 술롯에서 장비를 탈착하는 Command.
/// </summary>
public class UnequipCommand : ICommand
{
    Character character;
    /// <summary>
    /// 탈착할 장비슬롯의 타입.
    /// </summary>
    Equipment.EquipmentType equipmentSlotType;

    /// <summary>
    /// 탈착할 장비 슬롯의 index(zero-based).
    /// </summary>
    int slotIndex;

    public void Dispose()
    {
        character = null;
        slotIndex = -1;
    }

    public void Execute()
    {
        character.Behaviour.Unequip(slotIndex, equipmentSlotType);
    }

    public void SetData(params object[] values)
    {
        character = values[0] as Character;
        equipmentSlotType = (Equipment.EquipmentType)values[1];
        slotIndex = (int)values[2];
    }
}