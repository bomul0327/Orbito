using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    /*
     * 불변식: For item i => [(i.Amount == 0) <=> "i not exists in contents"] 
     */
    private Dictionary<int, int> contents = new Dictionary<int, int>();

    /// <summary>
    /// 인벤토리에 아이템 저장.
    /// </summary>
    /// <param name="item">저장 대상 아이템</param>
    /// <returns></returns>
    public void AddItem(Item item)
    {
        if (contents.ContainsKey(item.ID)) {
            contents[item.ID] += item.Amount;
        }
        else
        {
            contents.Add(item.ID, item.Amount);
        }
    }
    /// <summary>
    /// 지정한 수량만큼 아이템 반환. 수량이 부족하다면 null값 반환.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">반환 수량</param>
    /// <returns></returns>
    public Item GetItem(int id, int amount)
    {
        Item output = null;
        if (contents.ContainsKey(id))
        {
            if (contents[id] >= amount)
            {
                contents[id] -= amount;

                //추후 아이템 정보 테이블 구현시 해당 ID의 MaxAmount정보를 불러온 후 사용
                output = new Item(id, 255, amount);

                if (contents[id] == 0) contents.Remove(id);
            }
        }

        return output;
    }

    /// <summary>
    /// 아이템을 입력한 수량만큼 사용.
    /// 수량이 충분하면 소모 후 true 반환, 아니면 소모하지 않고 false 반환.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">소모 수량</param>
    /// <returns></returns>
    public bool UseItems(int id, int amount)
    {
        if (contents.ContainsKey(id))
        {
            if (contents[id] >= amount)
            {
                contents[id] -= amount;
                if (contents[id] == 0) contents.Remove(id);

                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 인벤토리 내 해당 아이템 수량 반환
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <returns></returns>
    public int GetAmount(int id)
    {
        int amount = 0;
        if (contents.TryGetValue(id, out amount)) { };

        return amount;
    }

    /// <summary>
    /// 주어진 아이템이 인벤토리에 존재하는지 확인합니다(갯수와 무관하게).
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasItem(Item item)
    {
        return contents.ContainsKey(item.ID);
    }

    /// <summary>
    /// 이 아이템이 주어진 갯수만큼 인벤토리에 있는지 확인합니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasSufficientItem(Item item)
    {
        if (!HasItem(item)) return false;

        return item.Amount >= contents[item.ID];
    }

    /// <summary>
    /// 이 인벤토리가 ItemRequirements의 조건을 만족하는 지 확인합니다. 
    /// </summary>
    /// <param name="itemRequirements"></param>
    public bool HasSufficientItems(List<Item> itemRequirements)
    {
        foreach (var item in itemRequirements)
        {
            if (!HasSufficientItem(item)) return false;
        }

        return true;
    }

    /// <summary>
    /// 인벤토리에 충분한 Item이 있는지 확인하고, 충분하다면 사용합니다.
    /// </summary>
    /// <param name="itemRequirements"></param>
    /// <returns></returns>
    public bool TryUseItems(List<Item> itemRequirements)
    {
        if (!HasSufficientItems(itemRequirements)) return false;

        //Requirements에 명시된 아이템을 소모합니다.
        foreach (var item in itemRequirements)
        {
            UseItems(item.ID, item.Amount);
        }

        return true;
    }

}
