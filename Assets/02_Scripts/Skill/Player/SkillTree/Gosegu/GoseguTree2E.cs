using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree2E : SkillTree
    {
        public struct DataStruct
        {
            [LabelText("게이지 증가량")] public float maxGauge;
            [LabelText("게이지당 데미지 증가량(%)")] public float dmgIncrement;
        }

        public DataStruct[] datas;
        private GoseguActiveAttachment attachment;
        private GoseguActiveSkill skill;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as GoseguActiveSkill;

            if (skill != null)
            {
                attachment ??= new GoseguActiveAttachment(new GoseguActiveStat());
                
                attachment.GoseguStat.maxGauge = datas[level-1].maxGauge;
                skill.RemoveAttachment(attachment);
                skill.AddAttachment(attachment);
                skill.OnMechaSpawn.RemoveListener(SetStat);
                skill.OnMechaSpawn.AddListener(SetStat);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.RemoveAttachment(attachment);
            skill?.OnMechaSpawn.RemoveListener(SetStat);
        }

        public void SetStat(SeguMecha mecha)
        {
            attachment.GoseguStat.finalDmgIncrement = Mathf.Clamp(
                skill.GetGaugeDifference() * datas[level - 1].dmgIncrement / 100f, 0,
                skill.MaxGauge * datas[level - 1].dmgIncrement / 100f);
        }
    }
}