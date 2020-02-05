using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character character, Vector3 planetPos 필요
/// </summary>
public class RevolveCommand : ICommand
{
    Character character;
    Vector3 planetPos;

    public void SetData(params object[] values)
    {
        this.character = (Character)values[0];
        this.planetPos = (Vector3)values[1];
    }

    public void Execute()
    {
        var antiClockwiseDir = Vector2.Perpendicular(planetPos - character.transform.position).normalized;
        bool isClockwise = Vector2.Dot(antiClockwiseDir, character.transform.up) > 0;
        character.Behaviour.Revolve(planetPos, isClockwise);
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
