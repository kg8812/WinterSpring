using System;
using System.Collections.Generic;
using chamwhy.Components;
using UnityEngine;

namespace Save.Schema
{
    public class SectorSaveData : SlotSaveData
    {
        [Serializable]
        public struct ShelterData
        {
            public string sceneName;
            public Vector2 savedPosition;
        }

        
        public int currentSector = -1;
        public HashSet<string> monsterKilled = new(); // 몬스터 처치여부
        public HashSet<string> objectObtained = new(); // 오브젝트 획득여부
        public ShelterData lastSavedPosition;

        protected override void OnBeforeSave()
        {
            GameManager.Save.currentSlotData.InfoData.PlayerType = GameManager.instance.Player.playerType;
            currentSector = GameManager.SectorMag.CurrentSectors[SectorManager.SectorType.MainWorld];
        }

        protected override void BeforeLoaded()
        {
            if (currentSector > 0)
            {
                GameManager.SectorMag.CurrentSectors[SectorManager.SectorType.MainWorld] = currentSector;
            }

            if (!string.IsNullOrEmpty(lastSavedPosition.sceneName))
            {
                SpawnPoint.spawnPos.Enqueue(lastSavedPosition.savedPosition);
            }
        }

        protected override void OnReset()
        {
            GameManager.SectorMag.CurrentSectors[SectorManager.SectorType.MainWorld] = GameManager.SectorMag.SectorBaseData.startIndex;
        }
    }
}