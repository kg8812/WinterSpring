// using System;
// using System.Collections.Generic;
// using _02_Scripts.Item.NewInven;
// using Apis;
// using chamwhy.Inven;
// using UI;
// using PresetType = AtkItemData.PresetType;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public class ItemEquipInvenDecorator : InvenPresetDecorator
//     {
//         
//         private readonly InvenGroupManager IgManager;
//         
//         
//         // invengroup manager의 equip은 실제 equip이 아닌, equip된 애들로 꾸려진 아이템 저장소이다.
//         // 실질적인 item equip 처리는 해당 inventory group에서 시행한다.
//         private InventoryGroup EquippedInven;
//
//         public Dictionary<InvenType, InventoryGroup> Invens => IgManager.Invens;
//         
//         public AtkItemData PresetData { get; private set; }
//
//         public ItemEquipInvenDecorator(int equipSlotCnt, int equipAvailSlotCnt, int invenSlotCnt,
//             int invenAvailSlotCnt)
//         {
//             IgManager = new();
//             IgManager.Init(equipSlotCnt, equipAvailSlotCnt, invenSlotCnt, invenAvailSlotCnt);
//
//             CurPresetType = PresetType.InvenPreset;
//             
//             IgManager.ItemEquipped += EquipItemFromIgManager;
//             IgManager.ItemUnEquipped += UnEquipItemFromIgManager;
//
//             EquippedInven.BeforeSlotChanged = CheckItemEquipOrUnEquip;
//             
//             ToggleItemEquipToggle(true);
//             
//         }
//
//
//         #region PresetSection
//         
//         public PresetType CurPresetType { get; private set; }
//         
//
//         #endregion
//
//         private void ToggleItemEquipToggle(bool isOn)
//         {
//             if (isOn)
//             {
//                 IgManager.ItemEquipped += Equipped;
//                 IgManager.ItemUnEquipped += UnEquipped;
//             }
//             else
//             {
//                 IgManager.ItemEquipped -= Equipped;
//                 IgManager.ItemUnEquipped -= UnEquipped;
//             }
//         }
//
//         private void UnEquipAllInEquippedInven()
//         {
//             for (int i = 0; i < EquippedInven.Count; i++)
//             {
//                 Item item = EquippedInven[i];
//                 EquippedInven[i] = null;
//             }
//         }
//
//         public void ApplyPreset(int index)
//         {
//             ToggleItemEquipToggle(false);
//             UnEquipAllInEquippedInven();
//             if (PresetData.Presets.TryGetValue(index, out var preset))
//             {
//                 CurPresetType = preset.presetType;
//                 for (int i = 0; i < preset.atkItems.Length; i++)
//                 {
//                     if(preset.atkItems[i] == null) continue;
//                     int invenInd = InvenManager.instance.AttackItem.GetIndex(preset.atkItems[i], InvenType.Inven);
//                     if (invenInd != -1)
//                     {
//                         InvenManager.instance.AttackItem.EquipWithoutSavePreset(invenInd);
//                     }
//                     else
//                     {
//                         Debug.Log($"{preset.atkItems[i]}를 가지고 있지 않음");
//                     }
//                 }
//             }
//         }
//
//         public void SavePreset(int index)
//         {
//             
//         }
//         
//         public bool CanUseAttackItem(int index)
//         {
//             return AtkItemInven.Count > index && AtkItemInven[index] != null;
//         }
//
//
//         private bool CheckEquipCondition(IAttackItem atkItem)
//         {
//             if (atkItem == null) return false;
//             return true;
//             return !Array.Exists(IgManager.Invens[InvenType.Equipment].Slots,
//                 x => x is IAttackItem item && (item == atkItem || item.Category == atkItem.Category));
//         }
//
//         public bool Add(Item item, InvenType type)
//         {
//             if (item is IAttackItem atkItem && (type != InvenType.Equipment || CheckEquipCondition(atkItem)) &&
//                 IgManager.Add(item, type))
//             {
//                 if (type == InvenType.Equipment)
//                     AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//
//             return false;
//         }
//
//         public bool Add(int index, Item item, InvenType type)
//         {
//             if (item is IAttackItem atkItem && (type != InvenType.Equipment || CheckEquipCondition(atkItem)) &&
//                 IgManager.Add(index, item, type))
//             {
//                 if (type == InvenType.Equipment)
//                     AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//                 return true;
//             }
//
//             return false;
//         }
//
//         public bool Equip(int index)
//         {
//             Item item = IgManager.Invens[InvenType.Inven][index];
//             if (item is not IAttackItem atkItem) return false;
//             if (CheckEquipCondition(atkItem) || !IgManager.Equip(index)) return false;
//             AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public bool UnEquip(int index)
//         {
//             if (!IgManager.UnEquip(index)) return false;
//             AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public bool UnEquipAll(bool unEquipWithAvailable = false)
//         {
//             if (!IgManager.UnEquipAll(unEquipWithAvailable)) return false;
//             AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public bool Change(int index1, InvenType type1, int index2, InvenType type2)
//         {
//             if (!IgManager.Change(index1, type1, index2, type2)) return false;
//             AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//             return true;
//         }
//
//         public Item Remove(int index, InvenType type)
//         {
//             return IgManager.Remove(index, type);
//             AttackItemManager.SavePreset((int)GameManager.instance.Player.playerType);
//         }
//
//         public Item Abandon(int index, InvenType type)
//         {
//             // weapon의 경우 장착한 개체를 버릴 수 없으니 예외처리
//             // if (type == InvenType.Equipment) return null;
//
//             Item item = IgManager.Abandon(index, type);
//             if (item != null && item is Weapon wp)
//             {
//                 Weapon_PickUp pu = GameManager.Item.WeaponPickUp.CreateExisting(wp);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             if (item != null && item is ActiveSkillItem sk)
//             {
//                 ActiveSkill_PickUp pu = GameManager.Item.ActiveSkillPickUp.CreateExisting(sk);
//                 pu.transform.position = GameManager.instance.Player.transform.position;
//             }
//
//             return item;
//         }
//
//         public Item ChangeAttackItem(Item item, int index = 0)
//         {
//             Item preItem = Invens[InvenType.Equipment][index];
//             UnEquipped(preItem);
//             Invens[InvenType.Equipment][index] = item;
//             Equipped(item);
//             return preItem;
//         }
//         
//         private void EquipItemFromIgManager(int equipInd, Item item) => EquipItem(equipInd, item, PresetType.InvenPreset);
//         
//         private void UnEquipItemFromIgManager(int equipInd, Item item) => UnEquipItem(equipInd, item, PresetType.InvenPreset);
//         
//         
//
//         private void EquipItem(int equipInd, Item item, PresetType presetType)
//         {
//             if (CurPresetType != presetType) return;
//             
//         }
//         private void UnEquipItem(int equipInd, Item item, PresetType presetType)
//         {
//             
//         }
//
//         private void CheckItemEquipOrUnEquip(int index, Item item)
//         {
//             
//         }
//
//         private void Equipped(Item item)
//         {
//             if (item != null && item is IAttackItem attackItem)
//             {
//                 // GameManager.instance.Player.Equip(wp,0);
//                 int index = IgManager.GetIndex(item.ItemName, InvenType.Equipment);
//                 attackItem.AtkSlotIndex = index;
//                 attackItem.SetIcon(UI_MainHud.Instance.atkItemIcons[index]);
//                 UI_MainHud.Instance.atkItemIcons[index].SetItem(attackItem);
//
//                 //아이콘 설정을 먼저해야함. 순서 변경해서 아이콘 설정보다 장착이 위로오면 아이콘 작동안함.
//                 attackItem.Equip(GameManager.instance.Player);
//             }
//         }
//
//         private void UnEquipped(Item item)
//         {
//             if (item != null && item is IAttackItem attackItem)
//             {
//                 // GameManager.instance.Player.UnEquip(weapon);
//                 UI_AtkItemIcon icon = UI_MainHud.Instance.atkItemIcons[attackItem.AtkSlotIndex];
//                 icon.SetItem(null);
//                 attackItem.UnEquip();
//                 item.SetParent(null);
//                 attackItem.SetIcon(null);
//             }
//         }
//     }
// }