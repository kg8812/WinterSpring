using chamwhy;
using UnityEngine;

namespace Apis
{
    public class LolipopTrain : Projectile
    {

        public override void Fire(bool rotateWithPlayerX = true)
        {
            base.Fire(rotateWithPlayerX);
            Direction = rigid.velocity.x > 0 ? EActorDirection.Right : EActorDirection.Left;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == Layers.Wall)
            {
                rigid.velocity = new Vector2(Mathf.Abs(firstVelocity.x) * -(int)Direction, rigid.velocity.y);
                Direction = rigid.velocity.x > 0 ? EActorDirection.Right : EActorDirection.Left;
            }
        }
    }
}