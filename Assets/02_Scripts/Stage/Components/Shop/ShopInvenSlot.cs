// using UnityEngine;
// using UnityEngine.EventSystems;
//
// namespace Apis
// {
//     public class ShopInvenSlot : ItemSlot
//     {
//         public override void OnPointerClick(PointerEventData eventData)
//         {
//         }
//
//         protected override AbsInvenManager Manager => null;
//
//         public override void Init(int idx1, int idx2, InvenType type)
//         {
//             base.Init(idx1, idx2, type);
//             // toggle.OnValueChanged.RemoveAllListeners();
//             // toggle.OnValueChanged.AddListener(isOn =>
//             // {
//             //     if(isOn)
//             //         ShowInfo();
//             // });
//         }
//
//         public override bool Add(Item item)
//         {
//             if (Item != null || item == null) return false;
//
//             // Item = item;
//             // item.slot = this;
//             // itemImage.sprite = item.Image;
//             // itemImage.gameObject.SetActive(true);
//             return true;
//         }
//
//         public override Item Remove()
//         {
//             itemImage.gameObject.SetActive(false);
//             if (Item == null) return null;
//             Item item = Item;
//             Item = null;
//             
//             return item;
//         }
//         [HideInInspector] public ShopInven shop;
//         public void ShowInfo()
//         {
//             if (Item == null) return;
//             
//             switch (Item.Type)
//             {
//                 case ItemType.Accessory:
//                     shop.accDesc.gameObject.SetActive(true);
//                     shop.wpDesc.gameObject.SetActive(false);
//                     shop.accDesc.ChangeInfo(Item);
//                     break;
//                 case ItemType.Weapon:
//                     shop.accDesc.gameObject.SetActive(false);
//                     shop.wpDesc.gameObject.SetActive(true);
//                     shop.wpDesc.ChangeInfo(Item);
//                     break;
//             }
//             shop.buyButton.gameObject.SetActive(false);
//         }
//         
//         // private void OnDisable()
//         // {
//         //     selectedImage.gameObject.SetActive(false);
//         // }
//     }
// }