using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Default;
using Directing;
using UnityEngine;

namespace Save.Schema
{
    public class NPCSaveData : SlotSaveData
    {
        public Dictionary<int,List<int>> alreadySpoke = new Dictionary<int,List<int>>();
        public HashSet<int> shownCutScene = new HashSet<int>();
        
        protected override void OnBeforeSave()
        {
            alreadySpoke = Utils.DeepCopyDictionary(DialogueDirector.alreadySpoke, value => value.ToList());
            shownCutScene = Utils.DeepCopyHashSet(Director.instance.shownCutScenes);
        }

        protected override void BeforeLoaded()
        {
            DialogueDirector.alreadySpoke = Utils.DeepCopyDictionary(alreadySpoke, value => value.ToList());
            Director.instance.shownCutScenes = new HashSet<int>(shownCutScene);
        }

        protected override void OnReset()
        {
            alreadySpoke = new();
            shownCutScene = new();
            DialogueDirector.alreadySpoke = Utils.DeepCopyDictionary(alreadySpoke, value => value.ToList());
            Director.instance.shownCutScenes = new HashSet<int>(shownCutScene);
        }
    }
}