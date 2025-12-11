// using Sirenix.OdinInspector;
// using System;
// using UnityEngine;
//
// namespace Apis
// {
//     public enum InvenType // 인벤토리 종류
//     {
//         Inven,
//         Equipment,
//     }
//
//     [Serializable]
//     public class AccInvenManager : AbsInvenManager
//     {
//         
//         BonusStat _bonusStat = new();
//
//         public BonusStat BonusStat
//         {
//             get
//             {
//                 _bonusStat ??= new();
//                 _bonusStat.Reset();
//                 foreach (var x in Invens[InvenType.Equipment].Slots)
//                 {
//                     if (x.Item == null) continue;
//                     var acc = x.Item as Accessory;
//                     if (acc != null) _bonusStat += acc.BonusStat;
//                 }
//                 return _bonusStat;
//             }
//         }
//
//         [Header("악세사리창")]
//         [SerializeField] AccInventory accInven;
//
//         [Header("장비창")]
//         [SerializeField] AccInventory equipment;
//
//         public AccInvenController controller;
//
//         [ReadOnly] public AccSlot changeSlot;
//         public override void Init()
//         {
//             //시작할 때 슬롯 생성 및 생성된 슬롯 리스트 딕셔너리에 추가
//
//             base.Init();
//             accInven.Init(InvenType.Inven);
//             equipment.Init(InvenType.Equipment);
//             Invens.Add(InvenType.Inven, accInven);
//             Invens.Add(InvenType.Equipment, equipment);           
//         }
//         public override bool Add(Item item, InvenType type) // 아이템 추가 함수 (아이템 오브젝트, 인벤토리 타입)
//         {
//             if (!Invens.TryGetValue(type, out var inven)) return false;
//             return item is Accessory && inven.AddItem(item);
//         }                   
//
//         protected override void InitMenu(InvenType type)
//         {           
//             switch (type)
//             {
//                 case InvenType.Inven:
//                     MenuWindow.Init(new AccMenu(selected));
//                     break;
//                 case InvenType.Equipment:
//                     MenuWindow.Init(new EquipMenu(selected));
//                     break;
//             }
//         }
//     }
// }