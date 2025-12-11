// using UnityEngine.EventSystems;
//
// namespace Apis
// {
//     public class AccSlot : ItemSlot
//     {
//         // 인벤토리 내 아이템 슬롯              
//
//         protected override AbsInvenManager Manager => InvenManager.instance.Acc;
//
//         public override Item Abandon() // 버리기
//         {
//             Item item = base.Abandon();
//             if (Type == InvenType.Equipment && item != null)
//             {
//                 item.UnEquip();
//             }
//
//             if (item != null && item is Accessory accessory)
//             {
//                 Acc_PickUp pu = GameManager.Item.AccPickUp.CreateExisting(accessory);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//             return item;
//         }
//
//         public override Item Remove()
//         {
//             Item item = base.Remove();
//             if (Type == InvenType.Equipment && item != null)
//             {
//                 item.UnEquip();
//             }
//
//             return item;
//         }
//
//         public override void Equip() // 아이템 장착
//         {
//             if (Item != null && Item is Accessory)
//             {
//                 if (InvenManager.instance.Acc.Add(Item, InvenType.Equipment))
//                 {
//                     Item.Equip(GameManager.instance.Player);
//                     Item = null;
//                     itemImage.gameObject.SetActive(false);
//                 }
//             }
//         }
//
//         public override void UnEquip() // 아이템 장착 해제
//         {
//             if (Item == null || Item is not Accessory)
//             {
//                 return;
//             }
//             if (InvenManager.instance.Acc.Add(Item, InvenType.Inven))
//             {
//                 Item.UnEquip();
//                 Item = null;
//                 itemImage.gameObject.SetActive(false);
//             }
//         }
//
//         public void Change()
//         {
//
//             if (InvenManager.instance.Acc.changeSlot == null)
//             {
//                 InvenManager.instance.Acc.changeSlot = this;
//                 InvenManager.instance.Acc.controller.ToChangeCtrl();
//             }
//             else
//             {
//                 Item temp = Remove();
//                 AccSlot slot = InvenManager.instance.Acc.changeSlot;
//                 switch (Type)
//                 {
//                     case InvenType.Inven:
//                         Add(slot.Remove());
//                         slot.Add(temp);
//                         if (temp != null)
//                         {
//                             temp.Equip(GameManager.instance.Player);
//                         }
//                         break;
//                     case InvenType.Equipment:
//                         Add(slot.Remove());
//
//                         if (Item != null)
//                         {
//                             Item.Equip(GameManager.instance.Player);
//                         }
//                         slot.Add(temp);
//                         break;
//                 }
//                 InvenManager.instance.Acc.changeSlot = null;
//             }
//         }
//
//         public override void OnPointerClick(PointerEventData eventData) // 클릭 함수
//         {
//             if (eventData.button == PointerEventData.InputButton.Right) // 우클릭
//             {
//                 if (InvenManager.instance.Wp.controller.Current is Acc_ChangeCtrl) return;
//
//                 UnSelect();
//                 Manager.MouseSelect(this);
//                 selectedImage.gameObject.SetActive(true);
//
//                 if (InvenManager.instance.Acc.selected != null && InvenManager.instance.Acc.selected.Item != null)
//                 {
//                     InvenManager.instance.Acc.OpenMenuWindow(Type);
//                 }
//             }
//             else if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭
//             {
//                 if (InvenManager.instance.Wp.controller.Current is Acc_ChangeCtrl)
//                 {
//                     if (InvenManager.instance.Acc.selected == this)
//                     {
//                         Acc_ChangeCtrl.Change(InvenManager.instance.Acc.controller.Current as Acc_ChangeCtrl);
//                         return;
//                     }
//                 }
//                 UnSelect();
//                 Manager.MouseSelect(this);
//                 selectedImage.gameObject.SetActive(true);
//             }
//         }
//     }
// }