using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save.Schema
{
    public class TaskSaveData : SlotSaveData
    {
        public HashSet<int> Tasks = new();
        protected override void OnBeforeSave()
        {
        }

        protected override void BeforeLoaded()
        {
        }

        protected override void OnReset()
        {
            Tasks = new();
        }
    }
}