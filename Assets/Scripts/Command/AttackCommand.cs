using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICommand
{
    Character attacker;
    Character victim;
    int dmg;

    AttackCommand(Character attacker, Character victim, int dmg)
    {
        this.attacker = attacker;
        this.victim = victim;
        this.dmg = dmg;
    }
    public void Execute()
    {
        attacker.Behaviour.Attack();
    }
}


