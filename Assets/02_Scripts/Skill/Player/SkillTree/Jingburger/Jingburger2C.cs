using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Apis.SkillTree
{
    public class Jingburger2C : SkillTree
    {
        private JingburgerActiveSkill skill;
        [FormerlySerializedAs("rush")] [LabelText("돌진 거리")] public float distance;
        [LabelText("돌진 속도")] public float speed;
        [LabelText("넉백 힘")] public float knockBackPower;
        [LabelText("넉백 각도")] public float knockBackAngle;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JingburgerActiveSkill;

            if (skill == null) return;
            
            skill.TurnOnMotion();
            skill.ChangeUseStrategy(new JingburgerActiveSkill.RushRasengan(skill,this));
            skill.OnExplosionSpawn.RemoveListener(InitExplosion);
            skill.OnExplosionSpawn.AddListener(InitExplosion);
        }
        public override void DeActivate()
        {
            base.DeActivate();
            skill?.TurnOffMotion();
            skill?.ChangeUseStrategy(new JingburgerActiveSkill.FireProjectile());
            skill?.OnExplosionSpawn.RemoveListener(InitExplosion);
        }

        void InitExplosion(AttackObject explosion)
        {
            explosion.isHitReaction = true;
            explosion.knockBackData.knockBackForce = knockBackPower;
            explosion.knockBackData.knockBackAngle = knockBackAngle;
            explosion.groggyKnockBackData.knockBackForce = knockBackPower;
            explosion.groggyKnockBackData.knockBackAngle = knockBackAngle;
        }
    }
}