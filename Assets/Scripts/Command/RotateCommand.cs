using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : ICommand
{
    Character character;
    Vector3 planetPos;

    public RotateCommand(Character character, Vector3 planetPos)
    {
        this.character = character;
        this.planetPos = planetPos;
    }

    public void Execute()
    {
        character.Behaviour.Rotate(planetPos);
    }
    public void Dispose()
    {

    }
}


