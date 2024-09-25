using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Tool Script", menuName = "Item/Consumable")]
public class ConsumableItems : ItemScript
{
    [Header("Consumables")]
    public float healthAdded;
    public override ItemScript GetItem() { return this; }
    public override ToolsScript GetTools() { return null; }
    public override MiscItems GetMisc() { return null; }
    public override ConsumableItems GetConsumables() { return null; }
}
