using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SmallHairPin : Accessory
    {
        [LabelText("기절 확률")] public float prob;
        [LabelText("기절 지속시간")] public float duration;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnBasicAttack,ApplyStun);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnBasicAttack,ApplyStun);
        }

        void ApplyStun(EventParameters param)
        {
            float rand = Random.Range(0, 100f);
            if (rand > prob) return;

            if (param?.target is Actor target)
            {
                target.StartStun(user,duration);
            }
        }
    }
}