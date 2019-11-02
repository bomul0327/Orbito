using System;

public class StateMachine
{
    public IState CurrentState{get; private set;}

    public StateMachine()
    {
        CurrentState = new IdleState();
    }

    public void ToState(IState NextState)
    {
        CurrentState.OnExit(NextState);
        NextState.OnEnter(CurrentState);
        CurrentState = NextState;
    }
}