using System;
using UnityEngine;

public class IdleState : IState
{
    void IState.OnEnter(IState prevState)
    {
        
    }

    void IState.OnExit(IState nextState)
    {
        
    }

    void IUpdatable.OnUpdate(float dt)
    {
        // 업데이트 될때 해당 State가 할 일
    }
}