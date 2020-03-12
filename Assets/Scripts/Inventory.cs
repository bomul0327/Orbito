using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public int Size { get; private set;}
    private int curSize;
    private Dictionary<int, List<Item>> inventory = new Dictionary<int, List<Item>>();

    public Inventory(int size)
    {
        this.Size = size;
        curSize = 0;
    }
    /// <summary>
    /// 인벤토리에 아이템 저장. 공간이 부족하다면 남는 만큼 아이템 반환, 아니면 null 반환.
    /// </summary>
    /// <param name="item">저장 대상 아이템</param>
    /// <returns></returns>
    public Item PutItem(Item item)
    {
        Item res = item;//해당 아이템이 기존에 없고 공간도 없을 경우 대상 아이템 그대로 반환
        List<Item> existItems = null;

        //기존에 해당 아이템이 있을 경우
        if (inventory.TryGetValue(item.id, out existItems))
        {
            Item lastItem = existItems[existItems.Count - 1];
            //아이템 최대 개수 넘을 경우(새로운 슬롯에 아이템 추가)
            if (lastItem.amount + item.amount > lastItem.maxAmount)
            {
                //여유 공간이 없다면 아이템 개수 채우고 나머지를 반환
                if(curSize == Size)
                {
                    res.amount -= lastItem.maxAmount - lastItem.amount;
                    lastItem.amount = lastItem.maxAmount;
                }
                else
                {
                    existItems.Add(new Item(item.id, item.amount + lastItem.amount - item.maxAmount, item.maxAmount));
                    curSize++;
                    lastItem.amount = lastItem.maxAmount;
                    res = null;
                }
            }
            else
            {
                lastItem.amount += item.amount;
                res = null;
            }
        }
        //해당 아이템이 없고 공간 여유가 있을 경우
        else if(curSize < Size)
        {
            List<Item> tmp = new List<Item>();
            tmp.Add(item);
            inventory.Add(item.id, tmp);
            curSize++;
            res = null;
        }
        
        return res;
    }
    /// <summary>
    /// 지정한 수량만큼 아이템 반환. 입력한 수량이 해당 아이템의 최대개수보다 크면 자동으로 최대개수 만큼 반환.
    /// 입력한 수량보다 보유한 수량이 적은 경우 보유한 수량만큼 반환.
    /// 해당 아이템이 존재하지 않는 경우 null 반환.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">반환 수량</param>
    /// <returns></returns>
    public Item GetItem(int id, int amount)
    {
        List<Item> existItems = null;
        Item res = null;
        if (inventory.TryGetValue(id, out existItems))
        {
            if (amount > existItems[0].maxAmount || amount >= CountItems(existItems))
                return GetMaxItem(id);

            Item lastItem = existItems[existItems.Count - 1];
            if (lastItem.amount > amount)
            {
                lastItem.amount -= amount;
            }
            else
            {
                existItems.Remove(lastItem);
                curSize--;
                existItems[existItems.Count - 1].amount -= amount - lastItem.amount;
            }
            res = new Item(id, amount, lastItem.maxAmount);
        }

        return res;
    }

    /// <summary>
    /// 인벤토리에서 해당 아이템을 전부 반환. 최대개수를 넘는다면 최대개수까지만 반환
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <returns></returns>
    public Item GetMaxItem(int id)
    {
        List<Item> existItems = null;
        Item res = null;
        if (inventory.TryGetValue(id, out existItems))
        {
            res = existItems[0];
            existItems.Remove(res);
            curSize--;
            if (existItems.Count == 0)
            {
                inventory.Remove(id);
            }
        }

        return res;
    }

    /// <summary>
    /// 아이템을 입력한 수량만큼 소모. 해당 아이템의 최대개수 이상도 소모할 수 있다.
    /// 수량이 충분하면 소모 후 true 반환, 아니면 소모하지 않고 false 반환.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">소모 수량</param>
    /// <returns></returns>
    public bool ConsumeItems(int id, int amount)
    {

        List<Item> existItems;
        if (inventory.TryGetValue(id, out existItems))
        {
            if (CountItems(existItems) < amount) return false;

            int remainAmount = amount;
            Item lastItem;
            while (remainAmount != 0)
            {
                lastItem = existItems[existItems.Count - 1];
                if (lastItem.amount > remainAmount)
                {
                    lastItem.amount -= remainAmount;
                    break;
                }
                else
                {
                    existItems.Remove(lastItem);
                    remainAmount -= lastItem.amount;
                }
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// 인벤토리 내 해당 아이템 수량 반환
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <returns></returns>
    public int GetItemNum(int id)
    {
        int res = 0;
        List<Item> existItems;
        if(inventory.TryGetValue(id, out existItems))
        {
            res = CountItems(existItems);
        }

        return res;
    }

    //아이템 리스트에서 아이템 총 수량 계산 후 반환
    private static int CountItems(List<Item> list)
    {
        int res = 0;
        foreach (Item item in list)
        {
            res += item.amount;
        }

        return res;
    }
}
