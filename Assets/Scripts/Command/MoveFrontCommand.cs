using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFrontCommand : ICommand
{
    Character character;

    public MoveFrontCommand(Character character)
    {
        this.character = character;
    }

    public void Execute()
    {
        character.Behaviour.MoveFront();
    }
    public void Dispose()
    {

    }
}



