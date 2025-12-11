using chamwhy;
using UnityEngine;

namespace Apis
{
    public class StormScytheWind : AttackObject
    {
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("EnemyEffect") && collision.TryGetComponent(out Projectile atk))
            {
                atk.Destroy();
            }
        }
    }
}