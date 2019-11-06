using System;

public interface IState : IUpdatable
{
    void OnEnter(IState prevState);
    void OnExit(IState nextState);
}