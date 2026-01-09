using System;
using Apis;
using chamwhy.UI;
using NewNewInvenSpace;
using UI;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace chamwhy
{
    public class ItemSlot: UIAsset_Toggle
    {
        // public static bool IsDragging;
        // public static UI_DragItem DragImg;
        // public static ItemSlot ToChangeSlot;
        
        [SerializeField] private Image itemImg;
        [SerializeField] private Image changeImg;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Color frozenColor;
        [SerializeField] private Image equippedImg;
        [SerializeField] private Image lockedImg;

        [HideInInspector] public int index;
        // [HideInInspector] public InventoryGroup inventoryGroup;

        public InventoryList InventoryList { get; set; }
        // tab 메뉴에만 해당.(예외처리)
        // [HideInInspector] public UI_InventoryContent tabInventory;

        public InvenType invenType;
        [HideInInspector] public Item curItem;

        private Guid _dragGuid;

        // true = start / false = end
        public Action<ItemSlot, bool> OnDragChanged;
        public Action<ItemSlot, bool> OnPointerChanged;


        public void UpdateItem()
        {
            OnSlotChanged(index, InventoryList[index]);
        }

        public void OnSlotChanged(int ind, Item item)
        {
            if (ind != index) return;
            curItem = item;
            
            if (item != null)
            {
                itemImg.sprite = item.Image;
                itemImg.enabled = true;
                item.slot = this;
                item.SaveData.slotIndex = ind;

                if (lockedImg != null)
                {
                    lockedImg.enabled = false;
                }

                if (equippedImg != null)
                {
                    equippedImg.enabled = item.IsEquip;
                }
            }
            else
            {
                itemImg.enabled = false;
                if (lockedImg != null)
                {
                    lockedImg.enabled = true;
                }
            }
        }

        public override void KeyControl()
        {
        }

        public override void GamePadControl()
        {
        }
        public override void FrozenToggle(bool isOn)
        {
            base.FrozenToggle(isOn);
            backgroundImg.color = isOn ? frozenColor : Color.white;
        }

        public void ChangedToggle(bool isOn)
        {
            if (changeImg == null) return;
            
            changeImg.enabled = isOn;
        }

        public void ToggleItemImg(bool isOn)
        {
            itemImg.enabled = isOn;
        }


        #region DragSection

        

        public override void OnBeginDrag(PointerEventData eventData)
        {
            OnDragChanged?.Invoke(this, true);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            OnDragChanged?.Invoke(this, false);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerChanged?.Invoke(this, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnPointerChanged?.Invoke(this, false);
        }
        
        #endregion
    }
}