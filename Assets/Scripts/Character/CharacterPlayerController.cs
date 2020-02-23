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
            character.battleActionDict["NormalBattleAction"].Trigger();
        }
    }
}