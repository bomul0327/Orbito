using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<int, int> contents = new Dictionary<int, int>();

    /// <summary>
    /// 인벤토리에 아이템 저장.
    /// </summary>
    /// <param name="item">저장 대상 아이템</param>
    /// <returns></returns>
    public void AddItem(Item item)
    {
        if (contents.ContainsKey(item.ID)){
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
        if(contents.ContainsKey(id))
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
        if(contents.ContainsKey(id))
        {
            if(contents[id] >= amount)
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

}
