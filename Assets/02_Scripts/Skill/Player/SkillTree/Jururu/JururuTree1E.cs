
namespace Apis.SkillTree
{
    public class JururuTree1E : SkillTree
    {
        private JururuPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as JururuPassiveSkill;

            if (skill == null) return;

            skill.useType = JururuPassiveSkill.StackUseType.All;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            skill.useType = JururuPassiveSkill.StackUseType.Once;
        }
    }
}