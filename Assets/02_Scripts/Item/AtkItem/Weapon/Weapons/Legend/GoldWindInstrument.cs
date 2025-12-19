namespace Apis
{
    public class GoldWindInstrument : Wand
    {
        private IWeaponAttack iatk;
        public override IWeaponAttack IAttack => iatk ??= new ProjectileAttack(this);
        public override AttackCategory Category => AttackCategory.Staff;

    }
}