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
    private static Dictionary<string, SoundSourcePath> soundDict = new Dictionary<string, SoundSourcePath>();
    private static Dictionary<string, ObjectPoolSpec> poolSpecDict = new Dictionary<string, ObjectPoolSpec>();
    private static string jsonPath = System.IO.Path.Combine(Application.streamingAssetsPath, "JsonFiles");

    static JsonManager()
    {
        ReadSoundSourcePaths("SoundSourcePath.json");
        ReadObjectPoolSpecs("ObjectPoolSpecs.json");
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

            soundDict.Add(sound.AssetName, sound);
        }
    }

    public static SoundSourcePath GetSoundSourcePath(string assetName)
    {
        if (soundDict.ContainsKey(assetName))
        {
            return soundDict[assetName];
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

            poolSpecDict.Add(spec.AssetName, spec);
        }
    }

    public static ObjectPoolSpec GetObjectPoolSpec(string assetName)
    {
        if(poolSpecDict.ContainsKey(assetName))
        {
            return poolSpecDict[assetName];
        }
        else
        {
            Debug.LogError("There is no " + assetName + "'s PoolSpec");
            return null;
        }
    }
   
}