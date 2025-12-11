using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class MemoryArt : Accessory
    {
        [LabelText("쿨타임 감소량")] public float cdReduce;
        [LabelText("공격력 증가량(%)")] public float dmg;

        private SkillAttachment _attachment;
        
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            _attachment ??= new(new SkillStat()
            {
                dmgRatio = dmg, baseCd = cdReduce
            });
            if (user is IWeaponSkillUser weapon)
            {
                weapon.AddWeaponSkillAttachment(_attachment);
            }

            if (user is IActiveSkillUser active)
            {
                active.AddActiveSkillAttachment(_attachment);
            }
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            if (user is IWeaponSkillUser weapon)
            {
                weapon.RemoveWeaponSkillAttachment(_attachment);
            }

            if (user is IActiveSkillUser active)
            {
                active.RemoveActiveSkillAttachment(_attachment);
            }
        }
    }
}