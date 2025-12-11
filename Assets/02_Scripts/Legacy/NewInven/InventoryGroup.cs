// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Apis;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public enum InvenType // 인벤토리 종류
//      {
//          Inven,
//          Equipment,
//      }
//     
//     public class InventoryGroup
//     {
//         public delegate void ItemChanged(int ind, Item item);
//
//         public Item[] Slots;
//
//         public Item this[int index]
//         {
//             get => Slots[index];
//             set
//             {
//                 BeforeSlotChanged?.Invoke(index, value);
//                 Slots[index] = value;
//                 OnSlotChanged?.Invoke(index, value);
//             }
//         }
//
//         public ItemChanged BeforeSlotChanged;
//         public ItemChanged OnSlotChanged;
//         public ItemChanged OnItemExcepted;
//
//         protected int count;
//         public int Count
//         {
//             get => count;
//             set
//             {
//                 if (value == count) return;
//                 if (value < count)
//                 {
//                     for (int i = value; i < count; i++)
//                     {
//                         if (Slots[i] != null)
//                         {
//                             OnItemExcepted?.Invoke(i, Slots[i]);
//                         }
//                     }
//                 }
//                 count = value;
//                 if (count < availableCnt)
//                 {
//                     availableCnt = count;
//                 }
//                 Array.Resize(ref Slots, count);
//             }
//         }
//
//         protected int availableCnt;
//
//         public int AvailableCnt
//         {
//             get => availableCnt;
//             set
//             {
//                 if (value < count) return;
//                 if (value > availableCnt)
//                 {
//                     // 잠겨있다가 새롭게 count 늘어나면 그때 equipped 호출.
//                     for (int i = availableCnt; i < value; i++)
//                     {
//                         
//                     }
//                 }
//                 availableCnt = value;
//             }
//         }
//         
//
//         protected InvenType Type;
//
//         public InventoryGroup(InvenType type, int slotCnt, int availableCnt)
//         {
//             Type = type;
//             count = slotCnt;
//             this.availableCnt = availableCnt;
//             Slots = new Item[slotCnt];
//         }
//         public bool IsFull
//         {
//             get
//             {
//                 int a = 0;
//                 foreach (Item item in Slots)
//                 {
//                     if (a >= availableCnt) break;
//                     if (item == null)
//                     {
//                         return false;
//                     }
//                     a++;
//                 }
//                 return true;
//             }
//         }
//
//         // empty가 없으면 -1 반환
//         public int GetEmptySlot(bool useAvailableCnt = false)
//         {
//             int ind = -1;
//             int cnt = useAvailableCnt ? availableCnt : count;
//             for (int i = 0; i < availableCnt; i++)
//             {
//                 if (Slots[i] == null)
//                 {
//                     ind = i;
//                     break;
//                 }
//             }
//             return ind;
//         }
//
//         public bool AddItem(int index, Item item, bool checkAvail = true)
//         {
//             if (item == null) return false;
//             if (this[index] != null) return false;
//             if (checkAvail && index >= availableCnt) return false;
//             // item.IsStore = true;
//             this[index] = item;
//             return true;
//         }
//
//         public bool AddItem(Item item)
//         {
//             if (item == null) return false;
//             for (int i = 0; i < availableCnt; i++)
//             {
//                 if (this[i] == null)
//                 {
//                     // item.IsStore = true;
//                     this[i] = item;
//                     return true;
//                 }
//             }
//             return false;
//         }
//
//         public bool Contains(string itemName)
//         {
//             foreach (var item in Slots)
//             {
//                 if (item != null && item.ItemName == itemName)
//                     return true;
//             }
//             return false;
//         }
//
//         public Item GetByName(string itemName)
//         {
//             foreach (var item in Slots)
//             {
//                 if (item != null && item.ItemName == itemName)
//                     return item;
//             }
//             return null;
//         }
//         public void Clear()
//         {
//             for (int i = 0; i < count; i++)
//             {
//                 this[i]?.Return();
//                 this[i] = null;
//             }
//         }
//     }
// }