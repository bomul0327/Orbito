using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveState : IState, IUpdatable
{
    private Character character;

    private Planet targetPlanet;

    public RevolveState(Character character, Planet targetPlanet)
    {
        this.character = character;
        this.targetPlanet = targetPlanet;
    }

    void IState.OnEnter(IState prevState)
    {
        
    }

    void IState.OnExit(IState nextState)
    {
        
    }

    void IUpdatable.OnUpdate(float dt)
    {
        character.Behaviour.Revolve(targetPlanet.transform.position);
    }
}
