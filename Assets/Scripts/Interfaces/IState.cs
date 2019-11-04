using System;

public interface IState
{
    void OnEnter(IState prevState);
    void OnExit(IState nextState);
}