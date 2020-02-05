﻿using System;
using UnityEngine;

public class StraightMoveState : IState, IUpdatable
{
    private Character character;

    public StraightMoveState(Character character)
    {
        this.character = character;
    }

    void IState.OnEnter(IState prevState)
    {
        
    }

    void IState.OnExit(IState nextState)
    {
        
    }

    void IUpdatable.OnUpdate(float dt)
    {
        character.Behaviour.MoveFront();
        character.CurrentFuel -= character.FuelReductionRatio;
    }
}