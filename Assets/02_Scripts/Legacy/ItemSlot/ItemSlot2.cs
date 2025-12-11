// using Apis;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace chamwhy.ItemSlot
// {
//     public abstract class ItemSlot2
//     {
//         [HideInInspector] public Item Item;
//         [ReadOnly][ShowInInspector] public InvenType Type { get; protected set; }
//
//
//         public virtual bool Add(Item item)
//         {
//             if (Item != null || item == null) return false;
//
//             Item = item;
//             
//             return true;
//         }
//
//         public virtual Item Remove()
//         {
//             var item = Item;
//             item.slot = null;
//             Item = null;
//             return item;
//         }
//         
//         public virtual Item Abandon()
//         {
//             var item = Item;
//             item.slot = null;
//             Item = null;
//             // TODO: Manager selected this라면 거시기 => 코드보고 수정
//             return item;
//         }
//
//         public virtual void Equip()
//         {
//             
//         }
//
//         public virtual void UnEquip()
//         {
//             
//         }
//     }
// }