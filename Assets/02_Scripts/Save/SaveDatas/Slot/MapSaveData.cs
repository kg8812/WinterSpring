using System.Collections.Generic;
using chamwhy;
using UnityEngine;

namespace Save.Schema
{
    public class MapSaveData: SlotSaveData
    {
        public Dictionary<int, bool> OpenedMapBox = new();
        /// <summary>
        /// int = mapImgId, bool = 클리어 여부
        /// </summary>
        public Dictionary<int, bool> OpenedMapImg = new();

        
        
        public Dictionary<string, MarkerSaveData> objectFound = new(); // 찾은 오브젝트들
        public Dictionary<int, HashSet<string>> objectObtain = new();

        public Dictionary<int,SectorSaveData.ShelterData> openedShelters = new();
        
        protected override void OnBeforeSave()
        {
            // open된 sector들 중, clear된 애들만 포함.
            // MapBoxGrids = Map.instance.MapBoxGrids;
            
            Map.instance.SaveAllMapImgTexture();
        }

        protected override void BeforeLoaded()
        {
            // Map.instance.MapBoxGrids = MapBoxGrids;
            // Debug.Log("Map save data before Loaded");
            
            Map.instance.InitByData();
            Map.instance.LoadSaveData();
        }

        protected override void OnReset()
        {
            OpenedMapBox = new();
            OpenedMapImg = new();
            objectFound = new();
            objectObtain = new();
            openedShelters = new();
            
            Map.instance.InitByData();
            Map.instance.LoadSaveData();
        }
    }
}