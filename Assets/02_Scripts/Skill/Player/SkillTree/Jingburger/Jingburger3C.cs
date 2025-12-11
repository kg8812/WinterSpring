using chamwhy.DataType;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger3C : SkillTree
    {
        public struct DataStruct
        {
            [LabelText("공격속도 상승량")] public float amount;
            [LabelText("지속시간")] public float duration;
            [LabelText("최대 스택")] public int maxStack;
        }

        public DataStruct[] datas;
        
        private JingburgerPassiveSkill skill;
        private BonusStat stat;


        private Buff _buff;

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill = passive as JingburgerPassiveSkill;
            BuffDataType data = new(SubBuffType.Buff_AtkSpeed)
            {
                buffPower = new[]{datas[level-1].amount}, buffCategory = 1,
                buffDuration = datas[level-1].duration,
                buffDispellType = 1, buffMaxStack = datas[level-1].maxStack, stackDecrease = 1, valueType = ValueType.Value,
                showIcon = true, buffIconPath = "BuffIcon_atkUp", buffName = 7017, buffDesc = 8017
            };
            if (_buff == null)
            {
                _buff = new(data, skill?.eventUser);
            }
            else
            {
                _buff.SetData(data);
            }

            if (skill == null) return;
            skill.OnAnimalFire -= AddStat;
            skill.OnAnimalFire += AddStat;
        }
        
        public override void DeActivate()
        {
            base.DeActivate();
            if (skill != null)
            {
                skill.OnAnimalFire -= AddStat;
            }
        }

        void AddStat()
        {
            if(skill == null) return;

            _buff.AddSubBuff(skill.Player, new EventParameters(skill.Player));
        }
    }
}