using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class JururuTree3D : SkillTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("토큰 추가 획득량")] public int count;
        }

        public DataStruct[] datas;
        
        private SkillAttachment attachment;
        private JururuPassiveSkill skill;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as JururuPassiveSkill;

            if (skill == null) return;

            attachment ??= new(new SkillStat());
            attachment.Stat.stackGain = datas[level - 1].count;
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