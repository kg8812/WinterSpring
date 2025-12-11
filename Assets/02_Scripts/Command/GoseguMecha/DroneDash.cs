using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "DroneDash", menuName = "ActorCommand/Drone/DroneDash")]
    public class DroneDash : DroneCommand
    {
        protected override void Invoke(SeguMecha drone)
        { 
            drone.ChangeState(SeguMecha.States.Dash);
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return !(drone.Controller.InputDir == DroneController.InputDirection.Up && drone.onAir && drone.dashCount < drone.maxDashCount);
        }
    }
}