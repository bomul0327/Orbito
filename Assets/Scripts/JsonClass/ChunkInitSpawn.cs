using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInitSpawn
{
    public string AssetName { get; set; }
    public ChunkDifficulty ChunkDifficulty { get; set; }
    public List<SpawnStruct> Targets { get; set; }
}
public struct SpawnStruct
{
    public string TargetPoolName;
    public Vector3 LocalPosition;
    public Quaternion Rotation;
}

