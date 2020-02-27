using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveState : IState, IUpdatable
{
    private Character character;

    private Planet targetPlanet;
    private bool isClockwise;

    public RevolveState(Character character, Planet targetPlanet)
    {
        this.character = character;
        this.targetPlanet = targetPlanet;
    }

    void IState.OnEnter(IState prevState)
    {
        var antiClockwiseDir = Vector2.Perpendicular(targetPlanet.transform.position - character.transform.position).normalized;
        isClockwise = Vector2.Dot(antiClockwiseDir, character.transform.up) > 0;
    }

    void IState.OnExit(IState nextState)
    {
        character = null;
        targetPlanet = null;
    }

    void IUpdatable.OnUpdate(float dt)
    {
        character.Behaviour.Revolve(targetPlanet.transform.position, isClockwise);
        CommandDispatcher.Publish(CommandFactory.GetOrCreate<IncreaseResourceCommand>(character));
    }
}
