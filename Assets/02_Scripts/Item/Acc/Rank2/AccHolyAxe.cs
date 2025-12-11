using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class AccHolyAxe : Accessory
    {
        [LabelText("처형 체력%")] public float hp;

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            this.user?.AddEvent(EventType.OnAttackSuccess,Execute);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            
            user?.RemoveEvent(EventType.OnAttackSuccess,Execute);
        }

        void Execute(EventParameters parameters)
        {
            if (parameters?.target is Actor target && target.Contains(SubBuffType.Debuff_Stun) &&
                target.CurHp / target.MaxHp <= hp)
            {
                target.CurHp = 0;
            }
        }
    }
}