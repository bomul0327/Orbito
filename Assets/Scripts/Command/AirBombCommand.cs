using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBombCommand : ICommand
{
    Character character;
    Vector3 dir;

    public AirBombCommand(Character character, Vector3 dir)
    {
        this.character = character;
        this.dir = dir;
    }


    public void Execute()
    {
        character.Behaviour.AirBomb(dir);
    }

}


