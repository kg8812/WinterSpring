// using System.Collections.Generic;
// using Apis;
// using Save.Schema;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// public abstract class Inventory : MonoBehaviour
// {
//     [Header("슬롯 프리팹")]
//     [SerializeField] protected GameObject itemSlot; // 슬롯 프리팹
//
//     [FormerlySerializedAs("Horizontal")] public int horizontal;
//     [FormerlySerializedAs("Vertical")] public int vertical;
//   
//     public abstract ItemSlot[,] Slots { get; }
//
//     [Header("초기 슬롯 개수")]
//     [SerializeField] protected int max;
//
//     protected int count;
//     public int Count => count;
//
//
//     protected int x;
//     protected int y;
//     protected InvenType type;
//     public int X => x;
//     public int Y => y;
//
//     public virtual ItemSlot[,] Init(InvenType type)
//     {
//         x = 0;
//         y = 0;
//         count = 0;
//
//         this.type = type;
//         return CreateSlots();
//     }
//
//     protected abstract ItemSlot[,] CreateSlots();
//     public abstract void AddSlot(); // 슬롯 생성
//     
//
//     public bool IsFull
//     {
//         get
//         {
//             int a = 0;
//             foreach (ItemSlot slot in Slots)
//             {
//                 if (a >= count) break;
//
//                 if (slot.Item == null)
//                 {
//                     return false;
//                 }
//                 a++;
//             }
//
//             return true;
//         }
//     }
//
//     protected virtual void Start()
//     {
//         GameManager.instance.OnGameReset.AddListener(Clear);
//     }
//     public bool AddItem(Item item)
//     {
//         int a = 0;
//
//         foreach (ItemSlot slot in Slots)
//         {
//             if (a >= count) break;
//
//             if (slot.Item == null)
//             {
//                 slot.Add(item);
//
//                 return true;
//             }
//             a++;
//         }
//         return false;
//     }
//
//     public bool AddItem(Item item, int idx)
//     {
//         if (idx >= Slots.Length) return false;
//         if (item == null) return false;
//         int count2 = Slots.GetLength(1);
//         return Slots[idx / count2, idx % count2].Add(item);
//     }
//     public Item Remove(Item item) // 아이템 제거 함수 (아이템)
//     {
//         return Remove(item.Name);
//     }
//     public virtual Item Remove(string itemName) // 아이템 제거 함수 (아이템 이름)
//     {
//         int a = 0;
//         foreach (ItemSlot v in Slots)
//         {
//             if (a >= count) break;
//
//             if (v.Item.Name == itemName)
//             {
//                 if (type == InvenType.Equipment)
//                 {
//                     v.UnEquip();
//                 }
//                 return v.Remove();
//             }
//             a++;
//
//         }
//
//         return null;
//     }
//
//     public void RemoveAll()
//     {
//         foreach (ItemSlot v in Slots)
//         {
//             if (v != null && v.Item != null)
//             {
//                 if (type == InvenType.Equipment)
//                 {
//                     v.UnEquip();
//                 }
//                 v.Remove();
//             }
//         }
//     }
//     
//     public bool Contains(string itemName) // 아이템 소지 체크 (아이템 이름)
//     {
//         int a = 0;
//
//         foreach (ItemSlot v in Slots)
//         {
//             if (a >= count) break;
//
//             if (v.Item != null && v.Item.Name == itemName)
//             {
//                 return true;
//             }
//             a++;
//
//         }
//
//         return false;
//     }
//
//     public Item GetItemByName(string itemName)
//     {
//         foreach (ItemSlot v in Slots)
//         {
//             if (v.Item != null && v.Item.Name == itemName)
//             {
//                 return v.Item;
//             }
//         }
//
//         return null;
//     }
//     public bool MoveLeft(bool checkLast = false, bool isLoop = true)
//     {
//         if (x == 0)
//         {
//             if (checkLast)
//             {
//                 return false;
//             }
//
//             if (!isLoop)
//                 return true;
//
//             x = Slots.GetLength(1) - 1;
//             int num = (y + 1) * Slots.GetLength(1);
//
//             if (num > count)
//             {
//                 x = (count - 1) % Slots.GetLength(1);
//             }
//         }
//         else
//         {
//             x--;
//         }
//         UnSelectAll();
//         Slots[y, x].Select();
//         return true;
//     }
//
//     public bool MoveRight(bool checkLast = false, bool isLoop = true)
//     {
//         int num = y * Slots.GetLength(1) + x;
//         if (num == count - 1 || (x + 1) % Slots.GetLength(1) == 0)
//         {
//             if (checkLast)
//             {
//                 return false;
//             }
//
//             if (!isLoop)
//                 return true;
//             x = 0;
//         }
//         else
//         {
//             x++;
//         }
//         UnSelectAll();
//         Slots[y, x].Select();
//         return true;
//     }
//
//     public int MoveUp()
//     {
//         if (y == 0)
//         {
//             return x;
//         }
//         UnSelectAll();
//         y--;
//         Slots[y, x].Select();
//         return -1;
//     }
//
//     public void Refresh()
//     {
//         Select(x,y);
//     }
//     public int MoveDown()
//     {
//         int num = (y + 1) * Slots.GetLength(1) + x;
//         if (y >= Slots.GetLength(0) - 1 || y >= count / Slots.GetLength(1))
//         {
//             return x;
//         }
//         UnSelectAll();
//
//         y++;
//
//         if (num >= count)
//         {
//             x = (count - 1) % Slots.GetLength(1);
//         }
//         Slots[y, x].Select();
//         return -1;
//     }
//     public void Select(int x, int y)
//     {
//         UnSelectAll();
//         y = Mathf.Clamp(y, 0, Slots.GetLength(0) - 1);
//         x = Mathf.Clamp(x, 0, Slots.GetLength(1) - 1);
//         this.x = x;
//         this.y = y;
//
//         Slots[y, x].Select();
//     }
//     private void OnDisable()
//     {
//         UnSelectAll();
//     }
//
//     public void UnSelectAll()
//     {
//         int num = 0;
//         if (Slots == null) return;
//         
//         foreach (var slot in Slots)
//         {
//             num++;
//             if (num > count) return;
//
//             slot.UnSelect();
//         }
//     }
//     public void OnEnter()
//     {
//         UnSelectAll();
//         x = 0;
//         y = 0;
//         Slots[y, x].Select();
//     }
//     public void OnExit()
//     {
//         UnSelectAll();
//     }
//
//     public virtual void Clear()
//     {
//         foreach(var slot in Slots)
//         {
//             Item item = slot.Remove();
//             if (item != null)
//             {
//                 // Debug.Log($"Item Reset {item.Name}");
//                 item.Return();
//             }
//         }
//     }
// }
