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
    /// 장착 중인 Weapon 타입 장비를 보관하는 슬롯.
    /// </summary>
    public Equipment[] weaponSlot = new Equipment[InitialWeaponSlotCount];

    /// <summary>
    /// 장착 중인 NonWeapon 타입 장비를 보관하는 슬롯.
    /// </summary>
    public Equipment[] nonWeaponSlot = new Equipment[InitialNonWeaponSlotCount];


    public static readonly int InitialWeaponSlotCount = 4;
    public static readonly int InitialNonWeaponSlotCount = 4;

    /// <summary>
    /// 스탯 증감 테스트를 위해 임의로 추가한 스탯.
    /// 지금 단계에서는 Character에 스탯이 이것 한 종류만 있다고 가정하고 작성함.
    /// </summary>
    public Stat statForTest = new Stat("StatForTest", 100);

    /// <summary>
    /// 현재 사용중인 Weapon타입 장비.
    /// </summary>
    public Equipment selectedWeapon;

    /// <summary>
    /// 현재 선택된 BattleAction.
    /// </summary>
    public ITriggerBattleAction selectedBattleAction;


    /// <summary>
    /// 테스트를 위해 생성한 Equipment 객체를 보관하는 Dictionary.
    /// 아직 Inventory 시스템이 완성되지 않았기에 임시로 Dictionary에 장비 객체 관리.
    /// </summary>
    public Dictionary<string, Equipment> equipmentDictForTest = new Dictionary<string, Equipment>();

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

        SetupEquipmentsForTest();
    }

    /// <summary>
    /// Equipment 시스템 테스트를 위해 Character에서 즉시 Equipment 객체를 생성하고, Dictionary에서 관리.
    /// 원래는 Equipment 생성 시, JSON에서 가져와야 할 것.
    /// </summary>
    private void SetupEquipmentsForTest()
    {
        //임시로 여기에서 Weapon 타입의 Equipment를 생성하고, 무기슬롯에 할당.
        weaponSlot[0] = Equipment.CreateEquipment("Normal Shooter", Equipment.EquipmentType.Weapon, battleActionDict["NormalBattleAction"]);
        weaponSlot[1] = Equipment.CreateEquipment("Power Shooter", Equipment.EquipmentType.Weapon, battleActionDict["MultiShotBattleAction"]);

        //임시로 여기에서 NonWeapon 타입의 Equipment를 생성.
        var normalShieldEquipment = Equipment.CreateEquipment("Normal Shield", Equipment.EquipmentType.NonWeapon, null);
        normalShieldEquipment.StatModifierList.Add(new StatModifier("StatForTest", 25f, 0f));
       
        var powerShieldEquipment = Equipment.CreateEquipment("Power Shield", Equipment.EquipmentType.NonWeapon, null);
        powerShieldEquipment.StatModifierList.Add(new StatModifier("StatForTest", 75f, 0f));
        
        var normalCoreEquipment = Equipment.CreateEquipment("Normal Core", Equipment.EquipmentType.NonWeapon, null);
        normalCoreEquipment.StatModifierList.Add(new StatModifier("StatForTest", 0, 0.2f));

        var powerCoreEquipment = Equipment.CreateEquipment("Power Core", Equipment.EquipmentType.NonWeapon, null);
        powerCoreEquipment.StatModifierList.Add(new StatModifier("StatForTest", 0, 0.3f));

        equipmentDictForTest.Add(normalShieldEquipment.name, normalShieldEquipment);
        equipmentDictForTest.Add(powerShieldEquipment.name, powerShieldEquipment);
        equipmentDictForTest.Add(normalCoreEquipment.name, normalCoreEquipment);
        equipmentDictForTest.Add(powerCoreEquipment.name, powerCoreEquipment);
    }
}
