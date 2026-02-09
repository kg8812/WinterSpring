using System;
using System.Collections.Generic;
using System.Linq;
using chamwhy.DataType;
using chamwhy.Managers;
using UI;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Spine.Unity.Playables;
using Default;
using Spine.Unity;

namespace Directing
{
    [Serializable]
    public struct TimeLineId
    {
        public int id;
        public TimelineAsset tl;
        public bool byTrigger;
    }
    [RequireComponent(typeof(PlayableDirector))]
    public class Director : SingletonPersistent<Director>, IController
    {
        public static bool IsPlaying;
        private PlayableDirector _playableDirector;
        public TimeLineId[] timelineAssets;
        public CutsceneObject[] CutsceneObjects;

        private Dictionary<int, TimelineAsset> _timelineAssets;
        private Dictionary<int, CutsceneObject> _cutsceneObject;
        public UnityAction EndAction;
        public UnityAction EndUIAction;
        private int curInGameCutSceneId;

        public static IController DialogueController;

        private int _inGameCutSceneScriptIndex;
        
        HashSet<int> _shownCutScene;

        public HashSet<int> shownCutScenes
        {
            get
            {
                return _shownCutScene ??= new();
            }
            set
            {
                _shownCutScene = value;
            }
        }

        public void AddCutSceneId(int id)
        {
            shownCutScenes.Add(id);
        }
        protected override void Awake()
        {
            base.Awake();
            _timelineAssets = new();
            _cutsceneObject = new();
            foreach (var value in timelineAssets)
            {
                _timelineAssets.Add(value.id, value.tl);
            }
            foreach (var obj in CutsceneObjects)
            {
                _cutsceneObject.Add(obj.TimelineAsset.id, obj);
            }

            _playableDirector = GetComponent<PlayableDirector>();
            SetCinemachineBrain(_playableDirector);
            // _dialogueDirectors = FindObjectsByType<DialogueDirector>(FindObjectsSortMode.None).ToDictionary(x => x.speakerId, x => x);

            // DontDestroyOnLoad(this);
        }


        public void AddCutsceneObject(int id, CutsceneObject obj)
        {
            if (_cutsceneObject.ContainsKey(id))
            {

                _cutsceneObject[id] = obj;
            }
            else
            {
                _cutsceneObject.Add(id, obj);
            }

        }

        private void OnEnable()
        {
            _playableDirector.stopped += OnTimeLineStopped;
        }

        private void OnDisable()
        {
            _playableDirector.stopped -= OnTimeLineStopped;
        }

        private void OnTimeLineStopped(PlayableDirector _)
        {
            if (GameManager.IsQuitting) return;
            LetterBoxer.instance.ToggleLetterBox(false, onStarted: () =>
            {
                EndAction?.Invoke();
                if (GameManager.Scene.CurSceneData.isPlayerMustExist)
                {
                    GameManager.UI.ToggleMainUI(true);
                }

                // TODO: 배틀 도중 interaction으로 들어갈 수 있기 때문에 거시기
                GameManager.UI.RemoveController(this);
            });
        }

        public void ShowDialogueForCutScene(DialogueDirector dialogueDirector)
        {
            if (InGameCutSceneModel.igDict.TryGetValue(curInGameCutSceneId, out var value))
            {
                if (value.Count <= _inGameCutSceneScriptIndex)
                {
                    Debug.LogError("테이블에 입력되어있는 인게임 컷신 스크립트 보다 타임라인에서 호출하는 시그널의 개수가 많음");
                }
                else
                {
                    InGameCutSceneDataType curData = value[_inGameCutSceneScriptIndex];
                    if (dialogueDirector.speakerId == curData.speaker)
                    {
                        if(!curData.isAuto)
                            _playableDirector.Pause();
                        _inGameCutSceneScriptIndex++;
                        UI_Dialogue ui = dialogueDirector.SpeakScript(new ScriptData(curData), curData.isAuto?null: () =>
                        {
                            _playableDirector.Resume();
                        });
                        void CloseUi()
                        {
                            ui.CloseOwnForce();
                            EndUIAction -= CloseUi;
                        }
                        EndUIAction += CloseUi;
                    }
                    else
                    {
                        Debug.LogError($"현재 씬에 {curData.speaker}의 dialogue director가 존재하지 않습니다.\nCutscene Object의 signal reciever에 잘못된 dialogue director가 할당됐을 수 있습니다.");
                    }
                }
            }
        }


