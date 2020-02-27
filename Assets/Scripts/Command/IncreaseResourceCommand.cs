using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseResourceCommand : ICommand
{
    Character character;

    public void SetData(params object[] values)
    {
        this.character = (Character)values[0];
    }

    public void Execute()
    {
        character.gimmickBehaviour.IncreaseResource();
    }

    public void Dispose()
    {

    }
}
