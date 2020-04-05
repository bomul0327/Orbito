using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveState : IState, IUpdatable
{
    private Character character;

    /// <summary>
    /// 현재 공전 중인 행성.
    /// </summary>
    private Planet targetPlanet;

    /// <summary>
    /// 현재 공전 궤도 반지름.
    /// </summary>
    private float revolveRadius;
    /// <summary>
    /// 현재 정찰선의 공전 방향.
    /// </summary>
    private bool isClockwise;

    /// <summary> 
    /// 공전 궤도 중심에서 정찰선으로의 unit 방향 벡터. 다음 공전 위치를 계산할 때 사용됩니다.
    /// </summary>
    private Vector3 normal;

    public RevolveState(Character character, Planet targetPlanet)
    {
        this.character = character;
        this.targetPlanet = targetPlanet;
    }

    void IState.OnEnter(IState prevState)
    {
        CommandDispatcher.Publish(CommandFactory.GetOrCreate<IncreaseResourceCommand>(character));

        // 매 프레임마다 공전 방향과 공전 반경을 계산하는 대신, 처음 공전을 시작했을 때 한 번만 계산해놓고 사용합니다.
        // revolve 함수는 미리 계산된 공전 방향과 반경 값을 바탕으로 공전을 수행하기 때문에
        // 약간의 성능을 절약할 수 있으며, 더 유동적으로 공전 운동을 조절할 수 있게 됩니다.
        Vector3 planetPos = targetPlanet.transform.position;
        Vector3 characterPos = character.transform.position;

        //현재 character의 위치를 기준으로 공전 반경을 설정합니다.
        revolveRadius = Vector2.Distance(planetPos, characterPos);
        
        // (공전 중심 -> 캐릭터 위치)로의 unit 방향 벡터를 설정합니다.
        // 공전 함수는 이 벡터를 회전시키면서 정찰선의 공전 위치를 계산하게 됩니다.
        normal = (characterPos - planetPos).normalized;

        //현재 character의 위치를 기준으로 공전 방향을 결정합니다.
        isClockwise = Circle.ClosestRevolveDirection(planetPos, characterPos, character.transform.up);
    }

    void IState.OnExit(IState nextState)
    {
        character = null;
        targetPlanet = null;
    }

    void IUpdatable.OnUpdate(float dt)
    {
        Vector3 planetPos = targetPlanet.transform.position;

        character.Behaviour.LookPerpendicular(planetPos, isClockwise);
        character.Behaviour.LocalRevolve(planetPos, revolveRadius, ref normal, isClockwise);

        Debug.Log(character.transform.forward);
    }
}
