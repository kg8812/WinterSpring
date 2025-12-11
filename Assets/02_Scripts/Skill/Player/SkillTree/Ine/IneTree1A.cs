namespace Apis.SkillTree
{
    public class IneTree1A : SkillTree
    {
        private InePassiveSkill skill;
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as InePassiveSkill;

            if (skill == null) return;
            
            skill.eventUser?.EventManager.AddEvent(EventType.OnKill,CreateFeathers);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.eventUser?.EventManager.RemoveEvent(EventType.OnKill,CreateFeathers);
        }

        void CreateFeathers(EventParameters _)
        {
            switch (level)
            {
                case 1:
                    skill.CreateFeather();
                    break;
                case 2:
                    skill.CreateUntilMax();
                    break;
            }
        }
    }
}