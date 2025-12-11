// using Apis;
//
// namespace chamwhy.ItemSlot
// {
//     public class AccItemSlot2: ItemSlot2
//     {
//         public override void Equip() // 아이템 장착
//         {
//             if (Item != null && Item is Accessory)
//             {
//                 if (InvenManager2.instance.Acc.Add(Item, InvenType.Equipment))
//                 {
//                     Item.Equip(GameManager.instance.Player);
//                     Item = null;
//                 }
//             }
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
//         public override void UnEquip() // 아이템 장착 해제
//         {
//             if (Item == null || Item is not Accessory)
//             {
//                 return;
//             }
//             if (InvenManager2.instance.Acc.Add(Item, InvenType.Inven))
//             {
//                 Item.UnEquip();
//                 Item = null;
//             }
//         }
//     }
// }