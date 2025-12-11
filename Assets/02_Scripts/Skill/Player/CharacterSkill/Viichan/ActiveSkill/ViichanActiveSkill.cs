using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Apis
{
    [CreateAssetMenu(fileName = "ViichanActive", menuName = "Scriptable/Skill/ViichanActive")]
    public class ViichanActiveSkill : PlayerActiveSkill
    {
        protected override bool UseGroggyRatio => false;
        
        [TitleGroup("스탯값")][LabelText("패링 지속시간")] public float parryingTime;
        [TitleGroup("스탯값")][LabelText("패링 무적시간")] [SerializeField] float invincibleTime;

        [TitleGroup("스탯값")][LabelText("이속 감소량")] public float moveDebuff;
        [TitleGroup("스탯값")][LabelText("게이지 회복 간격")] public float gaugeInterval;
        [TitleGroup("스탯값")][LabelText("쉴드 초당 회복량 (%)")] public float recover;

        float _curGauge;

        public float CurGauge
        {
            get => _curGauge;
            set => _curGauge = Mathf.Clamp(value,0,MaxGauge);
        }
        
        // 데미지가 없는 스킬이라 편의성을 위해 dmg 변수를 방패 수용량으로 사용함
        public float MaxGauge => Atk;

        // 마찬가지로 그로기가 없는 스킬이라 편의성을 위해 그로기 변수를 회복량으로 사용함

        public float Recover
        {
            get
            {
                if (stats?.Stat != null)
                {
                    return (recover + stats.Stat.groggy) * (1 + stats.Stat.groggyRatio / 100f);
                }

                return recover;
            }
        }

        private Sequence seq;

        [HideInInspector] public Action OnParrying;
        [HideInInspector] public Action OnShield;
        [HideInInspector] public Action OnShieldEnd;
        [HideInInspector] public Action OnShieldBreak;

        [HideInInspector] public HashSet<EventType> cancelTypes = new();

        public bool IsShield => IsToggleOn;
        public override void Decorate()
        {
            base.Decorate();

            if (isEquip)
            {
                StartRecovering(true);
            }
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Shield;

        protected override float TagIncrement => GameManager.Tag.Data.ShieldIncrement;

        public override void Init()
        {
            base.Init();
            cancelTypes = new();
            cancelTypes.Add(EventType.OnDash);
            cancelTypes.Add(EventType.OnAttack);
        }

        public override bool TryUse()
        {
            EPlayerState state = GameManager.instance.Player.GetState();
            return base.TryUse() && (IsShield || !IsShield &&
                state != EPlayerState.Dash && state != EPlayerState.Skill && GameManager.instance.Player.AbleDash);
        }

        protected override ActiveEnums _activeType => ActiveEnums.Toggle
        ;

        public void Parrying(EventParameters parameters)
        {
            seq?.Kill();
            parameters.hitData.hitDisable = true;
            parameters.hitData.dmg = 0;
            Use();
            CurCd = 0;
            SpawnEffect(Define.PlayerEffect.ViichanShieldParrying,0.5f,user.Position,false);
            GameManager.instance.StartCoroutine(Invincible());
            GameManager.Sound.Play("parry_dummy");
            OnParrying?.Invoke();
        }

        IEnumerator Invincible()
        {
            if (hit == null) yield break;
            Guid guid = hit.AddInvincibility();
            yield return new WaitForSeconds(invincibleTime);
            hit?.RemoveInvincibility(guid);
        }
        public void Shield(EventParameters parameters)
        {
            if (!IsShield) return;
            
            SpawnEffect(Define.PlayerEffect.ViichanGuard,0.5f,user.Position,true);
            GameManager.Sound.Play("viichanSkill_guard");
            
            if (CurGauge >= parameters.hitData.dmg)
            {
                CurGauge -= parameters.hitData.dmg;
                parameters.hitData.dmg = 0;
            }
            else
            {
                parameters.hitData.dmg -= CurGauge;
                CurGauge = 0;
                
                Use();
            }
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            CurGauge = MaxGauge;
            
            StartRecovering(true);

        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            StopRecovering();
        }

        [HideInInspector]public Coroutine shieldOnCoroutine;
        private Coroutine recoverCoroutine;
        public override void Active()
        {
            base.Active();

            if (Skill2 is ViichanActiveOff x)
            {
                x._skill = this;
            }
            
            shieldOnCoroutine = GameManager.instance.StartCoroutineWrapper(ShieldOn());
        }

        public void KnockBackOff(EventParameters parameters)
        {
            parameters.knockBackData.knockBackForce = 0;
            //TODO: 여기도 groggy knockback force 초기화?
            parameters.atkData.isHitReaction = false;
        }
        public void InvokeUse(EventParameters _)
        {
            CurCd = 0;
            Use();
        }
        IEnumerator ShieldOn()
        {
            if (IsShield) yield break;
            GameManager.instance.Player.CancelAttack();
            GameManager.Sound.Play("viichanSkill_shieldon");
            seq?.Kill();
            seq = DOTween.Sequence();
            CurCd = 0;
            isRecovering = false;
            seq.AppendCallback(() => eventUser?.EventManager.AddEvent(EventType.OnBeforeHit, Parrying));
            seq.AppendInterval(parryingTime);
            seq.AppendCallback(() => eventUser?.EventManager.RemoveEvent(EventType.OnBeforeHit, Parrying));
            seq.onKill += () =>
            {
                eventUser?.EventManager.RemoveEvent(EventType.OnBeforeHit, Parrying);
                eventUser?.EventManager.AddEvent(EventType.OnHit,Shield);
            };
            eventUser?.EventManager.AddEvent(EventType.OnBeforeHit,KnockBackOff);
            cancelTypes.ForEach(x =>
            {
                eventUser?.EventManager.AddEvent(x, InvokeUse);
            });
            statUser?.StatManager.AddStat(ActorStatType.MoveSpeed,-moveDebuff,ValueType.Ratio);

            SpawnEffect(Define.PlayerEffect.ViichanShieldOn,Define.PlayerEffect.ViichanShieldLoop,0.5f);
            RemoveEffect(Define.PlayerEffect.ViichanShieldGaugeLoop);
            OnShield?.Invoke();
        }
        
        

        private bool isRecovering;

        public void StartRecovering(bool checkShield)
        {
            if (isRecovering || Mathf.Approximately(CurGauge, MaxGauge) || CurGauge >= MaxGauge || (checkShield && IsShield)) return;

            StopRecovering();
            recoverCoroutine = GameManager.instance.StartCoroutineWrapper(ShieldGaugeRecover());
        }

        public void StopRecovering()
        {
            RemoveEffect(Define.PlayerEffect.ViichanShieldGaugeLoop);

            if (recoverCoroutine != null)
            {
                GameManager.instance.StopCoroutineWrapper(recoverCoroutine);
                isRecovering = false;
                recoverCoroutine = null;
            }
        }
        IEnumerator ShieldGaugeRecover()
        {
            if (isRecovering || Mathf.Approximately(CurGauge,MaxGauge) || CurGauge >= MaxGauge) yield break;

            SpawnEffect(Define.PlayerEffect.ViichanShieldGaugeLoop,0.5f,true);

            isRecovering = true;
            yield return new WaitForSeconds(gaugeInterval);

            while (CurGauge < MaxGauge)
            {
                if (IsShield)
                {
                    isRecovering = false;
                    yield break;
                }
                CurGauge += Recover * MaxGauge / 100f * Time.deltaTime;
                yield return null;
            }
            
            RemoveEffect(Define.PlayerEffect.ViichanShieldGaugeLoop);
            SpawnEffect(Define.PlayerEffect.ViichanShieldGaugeFull,0.5f,user.Position,true);
            
            isRecovering = false;
            GameManager.Sound.Play("viichanSkill_shieldfull");
        }

        public override void Cancel()
        {
            base.Cancel();
        }
    }
}