        public void PlayInGameCutScene(int ingameId, UnityAction onEnd = null)
        {
            if (shownCutScenes.Contains(ingameId))
            {
                Debug.Log("Already Shown Cutscene");
                return;
            }
            // Debug.Log($"time line length: {_timelineAssets.Count}");
            if (_cutsceneObject.TryGetValue(ingameId, out var value))
            {
                curInGameCutSceneId = ingameId;
                _inGameCutSceneScriptIndex = 0;

                _playableDirector.playableAsset = value.LocalDirector.playableAsset;
                SyncBinding(value.LocalDirector, _playableDirector);
                GameManager.UI.RegisterUIController(this);
                GameManager.UI.ToggleMainUI(false);

                void play(EventParameters info)
                {
                    LetterBoxer.instance.ToggleLetterBox(true, onStarted: () =>
                    {
                        _playableDirector.Play();
                    });
                }

                // 일단 씬에 플레이어가 항상 존재한다고 가정
                Player p = GameManager.instance.Player;

                if(p == null) Debug.LogWarning("Unable to play cutscene: Player does not exist in the scene (Contact: Sucrose)");

                BindPlayerToTimeline(p);
                SetCinemachineBrain(_playableDirector);

                p.AddInfo(EPlayerState.CutScene, new CutsceneInfo(){
                    Dummy = value.Dummy
                });

                p.StateEvent.AddEvent(EventType.OnCutScene, play);

                EndAction += onEnd;
                EndAction += () => {
                    EndAction = null;
                    p.StateEvent.ExecuteEventOnce(EventType.OnCutSceneEnd, null);
                };

                p.SetState(EPlayerState.CutScene);
            }
        }
        
        // ingame cutscene 
        public void KeyControl()
        {
            if (InputManager.GetKeyDown(KeySettingManager.GetGameKeyCode(Define.GameKey.Escape)))
            {
                _playableDirector.Pause();
                SystemManager.SystemCheck(LanguageManager.Str(10131105), isYes =>
                {
                    if (isYes)
                    {
                        EndUIAction?.Invoke();
                        _playableDirector.Stop();
                    }
                    else
                    {
                        _playableDirector.Resume();
                    }
                });
            }
            else
            {
                DialogueController?.KeyControl();
            }
        }

        public void GamePadControl()
        {
            if (InputManager.GetButtonDown(KeySettingManager.GetGameButton(Define.GameKey.Escape)))
            {
                _playableDirector.Pause();
                SystemManager.SystemCheck(LanguageManager.Str(10131105), isYes =>
                {
                    if (isYes)
                    {
                        Skip();
                    }
                    else
                    {
                        _playableDirector.Resume();
                    }
                });
            }
            else
            {
                DialogueController?.GamePadControl();
            }
        }

        private void Skip()
        {
            _playableDirector.time = _playableDirector.duration;
            _playableDirector.Evaluate();
            _playableDirector.Play();     // <- 이벤트 강제 발동용 (Play 후 바로 Stop)
            _playableDirector.time = _playableDirector.duration;
            _playableDirector.Evaluate();
            _playableDirector.Stop();
        }

