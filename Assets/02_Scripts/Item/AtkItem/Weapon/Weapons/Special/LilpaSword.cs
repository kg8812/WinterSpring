using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class LilpaSword : Weapon
    {
        private IWeaponAttack iSwordAttack;
        
        public override IWeaponAttack IAttack => iSwordAttack;

        public override float Atk => activeSkill.Atk;

        public override float BaseGroggyPower => activeSkill.BaseGroggyPower;

        [HideInInspector] public LilpaActiveSkill activeSkill;
        
        public struct RushInfo
        {
            public float distance;
            public float duration;
            public float dmg;
            public int groggy;
            public float radius;
            public Vector2 offset;
        }
        
        public override void BeforeAttack()
        {
            base.BeforeAttack();
        }

        public void Init(RushInfo rushInfo)
        {
            iSwordAttack = new LilpaSwordAttack(this, rushInfo);
        }
        
        public override void Attack(int count)
        {
Debug.Log("Attack");
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