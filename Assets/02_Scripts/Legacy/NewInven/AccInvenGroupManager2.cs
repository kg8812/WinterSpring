// using System;
// using System.Collections.Generic;
// using Apis;
// using chamwhy.Inven;
// using UI;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public class AccInvenGroupManager2 : IInvenGroupManager
//     {
//         public InvenGroupManager IgManager { get; private set; }
//         public Dictionary<InvenType, InventoryGroup> Invens => IgManager.Invens;
//
//
//         public virtual void Init(int equipSlotCnt, int equipAvailSlotCnt, int invenSlotCnt, int invenAvailSlotCnt)
//         {
//             IgManager = new();
//             IgManager.Init(equipSlotCnt, equipAvailSlotCnt, invenSlotCnt, invenAvailSlotCnt);
//             IgManager.ItemEquipped += Equipped;
//             IgManager.ItemUnEquipped += UnEquipped;
//         }
//
//         public bool Add(Item item, InvenType type)
//         {
//             if (item is Accessory acc && IgManager.Add(item, type))
//             {
//                 // if (type == InvenType.Equipment)
//                 //     AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//             return false;
//         }
//
//         public bool Add(int index, Item item, InvenType type)
//         {
//             if (item is Accessory acc && IgManager.Add(index, item, type))
//             {
//                 // if (type == InvenType.Equipment)
//                 //     AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//
//             return false;
//         }
//
//         public bool Equip(int index)
//         {
//             if (!IgManager.Equip(index)) return false;
//             // AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public bool UnEquip(int index)
//         {
//             if (!IgManager.UnEquip(index)) return false;
//             // AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public bool UnEquipAll(bool unEquipWithAvailable = false)
//         {
//             if (!IgManager.UnEquipAll(unEquipWithAvailable)) return false;
//             // AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public bool Change(int index1, InvenType type1, int index2, InvenType type2)
//         {
//             if (!IgManager.Change(index1, type1, index2, type2)) return false;
//             // AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public Item Remove(int index, InvenType type)
//         {
//             return IgManager.Remove(index, type);
//             AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//         }
//
//         public Item Abandon(int index, InvenType type)
//         {
//             // weapon의 경우 장착한 개체를 버릴 수 없으니 예외처리
//             // if (type == InvenType.Equipment) return null;
//
//             Item item = IgManager.Abandon(index, type);
//             if (item != null  && item is Accessory accessory)
//             {
//                 Acc_PickUp pu = GameManager.Item.AccPickUp.CreateExisting(GameManager.Item.Storage.Get(accessory));
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             return item;
//         }
//
//         private void Equipped(Item item)
//         {
//             if (item != null && item is Accessory acc)
//             {
//                 acc.Equip(GameManager.instance.Player);
//             }
//         }
//
//         private void UnEquipped(Item item)
//         {
//             if (item != null && item is Accessory acc)
//             {
//                 acc.UnEquip();
//             }
//         }
//     }
// }