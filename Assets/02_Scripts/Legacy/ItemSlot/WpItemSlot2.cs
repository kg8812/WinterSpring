// using Apis;
//
// namespace chamwhy.ItemSlot
// {
//     public class WpItemSlot2: ItemSlot2
//     {
//         public override void Equip()
//         {
//             if (Item != null && Item is Weapon wp && Type == InvenType.Inven)
//             {
//                 Item temp = Remove();
//                 InvenManager2.instance.Wp.Add(temp, InvenType.Equipment);
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
//
//             return item;
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
//             if (item != null && item is Weapon weapon)
//             {
//                 Weapon_PickUp pu = GameManager.Item.WeaponPickUp.CreateExisting(weapon);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             return item;
//         }
//     }
// }