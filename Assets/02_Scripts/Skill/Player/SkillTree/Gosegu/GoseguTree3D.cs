using chamwhy.DataType;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class GoseguTree3D : SkillTree
    {
        public struct Data
        {
            [LabelText("쉴드량 (%)")] public float shield;
            [LabelText("지속시간 (0일시 무한)")] public float duration;
        }

        private Buff buff;

        private GoseguActiveSkill skill;
        public Data[] datas;

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active, level);
            skill = active as GoseguActiveSkill;
            skill?.OnMechaSpawn.AddListener(AddShield);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnMechaSpawn.RemoveListener(AddShield);
            buff?.RemoveBuff(new EventParameters(skill?.Mecha));
        }

        void AddShield(SeguMecha mecha)
        {
            BuffDataType data = new(SubBuffType.Buff_Barrier)
            {
                buffCategory = 1, buffDispellType = 1, applyType = 0,
                buffPower = new[] { datas[level - 1].shield }, buffDuration = datas[level - 1].duration,
                buffMaxStack = 1, showIcon = false,
            };
            if (buff == null)
            {
                buff = new Buff(data, mecha);
            }
            else
            {
                buff.SetData(data);
            }

            buff?.AddSubBuff(mecha, new EventParameters(mecha));
        }
    }
}