using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

/// <summary>
/// Launchpad 제조에 필요한 재료 정보가 정의된 Spec 클래스.
/// </summary>
public class LaunchpadSpec 
{
    /// <summary>
    /// Spec 이름.
    /// </summary>
    public string Name;

    /// <summary>
    /// 제조에 필요한 아이템 리스트.
    /// </summary>
    public List<Item> Items = new List<Item>();

    /// <summary>
    /// 제조에 필요한 자원.
    /// </summary>
    public int Resource;

    /// <summary>
    /// LaunchpadSpec 생성자. JObject를 Parsing합니다.
    /// </summary>
    /// <param name="jObject">Json파일에서 로드된 JObject.</param>
    public LaunchpadSpec(JObject jObject)
    {
        Items.Clear();
        Name = jObject["Name"].ToString();

        Resource = int.Parse(jObject["Resource"].ToString());
        var itemDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(jObject["Items"].ToString());

        foreach (var itemName in itemDict.Keys)
        {
            int itemId = int.Parse(itemName);
            int itemAmount = itemDict[itemName];

            Items.Add(new Item(itemId, itemAmount, 0));
        }
    }

}