using System.Collections;
using System.Collections.Generic;
using Managers;
using Save.Schema;
using UnityEngine;

namespace Save.Schema
{
    public class GameData
    {
        public GameSaveData Data => GameManager.Save.GetData(PersistentDataKeys.DataTypes.GameData) as GameSaveData;

        public void Save()
        {
            GameManager.Save.SaveData(SaveManager.SaveType.Persistent,PersistentDataKeys.GetKey(PersistentDataKeys.DataTypes.GameData));
        }
    }
}