using System.Collections;
using System.Collections.Generic;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_ItemInfo : MonoBehaviour
{

    [ReadOnly] public Item item;

    public TextMeshProUGUI nameText;
    public Image icon;
    [FormerlySerializedAs("description")] public TextMeshProUGUI flavourText;
    public TextMeshProUGUI effect;

    public virtual void SetInfo(Item item)
    {
        this.item = item;
        nameText.text = item.Name;
        icon.sprite = item.Image;
        flavourText.text = item.FlavourText;
        effect.text = item.Description;
        
    }
}
