using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class WakomBrush : Accessory
    {
        [LabelText("체력 증가량")] public float hp;
        [LabelText("결속력 증가량")] public float cdReduction;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.MaxHp,hp);
            stat.SetValue(ActorStatType.CDReduction,cdReduction);
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