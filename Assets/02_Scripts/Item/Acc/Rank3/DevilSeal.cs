using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DevilSeal : Accessory
    {
        [LabelText("스탯 적용 체력 %")] public float hp1;
        [LabelText("공격력 증가량")] public float atk;
        [LabelText("공격속도 증가량")] public float atkSpeed;
        [LabelText("피해 감소 체력 %")] public float hp2;
        [LabelText("피해 감소량")] public float dmgReduce;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnHpDown,SetStat);
            user?.AddEvent(EventType.OnHpHeal,SetStat);
            SetStat(null);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnHpDown,SetStat);
            user?.RemoveEvent(EventType.OnHpHeal,SetStat);
        }

        void SetStat(EventParameters _)
        {
            if (user == null) return;
            float amount = user.CurHp / user.MaxHp * 100;
            if (amount >= hp1 || Mathf.Approximately(amount ,hp1))
            {
                BonusStat.SetValue(ActorStatType.Atk,atk);
                BonusStat.SetValue(ActorStatType.AtkSpeed,atkSpeed);
            }
            else
            {
                BonusStat.SetValue(ActorStatType.Atk,0);
                BonusStat.SetValue(ActorStatType.AtkSpeed,0);
            }

            if (amount <= hp2)
            {
                BonusStat.SetValue(ActorStatType.DmgReduce,dmgReduce);
            }
            else
            {
                BonusStat.SetValue(ActorStatType.DmgReduce,0);
            }
        }
    }
}