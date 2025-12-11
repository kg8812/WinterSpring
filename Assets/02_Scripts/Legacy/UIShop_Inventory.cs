using Apis;
using chamwhy.UI.Focus;
using NewNewInvenSpace;
using UnityEngine;

namespace chamwhy.UI
{
    public class UIShop_Inventory: UI_InventoryContent
    {
        /*
        public FocusParent ShopList;
        public FocusParent WpInven;
        public FocusParent AccInven;

        [SerializeField] private ItemSlot[] wpSlots;
        [SerializeField] private ItemSlot[] accSlots;

        [SerializeField] private AccDescription accDesc;
        [SerializeField] private AttackItemDescription wpDesc;


        private InventoryGroup _curInvenGroup;

        private ItemType _curInvenItemType;
        
        
        private void SlotInit(ItemSlot[] slotList, InventoryGroup inventoryGroup)
        {
            for (int i = 0; i < slotList.Length; i++)
            {
                ItemSlot slot = slotList[i];
                slot.invenType = InvenType.Storage;
                slot.InventoryList = inventoryGroup.Invens[InvenType.Storage];
                slot.index = i;
                // slot.tabInventory = this;
                slot.InitCheck();
                slot.OnValueChanged.AddListener(isOn =>
                {
                    if(isOn)
                        ShowInfo(slot.curItem);
                });
                inventoryGroup.Invens[InvenType.Storage].OnSlotChanged += slot.OnSlotChanged;
                // slot.SelectedForChange.AddListener(SelectedForChange);
                slot.UpdateItem();
            }
        }

        public void ShowInfo(Item item)
        {
            if (item is Weapon weapon)
            {
                accDesc.gameObject.SetActive(false);
                wpDesc.ChangeInfo(weapon);
                wpDesc.gameObject.SetActive(true);
            }else if (item is Accessory acc)
            {
                wpDesc.gameObject.SetActive(false);
                accDesc.ChangeInfo(acc);
                accDesc.gameObject.SetActive(true);
            }
        }

        public override void ChangeFocusParent(FocusParent fp)
        {
            base.ChangeFocusParent(fp);
            if (ReferenceEquals(fp, ShopList))
            {
                
            }
            else
            {
                
            }
        }

        public void TryChangeDiffInvenType(ItemSlot slot)
        {
            return;
        }
        
        #region KeyBoardMoveSection

        private void MoveLeft(int y)
        {
            // int realX = Default.FormatUtils.GetRatioIntByInt(x, Equipment.tableData.x, Inven.tableData.x);
            
            _curFocus = ShopList;
            ShopList.MoveTo(ShopList.curId);
        }

        private void MoveRight(int y)
        {
            // int realX = Default.FormatUtils.GetRatioIntByInt(x, Equipment.tableData.x, Inven.tableData.x);
            _curFocus = _curInvenItemType == ItemType.Accessory ? AccInven : WpInven;
            _curFocus.MoveTo(0);
        }

        #endregion
        */
    }
}