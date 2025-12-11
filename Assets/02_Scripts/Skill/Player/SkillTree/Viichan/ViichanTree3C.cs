namespace Apis.SkillTree
{
    public class ViichanTree3C : ViichanTree
    {
        private ViichanPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as ViichanPassiveSkill;

            if (skill == null) return;
            skill.isDemandEnhanced = true;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.isDemandEnhanced = false;
            }
        }
    }
}