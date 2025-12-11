using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class LilpaWeaponSkill : MagicSkill
    {
        protected override bool UseAtkRatio => false;

        protected override bool UseGroggyRatio => false;

        public override bool isActive => false;

        private ICdActive cdActive;
        public override ICdActive CDActive => cdActive ??= new StackCd(this);
        protected override CDEnums _cdType => CDEnums.Stack;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [TitleGroup("스탯값")] [LabelText("장착시 쿨타임 계수 (%)")][Tooltip("100 = 기존 속도")] public float ratio;
        public override float cdRatio => _weapon.IsEquip ? ratio : 100;

        public override float CurCd
        {
            get => base.CurCd;
            set
            {
                if (_weapon != null && _weapon.IsEquip) return;
                base.CurCd = value;
            }
        }
    }
}