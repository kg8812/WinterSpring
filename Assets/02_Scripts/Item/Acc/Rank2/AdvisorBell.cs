using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class AdvisorBell : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("기절 지속시간")] public float duration;
        [LabelText("기절 적용반경")] public float radius;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnHit,Stun);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnHit,Stun);
        }

        void Stun(EventParameters _)
        {
            if (isCd) return;
            
            var targets = user.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy).Select(x => x as Actor)
                .Where(x => x != null).ToList();
            
            targets.ForEach(x => x.StartStun(user,duration));
            GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
        }
    }
}