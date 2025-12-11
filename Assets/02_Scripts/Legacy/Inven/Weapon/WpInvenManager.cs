// using System;
// using UnityEngine;
// using System.Collections.Generic;
//
// namespace Apis
// {
//     [Serializable]
//     public class WpInvenManager : AbsInvenManager
//     {
//         [Header("인벤창")]
//         [SerializeField] WpInventory inven;
//         [Header("장비창")]
//         [SerializeField] WpInventory equip;
//
//         public WeaponController controller;
//         public override void Init()
//         {
//             //시작할 때 슬롯 생성 및 생성된 슬롯 리스트 딕셔너리에 추가
//             base.Init();
//             inven.Init(InvenType.Inven);
//             equip.Init(InvenType.Equipment);
//             Invens.Add(InvenType.Inven, inven);
//             Invens.Add(InvenType.Equipment, equip);
//         }
//
//         public override bool Add(Item item, InvenType type)
//         {
//             if (Invens.ContainsKey(type))
//             {
//                 if (item is Weapon)
//                 {
//                     return Invens[type].AddItem(item);
//                 }
//             }
//             return false;
//         }
//
//         protected override void InitMenu(InvenType type)
//         {
//             MenuWindow.Init(new WeaponInvenMenu(selected));
//         }
//         
//         public Queue<ItemSlot> GetInvenSlots()
//         {
//             Queue<ItemSlot> queue = new();
//
//             for (int i = 0; i < inven.Slots.GetLength(0); i++)
//             {
//                 for (int j = 0; j < inven.Slots.GetLength(1); j++)
//                 {
//                     queue.Enqueue(inven.Slots[i,j]);
//                 }
//             }
//
//             return queue;
//         }
//
//         public void ReturnInvenSlots(Queue<ItemSlot> slots)
//         {
//             while (slots.Count > 0)
//             {
//                 slots.Dequeue().transform.SetParent(inven.transform);
//             }
//         }
//     }
// }