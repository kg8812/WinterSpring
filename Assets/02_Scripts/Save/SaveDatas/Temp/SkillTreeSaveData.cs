using System;
using System.Collections;
using System.Collections.Generic;
using Apis;
using Apis.SkillTree;
using chamwhy;
using Default;
using Sirenix.Utilities;
using UnityEngine;

namespace Save.Schema
{
    [Serializable]
    public class SkillTreeSaveData : ISaveData
    {
        [Serializable]
        public struct SkillTreeSlotData
        {
            public int slotIndex;
            public SkillTreeSlot.SlotType slotType;
        }
        public Dictionary<int,SkillTreeSlotData> equippedSkillTrees = new();
        public HashSet<int> activatedSkillTrees = new();

        public void BeforeSave()
        {
            activatedSkillTrees = Utils.DeepCopyHashSet(activatedSkillTrees);
        }

        public void OnLoaded()
        {
            UI_SkillTree skillTreeUI = GameManager.UI.CreateUI("UI_SkillTree", UIType.Scene) as UI_SkillTree;
            skillTreeUI?.SetSlots();
            skillTreeUI?.CloseOwn();
            activatedSkillTrees.ForEach(x =>
            {
                SkillTreeDatas.activatedIndex.Add(x);
            });
        }

        public void Initialize()
        {
            equippedSkillTrees.Clear();
            activatedSkillTrees.Clear();
            SkillTreeDatas.activatedIndex.Clear();
        }
    }
}