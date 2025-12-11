using Apis;
using Default;
using Save.Schema;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UI_LilpaBullet : UI_Base
{
    private readonly CustomStack<Image> bullets = new();

    public Sprite[] bulletSprites;
    

    public override void Init()
    {
        base.Init();
        
        // UI_MainHud.Instance.setEvent.AddListener(x =>
        // {
        //     gameObject.SetActive(x != null && x.playerType is PlayerType.Lilpa);
        // });
        // UI_MainHud.Instance.afterSet.AddListener(x =>
        // {
        //     if (x?.ActiveSkill is not LilpaActiveSkill active) return;
        //
        //     lilpaSkill = active.lilpaShotgun.Skill;
        //     lilpaSkill.OnStackChange.AddListener(UpdateStack);
        //     UpdateStack(lilpaSkill.MaxStack);
        // });
        gameObject.SetActive(false);
    }

    void UpdateStack(int stack)
    {
        int count = bullets.Count;
        
        // for (int i = count; i < lilpaSkill.MaxStack; i++)
        // {
        //     bullets.Push(GameManager.UI.MakeSubItem("LilpaBulletUI",transform).GetComponent<Image>());
        // }
        //
        // for (int i = lilpaSkill.MaxStack; i < count; i++)
        // {
        //     RemoveSubItem(bullets.Pop().GetComponent<UI_Base>());
        // }
        
        for (int i = 0; i < stack; i++)
        {
            bullets[i].sprite = bulletSprites[1];
        }

        for (int i = stack; i < bullets.Count; i++)
        {
            bullets[i].sprite = bulletSprites[0];
        }
    }
}