using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class ViichanTree3E : ViichanTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("방패 회복량")] public float shieldAmount;
            [LabelText("야수 회복량")] public float beastAmount;
        }

        public DataStruct[] datas;
        
        private ViichanActiveSkill active;
        private ViichanPassiveSkill passive;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            this.active = active as ViichanActiveSkill;
        }

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            this.passive = passive as ViichanPassiveSkill;
            
            passive?.Player?.RemoveEvent(EventType.OnHitReaction,AddGauge);
            passive?.Player?.AddEvent(EventType.OnHitReaction,AddGauge);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            passive?.Player?.RemoveEvent(EventType.OnHitReaction,AddGauge);
        }

        void AddGauge(EventParameters _)
        {
            if (active != null)
            {
                active.CurGauge += datas[level-1].shieldAmount;
            }

            if (passive != null)
            {
                passive.CurGauge += datas[level-1].beastAmount;
            }
        }
    }
}