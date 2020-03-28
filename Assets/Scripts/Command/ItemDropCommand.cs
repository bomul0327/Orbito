using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropCommand : ICommand
{
    Vector3 enemyPos;

    public void SetData(params object[] values)
    {
        enemyPos = (Vector3)values[0];
    }

    public void Execute()
    {
        ResourceManager.Instance.CreateItem(enemyPos);
    }

    public void Dispose()
    {
        enemyPos = Vector3.zero;
    }
}
