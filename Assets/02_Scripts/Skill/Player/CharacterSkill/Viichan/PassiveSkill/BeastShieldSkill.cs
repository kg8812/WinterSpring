using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "BeastShieldSkill", menuName = "Scriptable/Skill/BeastShield")]
    public class BeastShieldSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        private bool isShield;
        public bool IsShield => isShield;

        protected override bool UseAtkRatio => false;

        protected override bool UseGroggyRatio => false;

        public override UI_AtkItemIcon Icon => UI_MainHud.Instance.mainSkillIcon;

        [TitleGroup("스탯값")][LabelText("방패 수용량 % (야수 공격력 비례)")][SerializeField] float maxGauge;
        [TitleGroup("스탯값")][LabelText("이속 감소량")] public float moveDebuff;
        [TitleGroup("스탯값")][LabelText("게이지 회복 간격")] public float gaugeInterval;
        [TitleGroup("스탯값")][LabelText("쉴드 초당 회복량 (%)")] public float recover;
        [TitleGroup("스탯값")] [LabelText("피격시 멈추는 시간")]
        public float durationIncrement;
        [TitleGroup("스탯값")] [LabelText("피격시 쉴드 획득량(받은 피해 %)")]
        public float shield;


        float _curGauge;

        public float CurGauge
        {
            get => _curGauge;
            set => _curGauge = Mathf.Clamp(value,0,MaxGauge);
        }
        private ISkillActive _active;

        private ViichanPassiveSkill passive => passiveUser?.PassiveSkill as ViichanPassiveSkill;
        public float MaxGauge => passive.Atk * maxGauge / 100;

        private Sequence seq;

        [HideInInspector] public Action OnShield;
        [HideInInspector] public Action OnShieldEnd;
        [HideInInspector] public Action OnShieldBreak;

        [HideInInspector] public HashSet<EventType> cancelTypes = new();

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
            return base.TryUse() && (isShield || !isShield &&
                state != EPlayerState.Dash && state != EPlayerState.Skill && GameManager.instance.Player.AbleDash);
        }

        
        void Shield(EventParameters parameters)
        {
            if (!isShield) return;
            
            GameManager.Sound.Play("viichanSkill_guard");
            if ((object)passive != null)
            {
                passive.PauseBeastDuration(durationIncrement);
                GameManager.instance.Player.AddBarrier(parameters.hitData.dmg * shield / 100f);
            }
            
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
            isShield = false;
            CurGauge = MaxGauge;
            
            StartRecovering();
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            ShieldOff();
            StopRecovering();
        }

        private Coroutine shieldOnCoroutine;
        private Coroutine recoverCoroutine;
        public override void Active()
        {
            base.Active();
            if (!isShield)
            { 
                shieldOnCoroutine = GameManager.instance.StartCoroutineWrapper(ShieldOn());
            }
            else
            {
                ShieldOff();
            }
        }

        void KnockBackOff(EventParameters parameters)
        {
            parameters.knockBackData.knockBackForce = 0;
            parameters.atkData.isHitReaction = false;
        }
        public void InvokeUse(EventParameters _)
        {
            CurCd = 0;
            Use();
        }
        IEnumerator ShieldOn()
        {
            if (isShield) yield break;
            
            animator?.animator.SetBool("IsShield",true);
            GameManager.Sound.Play("viichanSkill_shieldon");
            isShield = true;
            CurCd = 0.1f;

            eventUser?.EventManager.AddEvent(EventType.OnHit,Shield);
            isRecovering = false;
            
            eventUser?.EventManager.AddEvent(EventType.OnBeforeHit,KnockBackOff);
            cancelTypes.ForEach(x =>
            {
                eventUser?.EventManager.AddEvent(x, InvokeUse);
            });
            statUser.StatManager.AddStat(ActorStatType.MoveSpeed,-moveDebuff,ValueType.Ratio);

            OnShield?.Invoke();
        }
        
        void ShieldOff()
        {
            if (!isShield) return;

            animator?.animator.SetBool("IsShield",false);

            isShield = false;

            if (shieldOnCoroutine != null)
            {
                GameManager.instance.StopCoroutineWrapper(shieldOnCoroutine);
            }
            
            GameManager.Sound.Play("viichanSkill_shieldoff");

            eventUser?.EventManager.RemoveEvent(EventType.OnBeforeHit,KnockBackOff);
            eventUser?.EventManager.RemoveEvent(EventType.OnHit,Shield);
            cancelTypes.ForEach(x =>
            {
                eventUser?.EventManager.RemoveEvent(x, InvokeUse);
            });
            statUser.StatManager.AddStat(ActorStatType.MoveSpeed,moveDebuff,ValueType.Ratio);
            StartRecovering();

            if (Mathf.Approximately(CurGauge, 0))
            {
                GameManager.Sound.Play("viichanSkill_breakshield");
                OnShieldBreak?.Invoke();
            }
            OnShieldEnd?.Invoke();
        }

        public void StartRecovering()
        {
            if (isRecovering || Mathf.Approximately(CurGauge, MaxGauge) || CurGauge >= MaxGauge || IsShield) return;

            StopRecovering();
            recoverCoroutine = GameManager.instance.StartCoroutineWrapper(ShieldGaugeRecover());
        }
        
        public void StopRecovering()
        {
            if (recoverCoroutine != null)
            {
                GameManager.instance.StopCoroutineWrapper(recoverCoroutine);
                isRecovering = false;
                recoverCoroutine = null;
            }
        }
        private bool isRecovering;
        
        IEnumerator ShieldGaugeRecover()
        {
            if (isRecovering || Mathf.Approximately(CurGauge,MaxGauge)|| CurGauge >= MaxGauge) yield break;


            isRecovering = true;
            yield return new WaitForSeconds(gaugeInterval);

            while (CurGauge < MaxGauge)
            {
                if (isShield)
                {
                    isRecovering = false;
                    yield break;
                }
                CurGauge += recover * MaxGauge / 100f * Time.deltaTime;
                yield return null;
            }
            
            isRecovering = false;
            GameManager.Sound.Play("viichanSkill_shieldfull");
        }

        public override void Cancel()
        {
            base.Cancel();
            ShieldOff();
            StopRecovering();
        }
    }
}