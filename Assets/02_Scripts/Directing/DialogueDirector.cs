using System;
using System.Collections.Generic;
using chamwhy;
using chamwhy.DataType;
using GameStateSpace;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Directing
{
    public struct ScriptData
    {
        public int speakerId;
        public bool isAuto;
        public float[] duration;
        public string[] scriptsKor;
        public string[] scriptsEng;

        public ScriptData(ScriptDataType data)
        {
            this.speakerId = data.speaker;
            this.isAuto = data.isAuto;
            this.duration = data.duration;
            this.scriptsKor = data.scriptsKor;
            this.scriptsEng = data.scriptsEng;
        }

        public ScriptData(InGameCutSceneDataType data)
        {
            this.speakerId = data.speaker;
            this.isAuto = data.isAuto;
            this.duration = new float[] { data.duration };
            this.scriptsKor = new string[] { data.scriptKor };
            this.scriptsEng = new string[] { data.scriptEng };
        }
    }

    public class DialogueDirector : MonoBehaviour
    {
        public int speakerId;

        private List<ScriptDataType> myScripts;
        public static Dictionary<int, List<int>> alreadySpoke;

        private int _curPriority;
        private List<ScriptDataType> _curScripts;
        private ScriptDataType _curScript;
        [HideInInspector] public bool isFailed;


        private ScriptData dummyScript;

        private UI_Dialogue _dialogueUI;

        private void Awake()
        {
            _curPriority = -1;
            _curScripts = new();
            
            dummyScript = new();
            dummyScript.isAuto = false;
            dummyScript.scriptsKor = new[] { "..." };
            dummyScript.scriptsEng = new[] { "..." };
        }

        private void Start()
        {
            myScripts = NpcModel.scriptDict[speakerId];
            GameManager.Directing.RegisterDialogueDirector(this);
        }

        public ScriptDataType GetScriptDataType(int index)
        {
            foreach (var myScript in myScripts)
            {
                if (myScript.index == index)
                {
                    return myScript;
                }
            }
            return null;
        }

        private bool IsAlreadySpoke(int id)
        {
            if (!alreadySpoke.ContainsKey(speakerId)) return false;
            return alreadySpoke[speakerId].Contains(id);
        }

        public void PlayFixedScript(int index, UnityAction endAction = null)
        {
            _curScript = GetScriptDataType(index);
            SpeakScript(new ScriptData(_curScript), endAction);
        }
        

        public void SelectNpc(UnityAction endAction = null)
        {
            if (myScripts == null) return;

            _curPriority = 0;
            _curScripts.Clear();
            foreach (var value in myScripts)
            {
                if (!IsAlreadySpoke(value.id) && _curPriority <= 0)
                    _curPriority = value.priority;

                if (_curPriority > 0)
                {
                    if (value.priority == _curPriority)
                        _curScripts.Add(value);
                    else
                        break;
                }
            }


            if (_curScripts.Count == 0)
            {
                // 기획상으로는 나타나선 안되는 상황이지만 예외처리.
                SpeakScript(dummyScript, endAction);
                return;
            }

            _curScript = _curScripts[UnityEngine.Random.Range(0, _curScripts.Count)];

            if (_curScript.unique)
            {
                if (alreadySpoke.ContainsKey(speakerId))
                {
                    alreadySpoke[speakerId].Add(_curScript.id);
                }
                else
                {
                    List<int> newList = new();
                    newList.Add(_curScript.id);
                    alreadySpoke.Add(speakerId, newList);
                }

                if (GameManager.Save.currentSlotData != null)
                {
                    GameManager.Save.currentSlotData.NPCData.alreadySpoke = alreadySpoke;
                    GameManager.Save.SaveData(SaveManager.SaveType.Slot);
                }
            }

            SpeakScript(new ScriptData(_curScript), endAction);
        }

        public UI_Dialogue SpeakScript(ScriptData scriptData, UnityAction endAction = null)
        {
            _dialogueUI = GameManager.UI.CreateUI("UI_Dialogue", UIType.Ingame, withoutActivation: true) as UI_Dialogue;
            if (_dialogueUI == null) return null;
            Guid newGuid = GameManager.instance.TryOnGameState(GameStateType.InteractionState);
            _dialogueUI.whenDeactivated = () =>
            {
                if (!isFailed && endAction != null)
                {
                    endAction?.Invoke();
                }

                GameManager.instance.TryOffGameState(GameStateType.InteractionState, newGuid);
            };
            _dialogueUI.SetInfo(transform, this);
            _dialogueUI.SetScripts(scriptData);
            _dialogueUI.TryActivated();

            
            return _dialogueUI;
        }
    }
}