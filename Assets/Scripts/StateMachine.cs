using System;

public class StateMachine : IDisposable
{
    public IState CurrentState { get; private set; }

    public StateMachine()
    {
        CurrentState = new NoneState();
    }

    public void ChangeTo(IState nextState)
    {
        if (nextState == CurrentState)
        {
            return;
        }

        CurrentState.OnExit(nextState);
        nextState.OnEnter(CurrentState);
        CurrentState = nextState;
    }

    void IDisposable.Dispose()
    {
        CurrentState = null;
    }
}