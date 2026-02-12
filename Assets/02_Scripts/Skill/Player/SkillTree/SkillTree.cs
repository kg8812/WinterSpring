using chamwhy.Managers;
using Save.Schema;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apis.SkillTree
{
    public abstract class SkillTree : SerializedScriptableObject,ISkillVisitor
    {
        [SerializeField] [LabelText("인덱스")] int index;
        public Sprite icon;
        private string _name;
        private string lowDesc;
        private string highDesc;
        private PlayerType _playerType;
        private TreeTypeEnum treeType;
        private TagManager.SkillTreeTag[] tags;
        private int[] tagNames;
        private SlotTypeEnum _slotType;
        
        protected int level;
        public int Level => level;
        public enum TreeTypeEnum
        {
            Active,Passive,Support
        }

        public enum SlotTypeEnum
        {
            Low,Medium,High
        }
        public SlotTypeEnum SlotType => _slotType;
        
        public string Name => _name;
        public string HighDescription => highDesc;
        public string LowDescription => lowDesc;
        public int Index => index;
        public PlayerType PlayerType => _playerType;
        public TreeTypeEnum TreeType => treeType;
        public TagManager.SkillTreeTag[] Tags => tags;
        public int[] TagNames => tagNames;
        
        // 호출은 액티브 -> 패시브 순으로 호출됨.

        public virtual void Activate(PlayerActiveSkill active, int level)
        {
            this.level = level;
        }

        public virtual void Activate(PlayerPassiveSkill passive, int level)
        {
            this.level = level;
        }

        public virtual void DeActivate()
        {
            level = 0;
        }
        public virtual void Init()
        {
            if (SkillTreeDatas.TryGetSkillTreeData(index, out var data))
            {
                _name = LanguageManager.Str(data.name);
                lowDesc = LanguageManager.Str(data.lowDescription);
                highDesc = LanguageManager.Str(data.highDescription);
                _playerType = data.playerType;
                treeType = data.treeType;
                tags = data.tags;
                tagNames = data.tagNames;
                _slotType = data.slotType;

                level = 0;
            }
        }

        public bool CheckEquipable(SkillTreeSlot slot)
        {
            if (SlotType == SlotTypeEnum.Medium) return true;
            
            switch (slot.slotType)
            {
                case SkillTreeSlot.SlotType.Low:
                    return SlotType == SlotTypeEnum.Low;
                case SkillTreeSlot.SlotType.High:
                    return SlotType == SlotTypeEnum.High;
                case SkillTreeSlot.SlotType.Inven:
                    return true;
            }

            return false;
        }

        public bool CheckSlotIndex(SkillTreeSlot slot)
        {
            return slot.slotType != SkillTreeSlot.SlotType.Inven || slot.index == (Index % 100 - 1);
        }
    }
}