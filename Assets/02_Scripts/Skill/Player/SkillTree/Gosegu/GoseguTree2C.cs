using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class GoseguTree2C : SkillTree
    {
        private GoseguActiveSkill skill;

        [LabelText("레이저 설정")] public SeguMecha.LaserInfo laserInfo;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as GoseguActiveSkill;

            if (skill == null) return;
            skill.OnMechaSpawn.AddListener(ChangeAtkStrategy);
            ChangeAtkStrategy(skill.Mecha);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnMechaSpawn.RemoveListener(ChangeAtkStrategy);
            skill?.Mecha?.ChangeAtkStrategy(new SeguMecha.FireBullets(skill?.Mecha));
        }

        void ChangeAtkStrategy(SeguMecha mecha)
        {
            mecha?.ChangeAtkStrategy(new SeguMecha.FireLaser(mecha,laserInfo));
        }
    }
}