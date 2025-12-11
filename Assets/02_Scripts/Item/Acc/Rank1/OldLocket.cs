using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class OldLocket : Accessory
    {
        [LabelText("공속 증가량")] public float atkSpeed;
        [LabelText("이속 증가량")] public float moveSpeed;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.AtkSpeed,atkSpeed);
            stat.SetRatio(ActorStatType.MoveSpeed,moveSpeed);
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