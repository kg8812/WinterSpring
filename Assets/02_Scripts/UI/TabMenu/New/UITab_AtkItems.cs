using System.Collections.Generic;
using Apis;
using chamwhy.UI.Focus;
using NewNewInvenSpace;
using UnityEngine;

namespace chamwhy
{
    public class UITab_AtkItems: UITab_Inventory
    {
        protected override InventoryGroup invenGroupManager => InvenManager.instance.AttackItem;

        protected override string itemSlotAddress => "MagicSlot";
    }
}