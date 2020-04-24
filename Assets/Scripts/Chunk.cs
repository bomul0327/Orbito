using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

using Random = UnityEngine.Random;

public enum ChunkDifficulty{Easy, Normal, Hard, MAX}

public class Chunk
{
    public Dictionary<string,List<PooledUnityObject>> SpawnedObj;
    private Vector3 position;
    public Vector3 Position
    {
        set
        {
            position = value;
            Seed = position.magnitude;
            Random.InitState((int)Seed);
            Difficulty = (ChunkDifficulty)(int)(position.magnitude/MapSize * (int)ChunkDifficulty.MAX * Random.value);
            Debug.Log("New Chunk : "+Position+", and difficulty : "+Difficulty);
            Reset();
            Spawn();
        }
        get{return position;}
    }
    public float Seed;
    public ChunkDifficulty Difficulty;
    const int MapSize = 1000; 

    public Chunk(Vector3? pos = null)
    {
        SpawnedObj = new Dictionary<string, List<PooledUnityObject>>();
        SpawnedObj.Add("Enemy", new List<PooledUnityObject>());
        SpawnedObj.Add("Planet", new List<PooledUnityObject>());
        Position = pos ?? Vector3.zero;
    }
    public void Spawn ()
    {
        ChunkInitSpawn ChunkInitSpawn = new ChunkInitSpawn();
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
                SpawnedObj[target.TargetPoolName].Add(UnityObjectPool.GetOrCreate(target.TargetPoolName).Instantiate(GlobalPos(target.LocalPosition), target.Rotation));
        }
        // switch (ChunkInitSpawn.ChunkDifficulty)
        // {
        //     case ChunkDifficulty.Easy :
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2-20, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2+20, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2-20, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2+20, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         break;
        //     case ChunkDifficulty.Normal :
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2-20, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2+20, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2-20, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2+20, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         break;
        //     case ChunkDifficulty.Hard :
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2-20, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Enemy"].Add(UnityObjectPool.GetOrCreate("Enemy").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2+20, ChunkManager.Instance.Height/2)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2-20, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         SpawnedObj["Planet"].Add(UnityObjectPool.GetOrCreate("Planet").Instantiate(GlobalPos(new Vector3(ChunkManager.Instance.Width/2+20, ChunkManager.Instance.Height/2 + 10)), Quaternion.identity));
        //         break;
        // }
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
        foreach (var s in SpawnedObj)
        {
            foreach(var obj in s.Value)
            {
                UnityObjectPool.GetOrCreate("Enemy").Return(obj.GetComponent<PooledUnityObject>());
            }
            s.Value.Clear();
        }
    }
}

