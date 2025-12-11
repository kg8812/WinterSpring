using Managers;

namespace chamwhy.Components
{
    public class TA_DirectingSubCam: ITriggerActivate
    {
        private Trigger _triggerComponent;
        private int _subCamId;
        
        public TA_DirectingSubCam(Trigger triggerComponent, int subCamId)
        {
            _triggerComponent = triggerComponent;
            _subCamId = subCamId;
        }
        
        public void Activate()
        {
            if (_subCamId == 0)
            {
                CameraManager.instance.CloseDirecting();
            }
            else
            {
                CameraManager.instance.DirectingBySubCam(_subCamId);
            }
            
        }
    }
}