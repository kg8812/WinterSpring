using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class JururuTree3C : SkillTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("토큰 증가량")] public int count;
        }
        private SkillAttachment attachment;
        private JururuPassiveSkill skill;

        public DataStruct[] datas;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as JururuPassiveSkill;

            if (skill == null) return;

            attachment ??= new(new SkillStat());
            attachment.Stat.maxStack = datas[level - 1].count;
            
            skill.RemoveAttachment(attachment);
            skill.AddAttachment(attachment);
            skill.OnStackChange.Invoke(skill.CurStack);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveAttachment(attachment);
        }
    }
}