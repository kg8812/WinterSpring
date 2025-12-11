using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save.Schema
{
    public class LobbySaveData : SlotSaveData
    {
        public HashSet<int> ConvenienceOpenedList = new HashSet<int>();
        public HashSet<int> LobbyOpenedList = new HashSet<int>();
        
        protected override void OnBeforeSave()
        {
        }

        protected override void BeforeLoaded()
        {
        }

        protected override void OnReset()
        {
            ConvenienceOpenedList = new HashSet<int>();
            LobbyOpenedList = new HashSet<int>();
        }
    }
}