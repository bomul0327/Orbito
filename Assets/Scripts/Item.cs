using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id { get; private set; }
    public int amount;
    public int maxAmount { get; private set; }

    public Item(int id, int amount, int maxAmount)
    {
        this.id = id;
        this.amount = amount;
        this.maxAmount = maxAmount;
    }
}
