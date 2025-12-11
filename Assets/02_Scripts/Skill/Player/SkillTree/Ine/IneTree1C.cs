using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree1C : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("깃털 개수 증가량")] public int count;
        }

        private InePassiveSkill skill;

        private InePassiveAttachment attachment;

        public Data[] datas;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as InePassiveSkill;
            attachment ??= new InePassiveAttachment(new InePassiveStat());
            attachment.IneStat.maxFeather = datas[level - 1].count;

            skill?.RemoveAttachment(attachment);
            skill?.AddAttachment(attachment);
            skill?.ResetFeatherPositions();
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveAttachment(attachment);
            skill?.ResetFeatherPositions();
        }
    }
}