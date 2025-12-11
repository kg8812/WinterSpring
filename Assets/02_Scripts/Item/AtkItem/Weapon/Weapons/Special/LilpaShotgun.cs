using System.Linq;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Apis
{
    public class LilpaShotgun : ProjectileWeapon
    {
        private IWeaponAttack iGunAttack;
        
        public override IWeaponAttack IAttack => iGunAttack ??= new LilpaShotGunAttack(this);

        public override float Atk => activeSkill.Atk;
        public override float BaseGroggyPower => activeSkill.BaseGroggyPower;

        [HideInInspector] public LilpaActiveSkill activeSkill;

        public bool isEnhanced;
        
        public override void BeforeAttack()
        {
            isEnhanced = false;
            base.BeforeAttack();
        }

        public override void Attack(int count)
        {
            if (!Skill.TryUse()) return;
            Skill.Use();
            base.Attack(count);
        }

        public override bool TryAttack()
        {
            return Skill.TryUse();
        }

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
        }
    }
}