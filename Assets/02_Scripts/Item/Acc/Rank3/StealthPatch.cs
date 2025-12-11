using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class StealthPatch : Accessory
    {
        [LabelText("치명타 확률 증가량")] public float criticalProb;
        [LabelText("치명타 데미지 증가량")] public float criticalDmg;
        [LabelText("피격시 치명타 확률 감소량")] public float hitCriticalProb;
        [LabelText("지속시간")] public float duration;

        private Buff buff;
        public override void Init()
        {
            base.Init();
            BonusStat.SetValue(ActorStatType.CritProb,criticalProb);
            BonusStat.SetValue(ActorStatType.CritDmg,criticalDmg);
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Debuff_CritProb)
            {
                buffPower = new[] { hitCriticalProb }, buffDuration = duration, buffCategory = 1, buffDispellType = 1,
                buffMaxStack = 1,showIcon = false, valueType = ValueType.Value
            };
            buff = new(data, user);
            user?.AddEvent(EventType.OnHit,AddStat);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnHit,AddStat);
        }

        void AddStat(EventParameters _)
        {
            buff.AddSubBuff(user,null);
        }
    }
}