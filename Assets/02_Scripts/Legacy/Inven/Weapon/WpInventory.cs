// using UnityEngine;
//
// namespace Apis
// {
//     public class WpInventory : Inventory
//     {
//         ItemSlot[,] slots;
//         public override ItemSlot[,] Slots => slots;
//
//         public override void AddSlot()
//         {
//             if (count >= slots.Length) return;
//
//             GameObject obj = Instantiate(itemSlot, transform);
//
//             if (!obj.TryGetComponent(out WeaponSlot slot))
//             {
//                 obj.AddComponent<WeaponSlot>();
//             }
//
//             int length = Slots.GetLength(1);
//
//             Slots[count / length, count % length] = slot;
//             slot.Init(count / length, count % length, type);
//             slot.Index = count / length + count % length;
//             count++;
//         }
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
//             return Slots;
//         }
//
//         protected override void Start()
//         {
//             InvenManager.instance.Wp.OnSelect.AddListener(() =>
//             {
//                 x = InvenManager.instance.Wp.selected.Idx2;
//                 y = InvenManager.instance.Wp.selected.Idx1;
//             });
//         }
//         private void OnEnable()
//         {
//             InvenManager.instance.Wp.MenuWindow.gameObject.SetActive(false);
//         }
//     }
// }