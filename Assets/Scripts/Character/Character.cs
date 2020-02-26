using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character에 관한 정보와 컴포넌트를 가지고 있는 클래스
/// </summary>
public class Character : FieldObject
{
    public CharacterControllerBase Controller
    {
        get;
        private set;
    }

    public CharacterBehaviour Behaviour
    {
        get;
        private set;
    }

    public StateMachine CharacterStateMachine = new StateMachine();

    public int CurrentHP;

    public int MaxHP;

    public float CurrentFuel;

    public float MaxFuel;

    public float FuelReductionRatio;

    public float Defense;

    public float MoveSpeed;

    /// <summary>
    /// 사용할 수 있는 배틀액션들
    /// </summary>
    /// <typeparam name="string">배틀 액션 이름</typeparam>
    /// <typeparam name="ITriggerBattleAction">실제 사용하는 배틀 액션</typeparam>
    /// <returns></returns>
    public Dictionary<string, ITriggerBattleAction> battleActionDict = new Dictionary<string, ITriggerBattleAction>();

    /// <summary>
    /// Weapon 및 NonWeapon 슬롯.
    /// </summary>
    public Equipment[] weaponSlot = new Equipment[InitialWeaponSlotCount];
    public Equipment[] nonWeaponSlot = new Equipment[InitialNonWeaponSlotCount];


    public static readonly int InitialWeaponSlotCount = 4;
    public static readonly int InitialNonWeaponSlotCount = 4;

    /// <summary>
    /// 현재 사용중인 Wepon타입 장비.
    /// </summary>
    public Equipment selectedWeapon;

    private void Awake()
    {
        // Get Controller and Behaviour by something.
        // Json 데이터가 준비되면 Json을 통해서 받아올 것
        // 현재 JSON 데이터가 준비되어 있지 않기 때문에 Awake에서 바로 Controller와 Behaviour객체를 생성함.
        Controller = new CharacterPlayerController(this);
        Behaviour = new CharacterBehaviour(this);

        // FIXME: 예시를 위해서 임시로 추가한 코드입니다.
        battleActionDict.Add(typeof(NormalBattleAction).Name, new NormalBattleAction(this));
        battleActionDict.Add(typeof(MultiShotBattleAction).Name, new MultiShotBattleAction(this));

        //임시로 여기에서 Weapon 타입의 Equipment를 생성.
        weaponSlot[0] = new Equipment("Normal Shooter", Equipment.EquipmentType.Weapon, battleActionDict["NormalBattleAction"]);
        weaponSlot[1] = new Equipment("Power Shooter", Equipment.EquipmentType.Weapon, battleActionDict["MultiShotBattleAction"]);

        //임시로 여기에서 NonWeapon 타입의 Equipment를 생성.
        var normalShieldBattleAction = new HPStatusBattleAction(this, 50, HPStatusBattleAction.ModifyMethod.Fixed, true);
        var powerShieldBattleAction = new HPStatusBattleAction(this, 50, HPStatusBattleAction.ModifyMethod.Rate, true);

        nonWeaponSlot[0] = new Equipment("Normal Shield", Equipment.EquipmentType.NonWeapon, normalShieldBattleAction);
        nonWeaponSlot[1] = new Equipment("Super Shield", Equipment.EquipmentType.NonWeapon, powerShieldBattleAction);

        //임시로 여기에서 NonWeapon 타입의 Equipment를 Trigger.
        foreach (var nonWeapon in nonWeaponSlot)
        {
            if (nonWeapon == null) return;
            nonWeapon.Trigger();
        }

    }

}
