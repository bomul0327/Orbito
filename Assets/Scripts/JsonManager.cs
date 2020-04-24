using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

using FMOD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Debug = UnityEngine.Debug;

public static class JsonManager 
{
    public static Dictionary<string, SoundSourcePath> SoundDict = new Dictionary<string, SoundSourcePath>();
    public static Dictionary<string, ObjectPoolSpec> PoolSpecDict = new Dictionary<string, ObjectPoolSpec>();
    public static Dictionary<string, ChunkInitSpawn> ChunkDict = new Dictionary<string, ChunkInitSpawn>();
    private static string jsonPath = System.IO.Path.Combine(Application.streamingAssetsPath, "JsonFiles");

    static JsonManager()
    {
        ReadSoundSourcePaths("SoundSourcePath.json");
        ReadObjectPoolSpecs("ObjectPoolSpecs.json");
        ReadChunkInitSpawns("ChunkInitSpawn.json");
    }
    public static void ReadSoundSourcePaths(string fileName)
    {
        string filePath = System.IO.Path.Combine(jsonPath, fileName);
        string dataAsjson = File.ReadAllText(filePath);
        var data = JsonConvert.DeserializeObject<List<JObject>>(dataAsjson);

        foreach (JObject ob in data)
        {
            SoundSourcePath sound = new SoundSourcePath();
            sound.AssetName = ob["AssetName"].ToString();
            string[] modesString = ob["MODE"].ToString().Split(',');
            sound.Modes = new MODE[modesString.Length];
            try
            {
                for(int i = 0; i < modesString.Length; ++i)
                {
                    sound.Modes[i] = (MODE)Enum.Parse(typeof(MODE), modesString[i]);
                }
            }
            catch (ArgumentException)
            {
                Debug.Log("There is an error on " + ob["AssetName"].ToString() + "'s MODE");
            }
            sound.Path = ob["Path"].ToString();

            SoundDict.Add(sound.AssetName, sound);
        }
    }

    public static SoundSourcePath GetSoundSourcePath(string assetName)
    {
        if (SoundDict.ContainsKey(assetName))
        {
            return SoundDict[assetName];
        }
        else
        {
            Debug.LogError("There is no SoundSourcePath named " + assetName);
            return null;
        }
    }
    public static void ReadObjectPoolSpecs(string fileName)
    {
        string filePath = System.IO.Path.Combine(jsonPath, fileName);
        string dataAsJson = File.ReadAllText(filePath);
        var data = JsonConvert.DeserializeObject<List<JObject>>(dataAsJson);

        foreach(JObject info in data)
        {
            ObjectPoolSpec spec = new ObjectPoolSpec();
            spec.AssetName = info["AssetName"].ToString();
            spec.PoolCapacity = (int)info["PoolCapacity"];
            spec.MaxPoolCapacity = (int)info["MaxPoolCapacity"];
            try
            {
                spec.MyPoolScaleType = (PoolScaleType)Enum.Parse(typeof(PoolScaleType), info["MyPoolScaleType"].ToString());
                spec.MyPoolReturnType = (PoolReturnType)Enum.Parse(typeof(PoolReturnType), info["MyPoolReturnType"].ToString());
            }
            catch (ArgumentException)
            {
                Debug.Log("There is an error on " + info["AssetName"].ToString() + "'s MyPoolScaleType or MyPoolReturnType in ObjectPoolSpecs.json");
            }
            spec.AutoReturnTime = (float)info["AutoReturnTime"];

            PoolSpecDict.Add(spec.AssetName, spec);
        }
    }

    public static ObjectPoolSpec GetObjectPoolSpec(string assetName)
    {
        if(PoolSpecDict.ContainsKey(assetName))
        {
            return PoolSpecDict[assetName];
        }
        else
        {
            Debug.LogError("There is no " + assetName + "'s PoolSpec");
            return null;
        }
    }

    public static void ReadChunkInitSpawns(string fileName)
    {
        string filePath = System.IO.Path.Combine(jsonPath, fileName);
        string dataAsJson = File.ReadAllText(filePath);
        var data = JsonConvert.DeserializeObject<List<JObject>>(dataAsJson);

        foreach(JObject info in data)
        {
            ChunkInitSpawn chunk = new ChunkInitSpawn();
            chunk.Targets = new List<SpawnStruct>();
            chunk.AssetName = info["AssetName"].ToString();
            try
            {
                chunk.ChunkDifficulty = (ChunkDifficulty)Enum.Parse(typeof(ChunkDifficulty), info["ChunkDifficulty"].ToString());
            }
            catch (ArgumentException)
            {
                Debug.Log("There is an error on " + info["AssetName"].ToString() + "'s ChunkDifficulty in ChunkInitSpawn.json");
            }
            foreach(var Target in info["Targets"])
            {
                SpawnStruct s = new SpawnStruct();
                s.TargetPoolName = Target["TargetPoolName"].ToString();
                s.LocalPosition.x = (float)Target["LocalPos"]["X"];
                s.LocalPosition.y = (float)Target["LocalPos"]["Y"];
                s.Rotation = (Quaternion)(new Quaternion((float)Target["Quater"]["X"],(float)Target["Quater"]["Y"],(float)Target["Quater"]["Z"],(float)Target["Quater"]["W"]));
                chunk.Targets.Add(s);
            }

            ChunkDict.Add(chunk.AssetName, chunk);
        }
    }

    public static ChunkInitSpawn GetChunkInitSpawn(string assetName)
    {
        if(ChunkDict.ContainsKey(assetName))
        {
            return ChunkDict[assetName];
        }
        else
        {
            Debug.LogError("There is no " + assetName + "'s ChunkInitSpawn");
            return null;
        }
    }
   
}