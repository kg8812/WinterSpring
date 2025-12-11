using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class ViichanTree3D : ViichanTree
    {
        private ViichanPassiveSkill skill;

        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("회복량 (잃은 체력 비례 %)")] public float amount;
        }

        public DataStruct[] datas;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as ViichanPassiveSkill;

            if (skill == null) return;

            skill.OnBeastStart -= Heal;
            skill.OnBeastStart += Heal;
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill.OnBeastStart -= Heal;
        }

        void Heal()
        {
            if (skill?.Player != null)
            {
                skill.Player.CurHp += (skill.Player.MaxHp - skill.Player.CurHp) * datas[level-1].amount / 100f;
            }
        }
    }
}