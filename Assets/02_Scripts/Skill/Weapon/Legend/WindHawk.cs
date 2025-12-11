using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class WindHawk : MagicSkill
    {
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("투사체 설정")] public ProjectileInfo info;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("투사체 반경")] public float size;
       
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override void Active()
        {
            base.Active();

            Projectile dulgi = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "dulgiProjectile",
                user.Position);

            dulgi.Init(attacker,new AtkBase(attacker,info.dmg));
            dulgi.Init(info);
            dulgi.transform.localScale = Vector3.one * (size * 2);
            dulgi.Fire();
        }
    }
}