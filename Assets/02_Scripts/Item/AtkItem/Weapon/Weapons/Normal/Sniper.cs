namespace Apis
{
    public class Sniper : ProjectileWeapon
    {

        IWeaponAttack iattack;

        public override IWeaponAttack IAttack
        {
            get
            {
                iattack??=new ProjectileAttack(this);
                return iattack;
            }
        }

    }
}