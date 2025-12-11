using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "SeguMechaAttack", menuName = "Scriptable/Skill/Gosegu/SeguMechaAttack")]
    public class SeguMechaAttackSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Continuous;

        private SeguMecha mecha;

        public void Init(SeguMecha mecha)
        {
            this.mecha = mecha;
        }

        public override void Active()
        {
            base.Active();
            mecha.FireOn();
        }

        public override void Cancel()
        {
            base.Cancel();
            mecha.FireEnd();
        }
    }
}