using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger1E : SkillTree
    {
        [LabelText("고래 설정")] public JingburgerPassiveSkill.WhaleInfo whaleInfo;
        
        private JingburgerPassiveSkill skill;

        private JingburgerPassiveSkill.IFireEvent spawnWhale;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as JingburgerPassiveSkill;
            if (skill == null) return;
            
            spawnWhale ??= new JingburgerPassiveSkill.SpawnWhale(skill, whaleInfo);
            skill?.RemoveFireStrategy(spawnWhale);
            skill?.AddFireStrategy(spawnWhale);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveFireStrategy(spawnWhale);
        }
    }
}