using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree2D : SkillTree
    {
        private GoseguActiveSkill skill;
        [InfoBox("펄스건 설정은 해당 스킬 인스펙터창에 있습니다.")]
        [LabelText("펄스건 스킬")] public ActiveSkill _pulseGunSkill;

        private ActiveSkillItem skillItem;
        private SeguMechaKnockBackSkill pulseGunSkill;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as GoseguActiveSkill;
            if (skill == null) return;
            if (pulseGunSkill == null)
            {
                // ItemId - 4504 : 메카펄스건
                skillItem = InvenManager.instance.PresetManager.AddNewOverrideItem(4504) as ActiveSkillItem;
                pulseGunSkill = skillItem?.ActiveSkill as SeguMechaKnockBackSkill;
                pulseGunSkill?.Init(skill);
            }

            skill.pulseGunSkill = pulseGunSkill;
            // ItemId - 4504 : 메카펄스건
            InvenManager.instance.PresetManager.ModifyPresetItem(8,0,2,4504,skillItem);
            skill.Mecha?.UpdateInfos();
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            if (skill?.Mecha != null)
            {
                InvenManager.instance.PresetManager.ModifyPresetItem(8,0,2,0);
                skill.Mecha.UpdateInfos();
            }
        }
    }
}