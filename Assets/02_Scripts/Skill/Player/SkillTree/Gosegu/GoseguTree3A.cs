using System;
using chamwhy;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Apis.SkillTree
{
    public class GoseguTree3A : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("냉기 확률")] public float prob;
        }
        private GoseguPassive skill;
        public Data[] datas;
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as GoseguPassive;

            if (skill == null) return;

            skill.OnDroneAttack -= ApplyDebuff;
            skill.OnDroneAttack += ApplyDebuff;
        }

        public override void DeActivate()
        {
            base.DeActivate();
            if (skill != null)
            {
                skill.OnDroneAttack -= ApplyDebuff;
            }
        }

        void ApplyDebuff(AttackObject atk)
        {
            atk?.AddEventUntilInitOrDestroy(AddChill);
        }
        void AddChill(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                float rand = Random.Range(0, 100f);

                if (rand <= datas[level-1].prob)
                {
                    target.AddSubBuff(skill.eventUser,SubBuffType.Debuff_Chill);
                }
            }
        }
    }
}