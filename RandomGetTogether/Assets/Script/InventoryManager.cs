using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemScript itemToAdd;
    [SerializeField] private ItemScript itemToRemove;

   public List<ItemScript> items = new List<ItemScript>();

    private GameObject[] slots;

    public void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
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
