using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{

    public class GoatSegu : Weapon
    {
        [Title("오선지 설정")] 
        [LabelText("오선지 크기")]public Vector2 size;
        [LabelText("폭발 크기")]public Vector2 expSize;
        
        private IWeaponAttack iattack;
        public override AttackCategory Category => AttackCategory.Orb;

        public override IWeaponAttack IAttack => iattack??= new GoatSeguAtk(this,size,expSize);
    }
}