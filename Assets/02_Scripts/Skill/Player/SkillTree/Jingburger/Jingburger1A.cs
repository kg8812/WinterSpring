using System;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger1A : SkillTree
    {
        [Serializable]
        public struct DataStruct
        {
            [LabelText("발사 개수 증가량")] public int count;
        }

        private JingburgerPassiveSkill skill;
        private JingPassiveAttachment attachment;
        
        public DataStruct[] datas;
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as JingburgerPassiveSkill;

            attachment ??= new JingPassiveAttachment(new JingPassiveStat());

            attachment.JingStat.count = datas[level - 1].count;
            if (skill != null)
            {
                skill.RemoveAttachment(attachment);
                skill.AddAttachment(attachment);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.RemoveAttachment(attachment);
        }
    }
}