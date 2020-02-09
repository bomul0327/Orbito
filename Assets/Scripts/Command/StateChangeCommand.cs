using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangeCommand : ICommand
{
    StateMachine stateMachine;
    IState nextState;

    public void SetData(params object[] values)
    {
        this.stateMachine = (StateMachine)values[0];
        this.nextState = (IState)values[1];
    }

    public void Execute()
    {
        stateMachine.ChangeTo(nextState);
    }

    public void Dispose()
    {
        stateMachine = null;
        nextState = null;
    }
}
