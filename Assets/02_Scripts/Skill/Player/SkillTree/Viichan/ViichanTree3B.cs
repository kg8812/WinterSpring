using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class ViichanTree3B : ViichanTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("초당 추가 회복량(%)")] public float amount;
        }

        public DataStruct[] datas;
        
        private ViichanActiveSkill skill;
        private SkillAttachment attachment;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as ViichanActiveSkill;
            attachment ??= new SkillAttachment(new SkillStat());
            attachment.Stat.groggy = datas[level - 1].amount;
            
            if (skill == null) return;
            
            skill.RemoveAttachment(attachment);
            skill.AddAttachment(attachment);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.RemoveAttachment(attachment);
        }
    }
}