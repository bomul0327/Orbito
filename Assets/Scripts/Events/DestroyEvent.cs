using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : IEvent
{
    private FieldObject target;
    public FieldObject Target
    {
        get
        {
            return target;
        }
    }

    public DestroyEvent(FieldObject target)
    {
        this.target = target;
    }

}
