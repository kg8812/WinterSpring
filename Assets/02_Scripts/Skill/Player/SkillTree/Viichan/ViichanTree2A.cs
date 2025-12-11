using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class ViichanTree2A : ViichanTree
    {
        public struct DataStruct
        {
            [LabelText("적용 반경")] public float radius;
            [LabelText("그로기 수치")] public int groggy;
        }

        private ViichanActiveSkill skill;

        public DataStruct[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as ViichanActiveSkill;

            if (skill == null) return;

            skill.OnParrying -= AddGroggy;
            skill.OnParrying += AddGroggy;
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill == null) return;
            skill.OnParrying -= AddGroggy;
        }

        void AddGroggy()
        {
            Debug.Log("AddGroggy");
            var targets = skill.Player.gameObject.GetTargetsInCircle(datas[level-1].radius, LayerMasks.Enemy);
            targets.ForEach(x =>
            {
                if (x is Monster monster)
                {
                    monster.AddGroggyGauge(datas[level-1].groggy);
                }
            });
        }
    }
}