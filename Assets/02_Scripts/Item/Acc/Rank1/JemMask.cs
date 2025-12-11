using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class JemMask : Accessory
    {
        [LabelText("치명타 피해 증가량")] public float criticalDmg;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.CritDmg,criticalDmg);
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