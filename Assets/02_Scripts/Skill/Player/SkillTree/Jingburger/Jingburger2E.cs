using chamwhy;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class Jingburger2E : SkillTree
    {
        public struct DataStruct
        {
            [LabelText("데미지(%)")] public float dmg;
            [LabelText("지속시간")] public float duration;
        }

        public DataStruct[] datas;

        private JingburgerActiveSkill skill;
        private Buff buff;

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active, level);

            skill = active as JingburgerActiveSkill;
            skill?.OnExplosionSpawn.AddListener(AddDebuff);
            BuffDataType data = new(SubBuffType.Debuff_RasenganResidue)
            {
                buffPower = new[] { datas[level - 1].dmg }, buffDuration = datas[level - 1].duration, buffMaxStack = 1,
                showIcon = false,
                buffCategory = 1, buffDispellType = 1,
            };
            if (buff == null)
            {
                buff = new Buff(data, skill?.eventUser);
            }
            else
            {
                buff.SetData(data);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnExplosionSpawn.RemoveListener(AddDebuff);
        }

        void AddDebuff(AttackObject atk)
        {
            atk.AddEventUntilInitOrDestroy(x =>
            {
                if (x?.target is Actor target)
                {
                    buff.AddSubBuff(target, null);
                }
            });
        }
    }
}