using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBehaviour
{
    Character character;
    ResourceManager resourceManager;

    public GimmickBehaviour(Character character)
    {
        this.character = character;
        resourceManager = ResourceManager.Instance;
    }

    public void IncreaseResource()
    {
        resourceManager.currentResource++;
        Debug.Log(resourceManager.currentResource);
    }
}
