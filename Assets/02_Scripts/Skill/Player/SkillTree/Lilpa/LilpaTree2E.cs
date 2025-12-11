using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree2E : SkillTree
    {
        [System.Serializable]
        public struct GroggyInfo
        {
            [LabelText("최소 그로기 수치")] public int groggy;
            [LabelText("쿨타임 감소량")] public float cdReduce;
        }

        
        [LabelText("쿨감 설정")] public List<GroggyInfo> groggyInfos;
        
        private IEventUser user;
        private LilpaActiveSkill skill;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);

            if (active == null) return;
            
            skill = active as LilpaActiveSkill;
            user = active.eventUser;
            user.EventManager.AddEvent(EventType.OnBasicAttack,ReduceCd);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            user.EventManager.RemoveEvent(EventType.OnBasicAttack,ReduceCd);
        }

        void ReduceCd(EventParameters parameters)
        {
            if (skill.IsToggleOn) return;
            
            float reduce = 0;
            
            for (int i = 0; i < groggyInfos.Count; i++)
            {
                if (parameters.atkData.groggyAmount >= groggyInfos[i].groggy)
                {
                    reduce = groggyInfos[i].cdReduce;
                }
            }

            skill.lilpaWeapons.Values.ForEach(x =>
            {
                x.Skill.CurCd -= reduce;
            });
        }
    }
}