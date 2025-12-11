using System;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class GoseguTree3C : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("게이지 수급 증가량 (%)")] public float gaugeGainRatio;
        }

        public Data[] datas;
        private GoseguActiveAttachment attachment;

        private GoseguActiveSkill skill;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as GoseguActiveSkill;

            if (skill == null) return;

            attachment ??= new GoseguActiveAttachment(new GoseguActiveStat()
            {
                gaugeGainRatio = datas[level-1].gaugeGainRatio
            });
            skill.RemoveAttachment(attachment);
            skill.AddAttachment(attachment);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.RemoveAttachment(attachment);
        }
    }
}