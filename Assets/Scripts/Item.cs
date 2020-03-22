using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int ID { get; private set; }
    public int Amount;
    public int MaxAmount { get; private set; }

    public Item(int id, int amount, int maxAmount)
    {
        this.ID = id;
        this.Amount = amount;
        this.MaxAmount = maxAmount;
    }
}
