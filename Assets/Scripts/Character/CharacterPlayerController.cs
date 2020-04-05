using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 캐릭터에게 어떤 행동을 할 것인지를 정하는 클래스
/// </summary>
public class CharacterPlayerController : CharacterControllerBase, IUpdatable
{
    public Character character;
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
        if (Input.GetButtonDown("Revolve"))
        {
            // 우선순위가 가장 높은 행성 찾기
            var revolvePlanet = FindRevolvePlanet();

            if (revolvePlanet)
            {
                prevPlanet = revolvePlanet;
                using (var cmd = CommandFactory.GetOrCreate<StateChangeCommand>(charStateMachine, new RevolveState(character, revolvePlanet)))
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

    /// <summary>
    /// 공전 가능한 행성들 중에서, 가장 가까운 행성을 찾습니다.
    /// 이전에 공전했던 행성은 가장 낮은 우선순위를 가집니다.
    /// </summary>
    /// <returns></returns>
    private Planet FindRevolvePlanet()
    {
        //TODO: FindGameObjectsWithTag는 오버헤드가 크므로 다른 방법을 고려할 필요가 있음.
        var planetObjects = GameObject.FindGameObjectsWithTag("Planet");

        Planet minDistancePlanet = null;
        float minDistance = Mathf.Infinity;

        Vector3 characterPosition = character.transform.position;

        bool canUseLastPlanet = false;
        foreach (var planetObject in planetObjects)
        {
            var planet = planetObject.GetComponent<Planet>();
            float distance = Vector3.Distance(characterPosition, planetObject.transform.position);

            // 1. Character가 행성 공전 영역 밖에 있을 경우.
            if (distance > planet.Radius) continue;
            // 2. 이 행성이 가장 가까운 행성이 아닐 경우.
            if (distance > minDistance) continue;
            // 3. 이 행성이 이전에 공전했던 행성일 경우.
            if (prevPlanet != null && planet == prevPlanet)
            {
                canUseLastPlanet = true;
                continue;
            }

            minDistancePlanet = planet;
            minDistance = distance;
        }
        //다른 행성들이 공전 가능 대상에 없다면 prevPlanet 사용.
        if (minDistancePlanet == null && canUseLastPlanet)
        {
            minDistancePlanet = prevPlanet;
        }
            
        return minDistancePlanet;
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
