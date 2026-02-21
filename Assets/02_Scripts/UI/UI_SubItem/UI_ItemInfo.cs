using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInfo : MonoBehaviour
{

    [ReadOnly] public Item item;

    public TextMeshProUGUI nameText;
    public Image icon;
    public TextMeshProUGUI description;

    public virtual void SetInfo(Item item)
    {
        this.item = item;
        nameText.text = item.Name;
        icon.sprite = item.Image;
        description.text = item.Description;
    }
}
