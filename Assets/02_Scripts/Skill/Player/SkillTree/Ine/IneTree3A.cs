using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class IneTree3A : SkillTree
    {
        private InePassiveSkill skill;

        [LabelText("생성 확률")] public float prob;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as InePassiveSkill;

            if (skill == null) return;
            
            skill.OnAfterFire.AddListener(CreateFeather);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnAfterFire.RemoveListener(CreateFeather);
        }

        void CreateFeather()
        {
            float rand = Random.Range(0, 100);

            if (rand <= prob)
            {
                skill.CreateFeather();
            }
        }
    }
}