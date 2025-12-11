using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class Wand : ProjectileWeapon
    {
        private MagicAtk magic;
        public override IWeaponAttack IAttack => magic ??= new(this);
    }
}