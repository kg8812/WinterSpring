using chamwhy;
using UnityEngine;


namespace Apis
{
    public class RushEffect : AttackObject
    {
        [HideInInspector] public Vector2 startPos;

        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);

            if (ReferenceEquals(parameters?.target, null)) return;

            float distance = Vector2.Distance(startPos, parameters.target.transform.position);
            Attack(parameters, distance * DmgRatio);
        }
    }
}