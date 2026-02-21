using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewNewInvenSpace
{
    public class AtkItemInventoryList : InventoryList
    {
        public AtkItemInventoryList(int maxCnt, int cnt) : base(maxCnt, cnt)
        {
        }

        public override bool AddItem(Item item)
        {
            if (item is not IAttackItem atkItem) return false;
            int i = atkItem.InvenSlotIndex - 1;
            if(i < 0 || i >= Count) return false;
            this[i] = item;
            ItemAddedTo?.Invoke(i, item);
            return true;
        }

        public override bool AddItem(int index, Item item)
        {
            return AddItem(item);
        }
    }
}