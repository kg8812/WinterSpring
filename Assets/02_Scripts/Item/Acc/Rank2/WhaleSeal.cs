using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    public class WhaleSeal : Accessory
    {
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnAttackSuccess,ApplyPoison);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnAttackSuccess,ApplyPoison);
        }

        void ApplyPoison(EventParameters param)
        {
            if (Mathf.Approximately(user.Barrier, 0)) return;
            
            if(param?.target is Actor target)
            {
                target.AddSubBuff(user,SubBuffType.Debuff_Poison);
            }
        }
    }
}