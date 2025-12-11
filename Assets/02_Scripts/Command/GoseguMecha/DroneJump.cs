using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "Jump", menuName = "ActorCommand/Drone/Jump", order = 1)]
    public class DroneJump : DroneCommand
    {
        protected override void Invoke(SeguMecha drone)
        {
            drone.ChangeState(SeguMecha.States.Jump);
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return drone.ActorMovement.IsStick;
        }
    }
}