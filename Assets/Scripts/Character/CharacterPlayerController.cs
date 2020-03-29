using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 캐릭터에게 어떤 행동을 할 것인지를 정하는 클래스
/// </summary>
public class CharacterPlayerController : CharacterControllerBase, IUpdatable
{
    public Character character;
    GameObject[] planetArray;
    StateMachine charStateMachine;
    Planet prevPlanet = null;

    public CharacterPlayerController(Character character)
    {
        this.character = character;
        UpdateManager.Instance.AddUpdatable(this);
        charStateMachine = this.character.CharacterStateMachine;

        // 초기 상태 (직진)
        using (var cmd = CommandFactory.GetOrCreate<StateChangeCommand>(charStateMachine, new StraightMoveState(character)))
        {
            CommandDispatcher.Publish(cmd);
        }
    }

    ~CharacterPlayerController()
    {
        UpdateManager.Instance.RemoveUpdatable(this);
    }

    public void OnUpdate(float dt)
    {
        planetArray = GameObject.FindGameObjectsWithTag("Planet");

        // TODO: 공전영역을 벗어나면 prevPlanet = null;

        if (Input.GetButtonDown("Revolve"))
        {
            // 우선순위가 가장 높은 행성 찾기
            Planet minDistancePlanet = null;
            float minDis = Mathf.Infinity;
            foreach (GameObject planet in planetArray)
            {
                float t = Vector3.Distance(character.transform.position, planet.transform.position);
                // TODO : 공전 반경 안인지 확인
                if (minDis > t)
                {
                    if (prevPlanet != null && planet.GetComponent<Planet>() == prevPlanet)
                    {
                        // 이전에 공전한 행성이 가장 가까우면 무시
                        continue;
                    }
                    minDis = t;
                    minDistancePlanet = planet.GetComponent<Planet>();
                }
            }

            if (minDistancePlanet)
            {
                prevPlanet = minDistancePlanet;
                using (var cmd = CommandFactory.GetOrCreate<StateChangeCommand>(charStateMachine, new RevolveState(character, minDistancePlanet)))
                {
                    CommandDispatcher.Publish(cmd);
                }
            }
            else if (prevPlanet)
            {
                using (var cmd = CommandFactory.GetOrCreate<StateChangeCommand>(charStateMachine, new RevolveState(character, prevPlanet)))
                {
                    CommandDispatcher.Publish(cmd);
                }
            }
        }
        else if (Input.GetButtonUp("Revolve"))
        {
            using (var cmd = CommandFactory.GetOrCreate<StateChangeCommand>(charStateMachine, new StraightMoveState(character)))
            {
                CommandDispatcher.Publish(cmd);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            character.BattleActionDict["NormalBattleAction"].Trigger();
        }

        if (Input.GetButton("Fire2"))
        {
            if (character.SelectedWeapon != null)
            {
                character.SelectedBattleAction.Trigger();
            }
        }


        if (GetWeponSlotButtonsDown(out int slotNumber))
        {
            using (var cmd = CommandFactory.GetOrCreate<WeaponSelectCommand>(character, slotNumber))
            {
                CommandDispatcher.Publish(cmd);
            }
        }

        //테스트용 NonWeapon 4개를 일시에 장착.
        //FIXME: 테스트가 끝나면 반드시 삭제할 것.
        if (Input.GetKeyDown(KeyCode.E))
        {
            int equipSlotNumber = 0;
            foreach (var equipment in character.EquipmentDictForTest.Values)
            {
                using (var cmd = CommandFactory.GetOrCreate<EquipCommand>(character, equipment, equipSlotNumber++))
                {
                    CommandDispatcher.Publish(cmd);
                }
            }
        }

        //NonWeapon 슬롯에 있는 모든 장비를 일시에 탈착.
        //FIXME: 테스트가 끝나면 반드시 삭제할 것.
        if (Input.GetKeyDown(KeyCode.R))
        {
            int equipSlotNumber = 0;
            foreach (var weapon in character.NonweaponSlots)
            {
                using (var cmd = CommandFactory.GetOrCreate<UnequipCommand>(character, EquipmentType.NonWeapon, equipSlotNumber++))
                {
                    CommandDispatcher.Publish(cmd);
                }
            }
        }
    }

    private static readonly string[] WeaponSlotButtonNames = { "WeaponSelect_1", "WeaponSelect_2", "WeaponSelect_3", "WeaponSelect_4" };

    /// <summary>
    /// WeaponSlot 버튼들의 입력을 확인하고, 그에 해당하는 slot의 Index를 출력.
    /// </summary>
    /// <param name="slotIndex">슬롯 버튼이 눌렸다면, 그 버튼에 상응하는 무기 slot의 index를 출력</param>
    /// <returns>어떤 WeaponSlot 버튼이 눌렸다면, true. 아무 버튼도 눌리지 않았다면, false.</returns>
    private bool GetWeponSlotButtonsDown(out int slotIndex)
    {
        for (int i = 0; i < WeaponSlotButtonNames.Length; i++)
        {
            // 동시에 여러 개의 슬롯 버튼을 입력받았어도, 가장 앞쪽에 있는 슬롯 버튼의 입력 하나만 인정됨에 유의.
            // 따라서, 한 번에 여러 개의 슬롯이 선택되는 경우는 없다.
            if (Input.GetButtonDown(WeaponSlotButtonNames[i]))
            {
                slotIndex = i;
                return true;
            }
        }

        // 아무 버튼도 눌리지 않았을 경우, index를 -1로 설정하여 오용 방지.
        slotIndex = -1;
        return false;
    }
}
