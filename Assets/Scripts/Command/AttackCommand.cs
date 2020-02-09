using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character attacker, Character victim, int dmg �ʿ�
/// </summary>
public class AttackCommand : ICommand
{
    Character attacker;
    Character victim;
    int dmg;

    public void SetData(params object[] values)
    {
        this.attacker = (Character)values[0];
        this.victim = (Character)values[1];
        this.dmg = (int)values[2];
    }
    public void Execute()
    {
        attacker.Behaviour.Attack();
    }
    public void Dispose()
    {
        attacker = null;
        victim = null;
    }
}


