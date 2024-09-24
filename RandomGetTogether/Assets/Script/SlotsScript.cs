using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotsScript
{
    private ItemScript item;
    private int quantity;

    public SlotsScript (ItemScript _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;

    }

    public SlotsScript()
    {
        item = null;
        quantity = 0;
    }

    public ItemScript GetItem()
    {
        return item;
    
    }

    public int GetQuantity() 
    {
        return quantity;
    }

    public void AddQuantity(int _quantity)
    {
        quantity += _quantity;
    }
}
