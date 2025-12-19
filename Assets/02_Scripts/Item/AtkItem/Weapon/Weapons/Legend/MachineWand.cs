namespace Apis
{
    public class MachineWand : Wand
    {
        private IWeaponAttack iattack;
        public override AttackCategory Category => AttackCategory.Staff;
    }
}