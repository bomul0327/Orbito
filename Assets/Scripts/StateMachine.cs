using System;

public class StateMachine : IDisposable
{
    public IState CurrentState { get; private set; }

    public StateMachine()
    {
        CurrentState = new NoneState();
        UpdateManager.Instance.AddUpdatable(CurrentState);
    }

    public void ChangeTo(IState nextState)
    {
        if (nextState == CurrentState)
        {
            return;
        }

        CurrentState.OnExit(nextState);
        UpdateManager.Instance.RemoveUpdatable(CurrentState);

        nextState.OnEnter(CurrentState);
        UpdateManager.Instance.AddUpdatable(nextState);

        CurrentState = nextState;
    }

    void IDisposable.Dispose()
    {
        CurrentState = null;
    }
}