using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : ICommand
{
    Character character;
    Vector3 center;
    float radius;

    RotateCommand(Character character, Vector3 center, float radius)
    {
        this.character = character;
        this.center = center;
        this.radius = radius;
    }

    public void Execute()
    {
        character.Behaviour.Rotate(center, radius);
    }
}


