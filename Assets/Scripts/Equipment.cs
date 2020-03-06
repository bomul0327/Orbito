using UnityEngine;
using System.Collections.Generic;
public class Equipment
{
    public enum EquipmentType
    {
        /// <summary>
        /// 공격 조작을 통해 사용할 수 있는 장비 타입.
        /// </summary>
        Weapon,

        /// <summary>
        /// 별도로 사용하지 않아도 능력치를 올려주는 장비 타입.
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
    private ITriggerBattleAction triggerBattleAction;

    /// <summary>
    /// 장비의 스탯 변경 값.
    /// </summary>
    public StatModifier statModifier;

    public Equipment(string name, EquipmentType type, ITriggerBattleAction battleAction, StatModifier statModifier)
    {
        this.name = name;
        this.equipmentType = type;

        this.triggerBattleAction = battleAction;

        this.statModifier = statModifier;
    }


    /// <summary>
    /// 장비를 사용할 때 호출.
    /// </summary>
    public void Use()
    {
        // 현재까지는 장비 타입이 Weapon일 때만 battleAction을 사용하게 만듦.
        if (equipmentType == EquipmentType.Weapon)
        {
            triggerBattleAction.Trigger();
        }
    }

    /// <summary>
    /// Weapon 타입 장비 객체를 생성하는 정적 팩토리 메소드.
    /// </summary>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static Equipment CreateWeapon(string name, ITriggerBattleAction battleAction, StatModifier statModifier)
    {
        return new Equipment(name, EquipmentType.Weapon, battleAction, statModifier);
    }

    /// <summary>
    /// Non-Weapon 타입 장비 객체를 생성하는 정적 팩토리 메소드.
    /// </summary>
    /// <param name="statModifier">장비의 스탯 변경 값을 설정.</param>
    /// <returns></returns>
    public static Equipment CreateNonWeapon(string name, StatModifier statModifier)
    {
        return new Equipment(name, EquipmentType.NonWeapon, null, statModifier);
    }

}
