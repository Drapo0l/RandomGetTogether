using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Tool Script", menuName = "Item/Tool")]
public class ToolsScript : ItemScript
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        weapon,
        axe,
        sword
         
    }


    public override ItemScript GetItem() { return this; }
    public override ToolsScript GetTools() { return this; }
    public override MiscItems GetMisc()  {return null; }
    public override ConsumableItems GetConsumables() { return null; }
}
