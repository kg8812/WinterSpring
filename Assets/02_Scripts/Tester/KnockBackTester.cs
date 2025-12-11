using EventData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy.Tester
{
    public class KnockBackTester: MonoBehaviour
    {
        public Actor target;
        public float force, time, angle;

        [Button(ButtonSizes.Large)]
        public void KnockBackTest()
        {
            target.OnHit(new EventParameters(GameManager.instance.Player){atkData = new AttackEventData(){isHitReaction = true},knockBackData = new KnockBackData(){knockBackForce = force, knockBackTime = time, knockBackAngle = angle}});
        }

        private void Update()
        {
        }
    }
}