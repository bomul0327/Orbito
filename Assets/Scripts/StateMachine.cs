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

        if ( CurrentState is IUpdatable )
        {
            UpdateManager.Instance.RemoveUpdatable( (IUpdatable) CurrentState);
        }

        nextState.OnEnter(CurrentState);

        if ( nextState is IUpdatable )
        {
            UpdateManager.Instance.AddUpdatable( (IUpdatable) nextState);
        }

        CurrentState = nextState;
    }

    void IDisposable.Dispose()
    {
        CurrentState = null;
    }
}