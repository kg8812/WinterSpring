namespace Apis.SkillTree
{
    public class IneTree1D : SkillTree
    {
        private InePassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as InePassiveSkill;
            if (skill == null) return;
            
            skill.ChangeUseStrategy(new InePassiveFireAll(skill));
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.ChangeUseStrategy(new InePassiveFireOnce(skill));
        }
    }
}