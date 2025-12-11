// using chamwhy.UI;
// using Default;
// using Sirenix.OdinInspector;
// using TMPro;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// namespace Apis
// {
//     public class ShopSlot : UI_Base
//     {
//         // 상점 아이템 슬롯
//
//         [ReadOnly] public Item item; // 할당된 아이템
//         public TextMeshProUGUI nameText;
//         public TextMeshProUGUI priceText;
//         private EventTrigger eventTrigger;
//         private Button button;
//
//         private UIAsset_Toggle _toggle;
//         
//         [HideInInspector] public Shop shop;
//         bool Buyable
//         {
//             get
//             {
//                 if (item == null || item.Price > GameManager.instance.Soul)
//                 {
//                     Debug.Log($"{(item == null ? "null": "not null")}");
//                     return false;
//                 }
//
//                 switch (item.Type)
//                 {
//                     case ItemType.HpPotion:
//                         if (GameManager.instance.Player.CurrentPotionCapacity >=
//                             GameManager.instance.Player.MaxPotionCapacity)
//                         {
//                             Debug.Log($"hp");
//                             return false;
//                         }
//
//                         break;
//                     case ItemType.Accessory:
//                         if (InvenManager.instance.Acc.IsFull(InvenType.Inven))
//                         {
//                             Debug.Log($"acc");
//                             return false;
//                         }
//                         break;
//                     case ItemType.Weapon:
//                         if (InvenManager.instance.Wp.IsFull(InvenType.Inven))
//                         {
//                             Debug.Log($"wp");
//                             return false;
//                         }
//                         break;
//                 }
//                 Debug.Log($"can");
//                 return true;
//             }
//         }
//
//
//         public int ButType
//         {
//             get
//             {
//                 if (item == null) return 227;
//                 
//                 if (item.Price > GameManager.instance.Soul)
//                 {
//                     return 226;
//                 }
//
//                 switch (item.Type)
//                 {
//                     case ItemType.HpPotion:
//                         if (GameManager.instance.Player.CurrentPotionCapacity >=
//                             GameManager.instance.Player.MaxPotionCapacity)
//                         {
//                             Debug.Log($"hp");
//                             return 223;
//                         }
//
//                         break;
//                     case ItemType.Accessory:
//                         if (InvenManager.instance.Acc.IsFull(InvenType.Inven))
//                         {
//                             Debug.Log($"acc");
//                             return 224;
//                         }
//                         break;
//                     case ItemType.Weapon:
//                         if (InvenManager.instance.Wp.IsFull(InvenType.Inven))
//                         {
//                             Debug.Log($"wp");
//                             return 225;
//                         }
//                         break;
//                 }
//                 return 0;
//             }
//         }
//
//         public override void TryActivated(bool force = false)
//         {
//             GameManager.instance.OnSoulChange.RemoveListener(Invoke);
//             GameManager.instance.OnSoulChange.AddListener(Invoke);
//             base.TryActivated(force);
//         }
//
//         public override void Init()
//         {
//             base.Init();
//             // eventTrigger = GetComponent<EventTrigger>();
//             // button = GetComponent<Button>();
//             _toggle = GetComponent<UIAsset_Toggle>();
//             _toggle.InitCheck();
//             _toggle.OnValueChanged.AddListener((isOn =>
//             {
//                 if (isOn)
//                 {
//                     ShowInfo();
//                 }
//             }));
//         }
//
//         void Invoke(int _)
//         {
//             CheckBuyable();
//         }
//         void CheckBuyable()
//         {
//             if (!Buyable)
//             {
//                 nameText.color = Color.gray;
//                 priceText.color = item != null && item.Price > GameManager.instance.Soul ? Color.red : Color.gray;
//                 // eventTrigger.enabled = false;
//                 // _toggle.DisableOn();
//                 // button.enabled = false;
//             }
//             else
//             {
//                 nameText.color = Color.white;
//                 priceText.color = Color.white;
//                 // eventTrigger.enabled = true;
//                 // _toggle.DisableOff();
//                 // button.enabled = true;
//             }
//         }
//         public void ShowInfo()
//         {
//             if (item == null) return;
//             shop.selected = this;
//             switch (item.Type)
//             {
//                 case ItemType.Accessory:
//                     shop.accDesc.gameObject.SetActive(true);
//                     shop.wpDesc.gameObject.SetActive(false);
//                     shop.accDesc.ChangeInfo(item);
//                     break;
//                 case ItemType.Weapon:
//                     shop.accDesc.gameObject.SetActive(false);
//                     shop.wpDesc.gameObject.SetActive(true);
//                     shop.wpDesc.ChangeInfo(item);
//                     break;
//                 case ItemType.HpPotion:
//                     shop.accDesc.gameObject.SetActive(false);
//                     shop.wpDesc.gameObject.SetActive(false);
//                     break;
//             }
//             /*
//             if (!Buyable)
//             {
//                 switch (item.Type)
//                 {
//                     case ItemType.Accessory:
//                         shop.SetReasonText(156);
//                         break;
//                     case ItemType.Weapon:
//                         shop.SetReasonText(156);
//                         break;
//                     case ItemType.HpPotion:
//                         shop.SetReasonText(156);
//                         break;
//                 }
//             }
//             */
//             shop.buyButton.gameObject.SetActive(true);
//         }
//         // 슬롯에 아이템 추가
//         public void Add(Item item)
//         {
//             if (this.item != null)
//             {
//                 if (this.item is Accessory accessory)
//                 {
//                     GameManager.Item.Acc.Return(accessory);
//                 }
//                 else if (this.item is Weapon weapon)
//                 {
//                     GameManager.Item.Weapon.Return(weapon);
//                 }
//                 else
//                 {
//                     GameManager.Factory.Return(item.gameObject);
//                 }
//             }
//
//             this.item = item;
//             nameText.text = item.Name;
//             priceText.text = Mathf.RoundToInt(item.Price * Mathf.Clamp01(1 - FormulaConfig.shopDiscount)).ToString();
//             if (item.TryGetComponent(out SpriteRenderer render))
//             {
//                 render.enabled = false;
//             }
//
//             item.transform.parent = transform;
//             CheckBuyable();
//         }
//
//         // 해당 아이템 구매
//         public void Buy()
//         {
//             switch (item.Type)
//             {
//                 case ItemType.Accessory:
//                     InvenManager.instance.Acc.Add(item as Accessory, InvenType.Inven);
//                     break;
//                 case ItemType.Weapon:
//                     InvenManager.instance.Wp.Add(item, InvenType.Inven);
//                     break;
//                 case ItemType.HpPotion:
//                     GameManager.instance.Player.increasePotionCapacity(1);
//                     item.Return();
//                     break;
//             }
//
//             GameManager.instance.Soul -= Mathf.RoundToInt(item.Price * Mathf.Clamp01(1 - FormulaConfig.shopDiscount));
//             GetComponent<Image>().fillAmount = 0;
//             shop.OnBuy.Invoke(item);
//             item = null;
//             CheckBuyable();
//         }
//     }
// }