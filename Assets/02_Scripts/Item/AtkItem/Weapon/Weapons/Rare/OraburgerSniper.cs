namespace Apis
{
    public class OraburgerSniper : ProjectileWeapon
    {
        private IWeaponAttack iAttack;

        public override IWeaponAttack IAttack
        {
            get
            {
                iAttack ??= new ProjectileAttack(this);
                return iAttack;
            }
        }
    }
}