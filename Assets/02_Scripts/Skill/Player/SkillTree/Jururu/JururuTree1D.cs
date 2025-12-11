namespace Apis.SkillTree
{
    public class JururuTree1D : SkillTree
    {
        private JururuPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill =  passive as JururuPassiveSkill;
            if (skill == null) return;
            
            skill.ChangePatternType(FoxSoldier.PatternType.Following);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.ChangePatternType(FoxSoldier.PatternType.Normal);
        }
    }
}