using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger3B : SkillTree
    {
        public struct DataStruct
        {
            [LabelText("쿨타임 감소치")] public float cdReduce;
        }

        private JingburgerActiveSkill skill;
        public DataStruct[] datas;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as JingburgerActiveSkill;
            if (skill == null) return;
            skill.OnAttack.RemoveListener(Reduce);
            skill.OnAttack.AddListener(Reduce);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnAttack.RemoveListener(Reduce);
        }

        void Reduce(EventParameters parameters)
        {
            skill.CurCd -= datas[level-1].cdReduce;
        }

        private float curTime;
        private float time;
    }
}