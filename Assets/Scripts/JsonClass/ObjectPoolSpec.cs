using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSpec
{
    public string AssetName { get; set; }
    public int PoolCapacity { get; set; }
    public int MaxPoolCapacity { get; set; }
    public PoolScaleType MyPoolScaleType { get; set; }
    public PoolReturnType MyPoolReturnType { get; set; }
    public float AutoReturnTime { get; set; }

}
