using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "LilpaGunSkill", menuName = "Scriptable/Skill/Weapon/LilpaGunSkill")]
    public class LilpaGunSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Continuous;

        public override void Cancel()
        {
            base.Cancel();
            animator.animator.SetTrigger("AttackEnd");
        }
    }
}