using System;

public interface IState
{
    void OnEnter(IState state);
    void OnExit(IState state);
}