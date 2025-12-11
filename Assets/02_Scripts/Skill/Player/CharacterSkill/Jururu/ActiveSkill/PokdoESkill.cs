using UI;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "PokdoESkill",menuName = "Scriptable/Skill/PokdoESkill")]
    public class PokdoESkill : ActiveSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [HideInInspector] public PokdoStand pokdo;

        public override bool TryUse()
        {
            return base.TryUse() && pokdo != null && (pokdo.GetState() == PokdoStand.PokdoState.Idle || pokdo.GetState() == PokdoStand.PokdoState.Attack);
        }

        public override void Active()
        {
            base.Active();

            if (pokdo == null) return;
            pokdo.UseESkill();
            GameManager.instance.Player.ApplyPlayerPreset();
        }
    }
}