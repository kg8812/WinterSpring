using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "ViichanSwordSkill", menuName = "Scriptable/Skill/Weapon/ViichanSwordSkill")]

    public class ViichanSwordSkill : SwordSkill
    {
        [InfoBox("스킬 쿨타임을 필요 누적 피해량으로 설정해주시면 됩니다.")]

        private GaugeCd gcd;
        public override ICdActive CDActive => gcd ??= new GaugeCd(this);


        public override void Init(Weapon weapon)
        {
            base.Init(weapon);
            Item = weapon;
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            eventUser.EventManager.AddEvent(EventType.OnAfterAtk,AddGauge);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            eventUser.EventManager.RemoveEvent(EventType.OnAfterAtk, AddGauge);
        }

        void AddGauge(EventParameters param)
        {
            if (param != null)
            {
                CurCd -= param.hitData.dmgReceived;
                Use();
            }

        }
    }
}