using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class JururuTree3A : SkillTree
    {
        private SkillAttachment attachment;
        private JururuActiveSkill skill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("지속시간 증가치")] public float duration;
        }

        public DataStruct[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JururuActiveSkill;

            if (skill == null) return;

            attachment ??= new SkillAttachment(new SkillStat());
            attachment.Stat.duration = datas[level - 1].duration;
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