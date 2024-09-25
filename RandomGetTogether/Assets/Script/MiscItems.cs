using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Tool Script", menuName = "Item/Misc")]
public class MiscItems : ItemScript
{

    public override ItemScript GetItem() { return this; }
    public override ToolsScript GetTools() { return null; }
    public override MiscItems GetMisc() { return this; }
    public override ConsumableItems GetConsumables() 
    {
        return null;
    }
}