        public void SetCinemachineBrain(PlayableDirector director)
        {
            if(director.playableAsset == null) return;

            foreach (var output in director.playableAsset.outputs)
            {
                // Debug.Log(output.sourceObject.GetType() );
                if (output.streamName == "Cinemachine Track" )
                {
                    director.SetGenericBinding(output.sourceObject, CameraManager.instance.CineBrain);
           
                    var cinemachineTrack = output.sourceObject as CinemachineTrack;
                    var cutsceneCams = CameraManager.instance.GetCutsceneCams(curInGameCutSceneId);
                    foreach( var clip in cinemachineTrack.GetClips() ){
                        var cinemachineShot = clip.asset as CinemachineShot;
                        if (clip.displayName == "playerCam")
                        {
                            // Debug.LogError("clip is player Cam");
                            director.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, CameraManager.instance.PlayerCam);
                        }
                        else if(cutsceneCams != null && clip.displayName.Contains("CM vcam"))
                        {
                            int idx = int.Parse(clip.displayName[7..]) - 1;
                            if(idx < cutsceneCams.Length) 
                                director.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, cutsceneCams[idx]);
                        }
 
                    }
                }
            }
        }

        private readonly float fadingDuration = 1f;
        public void OnPlayVideoCutScene(int idx)
        {
            Debug.Log("Recieved signal" + " " + idx);

            PauseCutScene();

            // TODO: 사운드도 끄는 등등 용 임시 이벤트
            UnityEvent onPlayVideo = new();

            var fade = FadeManager.instance;


            void play()
            {
                var video = GameManager.Directing;
                onPlayVideo.Invoke();
                video.ShowVideoCutScene(idx, ResumeCutScene);
            }

            fade.Fading(play, null, fadingDuration);
        }
        
        private bool isPaused = false;
        private void PauseCutScene()
        {
            if (!_playableDirector.playableGraph.IsValid() || isPaused) return;

            isPaused = true;
            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        private void ResumeCutScene()
        {
            if (!_playableDirector.playableGraph.IsValid() || !isPaused) return;

            isPaused = false;
            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        private void SyncBinding(PlayableDirector src, PlayableDirector dst)
        {
            foreach(var track in src.playableAsset.outputs)
            {
                // 무조건 CutsceneObject binding 기준으로 sync
                dst.SetGenericBinding(track.sourceObject, src.GetGenericBinding(track.sourceObject));
            }
        }

        public void BindPlayerToTimeline(Player player)
        {
            if(player == null || _playableDirector == null)
                return;

            foreach (var tracks in ((TimelineAsset)_playableDirector.playableAsset).GetOutputTracks())
            {
                if (tracks is PlayerSkeletonAnimationTrack animTrack)
                {
                    foreach(var track in tracks.outputs)
                    {
                        var tmp = _playableDirector.GetGenericBinding(track.sourceObject);
                        _playableDirector.SetGenericBinding(track.sourceObject, player.CutsceneSkeleton.Skeleton);

                        EndAction += () => { _playableDirector.SetGenericBinding(track.sourceObject, tmp); };
                    }
                    
                    foreach(TimelineClip clip in animTrack.GetClips())
                    {
                        SpineAnimationStateClip spineClip = clip.asset as SpineAnimationStateClip;
                        
                        if(spineClip == null) continue;

                        var tmp = spineClip.template.animationReference;

                        string name = spineClip.template.animationReference.name;

                        spineClip.template.animationReference = ResourceUtil.Load<AnimationReferenceAsset>($"{player.playerType}_{name}_ref");

                        EndAction += () => { spineClip.template.animationReference = tmp; };
                    }
                }
                else
                {
                    foreach(var track in tracks.outputs)
                    {
                        // 트랙에 binding된 object
                        var obj = _playableDirector.GetGenericBinding(track.sourceObject);

                        // Player 관련된게 아니면 continue
                        if(obj is not Component comp) continue;

                        var go = comp.gameObject;

                        if(go.GetComponent<PlayerTimelineDummy>() == null && go.GetComponent<PlayerCutsceneSkeleton>() == null) continue;
                        
                        // 플레이어(컷씬)한테 없는 object면 continue
                        var pComp = player.CutsceneSkeleton.GetComponent(track.outputTargetType);

                        if(pComp == null) continue;

                        // binding object 변경
                        _playableDirector.SetGenericBinding(track.sourceObject, pComp);

                        // cutscene 종료 시 다시 더미로 교체 => 아니면 인스펙터에서 다른 플레이어 스켈레톤일 때 경고 나옴
                        EndAction += () => { _playableDirector.SetGenericBinding(track.sourceObject, obj); };
                    }
                }
            }
        }
    }
}