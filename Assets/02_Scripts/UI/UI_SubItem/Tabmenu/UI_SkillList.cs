using System.Collections;
using System.Collections.Generic;
using Apis;
using chamwhy;
using chamwhy.UI.Focus;
using NewNewInvenSpace;
using UnityEngine;

public class UI_SkillList : FocusParent
{
    public SkillSlot skillSlot;
    public List<SkillSlot> subSlots;
    
    public UI_EquipSkillInfo equipSkillInfo;

    void SetInfo(Skill skill)
    {
        equipSkillInfo.Set(skill);
    }
    public void Set(Skill skill)
    {
        int subCount = 0;

        Reset();
        skillSlot.CurSkill = skill;

        RegisterElement(skillSlot);
        skillSlot.OnValueChanged.RemoveAllListeners();
        skillSlot.OnValueChanged.AddListener(x => { SetInfo(x ? skillSlot.CurSkill : null); });
        
        if (skill is IPresetOwner presetOwner)
        {
            int presetId = presetOwner.PresetId;

            List<Item> items = InvenManager.instance.PresetManager.GetOverrideItems(presetId, 0);
            int count = items.Count;
            for (int i = 0; i < subSlots.Count; i++)
            {
                subSlots[i].OnValueChanged.RemoveAllListeners();
                if (i >= count)
                {
                    subSlots[i].CurSkill = null;
                }
                if (items[i] is ActiveSkillItem activeSkillItem)
                {
                    subSlots[i].CurSkill = activeSkillItem.ActiveSkill;
                    if (subSlots[i].CurSkill != null)
                    {
                        subCount++;
                        subSlots[i].OnValueChanged.AddListener(x =>
                        {
                            SetInfo(x ? activeSkillItem.ActiveSkill : null);
                        });
                    }
                }
                subSlots[i].UpdateSkill();
            }
        }
        else
        {
            foreach (var t in subSlots)
            {
                
                t.CurSkill = null;
                t.UpdateSkill();
            }
        }
        
        for (int i = 0; i < subCount; i++)
        {
            subSlots[i].enabled = true;
            RegisterElement(subSlots[i]);
        }

        for (int i = subCount; i < subSlots.Count; i++)
        {
            subSlots[i].enabled = false;
        }

        tableData.y = focusList.Count;
    }
}
