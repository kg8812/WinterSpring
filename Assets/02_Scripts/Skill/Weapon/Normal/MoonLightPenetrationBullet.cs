using chamwhy;
using Sirenix.OdinInspector;

namespace Apis
{
    public class MoonLightPenetrationBullet : MagicSkill
    {
        [TitleGroup("스탯값")]
        [LabelText("투사체 설정")] public ProjectileInfo projectileInfo;
        
        public override void Init()
        {
            base.Init();
            actionList.Add(Action1);
        }

        public override void Active()
        {
            mover?.Stop();
            base.Active();
        }

        protected virtual void Action1()
        {
            Projectile proj =
                GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "bullet", user.Position);
            proj.Init(attacker,new AtkBase(attacker,Atk));
            proj.Init(projectileInfo);
            proj.Init((int)BaseGroggyPower);
            
            proj.Fire();
        }

        protected override ActiveEnums _activeType => ActiveEnums.Charge;
    }
}