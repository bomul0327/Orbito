using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character character ÇÊ¿ä
/// </summary>
public class MoveFrontCommand : ICommand
{
    Character character;

    public void SetData(params object[] values)
    {
        this.character = (Character)values[0];
    }

    public void Execute()
    {
        character.Behaviour.MoveFront();
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}



