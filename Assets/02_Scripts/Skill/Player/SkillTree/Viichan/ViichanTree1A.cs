using chamwhy;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class ViichanTree1A : ViichanTree
    {
        [AssetsOnly][Required] public ActiveSkill _beastGrabSkill;

        private ViichanPassiveSkill skill;

        private ActiveSkillItem skillItem;
        private BeastGrabSkill grabSkill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive, level);
            skill = passive as ViichanPassiveSkill;
            if (skill == null) return;
            if (grabSkill == null)
            {
                // ItemId - 4604 : 늘어나는손
                skillItem = InvenManager.instance.PresetManager.AddNewOverrideItem(4604) as ActiveSkillItem;
                grabSkill = skillItem?.ActiveSkill as BeastGrabSkill;
            }

            skill.grabSkill = grabSkill;
            InvenManager.instance.PresetManager.ModifyPresetItem(9,0,1,4604,skillItem);
            skill.ApplyChange();
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                InvenManager.instance.PresetManager.ModifyPresetItem(9,0,1,0);
                skill.ApplyChange();
            }
        }
    }
}