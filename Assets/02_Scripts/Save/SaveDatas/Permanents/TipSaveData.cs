using System.Collections;
using System.Collections.Generic;
using Save.Schema;
using UnityEngine;

namespace Save.Schema
{
    public class TipSaveData : ISaveData
    {
        public HashSet<int> OpenedTips = new();
        public void BeforeSave()
        {
        }

        public void OnLoaded()
        {
        }

        public void Initialize()
        {
            OpenedTips = new();
        }
    }
}