using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonManager 
{

    private List<Dictionary<int, Object>> Cache;

    public void JasonManager()
    {
        Cache = new List<Dictionary<int, Object>>();
        //adding categories into list
        Cache.Add(new Dictionary<int, Object>());
        Cache.Add(new Dictionary<int, Object>());
        Cache.Add(new Dictionary<int, Object>());
    }

    public static Object toObject (string jsonfile, ObjectCategory Category)
    {
        Object ob = JsonConvert.DeserializeObject(jsonfile);
        Cache.IndexOf(Category).Add(ob.ObjectCode , ob);
        return ob;
    }

    public static void Remove (ObjectCode code)
    {
        Cache.IndexOf(FindCategory(code)).Remove(code);
    }

    public static Object Find (ObjectCode code)
    {
        return Cache.IndexOf(FindCategory(code))[code];
    }

    public static bool Have (ObjectCode code)
    {
        return Cache.IndexOf(FindCategory()).ContainsKey(code);
    }

    private enum ObjectCategory
    {
        Character, Enemy, Item
    };

    private enum ObjectCode
    {

    };

    private ObjectCategory FindCategory (ObjectCode code)
    {
        // character code is between 0 and 9
        if ( code >= 0 && code < 10 )
        {
            return ObjectCategory.Character;
        }
        // enemy code is between 10 and 99
        else if ( code >= 10 && code < 100 )
        {
            return ObjectCategory.Enemy;
        } 
        // Item code is over 100
        else if ( code >= 100 )
        {
            return ObjectCategory.Item;
        }
    }
}