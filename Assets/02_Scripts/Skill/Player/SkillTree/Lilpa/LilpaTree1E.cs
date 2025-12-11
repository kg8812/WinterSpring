namespace Apis.SkillTree
{
    public class LilpaTree1E : SkillTree
    {
        private LilpaPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as LilpaPassiveSkill;
            if (skill == null) return;
            skill.Player.AddEvent(EventType.OnSubBuffApply, AddSubBuff);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.Player.RemoveEvent(EventType.OnSubBuffApply, AddSubBuff);
        }

        void AddSubBuff(EventParameters parameters)
        {
            if (parameters is { target: Actor target } &&
                parameters.buffData.activatedSubBuff.Type == SubBuffType.HunterStack &&
                target.SubBuffCount(SubBuffType.HunterStack) <= 1)
            {
                target.AddSubBuff(skill.Player, SubBuffType.HunterStack);
            }
        }
    }
}