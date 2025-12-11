using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class IpariHairband : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user?.AddEvent(EventType.OnKill,ResetCD);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnKill,ResetCD);
        }

        void ResetCD(EventParameters param)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
            if (param is { atkData: { attackType: Define.AttackType.WeaponSkillAttack } } && user is Player player)
            {
                //player.AttackItemManager.Weapon.Skill.CurCd = 0;
            }
        }
    }
}