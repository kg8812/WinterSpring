using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class Jingburger3A : SkillTree
    {
        private SkillAttachment attachment;
        private JingburgerActiveSkill skill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("스택 증가치")] public int stack;
        }

        public DataStruct[] datas;

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active, level);
            skill = active as JingburgerActiveSkill;

            attachment ??= new SkillAttachment(new SkillStat());
            attachment.Stat.maxStack = datas[level - 1].stack;
            if (skill == null) return;

            skill.RemoveAttachment(attachment);
            skill.AddAttachment(attachment);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.RemoveAttachment(attachment);
            if (skill.CurStack > 1)
            {
                skill.CurStack = 1;
            }
        }
    }
}