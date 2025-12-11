using System.Collections;
using System.Collections.Generic;
using Apis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SilenceToken : Accessory
    {
        [LabelText("지속시간")] public float duration;
        [LabelText("쿨타임")] public float cd;

        public override void Init()
        {
            base.Init();
            OnDurationStart += () =>
            {
                user?.AddEvent(EventType.OnAttackSuccess, MakeCrit);
            };
            OnDurationEnd += AfterDuration;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnAfterAtk,CritCheck);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnAfterAtk,CritCheck);
        }

        void CritCheck(EventParameters parameters)
        {
            if (parameters == null || isCd) return;
            if (parameters.hitData.isCritApplied)
            {
                GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
                GameManager.instance.StartCoroutineWrapper(DurationCoroutine(duration));
            }
        }

        void MakeCrit(EventParameters parameters)
        {
            if (parameters == null) return;
            parameters.atkData.isfixedCrit = true;
        }

        void AfterDuration()
        {
            user?.RemoveEvent(EventType.OnAttackSuccess,MakeCrit);
        }
    }
}