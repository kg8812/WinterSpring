using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "AttackOff", menuName = "ActorCommand/Drone/AttackOff", order = 1)]
    public class DroneAttackOff : DroneCommand
    {
        protected override void Invoke(SeguMecha drone)
        {
            drone.FireEnd();
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return true;
        }
    }
}