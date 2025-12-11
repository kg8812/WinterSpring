using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SilverHair : Accessory
    {
        [LabelText("캐스팅 속도 증가량 (%)")] public float castingSpeed;
        [LabelText("영혼 증가량")] public float spirit;
        [LabelText("신체 증가량")] public float body;
        [LabelText("기량 증가량")] public float finesse;
        private BonusStat stat;

        BonusStat StatEvent()
        {
            if (stat == null)
            {
                stat = new();
                stat.SetValue(ActorStatType.CastingSpeed,castingSpeed);
                stat.SetValue(ActorStatType.Spirit,spirit);
                stat.SetValue(ActorStatType.Body,body);
                stat.SetValue(ActorStatType.Finesse,finesse);
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