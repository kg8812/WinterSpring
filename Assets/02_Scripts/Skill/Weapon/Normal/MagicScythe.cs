using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class MagicScythe : MagicSkill
    {
        [TitleGroup("스탯값")][LabelText("이동거리")] public float distance;
        [TitleGroup("스탯값")][LabelText("이동 Ease")] public Ease ease;
        [TitleGroup("스탯값")][LabelText("공중 이동각도")] public float angle;
        [TitleGroup("스탯값")][LabelText("이동속도")]public float moveTime;
        [TitleGroup("스탯값")] [LabelText("낫 공격설정")] public ProjectileInfo projInfo;
        [TitleGroup("스탯값")][LabelText("낫 반경")]public float projRadius;
        [TitleGroup("스탯값")][LabelText("낫 회전주기")] public float frequency;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);
            actionList.Clear();
            actionList.Add(InitCollider);
        }

        void SpawnProjectile(Vector2 endValue)
        {
            var blade = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                Define.DummyEffects.MagicScytheProjectile, user.Position);
            blade.transform.localScale = Vector3.one * (projRadius * 2);

            var rotate = blade.gameObject.GetOrAddComponent<RotatingEffect>();
            rotate.speed = 360 / frequency;
            rotate.decreaseSpeed = 180 / frequency;
            blade.Init(attacker,new AtkItemCalculation(attacker as Actor, this, projInfo.dmg));
            blade.Init(projInfo);
            blade.transform.DOMove(endValue, moveTime).onKill += () =>
            {
                blade.Destroy();
                Destroy(rotate);
            };
        }
        
        void GroundAttack()
        {
            mover.ActorMovement.SetGravityToZero();
            Vector2 endValue = user.Position +
                               Vector3.right * ((direction != null ? (int)direction.Direction : 1) * distance);
            tween = mover.Rb.DOMoveX(user.transform.position.x + (direction != null ?(int)direction.Direction : 1) * distance, moveTime)
                .SetEase(ease).SetUpdate(UpdateType.Fixed);
            tween.onKill += () =>
            {
                EndMotion();
                mover.ActorMovement.ResetGravity();
            };
            SpawnProjectile(endValue);
        }

        void AirAttack()
        {
            
            mover.ActorMovement.SetGravityToZero();
            Vector2 endValue;
            
            float rad = Mathf.Deg2Rad * angle;
            endValue = new Vector2(user.Position.x + direction.DirectionScale * Mathf.Cos(rad) * distance,
                user.Position.y + Mathf.Sin(rad) * distance);

            tween = mover.Rb.DOMove(endValue, moveTime)
                .SetEase(ease).SetUpdate(UpdateType.Fixed);
            tween.onKill += () =>
            {
                EndMotion();
                mover.ActorMovement.ResetGravity();
            };
            SpawnProjectile(endValue);
        }
        protected Tween tween;
        public override void Active()
        {
            base.Active();
            if (mover == null) return;

            if (mover.ActorMovement.IsStick)
            {
                GroundAttack();
            }
            else
            {
                AirAttack();
            }
        }

        void InitCollider()
        {
            GameManager.instance.Player.attackColliders.ForEach(x =>
            {
                x.Init(attacker, new AtkBase(attacker, Atk));
                x.Init((int)BaseGroggyPower);
            });
            
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }

        protected override ActiveEnums _activeType => ActiveEnums.Instant;
    }
}