using Apis;
using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "FireMissile", menuName = "ActorCommand/Drone/FireMissile", order = 1)]

    public class DroneFireMissile : DroneCommand
    {
        public ActiveSkill skill;

        protected override void Invoke(SeguMecha drone)
        {
            skill?.Use();
        }

        protected override bool InvokeCondition(SeguMecha drone)
        {
            return true;
        }
    }
}