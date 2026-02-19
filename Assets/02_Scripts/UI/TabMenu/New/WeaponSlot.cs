using System.Collections;
using System.Collections.Generic;
using chamwhy;
using NewNewInvenSpace;
using Save.Schema;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : ItemSlot
{
    [SerializeField] private WeaponSlotData _weaponSlotData;
    [SerializeField] private Image lockedImg;

    public override void OnSlotChanged(int ind, Item item)
    {
        base.OnSlotChanged(ind, item);
        if (ind != index) return;
        if (GameManager.Item.Weapon.WpDict.TryGetValue(_weaponSlotData.WeaponId, out var weapon))
        {
            itemImg.sprite = weapon.Image;
        }
        
        lockedImg.enabled = false;
        itemImg.color = Color.white;
        if (item == null && invenType == InvenType.Storage)
        {
            if (IsWeaponUnlocked())
            {
                itemImg.enabled = true;
                itemImg.color = Color.grey;
            }
            else
            {
                lockedImg.enabled = true;
                itemImg.enabled = false;
            }
        }
    }

    public bool IsWeaponUnlocked()
    {
        return _weaponSlotData != null && DataAccess.Codex.IsOpen(CodexData.CodexType.Item, _weaponSlotData.WeaponId);
    }
}
