using System.Collections;
using System.Collections.Generic;
using Save.Schema;
using UnityEngine;

namespace Save.Schema
{
    public class ItemOpenedSaveData : ISaveData
    {
        public HashSet<int> OpenedItemList = new();
        
        public void BeforeSave()
        {
        }

        public void OnLoaded()
        {
        }

        public void Initialize()
        {
            OpenedItemList = new();
        }
    }
}