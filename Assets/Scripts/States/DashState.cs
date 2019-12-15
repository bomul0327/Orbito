using System;
using UnityEngine;

public class DashState : IState, IUpdatable
{
    private Character character;

    public DashState(Character character)
    {
        this.character = character;
    }

    void IState.OnEnter(IState prevState)
    {
        
    }

    void IState.OnExit(IState nextState)
    {
        
    }

    void IUpdatable.OnUpdate(float dt)
    {
        // TODO: 속도를 늘린 상태에서 움직일 수 있는 기능 만들 것
        character.Behaviour.MoveFront();
        // 임의로 줄어드는 속도 2배로 설정
        character.CurrentFuel -= character.FuelReductionRatio * 2;
    }
}