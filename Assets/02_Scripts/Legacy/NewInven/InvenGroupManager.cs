// using System.Collections.Generic;
// using Apis;
// using chamwhy.Inven;
// using JetBrains.Annotations;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public class InvenGroupManager: IInvenGroupManager
//     {
//         public delegate void ToggleItemEquipped(int index, Item item);
//         
//         public Dictionary<InvenType, InventoryGroup> Invens { get; private set; }
//
//         public ToggleItemEquipped ItemEquipped;
//         public ToggleItemEquipped ItemUnEquipped;
//
//         public virtual void Init(int equipSlotCnt, int equipAvailSlotCnt, int invenSlotCnt, int invenAvailSlotCnt)
//         {
//             Invens = new();
//             Invens.Add(InvenType.Equipment, new InventoryGroup(InvenType.Equipment, equipSlotCnt, equipAvailSlotCnt));
//             Invens.Add(InvenType.Inven, new InventoryGroup(InvenType.Inven, invenSlotCnt, invenAvailSlotCnt));
//
//             Invens[InvenType.Equipment].OnItemExcepted = (int ind, Item item) =>
//             {
//                 if (!UnEquip(ind))
//                 {
//                     Abandon(ind, InvenType.Equipment);
//                 }
//             };
//             Invens[InvenType.Inven].OnItemExcepted = (int ind, Item item) => Abandon(ind, InvenType.Inven);
//         }
//
//         public virtual bool Add(Item item, InvenType type)
//         {
//             if (item == null) return false;
//             if (Invens.TryGetValue(type, out var inven))
//             {
//                 if (inven.AddItem(item))
//                 {
//                     if(type == InvenType.Equipment)
//                         Default.Utils.ActionOnPlayerReady(_ => ItemEquipped?.Invoke(GetIndex(item, InvenType.Equipment), item));
//                     item.SetParent(null);
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//         public virtual bool Add(int index, Item item, InvenType type)
//         {
//             if (item == null) return false;
//             if (Invens.TryGetValue(type, out var inven))
//             {
//                 if (inven.AddItem(index, item))
//                 {
//                     if(type == InvenType.Equipment)
//                         Default.Utils.ActionOnPlayerReady(_ => ItemEquipped?.Invoke(index, item));
//                     item.SetParent(null);
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//         public int GetIndex(string itemName, InvenType type)
//         {
//             if (Invens.TryGetValue(type, out var inven))
//             {
//                 for (int i = 0; i < inven.Count; i++)
//                 {
//                     if (inven[i] != null && inven[i].ItemName == itemName)
//                     {
//                         return i;
//                     }
//                 }
//             }
//
//             return -1;
//         }
//
//         public int GetIndex(Item item, InvenType type)
//         {
//             if (Invens.TryGetValue(type, out var inven))
//             {
//                 for (int i = 0; i < inven.Count; i++)
//                 {
//                     if (inven[i] != null && inven[i] == item)
//                     {
//                         return i;
//                     }
//                 }
//             }
//             return -1;
//         }
//
//         public bool Contains(string itemName, InvenType type)
//         {
//             if (Invens.TryGetValue(type, out var inven))
//             {
//                 return inven.Contains(itemName);
//             }
//
//             return false;
//         }
//
//         public bool IsFull(InvenType type)
//         {
//             if (Invens.TryGetValue(type, out var inven))
//             {
//                 return inven.IsFull;
//             }
//
//             return false;
//         }
//
//         public virtual bool Equip(int index)
//         {
//             if (!CheckInvenAndIndex(index, InvenType.Inven, out var inventory)) return false;
//             if (inventory[index] == null) return false;
//             if (!Invens.TryGetValue(InvenType.Equipment, out var inventory2)) return false;
//             int equipInd = inventory2.GetEmptySlot();
//             if (equipInd == -1) return false;
//
//             Change(index, InvenType.Inven, equipInd, InvenType.Equipment);
//             return true;
//         }
//
//         /// <summary>
//         /// preset update를 동반한 UnEquip라면, inven 자체 UnEquip가 아닌 attackItemManager UnEquip 호출 필요
//         /// </summary>
//         public virtual bool UnEquip(int index)
//         {
//             if (!CheckInvenAndIndex(index, InvenType.Equipment, out var inventory)) return false;
//             if (inventory[index] == null) return false;
//
//             if (!Invens.TryGetValue(InvenType.Inven, out var inventory2)) return false;
//             int invenInd = inventory2.GetEmptySlot();
//             if (invenInd == -1) return false;
//
//             Change(index, InvenType.Equipment, invenInd, InvenType.Inven);
//             return true;
//         }
//
//         /// <param name="unEquipWithAvailable">available count 기준으로 모두 unequip 할지 아니면 count 기준으로 할지</param>
//         public virtual bool UnEquipAll(bool unEquipWithAvailable = false)
//         {
//             int maxInd = unEquipWithAvailable ? Invens[InvenType.Equipment].AvailableCnt : Invens[InvenType.Equipment].Count;
//             for (int i = 0; i < maxInd; i++)
//             {
//                 if (!UnEquip(i)) return false;
//             }
//
//             return true;
//         }
//
//         public virtual bool Change(int index1, InvenType type1, int index2, InvenType type2)
//         {
//             // 로직 상, index1 -> index2 / index2 -> index1로 교체가 이루어지기 때문에
//             // equipment에서 먼저 빼주는 걸 선행하기 위함.
//             if (type1 == InvenType.Inven && type2 == InvenType.Equipment)
//                 return Change(index2, type2, index1, type1);
//
//             if (!CheckInvenAndIndex(index1, type1, out var inventory1)) return false;
//             if (!CheckInvenAndIndex(index2, type2, out var inventory2)) return false;
//
//             Item tempItem = inventory2[index2];
//
//             /*
//              * 원래 로직 상 this[index]로 바꿔주는 것이 아닌 Add로 하는것이 바람직 하지만 (그러면 UnEquiped, Equiped 처리를 add에서만 실행)
//              * 같은 type에서 change 하는 경우에는 unequiped, equiped 이벤트 자체를 발생 안시키는게 맞을 것 같아서
//              * add 함수로 처리하지 않고, check equip으로 type이 변경된 경우에만 unequiped, equiped 호출하도록 짬.
//              */
//
//             // index1 -> index2
//             inventory2[index2] = inventory1[index1];
//             CheckEquip(index1, inventory2[index2], type1, type2);
//
//             // index2 -> index1
//             inventory1[index1] = tempItem;
//             CheckEquip(index1, tempItem, type2, type1);
//
//             return true;
//         }
//
//         public Item Remove(string itemName, InvenType type)
//         {
//             if (Invens.TryGetValue(type, out var inventory))
//             {
//                 for (int i = 0; i < inventory.AvailableCnt; i++)
//                 {
//                     if (inventory[i].ItemName == itemName)
//                     {
//                         return Remove(i, type);
//                     }
//                 }
//             }
//
//             return null;
//         }
//
//         public virtual Item Remove(int index, InvenType type)
//         {
//             Item item = Invens[type][index];
//
//             Invens[type][index] = null;
//             if (type == InvenType.Equipment && item != null)
//             {
//                 ItemUnEquipped?.Invoke(index, item);
//             }
//             item.IsStore = false;
//             return item;
//         }
//
//         public virtual Item Abandon(int index, InvenType type)
//         {
//             return GameManager.Item.Storage.Get(Remove(index, type));
//         }
//
//         private void CheckEquip(int equipIndex, Item item, InvenType fromType, InvenType toType)
//         {
//             if (item == null) return;
//
//             if (fromType == InvenType.Equipment && toType == InvenType.Inven)
//             {
//                 ItemUnEquipped?.Invoke(equipIndex, item);
//             }
//             else if (fromType == InvenType.Inven && toType == InvenType.Equipment)
//             {
//                 ItemEquipped?.Invoke(equipIndex, item);
//             }
//         }
//
//         protected bool CheckInvenAndIndex(int index, InvenType type, out InventoryGroup inventory)
//         {
//             if (0 <= index && Invens.TryGetValue(type, out var inventoryTemp) && index < inventoryTemp.AvailableCnt)
//             {
//                 inventory = inventoryTemp;
//                 return true;
//             }
//
//             inventory = null;
//             return false;
//         }
//
//
//         // protected abstract void Equipped(Item item);
//         //
//         // protected abstract void UnEquipped(Item item);
//     }
// }