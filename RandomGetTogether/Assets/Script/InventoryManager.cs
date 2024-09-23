using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   public ItemScript itemToAdd;
   public ItemScript itemToRemove;

   public List<ItemScript> items = new List<ItemScript>();

    public void Start()
    {
        Add(itemToAdd);
        Remove(itemToRemove);
    }

    public void Add(ItemScript item)
    { 
        items.Add(item);
    
    
    }


    public void Remove(ItemScript item)
    {

        items.Remove(item);
    }

}
