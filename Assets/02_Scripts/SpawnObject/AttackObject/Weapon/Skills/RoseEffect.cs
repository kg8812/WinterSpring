using chamwhy;
using UnityEngine;

namespace Apis
{
    public class RoseEffect : AttackObject
    {

        [HideInInspector] public float hpRatio;

        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);
            _atkStrategy = new AtkBase(_attacker, DmgRatio);
            Attack(parameters);
            _atkStrategy = new HpBase(_onHit, hpRatio);
            Attack(parameters);
        }
    }
}