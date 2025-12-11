using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class JururuTree2E : SkillTree
    {
        [InfoBox("그랩 설정은 프리팹으로 가서 하세요.")]
        public PokdoMouseRSkill _grabSkill;

        private JururuActiveSkill skill;

        private ActiveSkillItem skillItem;
        private PokdoMouseRSkill grabSkill;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JururuActiveSkill;

            if (skill == null) return;
            if (grabSkill == null)
            {
                // ItemId - 4405 : 손아구
                skillItem = InvenManager.instance.PresetManager.AddNewOverrideItem(4405) as ActiveSkillItem;
                grabSkill = skillItem?.ActiveSkill as PokdoMouseRSkill;
                grabSkill?.Init(this);
            }

            skill.grabSkill = grabSkill;
            InvenManager.instance.PresetManager.ModifyPresetItem(7,0,2,4405,skillItem);
            skill.UpdatePokdo();
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill == null) return;
            
            InvenManager.instance.PresetManager.ModifyPresetItem(7,0,1,0);
            skill.UpdatePokdo();
        }
    }
}