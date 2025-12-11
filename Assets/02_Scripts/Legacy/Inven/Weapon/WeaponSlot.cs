// using UnityEngine.EventSystems;
//
// namespace Apis
// {
//     public class WeaponSlot : ItemSlot
//     {
//         protected override AbsInvenManager Manager
//         {
//             get
//             {
//                 try
//                 {
//                     return InvenManager.instance.Wp;
//                 }
//                 catch
//                 {
//                     return null;
//                 }
//             }
//         }
//         public override void Equip()
//         {
//             if (Item != null && Item is Weapon wp)
//             {
//                 Item temp = Remove();
//                 Add(InvenManager.instance.Wp.Invens[InvenType.Equipment].Slots[0, 0].Remove());
//                 InvenManager.instance.Wp.Add(temp, InvenType.Equipment);
//
//                 GameManager.instance.Player.Equip(wp,0);
//             }
//         }
//
//         public override Item Remove()
//         {
//             Item item = base.Remove();
//             if (Type != InvenType.Equipment || item == null) return item;
//             
//             item.UnEquip();
//
//             item.slot = null;
//             item.SaveData.slotIndex = -1;
//             return item;
//         }
//
//         public override void OnPointerClick(PointerEventData eventData)
//         {
//             if (eventData.button == PointerEventData.InputButton.Right) // 우클릭
//             {
//                 if (InvenManager.instance.Wp.controller.Current is Weapon_ChangeCtrl) return;
//
//                 UnSelect();
//                 Manager.MouseSelect(this);
//                 selectedImage.gameObject.SetActive(true);
//                 if (InvenManager.instance.Wp.selected != null)
//                 {
//                     if (InvenManager.instance.Wp.selected.Item != null)
//                     {
//                         InvenManager.instance.Wp.OpenMenuWindow(Type);
//                     }
//                 }
//             }
//             else if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭
//             {
//                 if (InvenManager.instance.Wp.controller.Current is Weapon_ChangeCtrl)
//                 {
//                     if(InvenManager.instance.Wp.selected == this)
//                     {
//                         Weapon_ChangeCtrl.Equip(InvenManager.instance.Wp.controller.Current as Weapon_ChangeCtrl);
//                         return;
//                     }
//                 }
//                 UnSelect();
//                 Manager.MouseSelect(this);
//                 selectedImage.gameObject.SetActive(true);
//             }
//         }
//
//         public override void UnEquip()
//         {
//             GameManager.instance.Player.UnEquip(Item as Weapon);
//         }
//
//         public override Item Abandon()
//         {
//             Item item = base.Abandon();
//
//             if (item != null && item is Weapon)
//             {
//                 Weapon_PickUp pu = GameManager.Item.WeaponPickUp.CreateExisting(item as Weapon);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             return item;
//         }
//     }
// }