// using UnityEngine;
//
// namespace Apis
// {
//     public class ShopInven : Inventory
//     {
//         private ItemSlot[,] slots;
//         
//         public override ItemSlot[,] Slots => slots;
//
//         public AccDescription accDesc;
//         public WeaponDescription wpDesc;
//         public GameObject buyButton;
//         public GameObject tab;
//
//         public void On()
//         {
//             tab.SetActive(true);
//             foreach (var slot in slots)
//             {
//                 slot?.gameObject.SetActive(true);
//             }
//         }
//
//         public void Off()
//         {
//             tab.SetActive(false);
//             foreach (var slot in slots)
//             {
//                 slot?.gameObject.SetActive(false);
//             }
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
//         public override void AddSlot()
//         {
//             Debug.Log("add slot");
//             if (count >= slots.Length) return;
//
//             GameObject obj = Instantiate(itemSlot, transform);
//
//             if (!obj.TryGetComponent(out ShopInvenSlot slot))
//             {
//                 Destroy(obj);
//                 return;
//             }
//
//             int length = slots.GetLength(1);
//             Debug.Log($"length : {length} {count / length} {count % length}");
//
//             slots[count / length, count % length] = slot;
//             slot.Init(count / length, count % length,type);
//
//             slot.shop = this;
//             count++;
//             // focusParent.RegisterElement(slot.GetComponent<UIElement>());
//         }
//
//         public override void Clear()
//         {
//             foreach (var a in slots)
//             {
//                 a?.Remove();
//             }
//         }
//     }
// }