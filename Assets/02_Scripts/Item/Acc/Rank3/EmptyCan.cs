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
    public class EmptyCan : Accessory
    {
        [LabelText("반경")] public float radius;
        [LabelText("지속시간")] public float duration;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnKill,ApplyFreeze);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnKill,ApplyFreeze);
        }

        void ApplyFreeze(EventParameters param)
        {
            if (param?.target is Actor target && target.Contains(SubBuffType.Debuff_Frozen))
            {
                var targets = user.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy).Select(x => x as Actor);
                targets.ForEach(x =>
                {
                    x?.SubBuffManager.AddCC(user, SubBuffType.Debuff_Frozen, duration);
                });
            }
        }
    }
}