using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseResourceCommand : ICommand
{
    Character character;
    IState currentState;

    public void SetData(params object[] values)
    {
        character = (Character)values[0];
        currentState = (IState)values[1];
    }

    public void Execute()
    {
        ResourceManager.Instance.StartIncreaseResource(character, currentState);
    }

    public void Dispose()
    {
        character = null;
        currentState = null;
    }
}
