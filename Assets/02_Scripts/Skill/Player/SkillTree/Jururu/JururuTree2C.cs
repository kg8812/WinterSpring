using chamwhy;

namespace Apis.SkillTree
{
    public class JururuTree2C : SkillTree
    {
        private JururuActiveSkill skill;
        
        private ActiveSkillItem skillItem;
        private PokdoESkill crySkill;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as JururuActiveSkill;

            if (skill == null) return;
            if (crySkill == null)
            {
                // ItemId - 4404 : 응애
                skillItem = InvenManager.instance.PresetManager.AddNewOverrideItem(4404) as ActiveSkillItem;
                crySkill = skillItem?.ActiveSkill as PokdoESkill;
            }

            skill.crySkill = crySkill;
            InvenManager.instance.PresetManager.ModifyPresetItem(7,0,1,4404,skillItem);
            skill.UpdatePokdo();
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                InvenManager.instance.PresetManager.ModifyPresetItem(7,0,1,0);
                skill.UpdatePokdo();
            }
        }
    }
}