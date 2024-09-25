using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotsScript
{
   [SerializeField] private ItemScript item;
   [SerializeField] private int quantity;

    public SlotsScript (ItemScript _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;

    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
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
    public void SubQuantity(int _quantity)
    {
        quantity -= _quantity;
    }

    public void AddItem(ItemScript item, int quantity)
    {
        this.quantity = quantity;

    }
}
