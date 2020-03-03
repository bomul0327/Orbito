using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class JsonManager 
{
    private static Dictionary<string, JObject> testDict;

    static JsonManager()
    {
        testDict = new Dictionary<string, JObject>();

        string jsonPath = Application.streamingAssetsPath + "/JsonFiles/";
        string dataAsJson = File.ReadAllText( jsonPath + "SoundSourcePath.json" );
        var data = JsonConvert.DeserializeObject<List<JObject>> (dataAsJson);

        foreach ( var ob in data)
        {
            testDict.Add(ob["AssetName"].ToString(), ob);
        }
    }

    public static JObject Find(string name)
    {
        if (testDict.ContainsKey(name))
        {
            return testDict[name];
        }
        else
        {
            Debug.Log("There is no JsonFile named like parameter");
            return null;
        }
    }
}