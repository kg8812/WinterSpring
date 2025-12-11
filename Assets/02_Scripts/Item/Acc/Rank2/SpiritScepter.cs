using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class SpiritScepter : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("반경")] public float radius;
        [LabelText("지속시간")] public float duration;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnHit,ApplyChill);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnHit,ApplyChill);
        }

        void ApplyChill(EventParameters _)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutine(CDCoroutine(cd));
            var targets = user.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy).Select(x => x as Actor);
            targets.ForEach(x =>
            {
                x?.SubBuffManager.AddSubBuff(SubBuffType.Debuff_Chill,user);
            });
        }
    }
}