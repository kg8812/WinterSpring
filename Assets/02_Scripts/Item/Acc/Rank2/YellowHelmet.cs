using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class YellowHelmet : Accessory
    {
        [LabelText("기본공격력 증가량(%)")] public float dmg;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnBasicAttack,AddDmg);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnBasicAttack,AddDmg);
        }

        void AddDmg(EventParameters param)
        {
            param?.statData.stat.AddValue(ActorStatType.ExtraDmg,dmg);
        }
    }
}