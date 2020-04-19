using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : IEvent
{
    FieldObject target;

    public DestroyEvent(FieldObject target)
    {
        this.target = target;
    }
}
