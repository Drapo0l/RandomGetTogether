using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemScript itemToAdd; 
    [SerializeField] private ItemScript itemToRemove;

    
   public List<SlotsScript> items = new List<SlotsScript>();

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
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().ItemImage;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";
            }

            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";

            }
        }
    
    }


    public void Add(ItemScript item)
    {
        SlotsScript slot = Contains(item);
        if (slot != null)
            slot.AddQuantity(1);

        else
        {
            items.Add(new SlotsScript(item, 1));

        }

        RefreshUI();
    }

    public void Remove(ItemScript item)
    {
        SlotsScript slotToRemove = null;
        foreach (SlotsScript slot in items)
        {
            if (slot.GetItem() == item)
            { 
                slotToRemove = slot;
                break;
            
            }
            
        }
        if (slotToRemove != null)
        { 
            items.Remove(slotToRemove);
        }

        items.Remove(slotToRemove);
        RefreshUI();
        
    }


    public SlotsScript Contains(ItemScript item)
    {
        foreach (SlotsScript slot in items)
        {
            if (slot.GetItem() == item)
            return slot;
        }

        return null;
    }

}
