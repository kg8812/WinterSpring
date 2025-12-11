using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class UnfinishDemension : Accessory
    {
        [LabelText("캐스팅 속도 증가량(%)")] public float castingSpeed;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            if (stat == null)
            {
                stat = new();
                stat.AddValue(ActorStatType.CastingSpeed, castingSpeed);
            }
            return stat;
        }
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnCastingCancel,AddCastingSpeed);
            user?.AddEvent(EventType.OnChargeCancel,AddCastingSpeed);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnCastingCancel,AddCastingSpeed);
            user.RemoveEvent(EventType.OnChargeCancel,AddCastingSpeed);
            RemoveCastingSpeed(null);
        }

        void AddCastingSpeed(EventParameters param)
        {
            user.BonusStatEvent += StatEvent;
            user.AddEvent(EventType.OnCastingEnd,RemoveCastingSpeed);
        }

        void RemoveCastingSpeed(EventParameters param)
        {
            user.BonusStatEvent -= StatEvent;
            user.RemoveEvent(EventType.OnCastingEnd,RemoveCastingSpeed);
        }
    }
}