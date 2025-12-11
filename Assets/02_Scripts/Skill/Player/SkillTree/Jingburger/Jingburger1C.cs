using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger1C : SkillTree
    {
        [LabelText("새 설정")] public JingburgerPassiveSkill.BirdInfo birdInfo;
        private JingburgerPassiveSkill skill;

        private JingburgerPassiveSkill.IFireEvent spawnBird;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as JingburgerPassiveSkill;
            if (skill == null) return;
            
            spawnBird ??= new JingburgerPassiveSkill.SpawnBird(skill, birdInfo);
            skill.RemoveFireStrategy(spawnBird);
            skill.AddFireStrategy(spawnBird);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveFireStrategy(spawnBird);
        }
    }
}