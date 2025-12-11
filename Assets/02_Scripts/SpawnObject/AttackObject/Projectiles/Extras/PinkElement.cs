using chamwhy;

namespace Apis
{
    public class PinkElement : Projectile
    {
        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);
            if (parameters.target is Actor t)
            {
                t.AddSubBuff(_eventUser, SubBuffType.Debuff_Burn);
            }
        }
    }
}