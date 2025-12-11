namespace chamwhy.Components
{
    public class TA_ExitArena: ITriggerActivate
    {
        private Trigger _triggerComponent;
        private readonly float _fadeTime;
        private string _sfx;
        public TA_ExitArena(Trigger triggerComponent,float fadeTime,string sfx)
        {
            _triggerComponent = triggerComponent;
            _fadeTime = fadeTime;
            _sfx = sfx;
        }
        
        public void Activate()
        {
            GameManager.instance.ToggleArena(false);
            StopMusic();
        }

        void StopMusic()
        {
            GameManager.Sound.StopArenaBGM(_fadeTime);
            GameManager.Sound.Play(_sfx);
        }
    }
}