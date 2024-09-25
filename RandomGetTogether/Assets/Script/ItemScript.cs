using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ItemScript : ScriptableObject
{
    [SerializeField] GameObject itemModel;

    public string itemName;
    public Sprite ItemImage;
    public bool isStackable = true;

    

//Getters
    public abstract ItemScript GetItem();
    public abstract ToolsScript GetTools();
    public abstract MiscItems GetMisc();
    public abstract ConsumableItems GetConsumables();
}
