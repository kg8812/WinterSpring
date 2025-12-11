using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SwordSkill : MagicSkill
    {
        [TitleGroup("스탯값")][LabelText("검기 설정")]
        public ProjectileInfo projectileInfo;
        [TitleGroup("스탯값")][LabelText("검기 크기")] public float size;

        public override void AfterDuration()
        {
            base.AfterDuration();

            eventUser.EventManager.RemoveEvent(EventType.OnAttack,Invoke);
        }

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override void Active()
        {
            base.Active();
            
            eventUser.EventManager.AddEvent(EventType.OnAttack,Invoke);
        }

        void Invoke(EventParameters parameters)
        {
            Projectile blade = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "SwordAura", user.Position);
            blade.transform.localScale = Vector3.one;
            blade.transform.localScale *= size;
            blade.Init(attacker,new AtkBase(attacker,Atk),3);
            blade.Init(projectileInfo);
            blade.Init(_weapon.CalculateGroggy(BaseGroggyPower));

            blade.Fire();
            
        }
    }
}