using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "Idle", menuName = "ActorCommand/Drone/Idle", order = 1)]
    public class DroneIdle : DroneCommand
    {
        protected override void Invoke(SeguMecha drone)
        {
            drone.ChangeState(SeguMecha.States.Idle);
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return !drone.BlockIdle;
        }
    }
}