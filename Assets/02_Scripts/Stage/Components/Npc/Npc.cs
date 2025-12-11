using System;
using Directing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy
{
    public enum EndActionType
    {
        None, SceneMove
    }
    [RequireComponent(typeof(DialogueDirector))]
    public class Npc : MonoBehaviour, IOnInteract
    {
        [Tooltip("global이라면 씬이 움직여도 unique 특성의 script는 말하지 않습니다.")]
        // public bool isGlobalNpc;
        public bool isShowInGame;

        public bool isFixedScript = false;
        public int fixedScriptIndex;
        
        [ShowIf("isShowInGame")] public int ingameId;
        protected DialogueDirector _dialogue;


        public EndActionType endActionType;

        [ShowIf("endActionType", EndActionType.SceneMove)] [SerializeField]
        protected string sceneMoveName;

        private void Awake()
        {
            // alreadySpoke ??= new();
            
            _dialogue = GetComponent<DialogueDirector>();

        }

        private void Start()
        {
        }


        protected void EndAction()
        {
            switch (endActionType)
            {
                case EndActionType.SceneMove:
                    GameManager.Scene.SceneLoad(sceneMoveName);
                    break;
            }
        }

        protected void StartScripting()
        {
            if (isShowInGame)
            {
                Director.instance.PlayInGameCutScene(ingameId);
            }
            else
            {
                if (isFixedScript)
                {
                    _dialogue.PlayFixedScript(fixedScriptIndex, EndAction);
                }
                else
                {
                    _dialogue.SelectNpc(EndAction);
                }
            }
        }


        public Func<bool> InteractCheckEvent { get; set; }

        public virtual void OnInteract()
        {
            StartScripting();
        }
    }
}