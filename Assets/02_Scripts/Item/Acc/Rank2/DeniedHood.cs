using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DeniedHood : Accessory
    {
        [LabelText("보호막 량 (최대체력 %)")] public float amount;
        [LabelText("보호막 지속시간")] public float duration;
        [LabelText("방어력 증가량")] public float def;
        [LabelText("쿨타임")] public float cd;

        private Buff _buff;

        public override void Init()
        {
            base.Init();
            BonusStat.SetValue(ActorStatType.Def, def);
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Buff_Barrier)
            {
                buffPower = new[] { amount }, buffCategory = 1, buffDuration = duration,
                buffDispellType = 1, applyStrategy = 0, buffMaxStack = 1, stackDecrease = 0, showIcon = false
            };
            _buff = new(data, user);
            user.AddEvent(EventType.OnAfterHit,AddBarrier);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnAfterHit,AddBarrier);
        }

        void AddBarrier(EventParameters _)
        {
            if (isCd) return;

            GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
            _buff.AddSubBuff(user, null);
        }
    }
}