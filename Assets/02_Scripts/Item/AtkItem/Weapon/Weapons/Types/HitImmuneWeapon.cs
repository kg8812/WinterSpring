using System;
using UnityEngine;

namespace Apis
{
    public class HitImmuneWeapon : Weapon
    {
        private Guid guid;
        public override void Init()
        {
            base.Init();
            IAttack?.OnAfterAtk.AddListener(() =>
            {
                Player.RemoveHitImmunity(guid);
            });
        }

        public override void BeforeAttack()
        {
            base.BeforeAttack();
            guid = Player.AddHitImmunity();
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            Player.RemoveHitImmunity(guid);
        }

        public override void OnAttackItemChange()
        {
            base.OnAttackItemChange();
            Player.RemoveHitImmunity(guid);
        }
    }
}