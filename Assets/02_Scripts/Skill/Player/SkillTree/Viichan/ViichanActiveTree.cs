namespace Apis.SkillTree
{
    public class ViichanTree : SkillTree
    {
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);

            if (active is ViichanActiveSkill skill)
            {
                skill.StartRecovering(true);
            }
        }
    }
}