using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Apis
{
    public class BlueFlymachine : Accessory
    {
        [LabelText("무기 스킬 공격력 증가량 (%)")] public float dmg;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnAttackSuccess,AddDmg);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnAttackSuccess,AddDmg);
        }

        void AddDmg(EventParameters param)
        {
            if (param == null) return;

            if (param.atkData.attackType == Define.AttackType.WeaponSkillAttack)
            {
                param.statData.stat.AddValue(ActorStatType.ExtraDmg,dmg);
            }
        }
    }
}