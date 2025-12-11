// using System.Collections.Generic;
// using TMPro;
// using UnityEngine.UI;
//
// namespace Apis
// {
//     public abstract class AbsInvenMenu
//     {
//         public abstract void ApplyButtons(List<Button> buttons);
//         protected readonly ItemSlot slot;
//
//         public AbsInvenMenu(ItemSlot slot)
//         {
//             this.slot = slot;
//         }
//     }
//
//     public class AccMenu : AbsInvenMenu
//     {
//         public AccMenu(ItemSlot slot) : base(slot)
//         {
//         }
//
//         public override void ApplyButtons(List<Button> buttons)
//         {
//             buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "장착";
//             buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "버리기";
//             buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "취소";
//             buttons[0].onClick.AddListener(() =>
//             {
//                 if(slot is AccSlot sl)
//                 {
//                     sl.Change();
//                 }
//             });
//             buttons[1].onClick.AddListener(() => slot.Abandon());
//
//             foreach (var button in buttons)
//             {
//                 button.onClick.AddListener(InvenManager.instance.Acc.OnMenuClose.Invoke);
//             }
//         }
//
//     }
//
//     public class EquipMenu : AbsInvenMenu
//     {
//         public EquipMenu(ItemSlot slot) : base(slot)
//         {
//         }
//
//         public override void ApplyButtons(List<Button> buttons)
//         {
//             buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "해제";
//             buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "교체";
//             buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "취소";
//             buttons[0].onClick.AddListener(slot.UnEquip);
//             buttons[1].onClick.AddListener(() =>
//             {
//                 if(slot is AccSlot sl)
//                 {
//                     sl.Change();
//                 }
//             });
//
//
//             foreach (var button in buttons)
//             {
//                 button.onClick.AddListener(InvenManager.instance.Acc.OnMenuClose.Invoke);
//             }
//         }
//     }
//
//     public class WeaponInvenMenu : AbsInvenMenu
//     {
//         public WeaponInvenMenu(ItemSlot slot) : base(slot)
//         {
//         }
//
//         public override void ApplyButtons(List<Button> buttons)
//         {
//             // Weapon weapon = GameManager.instance.Player.AttackItemManager.Weapon;
//             //
//             // if (weapon != null && slot.Item != null && slot.Item.Equals(weapon))
//             // {
//             //     buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "교체";
//             //     buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "해제";
//             //     buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "취소";
//             //     buttons[0].onClick.AddListener(InvenManager.instance.Wp.controller.ToChangeCtrl);
//             // }
//             // else
//             // {
//             //     buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "장착";
//             //     buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "버리기";
//             //     buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "취소";
//             //     buttons[0].onClick.AddListener(slot.Equip);
//             //     buttons[0].onClick.AddListener(InvenManager.instance.Wp.Invens[InvenType.Inven].Refresh);
//             //     buttons[1].onClick.AddListener(() => slot.Abandon());
//             // }
//             //
//             // foreach (var button in buttons)
//             // {
//             //     button.onClick.AddListener(InvenManager.instance.Wp.OnMenuClose.Invoke);
//             // }
//         }
//     }
// }
