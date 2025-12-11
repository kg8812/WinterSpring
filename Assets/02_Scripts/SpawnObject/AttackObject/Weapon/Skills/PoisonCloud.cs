using chamwhy;

namespace Apis
{
    public class PoisonCloud : AttackObject
    {
        private bool added;

        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);
            
            if (parameters?.target is Actor actor)
            {
                actor.AddSubBuff(_eventUser, SubBuffType.Debuff_Poison);
            }
        }
    }
}