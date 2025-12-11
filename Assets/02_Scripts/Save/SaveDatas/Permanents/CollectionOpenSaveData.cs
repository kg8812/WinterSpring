using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save.Schema
{
    public class CollectionOpenSaveData : ISaveData
    {
        // 저장해야돼서 Readonly 하면 안됨
        public HashSet<int> ItemList = new();
        public HashSet<int> MonsterList = new();
        public HashSet<int> MemoryList = new();
        public HashSet<int> BackgroundList = new();
        
        public void BeforeSave()
        {
        }

        public void OnLoaded()
        {
        }

        public void Initialize()
        {
            ItemList = new();
            MonsterList = new();
            MemoryList = new();
            BackgroundList = new();
        }
    }
}