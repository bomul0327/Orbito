using UnityEngine;
using System.Collections.Generic;
public class Equipment
{
    public enum EquipmentType
    {
        /// <summary>
        /// 공격 조작을 통해 사용할 수 있는 무기 장비 타입.
        /// </summary>
        Weapon,

        /// <summary>
        /// 별도로 사용하지 않아도 능력치를 올려주는 무기 외 장비 타입.
        /// </summary>
        NonWeapon
    }

    /// <summary>
    /// 장비의 이름, 또는 ID.
    /// </summary>
    public readonly string name;

    /// <summary>
    /// 장비의 타입.
    /// </summary>
    public readonly EquipmentType equipmentType;

    /// <summary>
    /// 장비의 BattleAction. 기본적으로 Weapon 타입 장비에서 사용됨.
    /// </summary>
    public ITriggerBattleAction BattleAction
    {
        get;
        private set;
    }

    /// <summary>
    /// 장비의 스탯 변경 값.
    /// </summary>
    public List<StatModifier> StatModifierList
    {
        get;
        private set;
    }

    public Equipment(string name, EquipmentType equipmentType, ITriggerBattleAction battleAction)
    {
        this.name = name;
        this.equipmentType = equipmentType;

        this.BattleAction = battleAction;

        this.StatModifierList = new List<StatModifier>();
    }

    /// <summary>
    /// 장비 객체를 생성하는 정적 팩토리 메소드.
    /// </summary>
    /// <param name="battleAction">장비에서 사용되는 BattleAction.</param>
    /// <returns></returns>
    public static Equipment CreateEquipment(string name, EquipmentType equipmentType, ITriggerBattleAction battleAction)
    {
        return new Equipment(name, equipmentType, battleAction);
    }
}
