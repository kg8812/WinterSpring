// using System;
// using Apis;
// using UI;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public class AttackItemInvenGroupManager : InvenGroupManager
//     {
//         private bool CheckEquipCondition(IAttackItem atkItem)
//         {
//             return true;
//             return !Array.Exists(Invens[InvenType.Equipment].Slots,
//                 x => x is IAttackItem item && (item == atkItem || item.Category == atkItem.Category));
//         }
//
//         public bool AddOrigin(Item item, InvenType type) => base.Add(item, type);
//         public bool AddOrigin(int index, Item item, InvenType type) => base.Add(index, item, type);
//
//         public override bool Add(Item item, InvenType type)
//         {
//             if (item is IAttackItem atkItem && CheckEquipCondition(atkItem) && AddOrigin(item, type))
//             {
//                 if(type == InvenType.Equipment)
//                     AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//             return false;
//         }
//
//         public override bool Add(int index, Item item, InvenType type)
//         {
//             if (item is IAttackItem atkItem && CheckEquipCondition(atkItem) && AddOrigin(index, item, type))
//             {
//                 if(type == InvenType.Equipment)
//                     AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//             return false;
//         }
//
//         public override bool Equip(int index)
//         {
//             if (EquipWithoutSavePreset(index))
//             {
//                 AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//             return false;
//         }
//
//         public bool EquipWithoutSavePreset(int index)
//         {
//             if (!CheckInvenAndIndex(index, InvenType.Inven, out var inventory)
//                 || inventory[index] == null
//                 || inventory[index] is not IAttackItem atkItem
//                 || !CheckEquipCondition(atkItem)
//                 || !Invens.TryGetValue(InvenType.Equipment, out var inventory2)) return false;
//             int equipInd = inventory2.GetEmptySlot();
//             if (equipInd == -1) return false;
//
//             Change(index, InvenType.Inven, equipInd, InvenType.Equipment);
//             
//             return true;
//         }
//
//         public override Item Abandon(int index, InvenType type)
//         {
//             // weapon의 경우 장착한 개체를 버릴 수 없으니 예외처리
//             // if (type == InvenType.Equipment) return null;
//
//             Item item = base.Abandon(index, type);
//             if (item != null && item is Weapon wp)
//             {
//                 Weapon_PickUp pu = GameManager.Item.WeaponPickUp.CreateExisting(wp);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             if (item != null && item is ActiveSkillItem sk)
//             {
//                 ActiveSkill_PickUp pu = GameManager.Item.ActiveSkillPickUp.CreateExisting(sk);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             return item;
//         }
//
//         public Item ChangeAttackItem(Item item, int index = 0)
//         {
//             Item preItem = Invens[InvenType.Equipment][index];
//             UnEquipped(preItem);
//             Invens[InvenType.Equipment][index] = item;
//             Equipped(item);
//             return preItem;
//         }
//
//         protected override void Equipped(Item item)
//         {
//             if (item != null && item is IAttackItem attackItem)
//             {
//                 // GameManager.instance.Player.Equip(wp,0);
//                 int index = GetIndex(item.ItemName, InvenType.Equipment);
//                 attackItem.AtkSlotIndex = index;
//                 attackItem.SetIcon(UI_MainHud.Instance.atkItemIcons[index]);
//                 UI_MainHud.Instance.atkItemIcons[index].SetItem(attackItem);
//                 
//                 //아이콘 설정을 먼저해야함. 순서 변경해서 아이콘 설정보다 장착이 위로오면 아이콘 작동안함.
//                 attackItem.Equip(GameManager.instance.Player);
//             }
//         }
//
//         protected override void UnEquipped(Item item)
//         {
//             if (item != null && item is IAttackItem attackItem)
//             {
//                 // GameManager.instance.Player.UnEquip(weapon);
//                 UI_AtkItemIcon icon = UI_MainHud.Instance.atkItemIcons[attackItem.AtkSlotIndex];
//                 icon.SetItem(null);
//                 attackItem.UnEquip();
//                 item.SetParent(null);
//                 attackItem.SetIcon(null);
//             }
//         }
//     }
// }