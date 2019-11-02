using System;
using UnityEngine;

public class NullState : IState
{
    public void OnEnter(IState state)
    {
        if ( state is IdleState ){
            Debug.Log("It is from IdleState");
        } else if ( state is NullState ) {
            Debug.Log("It is from NullState");
        }
    }

    public void OnExit(IState state)
    {
        if ( state is IdleState ){
            Debug.Log("It is Heading for IdleState");
        } else if ( state is NullState ) {
            Debug.Log("It is Heading for NullState");
        }
    }
}