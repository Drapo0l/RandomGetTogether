using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0;i < slotHolder.transform.childCount; i++)
        slots[i] = slotHolder.transform.GetChild(i).gameObject;

        RefreshUI();

        Add(itemToAdd);
        Remove(itemToRemove);
    }

    public void RefreshUI()
    { 
        for (int i = 0; i < slots.Length; i++) 
        {
            try
            {
                slots[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = items[i].ItemImage;
            }

            catch
            {
                slots[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;

            }
        }
    
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
