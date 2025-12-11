using System.Collections;
using System.Collections.Generic;
using chamwhy.Components;
using Cinemachine;
using Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace Directing {
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneObject : MonoBehaviour
    {
        [SerializeField] private TimeLineId timeLineId;
        public TimeLineId TimelineAsset => timeLineId;
        private Director director;
        private PlayableDirector localDirector;
        public PlayableDirector LocalDirector => localDirector;
        [SerializeField] private Trigger trigger;
        [SerializeField] private PlayerTimelineDummy dummy;
        public PlayerTimelineDummy Dummy => dummy;
        [SerializeField] private CinemachineVirtualCamera[] cams;

        public void Start()
        {
            director = Director.instance;
            localDirector = GetComponent<PlayableDirector>();

            director.AddCutsceneObject(TimelineAsset.id, this);

            trigger.enabled = timeLineId.byTrigger;
            CameraManager.instance.AddCutsceneCams(timeLineId.id, cams);
        }

        public void ShowDialoueForCutscene(DialogueDirector dir)
        {
            director.ShowDialogueForCutScene(dir);
        }
    }
}
