using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger2A : SkillTree
    {
        private JingburgerActiveSkill active;

        [LabelText("나선수리검 투사체정보")] public ProjectileInfo projInfo;
        
        public override void Activate(PlayerActiveSkill _active, int level)
        {
            base.Activate(active,level);
            active = _active as JingburgerActiveSkill;
            if (active == null) return;
            active.ChangeSpawnStrategy(new JingburgerActiveSkill.SpawnRasenShuriken(active,this));
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            active?.ChangeSpawnStrategy(new JingburgerActiveSkill.SpawnRasengan(active));
        }
    }
}