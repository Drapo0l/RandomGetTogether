using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemScript itemToAdd; 
    [SerializeField] private ItemScript itemToRemove;

    [SerializeField] private SlotsScript[] startingItems;

    private SlotsScript[] items;

    private SlotsScript movingSlot;


    private GameObject[] slots;

    public void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotsScript[slots.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotsScript();

        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }


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

                    if (items[i].GetItem().isStackable)
                slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";
                
                    else
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = "";

            }

            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";

            }
        }
    
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(GetClosestSlot());

        }
    }
    public bool Add(ItemScript item)
    {
        SlotsScript slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
            slot.AddQuantity(1);

        else
        {
           for (int i =0; i < items.Length; i++ )
            {
                if (items [i].GetItem() == null)
                {
                    items[i].AddItem(item, 1);
                    break;
                }
            }
        }



        RefreshUI();
        return true;
    }

    public bool Remove(ItemScript item)
    {
        SlotsScript temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
                temp.SubQuantity(1);

            else
            {
               int slotToRemoveIndex = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
            RefreshUI();
            return true;

        }
        else
        {
            return false;
        }
       
    }

    SlotsScript Contains(ItemScript item)
    {
        foreach (SlotsScript slot in items)
        {
            if (slot.GetItem() == item)
            return slot;
        }

        return null;
    }

    private SlotsScript GetClosestSlot()
    {
        Debug.Log(Input.mousePosition);

        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
                    return items[i];
        }

        return null;
    }
    
}
