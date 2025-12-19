using UnityEngine;

namespace Apis

{
    public class DoorLockKey : Weapon
    {
        public override AttackCategory Category => AttackCategory.Orb;

        public override void UseAttack()
        {
            Skill.Use();
        }
    }
}