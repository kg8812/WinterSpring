using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class RasenShuriken : Projectile
    {
        private HashSet<IOnHit> targets = new();

        public override void Fire(bool rotateWithPlayerX = true)
        {
            base.Fire(rotateWithPlayerX);
            lastPos = transform.position;
            targets.Clear();
        }

        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);
            if (parameters?.target != null)
            {
                if (parameters.target is IMovable mover)
                {
                    mover.MoveCCOn();
                }
                if (parameters.target is Actor actor)
                {
                    actor.AttackOff();
                }

                targets.Add(parameters.target);
            }
        }

        private Vector2 lastPos;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fired)
            {
                float distance = transform.position.x - lastPos.x;

                var temp = targets.ToList();
                temp.ForEach(x => { x.transform.Translate(Vector2.right * distance); });
                lastPos = transform.position;
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            targets.ForEach(x =>
            {
                if (x is IMovable mover)
                {
                    mover.MoveCCOff();
                }
                if (x is Actor t)
                {
                    t.AttackOn();
                }
            });
            targets.Clear();
        }
    }
}