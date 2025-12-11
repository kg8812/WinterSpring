using System;
using System.Collections.Generic;
using UnityEngine;

namespace Save.Schema
{
    public class CodexData
    {
        public enum CodexType
        {
            Item,Monster, Memory, BackGround , CutScene,Tip
        }

        public CollectionOpenSaveData CollectionOpenSaveData => GameManager.Save.GetData(PersistentDataKeys.DataTypes.CollectionOpened) as CollectionOpenSaveData;
        public CutSceneSaveData CutSceneSaveData => GameManager.Save.GetData(PersistentDataKeys.DataTypes.CutSceneOpened) as CutSceneSaveData;
        public TipSaveData TipSaveData => GameManager.Save.GetData(PersistentDataKeys.DataTypes.TipOpened) as TipSaveData;
        
        public bool IsOpen(CodexType cType, int id)
        {
            HashSet<int> temp = DataInt(cType);
            return temp != null && temp.Contains(id);
        }

        public void UnLock(CodexType cType, int id)
        {
            DataInt(cType)?.Add(id);
            
            GameManager.instance.WhenUnlock.Invoke();
            switch (cType)
            {
                case CodexType.Item:
                case CodexType.Monster:
                case CodexType.Memory:
                case CodexType.BackGround:
                    SaveCollectionData();
                    break;
                case CodexType.CutScene:
                    SaveCutSceneData();
                    break;
                case CodexType.Tip:
                    SaveTipData();
                    break;
            }
        }

        public HashSet<int> DataInt(CodexType type) => type switch
        {
            CodexType.Monster => CollectionOpenSaveData.MonsterList,
            CodexType.Memory => CollectionOpenSaveData.MemoryList,
            CodexType.BackGround => CollectionOpenSaveData.BackgroundList,
            CodexType.Item => CollectionOpenSaveData.ItemList,
            CodexType.CutScene => CutSceneSaveData.OpenedCutSceneList,
            CodexType.Tip => TipSaveData.OpenedTips,
            _ => null,
        };

        public void SaveCollectionData()
        {
            if (CollectionOpenSaveData != null)
            {
                GameManager.Save.SaveData(PersistentDataKeys.GetKey(PersistentDataKeys.DataTypes.CollectionOpened),
                    CollectionOpenSaveData);
            }
        }

        public void SaveCutSceneData()
        {
            if (CutSceneSaveData != null)
            {
                GameManager.Save.SaveData(PersistentDataKeys.GetKey(PersistentDataKeys.DataTypes.CutSceneOpened),
                    CutSceneSaveData);
            }
        }

        public void SaveTipData()
        {
            if (TipSaveData != null)
            {
                GameManager.Save.SaveData(PersistentDataKeys.GetKey(PersistentDataKeys.DataTypes.TipOpened), TipSaveData);
            }
        }
    }
}