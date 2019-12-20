using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 캐릭터에게 어떤 행동을 할 것인지를 정하는 클래스
/// </summary>
public class CharacterPlayerController : CharacterControllerBase, IUpdatable
{
    public Character character;

    public CharacterPlayerController(Character character)
    {
        this.character = character;
        UpdateManager.Instance.AddUpdatable(this);
    }

    public void OnUpdate(float dt)
    {
        if (Input.GetButton("Revolve"))
        {
            // TODO : 공전궤도 안인지 확인하고 Vector3.zero를 planet의 position으로 변경
            CommandDispatcher.Publish(new RevolveCommand(character, Vector3.zero));

        }
        else
        {
            CommandDispatcher.Publish(new MoveFrontCommand(character));

        }
        CommandDispatcher.Handle();
    }
}