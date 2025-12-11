// using Sirenix.OdinInspector;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace Apis
// {
//     public abstract class AbsInvenManager
//     {
//         [field: Header("선택 메뉴창")]
//         public InvenMenuWindow MenuWindow;
//         [ReadOnly] public ItemSlot selected;
//
//        UnityEvent _onSelect;
//        UnityEvent _beforeSelect;
//        UnityEvent _onMouseSelect;
//        UnityEvent _onMenuOpen;
//        UnityEvent _onMenuClose;
//        public UnityEvent OnSelect => _onSelect ??= new();
//        public UnityEvent BeforeSelect => _beforeSelect ??= new();
//        public UnityEvent OnMouseSelect => _onMouseSelect ??= new();
//        public UnityEvent OnMenuOpen => _onMenuOpen ??= new();
//        public UnityEvent OnMenuClose => _onMenuClose ??= new();
//
//         [ES3Serializable]
//         public Dictionary<InvenType, Inventory> Invens = new();
//
//         public virtual void Init()
//         {
//             OnSelect.AddListener(CloseMenuWindow);
//         }
//
//         public bool IsFull(InvenType type)
//         {
//             return Invens[type].IsFull;
//         }
//
//         public abstract bool Add(Item item, InvenType type);
//         public virtual Item Remove(string itemName, InvenType type)
//         {
//             if (Invens.ContainsKey(type))
//             {
//                 return Invens[type].Remove(itemName);
//             }
//             return null;
//         }
//         
//         public virtual bool Contains(string itemName, InvenType type) // 아이템 소지 체크 (아이템 이름, 인벤토리 타입)
//         {
//             if (Invens.ContainsKey(type))
//             {
//                 return Invens[type].Contains(itemName);
//             }
//
//             return false;
//         }       
//
//         public virtual void OpenMenuWindow(InvenType type)
//         {
//             InitMenu(type);
//             MenuWindow.gameObject.SetActive(true);
//             MenuWindow.transform.position = selected.transform.position;
//             MenuWindow.OnEnter();
//             OnMenuOpen.Invoke();
//         }
//         protected abstract void InitMenu(InvenType type);
//         public virtual void CloseMenuWindow()
//         {
//             MenuWindow.gameObject.SetActive(false);
//             OnMenuClose.Invoke();
//         }
//
//         public void MouseSelect(ItemSlot slot)
//         {
//             BeforeSelect.Invoke();
//             selected = slot;
//
//             OnSelect.Invoke();
//             OnMouseSelect.Invoke();
//         }
//     }
//     
//     
// }