using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "Stop", menuName = "ActorCommand/Drone/Stop", order = 1)]

    public class DroneStop : DroneCommand
    {
        protected override void Invoke(SeguMecha drone)
        {
            drone.ActorMovement.StopWithFall();
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return true;
        }
    }
}