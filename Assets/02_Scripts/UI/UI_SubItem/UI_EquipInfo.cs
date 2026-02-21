using System.Collections;
using System.Collections.Generic;
using Apis;
using Default;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipInfo : UI_Base
{
    private Image itemIcon;

    public UI_ItemInfo wpInfo;
    public UI_ItemInfo accInfo;

    public void Set(Item item)
    {
        if (item == null)
        {
            accInfo.gameObject.SetActive(false);
            wpInfo.gameObject.SetActive(false);
            return;
        }
        if (item is Accessory)
        {
            accInfo.gameObject.SetActive(true);
            accInfo.SetInfo(item);
            wpInfo.gameObject.SetActive(false);
        }
        else
        {
            accInfo.gameObject.SetActive(false);
            wpInfo.SetInfo(item);
            wpInfo.gameObject.SetActive(true);
        }
    }
}
