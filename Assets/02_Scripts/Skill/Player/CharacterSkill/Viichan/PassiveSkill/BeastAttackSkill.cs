
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "BeastAttack", menuName = "Scriptable/Skill/Viichan/BeastAttack", order = 1)]
    public class BeastAttackSkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override bool TryUse()
        {
            return base.TryUse();
        }

        public override void Active()
        {
            base.Active();
            GameManager.instance.Player.SetState(EPlayerState.Attack);
        }
    }
}