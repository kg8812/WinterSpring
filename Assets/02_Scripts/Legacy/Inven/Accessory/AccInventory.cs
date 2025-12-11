// using System.Collections.Generic;
// using Save.Schema;
// using UnityEngine;
//
// namespace Apis
// {
//     public class AccInventory : Inventory
//     {
//         ItemSlot[,] slots;
//         public override ItemSlot[,] Slots => slots;
//
//         protected override ItemSlot[,] CreateSlots()
//         {
//             slots = new ItemSlot[vertical, horizontal];
//
//             // 슬롯 최대치만큼 슬롯 생성       
//             for (int i = 0; i < vertical; i++)
//             {
//                 for (int j = 0; j < horizontal; j++)
//                 {
//                     if (count >= max) break;
//                     AddSlot();
//                 }
//                 if (count >= max) break;
//             }
//
//             return slots;
//         }
//         public override void AddSlot() // 슬롯 추가
//         {
//             if (count >= slots.Length) return;
//
//             GameObject obj = Instantiate(itemSlot, transform);
//
//             if (!obj.TryGetComponent(out AccSlot slot))
//             {
//                 obj.AddComponent<AccSlot>();
//             }
//
//             int length = slots.GetLength(1);
//
//             slots[count / length, count % length] = slot;
//             slot.Init(count / length, count % length,type);
//             slot.Index = count / length + count % length;
//             count++;
//         }
//
//         protected override void Start()
//         {
//             InvenManager.instance.Acc.OnSelect.AddListener(() =>
//             {
//                 x = InvenManager.instance.Acc.selected.Idx2;
//                 y = InvenManager.instance.Acc.selected.Idx1;
//             });
//         }
//         private void OnEnable()
//         {
//             InvenManager.instance.Acc.MenuWindow.gameObject.SetActive(false);
//         }
//     }
// }