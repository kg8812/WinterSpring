using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class Burdock : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("쉴드량 (%)")] public float shield;
        [LabelText("쉴드 지속시간")] public float duration;
        [LabelText("공격속도 증가량")] public float atkSpeed;

        private Buff buff;
        private BonusStat _stat;
        
        BonusStat AtkSpeedStat()
        {
            _stat ??= new();
            float barrier = user.Barrier;
            if (!Mathf.Approximately(barrier,0) && barrier > 0)
            {
                _stat.Stats[ActorStatType.AtkSpeed].Value = atkSpeed;
            }
            else
            {
                _stat.Stats[ActorStatType.AtkSpeed].Value = 0;
            }

            return _stat;
        }
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Buff_Barrier)
            {
                buffPower = new[] { shield }, buffDuration = duration, buffDispellType = 1, buffMaxStack = 1,buffCategory = 1,
                valueType = ValueType.Ratio, showIcon = false, applyStrategy = 0,
            };

            buff = new(data, user);
            user.AddEvent(EventType.OnCrit, AddBarrier);
            user.BonusStatEvent += AtkSpeedStat;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnCrit, AddBarrier);
            user.BonusStatEvent -= AtkSpeedStat;
        }

        void AddBarrier(EventParameters param)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutine(CDCoroutine(cd));
            SubBuff sub = buff.AddSubBuff(user, null);
            sub.Stat.AddValue(ActorStatType.AtkSpeed, atkSpeed);
        }
    }
}