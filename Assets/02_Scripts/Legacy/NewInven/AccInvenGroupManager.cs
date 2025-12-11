// using Apis;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public class AccInvenGroupManager: InvenGroupManager
//     {
//         private BonusStat _bonusStat;
//         
//         // 가져갈때마다 Reset 하는것 같지만 정작 호출은 Player에서만 해서 최적화 불필요
//         public BonusStat BonusStat
//         {
//             get
//             {
//                 _bonusStat ??= new BonusStat();
//                 _bonusStat.Reset();
//                 for (int i = 0; i < Invens[InvenType.Equipment].AvailableCnt; i++)
//                 {
//                     Item item = Invens[InvenType.Equipment][i];
//                     if (item != null && item is Accessory acc)
//                     {
//                         _bonusStat += acc.BonusStat;
//                     } 
//                 }
//                 return _bonusStat;
//             }
//         }
//
//
//         public override bool Add(Item item, InvenType type) => item is Accessory && base.Add(item, type);
//         public override bool Add(int index, Item item, InvenType type) => item is Accessory && base.Add(index, item, type);
//         
//
//         public override Item Abandon(int index, InvenType type)
//         {
//             Item item = base.Abandon(index, type);
//             
//             if (item != null  && item is Accessory accessory)
//             {
//                 Acc_PickUp pu = GameManager.Item.AccPickUp.CreateExisting(GameManager.Item.Storage.Get(accessory));
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             return item;
//         }
//
//         protected override void Equipped(Item item)
//         {
//             if (item != null && item is Accessory acc)
//             {
//                 acc.Equip(GameManager.instance.Player);
//             }
//         }
//
//         protected override void UnEquipped(Item item)
//         {
//             if (item != null && item is Accessory acc)
//             {
//                 acc.UnEquip();
//             }
//         }
//
//         
//     }
// }