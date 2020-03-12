using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseResourceCommand : ICommand
{
    Character character;

    public void SetData(params object[] values)
    {
        character = (Character)values[0];
    }

    public void Execute()
    {
        Debug.Log("start - " + Time.time);
        ResourceManager.Instance.StartCoroutine(ResourceManager.Instance.IncreaseResource(character));
    }

    public void Dispose()
    {
        character = null;
    }
}
