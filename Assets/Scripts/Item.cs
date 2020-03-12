using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int Id { get; private set; }
    public int Amount;
    public int MaxAmount { get; private set; }

    public Item(int id, int amount, int maxAmount)
    {
        this.Id = id;
        this.Amount = amount;
        this.MaxAmount = maxAmount;
    }
}
