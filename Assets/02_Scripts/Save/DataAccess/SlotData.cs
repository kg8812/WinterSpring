using System.Collections;
using System.Collections.Generic;
using Managers;
using Save.Schema;
using UnityEngine;

namespace Save.Schema
{
    public abstract class SlotSaveData : ISaveData
    {
        public string SlotId;

        public void BeforeSave()
        {
            if (SlotId == GameManager.Save.currentSlotData.slotId)
            {
                OnBeforeSave();
            }
        }

        public void OnLoaded()
        {
            // Debug.Log($"slot data on loaded {SlotId == GameManager.Save.currentSlotData.slotId}");
            if (SlotId == GameManager.Save.currentSlotData.slotId)
            {
                BeforeLoaded();
            }
        }

        public void Initialize()
        {
            if (SlotId == GameManager.Save.currentSlotData.slotId)
            {
                OnReset();
            }
        }

        protected abstract void OnBeforeSave();
        protected abstract void BeforeLoaded();
        protected abstract void OnReset();
    }

    public class SlotData
    {
        public readonly string slotId;

        public SlotData(string slotID)
        {
            GameManager.Save.SetSlotData(slotID);
            slotId = slotID;
            InfoData.SlotId = slotID;
            GrowthSaveData.SlotId = slotID;
            NPCData.SlotId = slotID;
            LobbyData.SlotId = slotID;
            TempSaveData.SlotId = slotID;
            SectorSaveData.SlotId = slotID;
            TaskSaveData.SlotId = slotID;
            MapSaveData.SlotId = slotID;
        }

        public SlotInfoSaveData InfoData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.SlotInfo, slotId) as SlotInfoSaveData;

        public PermanentGrowthSaveData GrowthSaveData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.Growth, slotId) as PermanentGrowthSaveData;

        public NPCSaveData NPCData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.NPC, slotId) as NPCSaveData;

        public LobbySaveData LobbyData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.Lobby, slotId) as LobbySaveData;

        public TempSaveData TempSaveData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.Temp, slotId) as TempSaveData;

        public SectorSaveData SectorSaveData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.Sector, slotId) as SectorSaveData;

        public TaskSaveData TaskSaveData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.Task, slotId) as TaskSaveData;
        
        public MapSaveData MapSaveData =>
            GameManager.Save.GetData(SlotDataKeys.DataTypes.Map, slotId) as MapSaveData;
        
        public void UpdateSlotDataToGameData()
        {
            int index = -1;
            var slotDatas = DataAccess.GameData.Data.SlotDatas;

            for (int i = 0; i < slotDatas.Count; i++)
            {
                if (slotDatas[i].SlotId == slotId)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                DataAccess.GameData.Data.SlotDatas.Add(InfoData);
            }
            else
            {
                DataAccess.GameData.Data.SlotDatas[index] = InfoData;
            }

            DataAccess.GameData.Save();
        }
    }
}