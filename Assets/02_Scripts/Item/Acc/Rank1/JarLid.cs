using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class JarLid : Accessory
    {
        [LabelText("중독부여 확률")] public float probability;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnBasicAttack,ApplyPoison);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnBasicAttack,ApplyPoison);
        }

        void ApplyPoison(EventParameters param)
        {
            float rand = Random.Range(0, 100f);
            if (rand > probability) return;

            if (param?.target is Actor target)
            {
                target.AddSubBuff(user,SubBuffType.Debuff_Poison);
            }
        }
    }
}