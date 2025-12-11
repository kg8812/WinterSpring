namespace Apis
{
    public class NoteProjectile : Boomerang
    {
        protected override void FirstAttackInvoke(EventParameters parameters)
        {
            base.FirstAttackInvoke(parameters);

            if (parameters?.target is Actor actor)
            {
                actor.AddSubBuff(_eventUser,SubBuffType.Debuff_Burn);
            }
        }

        protected override void ReturnAttackInvoke(EventParameters parameters)
        {
            base.ReturnAttackInvoke(parameters);
            if (parameters?.target is Actor actor)
            {
                actor.AddSubBuff(_eventUser,SubBuffType.Debuff_Burn);
            }
        }
    }
}