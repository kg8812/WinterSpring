using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ScholarPride : Accessory
    {
        [LabelText("초기화 확률 (%)")] public float probability;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnSkill, ResetCD);
            user.AddEvent(EventType.OnWeaponSkillUse, ResetCD);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnSkill, ResetCD);
            user.RemoveEvent(EventType.OnWeaponSkillUse, ResetCD);
        }

        void ResetCD(EventParameters param)
        {
            float rand = Random.Range(0, 100f);
            if (rand > probability) return;
            if (param?.skillData.usedSkill != null)
            {
                param.skillData.usedSkill.CurCd = 0;
            }
        }
    }
}