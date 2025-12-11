using System.Collections.Generic;

namespace chamwhy.Components
{
    public class TA_EnterArena: ITriggerActivate
    {
        private Trigger _triggerComponent;
        private List<string> _bgm;
        private float _fadeTime;
        private string sfxAddress;
        public TA_EnterArena(Trigger triggerComponent,List<string> bgm,float fadeTime,string sfx)
        {
            _triggerComponent = triggerComponent;
            _bgm = bgm;
            _fadeTime = fadeTime;
            sfxAddress = sfx;
        }
        
        public void Activate()
        {
            GameManager.instance.ToggleArena(true);
            PlaySound();
        }

        void PlaySound()
        {
            GameManager.Sound.PlayArenaBGM(_bgm,_fadeTime);
            GameManager.Sound.Play(sfxAddress);
        }
    }
}