using System.Collections;
using System.Collections.Generic;
using Apis;
using Default;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipInfo : UI_Base
{
    enum Texts
    {
        Name,
    }

    enum Images
    {
        Icon
    }
    enum GameObjects
    {
        Ine,Jing,Lilpa,Jururu,Segu,Vii,
    }

    private List<GameObject> playerIcons;
    private TextMeshProUGUI nameText;
    private Image itemIcon;

    public UI_ItemInfo wpInfo;
    public UI_ItemInfo accInfo;
    public override void Init()
    {
        base.Init();
        
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        playerIcons = new()
        {
            Get<GameObject>((int)GameObjects.Ine),
            Get<GameObject>((int)GameObjects.Jing),
            Get<GameObject>((int)GameObjects.Lilpa),
            Get<GameObject>((int)GameObjects.Jururu),
            Get<GameObject>((int)GameObjects.Segu),
            Get<GameObject>((int)GameObjects.Vii)
        };
    }

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
    protected override void Activated()
    {
        base.Activated();
        
    }
}
