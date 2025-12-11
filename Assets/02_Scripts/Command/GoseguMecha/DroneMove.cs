using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "Move", menuName = "ActorCommand/Drone/Move", order = 1)]
    public class DroneMove : DroneCommand
    {
        public EActorDirection dir;

        protected override void Invoke(SeguMecha drone)
        {
            drone.moveDir = dir;
            drone.SetDirection(dir);
            drone.ChangeState(SeguMecha.States.Move);
            drone.DoMove();
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return drone.GetState() != SeguMecha.States.Dash;
        }
    }
}