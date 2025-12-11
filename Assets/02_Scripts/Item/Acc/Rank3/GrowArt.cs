using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GrowArt : Accessory
    {
        [LabelText("고유트리 공격력 증가량 (%)")] public float dmg;
        [LabelText("결속력 증가량")] public float cdReduction;
        [LabelText("결속력 지속시간")] public float duration;

        private SkillAttachment attachment;
        public override void Init()
        {
            base.Init();
            attachment ??= new(new SkillStat()
            {
                dmgRatio = dmg
            });
            OnDurationStart += () =>
            {
                BonusStat.SetValue(ActorStatType.CDReduction,cdReduction);
            };
            OnDurationEnd += () =>
            {
                BonusStat.SetValue(ActorStatType.CDReduction, 0);
            };
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            if (user is Player player)
            {
                player.ActiveSkill?.AddAttachment(attachment);
                player.PassiveSkill?.AddAttachment(attachment);
            }
            
            user?.AddEvent(EventType.OnSkill,StartDuration);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            if (user is Player player)
            {
                player.ActiveSkill?.RemoveAttachment(attachment);
                player.PassiveSkill?.RemoveAttachment(attachment);
            }
            user?.RemoveEvent(EventType.OnSkill,StartDuration);
        }

        void StartDuration(EventParameters parameters)
        {
            GameManager.instance.StartCoroutineWrapper(DurationCoroutine(duration));
        }
    }
}