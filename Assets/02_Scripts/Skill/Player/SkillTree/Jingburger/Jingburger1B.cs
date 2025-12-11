using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class Jingburger1B : SkillTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("관통 추가횟수")] public int count;
        }

        public DataStruct[] datas;

        private JingburgerPassiveSkill skill;
        
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as JingburgerPassiveSkill;

            if (skill == null) return;

            skill.OnFire -= AddPenetration;
            skill.OnFire += AddPenetration;
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.OnFire -= AddPenetration;
        }

        void AddPenetration(JingPaint paint)
        {
            paint.targetConflictType = ProjectileConflictType.Penetrate;
            paint.penetrationMax += datas[level-1].count;
            Debug.Log(paint.penetrationMax);
        }
    }
}