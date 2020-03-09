using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCommand : ICommand
{
    FieldObject target;
    DamageInfo damageInfo;

    public void SetData(params object[] values)
    {
        this.target = (FieldObject)values[0];
        this.damageInfo = (DamageInfo)values[1];
    }

    public void Execute()
    {
        if (target == null)
        {
            return;
        }

        target.HP -= damageInfo.Damage;
    }
    public void Dispose()
    {
        target = null;
    }
}
