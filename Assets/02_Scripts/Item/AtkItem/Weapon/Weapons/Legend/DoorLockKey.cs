using UnityEngine;

namespace Apis

{
    public class DoorLockKey : Weapon
    {
        public override void UseAttack()
        {
            Skill.Use();
        }
    }
}