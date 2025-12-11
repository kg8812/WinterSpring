using System;
using Apis.SkillTree;
using Save.Schema;

namespace chamwhy.DataType
{
    [Serializable]
    public class SkillTreeDataType
    {
        public int index;
        public PlayerType playerType;
        public TagManager.SkillTreeTag[] tags;
        public SkillTree.TreeTypeEnum treeType;
        public SkillTree.SlotTypeEnum slotType;
        public int name;
        public int description;
        public int[] tagNames;
        
    }
}
