using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy.Tester
{
    public class CCTester: MonoBehaviour
    {
        public Actor actor;

        [Button(ButtonSizes.Large)]
        public void CCGive()
        {
            // actor.AnimPauseOn();
            //actor.MoveCCOn();
        }
        
        [Button(ButtonSizes.Gigantic)]
        public void CCOff()
        {
            // actor.AnimPauseOff();
            //actor.MoveCCOff();
        }
    }
}