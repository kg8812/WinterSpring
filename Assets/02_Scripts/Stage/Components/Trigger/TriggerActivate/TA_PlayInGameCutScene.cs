using System.Collections.Generic;
using chamwhy.Components;
using Directing;
using UnityEngine;
using UnityEngine.Events;

namespace chamwhy
{
    public class TA_PlayInGameCutScene: ITriggerActivate
    {
        private int _ingameId;
        private bool _dontSave;
        private UnityEvent endEvent;
        
        public TA_PlayInGameCutScene(int ingameId,bool dontSave,UnityEvent endEvent)
        {
            _ingameId = ingameId;
            _dontSave = dontSave;
            this.endEvent = endEvent;
        }
        
        public void Activate()
        {
            Director.instance.PlayInGameCutScene(_ingameId , () =>
            {
                endEvent?.Invoke();
                if (_dontSave) return;
                Director.instance.AddCutSceneId(_ingameId);
            });
        }
    }
}