using chamwhy;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Apis
{
    public class LargeWindBall : Projectile
    {
        [HideInInspector] public float explosionDmg;
        [HideInInspector] public float explosionScale;
        [HideInInspector] public float explosionDuration;
        [HideInInspector] public float explosionGroggy;
        
        private Transform targetPos;

        public override void Init(AttackObjectInfo atkObjectInfo)
        {
            base.Init(atkObjectInfo);
            targetPos = transform;
        }

        protected override void AttackInvoke(EventParameters parameters)
        {
            DmgRatio = 0;
            targetPos = parameters.target.transform;
            base.AttackInvoke(parameters);
        }

        public override void Destroy()
        {
            base.Destroy();
            AttackObject atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "WindBallExplosion", targetPos.position);
            atk.Init(_attacker,new AtkBase(_attacker,explosionDmg),explosionDuration);
            //atk.Init(GameManager.instance.Player.AttackItemManager.Weapon.CalculateGroggy(explosionGroggy));
            atk.transform.localScale = new Vector3(explosionScale, explosionScale, 1);
        }
    }
}