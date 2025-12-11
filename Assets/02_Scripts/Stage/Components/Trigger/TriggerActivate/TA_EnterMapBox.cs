namespace chamwhy.Components
{
    public class TA_EnterMapBox: ITriggerActivate
    {
        private int _mapBoxId;
        public TA_EnterMapBox(int mapBoxId)
        {
            _mapBoxId = mapBoxId;
        }
        
        public void Activate()
        {
            Map.instance.EnterMapBox(_mapBoxId);
        }
    }
}