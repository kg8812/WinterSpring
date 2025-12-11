// using chamwhy.UI;
// using Save.Schema;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using Sirenix.OdinInspector;
//
// namespace Apis
// {
//     public abstract class ItemSlot : MonoBehaviour, IPointerClickHandler
//     {
//         [Header("이미지 UI 오브젝트")]
//         public Image itemImage; // 아이템 이미지
//
//         [Header("선택 이미지 UI 오브젝트")]
//         public Image selectedImage; // 선택됐을 때 표시할 배경 이미지
//
//         [HideInInspector] public Item Item; // 슬롯 내 아이템
//
//
//         protected int idx1, idx2;
//         [ReadOnly][ShowInInspector] public InvenType Type { get; protected set; }
//         public int Idx1 => idx1;
//         public int Idx2 => idx2;
//         public abstract void OnPointerClick(PointerEventData eventData);
//
//         public int Index;
//         protected abstract AbsInvenManager Manager { get; }
//
//         public virtual bool Add(Item item)
//         {
//             if (Item != null || item == null) return false;
//
//             Item = item;
//             //item.slot = this;
//             
//             itemImage.sprite = item.Image;
//             itemImage.gameObject.SetActive(true);
//
//             SetParent();
//             if (Manager.selected == this)
//             {
//                 Select();
//             }
//             return true;
//         }
//
//         private void SetParent()
//         {
//             switch(Type)
//             {
//                 case InvenType.Inven:
//                     Item.transform.SetParent(transform);
//                     break;
//                 case InvenType.Equipment:
//                     Item.transform.SetParent(GameManager.instance.PlayerTrans);
//                     break;
//             }
//         }
//         public virtual Item Abandon() // 버리기
//         {
//             itemImage.gameObject.SetActive(false);
//
//             if (Item == null) return null;
//
//             var item = Item;
//             item.slot = null;
//             Item = null;
//
//             if (Manager.selected == this)
//             {
//                 Select();
//             }
//             return item;
//         }
//
//         public virtual Item Remove()
//         {
//             itemImage.gameObject.SetActive(false);
//             if (Item == null) return null;
//             Item item = Item;
//             item.slot = null;
//             Item = null;
//             if (Manager.selected == this)
//             {
//                 Select();
//             }
//             return item;
//         }
//         public virtual void Equip()
//         {
//         }
//
//         public virtual void UnEquip()
//         {
//             
//         }
//
//         public virtual void Init(int idx1, int idx2, InvenType type)
//         {
//             this.idx1 = idx1;
//             this.idx2 = idx2;
//             Type = type;
//         }
//         
//         public virtual void Select()
//         {
//             selectedImage.gameObject.SetActive(true);
//             if (Manager != null)
//             {
//                 Manager.selected = this;
//                 Manager.OnSelect.Invoke();
//             }
//         }
//
//         public virtual void UnSelect()
//         {
//             selectedImage.gameObject.SetActive(false);
//             if (Manager != null)
//             {
//                 Manager.selected = null;
//             }
//         }
//     }
// }