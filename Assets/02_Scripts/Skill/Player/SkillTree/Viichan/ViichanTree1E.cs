using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class ViichanTree1E : ViichanTree
    {
        [LabelText("초기 연장 시간")] public float startTime;
        [LabelText("타격당 감소량")] public float decrement;
        [LabelText("최소 연장 시간")] public float minTime;

        private ViichanPassiveSkill skill;

        private float curIncrement;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as ViichanPassiveSkill;
            if (skill == null) return;
            
            skill.OnBeastStart -= SetEvent;
            skill.OnBeastEnd -= RemoveEvent;
            skill.OnBeastStart += SetEvent;
            skill.OnBeastEnd += RemoveEvent;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill == null) return;

            skill.OnBeastStart -= SetEvent;
            skill.OnBeastEnd -= RemoveEvent;
        }

        void SetEvent()
        {
            curIncrement = startTime;
            skill.Player.AddEvent(EventType.OnFirstAttack,AddDuration);
        }

        void RemoveEvent()
        {
            skill.Player.RemoveEvent(EventType.OnFirstAttack,AddDuration);
        }
        void AddDuration(EventParameters _)
        {
            skill.IncreaseBeastDuration(curIncrement);
            curIncrement = Mathf.Clamp(curIncrement - decrement, minTime, startTime);
        }
    }
}