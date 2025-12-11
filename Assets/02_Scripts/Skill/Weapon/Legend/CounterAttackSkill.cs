using System;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;
using Default;
using DG.Tweening;

namespace Apis
{
    public class CounterAttackSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        protected override bool UseAtkRatio => false;

        [InfoBox("플레이어보다 크기를 크게 설정해주세요. (플레이어 크기 0.75,1.5)")]
        [TitleGroup("스탯값")][LabelText("반격 범위 (플레이어 기준 사각형)")] public Vector2 size; 
        [TitleGroup("스탯값")] [LabelText("무적 지속시간")]
        public float invincibilityDuration;

        [TitleGroup("스탯값")] [LabelText("검기 설정")]
        public ProjectileInfo atkInfo;

        [TitleGroup("스탯값")] [LabelText("검기 크기")]
        public float radius;

        [TitleGroup("스탯값")] [LabelText("반사 투사체 정보")]
        public ProjectileInfo projInfo;

        private TriggerEvent counterCollider;
        Guid invincibleGuid1;
        Guid invincibleGuid2;
        
        public override void Init()
        {
            base.Init();

            actionList.Clear();
            actionList.Add(SpawnSlash);
        }

        private Guid guid;
        public override void Active()
        {
            animator?.animator.ResetTrigger("CounterAtk");
            base.Active();
            invincibleGuid1 = hit.AddInvincibility();

            counterCollider = GameManager.Factory.Get<TriggerEvent>(FactoryManager.FactoryType.Normal,
                Define.CommonObjects.TriggerEventCollider, user.Position);
            counterCollider.triggerEnterEvent.RemoveListener(CounterAtk);
            counterCollider.triggerEnterEvent.AddListener(CounterAtk);
            counterCollider.transform.localScale = size;
            if (hit != null)
            {
                guid = hit.AddHitImmunity();
            }
        }

        public override void AfterDuration()
        {
            base.AfterDuration();
            hit.RemoveInvincibility(invincibleGuid1);
            counterCollider.triggerEnterEvent.RemoveListener(CounterAtk);
            GameManager.Factory.Return(counterCollider.gameObject);
            if (hit != null)
            {
                Utils.ActionAfterFrame(() => { hit.RemoveHitImmunity(guid); });
            }

            EndMotion();
        }

        void CounterAtk(Collider2D other)
        {
            if (other.gameObject.CompareTag("EnemyEffect"))
            {
                animator?.animator.SetTrigger("CounterAtk");
                Cancel();
                invincibleGuid2 = hit.AddInvincibility();
                
                Sequence seq = DOTween.Sequence();
                seq.SetDelay(invincibilityDuration);
                seq.AppendCallback(() =>
                {
                    hit.RemoveInvincibility(invincibleGuid2);
                });
                
                if (other.TryGetComponent(out Projectile proj))
                {
                    Reflect(proj);
                }
            }
        }

        void SpawnSlash()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.WeaponEffects.MagicSlash, user.Position);
            obj.transform.localScale = Vector3.one * (radius * 2 * (direction != null ? (int)direction.Direction : 1));
            obj.Init(attacker, new AtkBase(attacker, atkInfo.dmg), 1);
            obj.Init(atkInfo);
            obj.Init((int)BaseGroggyPower);
        }

        void Reflect(Projectile proj)
        {
            var target = proj;
            if (target == null)
            {
                Debug.LogWarning("target is null");
                return;
            }

            if (target.gameObject.CompareTag("EnemyEffect"))
            {
                proj.Init(attacker, new AtkItemCalculation(attacker as Actor ,this,projInfo.dmg));
                proj.Init(projInfo);
                proj.Fire();
            }
        }
    }
}