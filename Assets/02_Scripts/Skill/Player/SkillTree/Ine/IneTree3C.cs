using System;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class IneTree3C : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("마나 증가량")] public int manaIncrement;
        }
        private IneActiveSkill skill;
        
        private IneActiveAttachment attachment;
        public Data[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as IneActiveSkill;
            attachment ??= new IneActiveAttachment(new IneActiveStat());

            attachment.IneStat.maxMana = datas[level - 1].manaIncrement;
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