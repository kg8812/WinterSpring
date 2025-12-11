using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class JururuTree2A : SkillTree
    {
        private JururuActiveSkill skill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("사이즈 증가량(%)")] public float size;
        }

        public DataStruct[] datas;
        
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as JururuActiveSkill;
            if (skill == null) return;

            skill.SetSize((100 + datas[level-1].size) / 100f);
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (skill != null)
            {
                skill.SetSize(1);
            }
        }
    }
}