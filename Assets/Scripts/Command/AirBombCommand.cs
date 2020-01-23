using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character character, Vector3 dir ÇÊ¿ä
/// </summary>
public class AirBombCommand : ICommand
{
    Character character;
    Vector3 dir;

    public void SetData(params object[] values)
    {
        this.character = (Character)values[0];
        this.dir = (Vector3)values[1];
    }

    public void Execute()
    {
        character.Behaviour.AirBomb(dir);
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}


