using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class TeaSet : Accessory
    {
        [LabelText("공격력 증가 원념 필요량")] public int atkGold;
        [LabelText("공격력 증가량(%)")] public float atk;
        [LabelText("스탯 원념 필요량")] public int statGold;
        [LabelText("스탯 증가량")] public float statAmount;
        [LabelText("스탯 최대 증가량")] public float statMaxAmount;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            stat.SetRatio(ActorStatType.Atk,atk * (GameManager.instance.Soul / atkGold));
            stat.SetValue(ActorStatType.Body,Mathf.Clamp(statAmount * (GameManager.instance.Soul / statGold),0,statMaxAmount));
            stat.SetValue(ActorStatType.Spirit,Mathf.Clamp(statAmount * (GameManager.instance.Soul / statGold),0,statMaxAmount));
            stat.SetValue(ActorStatType.Finesse,Mathf.Clamp(statAmount * (GameManager.instance.Soul / statGold),0,statMaxAmount));
            
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