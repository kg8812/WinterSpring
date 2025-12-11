using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DaggerSkill : MagicSkill
    {
        [TitleGroup("스탯값")] 
        [LabelText("투사체 설정")]public ProjectileInfo projectileInfo;
        
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(ThrowKnife);
        }

        void ThrowKnife()
        {
            Projectile knife = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                "KnifeProjectile", user.Position);
            knife.Init(attacker,new AtkBase(attacker,Atk),3);
            knife.Init(projectileInfo);
            knife.Init(_weapon.CalculateGroggy(BaseGroggyPower));
            knife.AddAtkEventOnce(Teleport);
            knife.Fire();
        }

        void Teleport(EventParameters parameters)
        {
            if (parameters?.target is Actor a)
            {
                float distance = a is IMovable mover ? mover.ActorMovement.Width / 2 : 0.5f;

                var ray = Physics2D.Raycast(a.Position, Vector2.right * -(int)a.Direction, distance,
                    LayerMasks.Wall | LayerMasks.Map);
                if (ray.collider == null)
                {
                    user.transform.position =
                        a.transform.position + Vector3.right * (-(int)a.Direction * distance);
                }
                else
                {
                    user.Position = user.transform.position;
                }

                direction?.SetDirection(user.transform.position.x > a.transform.position.x
                    ? EActorDirection.Left
                    : EActorDirection.Right);
                if (user is Player player)
                {
                    player.CancelAttack();
                }
            }
        }
    }
}