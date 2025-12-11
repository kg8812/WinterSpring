using Apis;
using Default;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UI_JururuStack : UI_Base
{
    private CustomQueue<Image> stacks = new();

    private JururuPassiveSkill _passive;

    public Sprite[] stackSprites;
    
    public override void Init()
    {
        base.Init();
        
        UI_MainHud.Instance.setEvent.AddListener(x =>
        {
            CheckSkill(x.PassiveSkill);
            x.OnPassiveSkillChange += CheckSkill;
        });
        
        UI_MainHud.Instance.afterSet.AddListener(x =>
        {
            SetSkill(x.PassiveSkill);
            x.OnPassiveSkillChange += SetSkill;
        });
    }

    void CheckSkill(PassiveSkill passive)
    {
        if (passive is JururuPassiveSkill v)
        {
            _passive = v;
            gameObject.SetActive(true);
        }
        else
        {
            _passive = null;
            gameObject.SetActive(false);
        }
    }

    void SetSkill(PassiveSkill passive)
    {
        if (passive is JururuPassiveSkill v)
        {
            v.OnStackChange.AddListener(UpdateStack);
            UpdateStack(_passive.CurStack);
        }
    }
    void UpdateStack(int stack)
    {
        int count = stacks.Count;
        
        for (int i = count; i < _passive.MaxStack; i++)
        {
            stacks.Enqueue(GameManager.UI.MakeSubItem("JururuStack",transform).GetComponent<Image>());
        }

        for (int i = _passive.MaxStack; i < count; i++)
        {
            RemoveSubItem(stacks.Dequeue().GetComponent<UI_Base>());
        }
        
        for (int i = 0; i < stack; i++)
        {
            stacks[i].sprite = stackSprites[1];
        }

        for (int i = stack; i < stacks.Count; i++)
        {
            stacks[i].sprite = stackSprites[0];
        }
    }
}
