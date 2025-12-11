using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class RiddleToad : Accessory
    {
        [LabelText("기량 증가량")] public float finesse;
        [LabelText("최대 스택")] public int maxStack;
        [LabelText("최대치 도달시 증가량")] public float finesseIncrement;

        private int stack;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetValue(ActorStatType.Finesse,
                stack == maxStack ? stack * finesse + finesseIncrement : stack * finesse);

            return stat;
        }
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);

            user.BonusStatEvent += StatEvent;
            user.AddEvent(EventType.OnSubBuffApply,AddStack);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.BonusStatEvent -= StatEvent;
            user.RemoveEvent(EventType.OnSubBuffApply,AddStack);
        }

        void AddStack(EventParameters param)
        {
            if (stack >= maxStack) return;
            
            if (param.buffData.activatedSubBuff.Type == SubBuffType.Debuff_Poison)
            {
                stack++;
            }
        }
    }
}