using chamwhy;
using UnityEngine;

namespace Apis
{
    public class IneFeather : Projectile
    {
        private float speed = 50;
        private float radius = 1;

        [HideInInspector] public CircleAround move;
        
        public override void Init(AttackObjectInfo attackObjectInfo)
        {
            base.Init(attackObjectInfo);
            move = new(_attacker, transform, radius, speed);
        }

        protected override void Update()
        {
            base.Update();
            if (!fired)
            {
                move.Update();
            }
        }
        
        public override void Destroy()
        {
            ParticleSystem hit = GameManager.Factory.Get<ParticleSystem>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.IneFeatherHit, transform.position);
            GameManager.Factory.Return(hit.gameObject, hit.main.duration);
            base.Destroy();
        }
    }
}