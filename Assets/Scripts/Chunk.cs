using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

using Random = UnityEngine.Random;

public enum ChunkDifficulty{Easy, Normal, Hard, MAX}

public class Chunk
{
    public Dictionary<string,List<PooledUnityObject>> SpawnedObjDict;
    private Vector3 position;
    public Vector3 Position
    {
        set
        {
            position = value;
            Seed = position.magnitude;
            Random.InitState((int)Seed);
            Difficulty = (ChunkDifficulty)(int)(position.magnitude/MapSize * (int)ChunkDifficulty.MAX * Random.value);
            // Debug.Log("New Chunk : "+Position+", and difficulty : "+Difficulty);
            Reset();
            Spawn();
        }
        get{return position;}
    }
    public float Seed;
    public ChunkDifficulty Difficulty;
    const int MapSize = 1000; 
    ChunkInitSpawn ChunkInitSpawn;

    public Chunk(Vector3 pos = default(Vector3))
    {
        SpawnedObjDict = new Dictionary<string, List<PooledUnityObject>>();
        SpawnedObjDict.Add("Enemy", new List<PooledUnityObject>());
        SpawnedObjDict.Add("Planet", new List<PooledUnityObject>());
        ChunkInitSpawn = new ChunkInitSpawn();
        Position = pos;
    }
    public void Spawn ()
    {
        switch (Difficulty)
        {
            case ChunkDifficulty.Easy:
                ChunkInitSpawn = JsonManager.GetChunkInitSpawn("EasyChunk");
                break;
            case ChunkDifficulty.Normal:
                ChunkInitSpawn = JsonManager.GetChunkInitSpawn("NormalChunk");
                break;
            case ChunkDifficulty.Hard:
                ChunkInitSpawn = JsonManager.GetChunkInitSpawn("HardChunk");
                break;
        }

        foreach (var target in ChunkInitSpawn.Targets)
        {
                SpawnedObjDict[target.TargetPoolName].Add(
                    UnityObjectPool.GetOrCreate(target.TargetPoolName).
                        Instantiate(GlobalPos(target.LocalPosition), target.Rotation)
                    );
        }
        return;
    }

    /// <summary>
    /// Global Position을 Chunk의 Local Position으로 변환해줍니다. 
    /// </summary>
    public Vector3 LocalPos (Vector3 globalPos)
    {
        return globalPos - position + 
                new Vector3(ChunkManager.Instance.Width*1/2, ChunkManager.Instance.Height*1/2, 0);
    }
    /// <summary>
    /// Chunk의 Local Position을 Global Position으로 변환해줍니다.
    /// </summary>
    public Vector3 GlobalPos (Vector3 localPos)
    {
        return localPos + position - 
                new Vector3(ChunkManager.Instance.Width*1/2, ChunkManager.Instance.Height*1/2, 0);
    }

    public void Reset()
    {
        foreach (var s in SpawnedObjDict)
        {
            foreach(var obj in s.Value)
            {
                UnityObjectPool.GetOrCreate("Enemy").Return(obj.GetComponent<PooledUnityObject>());
                UnityObjectPool.GetOrCreate("Planet").Return(obj.GetComponent<PooledUnityObject>());
            }
            s.Value.Clear();
        }
    }
}

