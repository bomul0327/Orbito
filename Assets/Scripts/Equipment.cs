using UnityEngine;
using System.Collections.Generic;
public class Equipment
{
    public enum EquipmentType
    {
        /// <summary>
        /// 공격 조작을 통해 사용할 수 있는 장비 타입
        /// </summary>
        Weapon,

        /// <summary>
        /// 별도로 사용하지 않아도 능력치를 올려주는 장비 타입.
        /// </summary>
        NonWeapon
    }

    public readonly string name;

    public readonly EquipmentType equipmentType;

    private ITriggerBattleAction triggerBattleAction;

    public Equipment(string name, EquipmentType type, ITriggerBattleAction battleAction)
    {
        this.name = name;
        this.equipmentType = type;

        this.triggerBattleAction = battleAction;
    }

    public void Trigger()
    {
        triggerBattleAction.Trigger();
    }

}
