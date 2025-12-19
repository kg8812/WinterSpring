using chamwhy.UI.Focus;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy
{
    public class UITab_Accessory : UITab_Inventory
    {
        protected override InventoryGroup invenGroupManager => InvenManager.instance.Acc;
    }
}