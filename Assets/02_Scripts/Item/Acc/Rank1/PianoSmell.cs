using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class PianoSmell : Accessory
    {
        [LabelText("데미지 증가량(%)")] public float dmg;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            if (stat == null)
            {
                stat = new();
                stat.AddValue(ActorStatType.CastingDmg,dmg);
            }

            return stat;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.BonusStatEvent += StatEvent;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.BonusStatEvent -= StatEvent;
        }
    }
}