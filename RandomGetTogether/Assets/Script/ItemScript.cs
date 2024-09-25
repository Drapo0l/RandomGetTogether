using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScript : ScriptableObject
{
    public string itemName;
    public Sprite ItemImage;


//Getters
    public abstract ItemScript GetItem();
    public abstract ToolsScript GetTools();
    public abstract MiscItems GetMisc();
    public abstract ConsumableItems GetConsumables();
}
