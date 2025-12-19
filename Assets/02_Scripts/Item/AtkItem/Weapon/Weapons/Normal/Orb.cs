using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class Orb : MagicWeapon
    {
        public override AttackCategory Category => AttackCategory.Orb;

        private IWeaponAttack iAttack;
        [TitleGroup("공격설정")]
        [TabGroup("공격설정/설정","지상")][LabelText("오브설정")] public List<OrbAtk.OrbAtkInfo> groundOrbInfos;
        [TitleGroup("공격설정")]
        [TabGroup("공격설정/설정","공중")][LabelText("오브설정")] public List<OrbAtk.OrbAtkInfo> airOrbInfos;
        public override IWeaponAttack IAttack => iAttack ??= new OrbAtk(this, groundOrbInfos,airOrbInfos);
    }
}