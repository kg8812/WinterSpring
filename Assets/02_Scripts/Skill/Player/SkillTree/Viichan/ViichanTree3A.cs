using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class ViichanTree3A : ViichanTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("방패 회복량(%)")] public float amount;
            [LabelText("추가량 최소 그로기")] public float minGroggy;
            [LabelText("추가 회복량(%)")] public float amount2;
        }

        public DataStruct[] datas;
        
        private ViichanActiveSkill skill;

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active, level);
            skill = active as ViichanActiveSkill;

            if (skill == null) return;

            skill.eventUser.EventManager.AddEvent(EventType.OnBasicAttack, AddGauge);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.eventUser?.EventManager.RemoveEvent(EventType.OnBasicAttack, AddGauge);
        }

        void AddGauge(EventParameters parameters)
        {
            if (skill != null && parameters != null)
            {
                skill.CurGauge += skill.MaxGauge * datas[level-1].amount / 100f;
                if (parameters.atkData.groggyAmount >= datas[level-1].minGroggy)
                {
                    skill.CurGauge += skill.MaxGauge * datas[level-1].amount2 / 100f;
                }
            }
        }
    }
}