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

        string currentDirectory = Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\";
        // test용이라 compile 에러 나서 주석처리했습니다.
        // string dataAsJson = File.ReadAllText( currentDirectory + "JsonManagerTestData.json" );
        // var data = JsonConvert.DeserializeObject<List<JObject>> (dataAsJson);

        // foreach ( var ob in data)
        // {
        //     testDict.Add(ob["AssetName"].ToString(), ob);
        // }
    }

    public static void Find(string name, ref JObject jObject)
    {
        if (testDict.ContainsKey(name))
        {
            jObject =  testDict[name];
        }
        else
        {
            Debug.Log("There is no JsonFile named like parameter");
            jObject = null;
        }
    }
}