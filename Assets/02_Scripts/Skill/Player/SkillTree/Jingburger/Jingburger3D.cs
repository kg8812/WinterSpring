using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger3D : SkillTree
    {
        private JingburgerPassiveSkill skill;

        public struct DataStruct
        {
            [LabelText("횟수 감소량")] public int count;
        }

        public DataStruct[] datas;
        private JingPassiveAttachment attachment;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);

            skill = passive as JingburgerPassiveSkill;

            if (attachment == null)
            {
                attachment = new JingPassiveAttachment(new JingPassiveStat());
                attachment.JingStat._maxStack = -datas[level - 1].count;
            }
            if (skill == null) return;
            skill.AddAttachment(attachment);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill.RemoveAttachment(attachment);
        }
    }
}