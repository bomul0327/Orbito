using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveCommand : ICommand
{
    Character character;
    Vector3 planetPos;

    public RevolveCommand(Character character, Vector3 planetPos)
    {
        this.character = character;
        this.planetPos = planetPos;
    }

    public void Execute()
    {
        character.Behaviour.Revolve(planetPos);
    }
}
