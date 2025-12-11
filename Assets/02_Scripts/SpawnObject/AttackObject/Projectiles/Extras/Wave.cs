using chamwhy;
using UnityEngine;

namespace Apis
{
    public class Wave : Projectile
    {
        [HideInInspector] public float stunDuration;

        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);
            
            if (parameters.target is Actor act)
            {
                act.StartStun(_eventUser,stunDuration);
            }
        }
    }
}