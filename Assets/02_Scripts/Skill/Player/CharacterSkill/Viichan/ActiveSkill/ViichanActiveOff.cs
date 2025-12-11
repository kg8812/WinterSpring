using System.Collections;
using System.Collections.Generic;
using Apis;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "ViichanActiveOff", menuName = "Scriptable/Skill/ViichanActiveOff")]

    public class ViichanActiveOff : PlayerActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        [HideInInspector] public ViichanActiveSkill _skill;
        
        public override void Active()
        {
            base.Active();
            ShieldOff();
        }

        void ShieldOff()
        {
            _skill.CDActive.StartCd();
            if (_skill.shieldOnCoroutine != null)
            {
                GameManager.instance.StopCoroutineWrapper(_skill.shieldOnCoroutine);
                _skill.shieldOnCoroutine = null;
            }
            
            GameManager.Sound.Play("viichanSkill_shieldoff");

            _skill.eventUser?.EventManager.RemoveEvent(EventType.OnBeforeHit, _skill.Parrying);
            _skill.eventUser?.EventManager.RemoveEvent(EventType.OnBeforeHit,_skill.KnockBackOff);
            _skill.eventUser?.EventManager.RemoveEvent(EventType.OnHit,_skill.Shield);
            _skill.cancelTypes.ForEach(x =>
            {
                _skill.eventUser?.EventManager.RemoveEvent(x, _skill.InvokeUse);
            });
            statUser?.StatManager.AddStat(ActorStatType.MoveSpeed,_skill.moveDebuff,ValueType.Ratio);
            _skill.StartRecovering(false);
            _skill.RemoveEffect(Define.PlayerEffect.ViichanShieldOn);
            _skill.RemoveEffect(Define.PlayerEffect.ViichanShieldLoop);

            if (Mathf.Approximately(_skill.CurGauge, 0))
            {
                _skill.SpawnEffect(Define.PlayerEffect.ViichanShieldBroken,0.5f,user.Position,false);
                GameManager.Sound.Play("viichanSkill_breakshield");
                _skill.OnShieldBreak?.Invoke();
            }
            
            _skill.OnShieldEnd?.Invoke();
        }
    }
}