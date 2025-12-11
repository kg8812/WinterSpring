using System.Collections;
using System.Collections.Generic;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class LightHouseArt : Accessory
    {
        [LabelText("기절 적용 반경")] public float radius;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnSubBuffApply,AddStuns);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnSubBuffApply,AddStuns);
        }

        void AddStuns(EventParameters parameters)
        {
            if (parameters == null || parameters.buffData.activatedSubBuff.Type != SubBuffType.Debuff_Stun || parameters.target is not Actor target) return;

            var targets = target.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy);
            targets.Remove(target);
            
            targets.ForEach(x =>
            {
                if (x is Actor t)
                {
                    t.StartStun(user,parameters.buffData.activatedSubBuff.Duration);
                }
            });
        }
    }
}