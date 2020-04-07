using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character character, Vector3 dir �ʿ�
/// </summary>
public class AirBombCommand : ICommand
{
    Character character;
    Vector3 explosionCenter;

    public void SetData(params object[] values)
    {
        this.character = (Character)values[0];
        this.explosionCenter = (Vector3)values[1];
    }

    public void Execute()
    {
        Vector3 affectedDirection = character.transform.position - explosionCenter;
        character.Behaviour.LookDirection(affectedDirection);
    }

    public void Dispose()
    {
        character = null;
    }
}


