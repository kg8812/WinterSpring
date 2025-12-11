using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class MonsterTail : Accessory
    {
        [LabelText("화상부여 확률")] public float probability;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnBasicAttack,ApplyBurn);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnBasicAttack,ApplyBurn);
        }

        void ApplyBurn(EventParameters param)
        {
            float rand = Random.Range(0, 100f);
            if (rand > probability) return;

            if (param?.target is Actor target)
            {
                target.AddSubBuff(user,SubBuffType.Debuff_Burn);
            }
        }
    }
}