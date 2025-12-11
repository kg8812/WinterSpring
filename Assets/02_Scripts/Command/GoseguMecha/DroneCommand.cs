
namespace Command
{
    public abstract class DroneCommand : ActorCommand
    {
        public override void Invoke(Actor go)
        {
            Invoke(go as SeguMecha);
        }

        public override bool InvokeCondition(Actor actor)
        {
            return InvokeCondition(actor as SeguMecha);
        }

        protected abstract void Invoke(SeguMecha drone);
        protected abstract bool InvokeCondition(SeguMecha drone);
    }
}