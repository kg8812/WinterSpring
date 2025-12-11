
using System;
using Managers;

namespace Save.Schema
{
    [Serializable]
    public class TempSaveData : SlotSaveData
    {
        public PlayerSaveData playerData = new();
        public InvenSaveData InvenData = new();
        public SkillTreeSaveData SkillTreeData = new();

        protected override void OnBeforeSave()
        {
            playerData.BeforeSave();
            InvenData.BeforeSave();
            SkillTreeData.BeforeSave();
        }

        protected override void BeforeLoaded()
        {
            playerData.OnLoaded();
            InvenData.OnLoaded();
            SkillTreeData.OnLoaded();
        }

        protected override void OnReset()
        {
            playerData.Initialize();
            InvenData.Initialize();
            SkillTreeData.Initialize();
        }
    }
}