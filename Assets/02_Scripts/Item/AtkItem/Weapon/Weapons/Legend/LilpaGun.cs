using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    public class LilpaGun : ProjectileWeapon
    {
        private LilpaGunAttack _attack;

        public override IWeaponAttack IAttack => _attack ??= new(this);

        public class LilpaGunAttack : ProjectileAttack
        {
            public override int AttackType => 2;

            public LilpaGunAttack(ProjectileWeapon weapon) : base(weapon)
            {
            }
        }

        public override void Attack(int count)
        {
            base.Attack(count);
        }

        public override void OnAnimEnter(int index)
        {
            base.OnAnimEnter(index);

            if (index == 2)
            {
                Skill?.Use();
            }
        }

        public override void EndAttack()
        {
            base.EndAttack();
            Skill?.DeActive();
        }
    }
}