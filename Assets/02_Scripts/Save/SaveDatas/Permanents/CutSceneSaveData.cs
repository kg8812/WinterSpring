using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save.Schema
{
    public class CutSceneSaveData : ISaveData
    {
        public HashSet<int> OpenedCutSceneList = new();
        
        public void BeforeSave()
        {
        }

        public void OnLoaded()
        {
        }

        public void Initialize()
        {
            OpenedCutSceneList = new();
        }
    }
}