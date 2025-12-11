namespace Apis
{
    public class GreenElement : BeamEffect
    {
        protected override void AttackInvoke(EventParameters parameters)
        {
            if (parameters?.target is not Actor at) return;
            
            base.AttackInvoke(parameters);

            Attack(parameters);
        }
    }
}