using Apis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "Attack", menuName = "ActorCommand/Drone/Attack", order = 1)]

    public class DroneAttack : DroneCommand
    {
        [InfoBox("사용할 무장 인덱스 (0부터 시작)")]
        public int index;

        protected override void Invoke(SeguMecha go)
        {
            AttackItemManager.Attack(index);
        }

        protected override bool InvokeCondition(SeguMecha mecha)
        {
            return true;
        }
    }
}