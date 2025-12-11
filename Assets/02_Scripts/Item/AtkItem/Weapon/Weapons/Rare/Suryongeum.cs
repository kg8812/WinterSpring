using Sirenix.OdinInspector;

namespace Apis
{
    public class Suryongeum : Weapon
    {
        private IWeaponAttack iAttack;

        [LabelText("단풍나무")] public SuryongeumAtk.MapleTreeInfo info;
        
        public override IWeaponAttack IAttack => iAttack ??= new SuryongeumAtk(this,info);
    }
}