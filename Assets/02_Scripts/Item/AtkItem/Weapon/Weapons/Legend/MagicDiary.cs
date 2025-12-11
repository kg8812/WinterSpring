namespace Apis
{
    public class MagicDiary : ProjectileWeapon
    {
        private IWeaponAttack iattack;

        public override IWeaponAttack IAttack =>
            iattack ??= new ProjectileAttack(this);

       public class MagicDiaryAtk : Weapon_BasicAttack
       {
           public MagicDiaryAtk(Weapon weapon) : base(weapon)
           {
           }

           public override void GroundAttack(int index)
           {
               base.GroundAttack(index);
           }

           public override void AirAttack(int index)
           {
               base.AirAttack(index);
           }
       }
    }
}