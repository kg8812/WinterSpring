using chamwhy;

namespace Apis.SkillTree
{
    public class IneTree2E : SkillTree
    {
        private IneActiveSkill skill;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as IneActiveSkill;

            if (skill != null)
            {
                skill.maxCircle = 4;
                skill.OnMaxManaChange.Invoke();
                // ItemId - 4106 : 서클4
                InvenManager.instance.PresetManager.ModifyPresetItem(10,0,3,4106);
                ActiveSkillItem item = InvenManager.instance.PresetManager.GetOverrideItem(4106) as ActiveSkillItem;
                if (item?.ActiveSkill is IneCircleMagic magic)
                {
                    magic.Init(skill);
                }
                // ItemId - 4107 : 빔공격
                item = InvenManager.instance.PresetManager.GetOverrideItem(4107) as ActiveSkillItem;
                if (item?.ActiveSkill is IneCircle4Beam beam)
                {
                    beam.Init(skill);
                }
                skill.WhenChanged();
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            if (skill != null)
            {
                skill.maxCircle = 3;
                skill.OnMaxManaChange.Invoke();
                InvenManager.instance.PresetManager.ModifyPresetItem(10,0,3,0);
                skill.WhenChanged();
            }
        }
    }
}