using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : Gimmick
{
    public ItemBehaviour ItemBehaviour
    {
        get; private set;
    }

    private void Awake()
    {
        ItemBehaviour = new ItemBehaviour(this);
    }
}
