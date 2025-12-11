using System;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class IneTree3B : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("주기 감소량")] public float frequency;
        }

        public Data[] datas;
        
        private InePassiveAttachment attachment;

        private InePassiveSkill skill;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            attachment ??= new InePassiveAttachment(new InePassiveStat());
            attachment.IneStat.frequency = -datas[level - 1].frequency;
            skill = passive as InePassiveSkill;
            skill?.AddAttachment(attachment);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveAttachment(attachment);
        }
    }
}