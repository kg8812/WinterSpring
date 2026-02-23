using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ElkSeal : Accessory
    {
        [LabelText("체력 (%)")] public float hpRate;
        [LabelText("회복 증가량 (%)")] public float healRate;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            Player player = GameManager.instance.Player;
            
            if (player == null)
            {
                stat.SetValue(ActorStatType.HealRate,0);
                return stat;
            }

            float hpRatio = player.CurHp / player.MaxHp;

            stat.SetValue(ActorStatType.HealRate,hpRatio <= hpRate ? healRate : 0);
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