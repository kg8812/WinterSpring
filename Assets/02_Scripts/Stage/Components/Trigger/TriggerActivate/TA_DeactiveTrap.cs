using Managers;
using UnityEngine;

namespace chamwhy.Components
{
    
    public class TA_DeactiveTrap: ITriggerActivate
    {
        private Trigger _triggerComponent;
        private Trap _trap;
        
        public TA_DeactiveTrap(Trigger triggerComponent)
        {
            _triggerComponent = triggerComponent;
            _trap = _triggerComponent.GetComponent<Trap>();
            if (_trap == null)
            {
                Debug.LogError("trigger trap 형식인데 trap 코드가 없음");
            }
        }
        
        public void Activate()
        {
            _trap.Deactive();
        }
    }
}