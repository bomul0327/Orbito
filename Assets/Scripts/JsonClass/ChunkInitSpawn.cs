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
    // position 을 절대값으로 할지 아니면 비율로 할지 고민입니다.
    public Vector3 LocalPosition;
    public Quaternion Rotation;
}

