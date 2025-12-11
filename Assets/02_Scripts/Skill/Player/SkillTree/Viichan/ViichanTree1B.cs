using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class ViichanTree1B : ViichanTree
    {
        [AssetsOnly][Required] public ActiveSkill _beastShieldSkill;

        private ActiveSkill beastShieldSkill;
        
        private ViichanPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as ViichanPassiveSkill;

            if (skill == null) return;

            if (beastShieldSkill == null)
            {
                _beastShieldSkill.Init();
                beastShieldSkill = Instantiate(_beastShieldSkill);
                beastShieldSkill.Init();
            }
            skill.shieldSkill = beastShieldSkill;

            skill.ApplyChange();
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill == null) return;
            
            skill.shieldSkill = null;
            skill.ApplyChange();

        }
    }
}