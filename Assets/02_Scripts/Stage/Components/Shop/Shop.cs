// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
// using chamwhy.DataType;
// using chamwhy.UI;
// using chamwhy.UI.Focus;
// using Default;
// using Managers;
// using Sirenix.OdinInspector;
// using TMPro;
// using UnityEngine.Events;
// using UnityEngine.Serialization;
//
// namespace Apis
// {
//     public class Shop : UI_Base, IController
//     {
//         // [field: SerializeField] public Shop_BuyUI BuyUI { get; private set; } // 구매창
//         [FormerlySerializedAs("accInfo")] public AccDescription accDesc;
//         [FormerlySerializedAs("wpInfo")] public WeaponDescription wpDesc;
//
//         public FocusParent slotFocusParent;
//         [SerializeField] private TextMeshProUGUI reasonText;
//         
//         ShopSlot[] slots; // 슬롯 배열
//         public GameObject slotList;
//
//         [ReadOnly]public ShopSlot selected;
//         public GameObject buyButton;
//
//
//         public UnityEvent<Item> OnBuy = new();
//
//
//         public override void TryActivated(bool force = false)
//         {
//             accDesc.gameObject.SetActive(false);
//             wpDesc.gameObject.SetActive(false);
//             base.TryActivated(force);
//         }
//         
//         protected override void Deactivated()
//         {
//             base.Deactivated();
//             accDesc.ChangeInfo(null);
//             wpDesc.ChangeInfo(null);
//         }
//
//         public void Init(ShopDataType shopData)
//         {
//             var list = ShopData.GetShopItemList(shopData.sellingGroup);
//             AddRandomItems(list);
//             OnBuy.RemoveAllListeners();
//             OnBuy.AddListener(_ =>
//             {
//                 accDesc.gameObject.SetActive(false);
//                 wpDesc.gameObject.SetActive(false);
//             });
//             
//             // 첫번째 요소 선택
//             slotFocusParent.InitCheck();
//             slotFocusParent.FocusReset();
//             // (subItems[0] as ShopSlot)?.ShowInfo();
//         }
//         void AddRandomItems(Dictionary<Item, int> dict)
//         {
//             List<Item> list = new();
//
//             foreach (var key in dict.Keys)
//             {
//                 for (int i = 0; i < dict[key]; i++)
//                 {
//                     list.Add(key);
//                 }
//             }
//
//             list = AddSlot(list, ItemType.HpPotion, 1);
//             list = AddSlot(list, ItemType.Weapon, 2);
//             AddSlot(list, ItemType.Accessory, 3);
//         }
//
//         List<Item> AddSlot(List<Item> list, ItemType type, int count)
//         {
//             var items = list.Where(x => x.Type == type).ToList();
//         
//             for (int i = 0; i < count; i++)
//             {
//                 if (items.Count <= 0) continue;
//                 int rand = Random.Range(0, items.Count);
//                 var item = items[rand];
//                 
//                 var slot  = GameManager.UI.MakeSubItem("ShopSlot", slotList.transform).GetComponent<ShopSlot>();
//                 subItems.Add(slot);
//                 slot.Add(item);
//                 slot.shop = this;
//                 items = items.Where(x => x != item).ToList();
//                 list = list.Where(x => x != item).ToList();
//
//                 UIAsset_Toggle el = slot.GetComponent<UIAsset_Toggle>();
//                 el.InitCheck();
//                 slotFocusParent.RegisterElement(el);
//                 el.OnValueChanged.AddListener(isOn =>
//                 {
//                     SetReasonText(isOn ? slot.ButType : 0);
//                 });
//             }
//         
//             return list;
//         }
//
//         public void SetReasonText(int errorId)
//         {
//             if (errorId == 0)
//             {
//                 reasonText.enabled = false;
//             }
//             else
//             {
//                 reasonText.text = GameManager.Data.Str(errorId);
//                 reasonText.enabled = true;
//             }
//         }
//         
//         // 구매창 열기
//         public void Open()
//         {
//             if (selected.item != null)
//             {
//                 SystemManager.SystemCheck($"'{selected.item.Name}'를 구매하시겠습니까?", (isYes) =>
//                 {
//                     if (isYes)
//                         selected.Buy();
//                 });
//                 // BuyUI.gameObject.SetActive(true);
//                 // BuyUI.Set(selected.item, selected.Buy);
//             }
//         }
//
//         public void Control()
//         {
//             slotFocusParent.Control();
//         }
//     }
// }
