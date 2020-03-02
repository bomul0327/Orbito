using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCommand : ICommand
{
    Collider2D target;
    Projectile.DamageInfo damageInfo;

    public void SetData(params object[] values)
    {
        this.target = (Collider2D)values[0];
        this.damageInfo = (Projectile.DamageInfo)values[1];
    }

    public void Execute()
    {
        FieldObject fieldObject = target.gameObject.GetComponentInParent<FieldObject>();
        if (fieldObject == null)
        {
            return;
        }

        fieldObject.HP -= damageInfo.Damage;
    }
    public void Dispose()
    {
        target = null;
    }
}
