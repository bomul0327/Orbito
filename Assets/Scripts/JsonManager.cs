using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonManager 
{

    private static List<Dictionary<ObjectCode, Object>> Cache;

    public void JasonManager()
    {
        Cache = new List<Dictionary<ObjectCode, Object>>();
        //adding categories into list
        Cache.Add(new Dictionary<ObjectCode, Object>());
        Cache.Add(new Dictionary<ObjectCode, Object>());
        Cache.Add(new Dictionary<ObjectCode, Object>());
    }

    public static Object toObject (string jsonfile, ObjectCategory Category)
    {
        Object ob = (Object)JsonConvert.DeserializeObject(jsonfile);
        // Cache.IndexOf(Category).Add(ob.ObjectCode , ob);
        return ob;
    }

    public static void Remove (ObjectCode code)
    {
        // Cache.IndexOf((int)FindCategory(code)).Remove(code);
    }

    public static Object Find (ObjectCode code)
    {
        // return Cache.IndexOf((int)FindCategory(code))[code];
        return null;
    }

    public static bool Contains (ObjectCode code)
    {
        // return Cache.IndexOf((int)FindCategory(code)).ContainsKey(code);
        return true;
    }

    public enum ObjectCategory
    {
        Character, Enemy, Item, Unknown
    };

    public enum ObjectCode
    {
        Firstcharacter =0, Lastcharacter =9, Firstenemy =10, Lastenemy =99, Firstitem =100, Lastitem =999
        // string path = Application.StartupPath; 
        // path += "GameObjectsInfos.csv"
        // //CSV 파일로 ObjectCode들을 받을 수 있도록
        // if (!File.Exists(path))
        // {
        //     using (StreamReader sr = File.OpenText(path))
        //     {
        //         string s;
        //         while ((s = sr.ReadLine()) != null )
        //         {
        //             Console.WriteLine(s);
        //         }
        //     }
        // }
    };

    private static ObjectCategory FindCategory (ObjectCode code)
    {
        // character code is between 0 and 9
        if ( code >= ObjectCode.Firstcharacter && code < ObjectCode.Lastcharacter )
        {
            return ObjectCategory.Character;
        }
        // enemy code is between 10 and 99
        else if ( code >= ObjectCode.Firstenemy && code < ObjectCode.Lastenemy )
        {
            return ObjectCategory.Enemy;
        } 
        // Item code is over 100
        else if ( code >= ObjectCode.Firstitem )
        {
            return ObjectCategory.Item;
        }
        return ObjectCategory.Unknown;
    }
}