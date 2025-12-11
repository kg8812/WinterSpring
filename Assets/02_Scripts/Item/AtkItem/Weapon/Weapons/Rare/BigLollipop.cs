
namespace Apis
{
    public class BigLollipop : HitImmuneWeapon
    {
        public override void Activate()
        {
            base.Activate();
            if (Skill is TrainDeparture s)
            {
               // s.stack = 0; 
            }
        }
    }
}
