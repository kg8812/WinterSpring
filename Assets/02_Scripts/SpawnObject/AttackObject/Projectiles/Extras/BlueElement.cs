using chamwhy;

namespace Apis
{
    public class BlueElement : Projectile
    {
        protected override void AttackInvoke(EventParameters parameters)
        {
            if (parameters.target is Actor actor)
            {
                actor.AddSubBuff(actor, SubBuffType.Debuff_Chill);
            }
            base.AttackInvoke(parameters);
        }
    }
}