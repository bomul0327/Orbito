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

        if (CurrentState is IUpdatable)
        {
            UpdateManager.Instance.RemoveUpdatable((IUpdatable)CurrentState);
        }

        CurrentState.OnExit(nextState);

        var prevState = CurrentState;
        CurrentState = nextState;

        CurrentState.OnEnter(prevState);

        if (CurrentState is IUpdatable)
        {
            UpdateManager.Instance.AddUpdatable((IUpdatable)CurrentState);
        }
    }

    void IDisposable.Dispose()
    {
        CurrentState = null;
    }
}