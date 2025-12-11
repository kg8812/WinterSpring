using System;
using chamwhy;
using chamwhy.DataType;
using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class RingOfSpear : MagicSkill
    {
        protected override bool UseAtkRatio => false;
        private Buff buff;
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [InfoBox("플레이어보다 크기를 크게 설정해주세요. (플레이어 크기 0.75,1.5)")]
        [TitleGroup("스탯값")][LabelText("반격 범위 (플레이어 기준 사각형)")] public Vector2 size; 
        [TitleGroup("스탯값")] [LabelText("무적 지속시간")] public float invincibilityDuration;
        [TitleGroup("스탯값")][LabelText("검기 설정")] public ProjectileInfo atkInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("검기 크기")] public float radius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("스택당 데미지 증가량")] public float amount;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("최대 스택")] public int _maxStack;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("스턴 지속시간")] public int stunDuration;
        
        
        private TriggerEvent counterCollider;
        Guid invincibleGuid1;
        Guid invincibleGuid2;
        private Guid hitGuid;
        
        public override void Init()
        {
            base.Init();
            
            actionList.Clear();
            actionList.Add(SpawnSlash);
        }
        public override void Active()
        {
            animator.animator.ResetTrigger("CounterAtk");

            base.Active();

            invincibleGuid1 = hit.AddInvincibility();
            counterCollider = GameManager.Factory.Get<TriggerEvent>(FactoryManager.FactoryType.Normal,
                Define.CommonObjects.TriggerEventCollider, user.Position);
            counterCollider.triggerEnterEvent.RemoveListener(CounterAtk);
            counterCollider.triggerEnterEvent.AddListener(CounterAtk);
            counterCollider.transform.localScale = size;
            if (hit != null)
            {
                hitGuid = hit.AddHitImmunity();
            }

            if (buff == null)
            {
                BuffDataType data = new(SubBuffType.Debuff_DmgReduce)
                {
                    buffPower = new []{amount}, buffCategory = 1, buffDuration = 0,
                    buffDispellType = 2, buffMaxStack = _maxStack, stackDecrease = 0, valueType = ValueType.Value,
                    showIcon = false
                };

                buff = new(data, eventUser);
            }
        }

        public override void AfterDuration()
        {
            base.AfterDuration();

            counterCollider.triggerEnterEvent.RemoveListener(CounterAtk);
            GameManager.Factory.Return(counterCollider.gameObject);
            hit.RemoveInvincibility(invincibleGuid1);
            
            if (hit != null)
            {
                Utils.ActionAfterFrame(() => { hit.RemoveHitImmunity(hitGuid); });
            }

            EndMotion();
        }
        
        void CounterAtk(Collider2D other)
        {
            if (other.gameObject.CompareTag("EnemyEffect"))
            {
                invincibleGuid2 = hit.AddInvincibility();
                animator?.animator.SetTrigger("CounterAtk");
                Cancel();
                Sequence seq = DOTween.Sequence();
                seq.SetDelay(invincibilityDuration);
                seq.AppendCallback(() =>
                {
                    hit.RemoveInvincibility(invincibleGuid2);
                });
            }
        }

        void SpawnSlash()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.WeaponEffects.MagicSlash, user.Position);
            obj.transform.localScale = Vector3.one * (radius * 2 * (direction != null ? (int)direction.Direction : 1));
            obj.Init(attacker,new AtkBase(attacker,atkInfo.dmg),1);
            obj.Init(atkInfo);
            obj.Init((int)(BaseGroggyPower));
            obj.AddEventUntilInitOrDestroy(AddBuff);
        }

        void AddBuff(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                for (int i = 0; i < _maxStack; i++)
                {
                    buff.AddSubBuff(target, null);
                }
                target.SubBuffManager.AddCC(eventUser,SubBuffType.Debuff_Stun,stunDuration);
            }
        }
    }
}