using System;
using System.Collections.Generic;
using Apis;
using chamwhy;
using NewNewInvenSpace;
using UnityEngine;

namespace Save.Schema
{
    [Serializable]
    public class InvenSaveData : ISaveData
    {
        public Queue<ItemSaveData> WeaponsEquip = new();
        public Queue<ItemSaveData> WeaponsInven = new();
        public Queue<ItemSaveData> AccEquip = new();
        public Queue<ItemSaveData> AccInven = new();
        public void BeforeSave()
        {
            WeaponsEquip.Clear();
            foreach (Item item in InvenManager.instance.AttackItem.Invens[InvenType.Equipment].Slots)
            {
                if (item == null) continue;
                WeaponsEquip.Enqueue(item.SaveData);
                item.SaveData.OnItemSave();
            }
            
            WeaponsInven.Clear();
            foreach (Item item in InvenManager.instance.AttackItem.Invens[InvenType.Storage].Slots)
            {
                if (item == null) continue;
                WeaponsInven.Enqueue(item.SaveData);
                item.SaveData.OnItemSave();
            }
            
            AccEquip.Clear();
            foreach (Item item in InvenManager.instance.Acc.Invens[InvenType.Equipment].Slots)
            {
                if (item == null) continue;
                AccEquip.Enqueue(item.SaveData);
                item.SaveData.OnItemSave();
            }
            
            AccInven.Clear();
            foreach (Item item in InvenManager.instance.Acc.Invens[InvenType.Storage].Slots)
            {
                if (item == null) continue;
                AccInven.Enqueue(item.SaveData);
                item.SaveData.OnItemSave();
            }
            
            // TODO: attack item hidden inven save?
        }

        public void OnLoaded()
        {
            InvenManager.instance.HardReset();

            while (WeaponsEquip.Count > 0)
            {
                ItemSaveData data = WeaponsEquip.Dequeue();
                Weapon item = GameManager.Item.Weapon.CreateNew(data.ItemId);
                if (item == null)
                {
                    Debug.LogError($"{data.ItemId}이 존재하지않음");
                    continue;
                }
                item.SaveData = data;
                InvenManager.instance.AttackItem.Add(item.SaveData.slotIndex,item, InvenType.Equipment);
            }
            
            while (WeaponsInven.Count > 0)
            {
                ItemSaveData data = WeaponsInven.Dequeue();
                Weapon item = GameManager.Item.Weapon.CreateNew(data.ItemId);
                if (item == null)
                {
                    Debug.LogError($"{data.ItemId}이 존재하지않음");
                    continue;
                }
                item.SaveData = data;
                InvenManager.instance.AttackItem.Add(item.SaveData.slotIndex,item, InvenType.Storage);
            }

            while (AccEquip.Count > 0)
            {
                ItemSaveData data = AccEquip.Dequeue();
                Accessory item = GameManager.Item.Acc.CreateNew(data.ItemId);
                if (item == null)
                {
                    Debug.LogError($"{data.ItemId}이 존재하지않음");
                    continue;
                }
                item.SaveData = data;
                InvenManager.instance.AttackItem.Add(item.SaveData.slotIndex,item, InvenType.Equipment);
            }
           
            while (AccInven.Count > 0)
            {
                ItemSaveData data = AccInven.Dequeue();
                Accessory item = GameManager.Item.Acc.CreateNew(data.ItemId);
                if (item == null)
                {
                    Debug.LogError($"{data.ItemId}이 존재하지않음");
                    continue;
                }
                item.SaveData = data;
                InvenManager.instance.AttackItem.Add(item.SaveData.slotIndex,item, InvenType.Storage);
            }
        }
        
        public void Initialize()
        {
            WeaponsEquip.Clear();
            WeaponsInven.Clear();
            AccEquip.Clear();
            AccInven.Clear();
            InvenManager.instance.HardReset();

        }
    }
}