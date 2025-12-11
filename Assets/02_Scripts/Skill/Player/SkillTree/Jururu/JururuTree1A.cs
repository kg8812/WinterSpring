namespace Apis.SkillTree
{
    public class JururuTree1A : SkillTree
    {
        private JururuPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as JururuPassiveSkill;
            if (skill != null)
            {
                skill.AddSoldierType(JururuPassiveSkill.SoldierTypes.Gun);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveSoliderType(JururuPassiveSkill.SoldierTypes.Gun);
        }
    }
}