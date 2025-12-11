namespace Apis
{
    public interface ISkillUse
    {
        public void UseSkill();
    }

    public class InePassiveFireOnce : ISkillUse
    {
        private InePassiveSkill skill;

        public InePassiveFireOnce(InePassiveSkill skill)
        {
            this.skill = skill;
        }

        public void UseSkill()
        {
            skill.Fire(0,0,1);
        }
    }
    
    public class InePassiveFireAll : ISkillUse
    {
        private InePassiveSkill skill;
        
        public InePassiveFireAll(InePassiveSkill skill)
        {
            this.skill = skill;
        }
        public void UseSkill()
        {
            GameManager.instance.StartCoroutine(skill.FireAll());
        }
    }
}