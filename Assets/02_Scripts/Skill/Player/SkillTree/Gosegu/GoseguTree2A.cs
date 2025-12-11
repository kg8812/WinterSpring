using chamwhy;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class GoseguTree2A : SkillTree
    {
        private GoseguActiveSkill skill;
        [InfoBox("대포관련 설정은 해당 스킬 인스펙터창에 있습니다.")]
        [LabelText("대포 스킬")] public ActiveSkill _cannonSkill;

        private ActiveSkillItem skillItem;
        private SeguMechaCannonSkill cannonSkill;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as GoseguActiveSkill;
            if (skill == null) return;
            if (cannonSkill == null)
            {
                // ItemId - 4505 : 메카캐논
                skillItem = InvenManager.instance.PresetManager.AddNewOverrideItem(4505) as ActiveSkillItem;
                cannonSkill = skillItem?.ActiveSkill as SeguMechaCannonSkill;
            }

            skill.cannonSkill = cannonSkill;
            // ItemId - 4505 : 메카캐논
            InvenManager.instance.PresetManager.ModifyPresetItem(8,0,1,4505,skillItem);
            skill.Mecha?.UpdateInfos();
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            if (skill?.Mecha != null)
            {
                InvenManager.instance.PresetManager.ModifyPresetItem(8,0,1,0);
                skill.Mecha.UpdateInfos();
            }
        }
    }
}