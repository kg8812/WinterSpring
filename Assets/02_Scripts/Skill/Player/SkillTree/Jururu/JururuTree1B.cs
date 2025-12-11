namespace Apis.SkillTree
{
    public class JururuTree1B : SkillTree
    {
        private JururuPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as JururuPassiveSkill;
            if (skill != null)
            {
                skill.AddSoldierType(JururuPassiveSkill.SoldierTypes.Shield);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveSoliderType(JururuPassiveSkill.SoldierTypes.Shield);
        }
    }
}