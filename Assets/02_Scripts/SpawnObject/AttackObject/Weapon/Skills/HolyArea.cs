using chamwhy;
using Sirenix.OdinInspector;

namespace Apis
{
    public class HolyArea : AttackObject
    {
        [LabelText("회복량 (백분율)")] public float healAmount;

        protected override void AttackInvoke(EventParameters parameters)
        {
            if (parameters?.target == null) return;
            if (parameters.target is not IMovable target) return;
            if (!target.ActorMovement.IsStick) return;

            base.AttackInvoke(parameters);
            if (_onHit != null)
            {
                _onHit.CurHp += _onHit.MaxHp / 100 * healAmount;
            }
        }
    }
}