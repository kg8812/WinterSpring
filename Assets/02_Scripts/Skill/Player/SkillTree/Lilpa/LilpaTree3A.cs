using chamwhy.DataType;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree3A : SkillTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("데미지 증가량(%)")] public float dmgIncrement;
            [LabelText("스택 최대치")] public int maxStack;
        }
        private LilpaActiveSkill skill;
        private SkillAttachment _attachment;
        private SkillAttachment attachment 
        {
            get
            {
                _attachment ??= new SkillAttachment(new SkillStat());
                _attachment.Stat.dmgRatio = skill.Player.SubBuffCount(SubBuffType.ExpansionBullet) *
                                            datas[level - 1].dmgIncrement;
                return _attachment;
            }
        }

        public DataStruct[] datas;


        private Buff buff;

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            
            skill = active as LilpaActiveSkill;
           
            if (skill == null) return;
            
            BuffDataType data = new(SubBuffType.ExpansionBullet)
            {
                buffCategory = 1,buffDispellType = 2,
                applyType = 0,buffMaxStack = datas[level-1].maxStack, stackDecrease = 1,showIcon = true, buffIconPath = "BuffIcon_atkUp",
            };
            if (buff == null)
            {
                buff = new(data,skill.Player);
            }
            else
            {
                buff.SetData(data);
            }

            skill.Player.AddEvent(EventType.OnHitReaction, ResetDmg);
            skill.RemoveAttachment(attachment);
            skill.AddAttachment(attachment);
            skill.lilpaWeapons.Values.ForEach(x =>
            {
                x.Skill.OnActive.RemoveListener(IncreaseDmg);
                x.Skill.OnActive.AddListener(IncreaseDmg);
            });
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.lilpaWeapons.Values.ForEach(x =>
            {
                x.Skill.OnActive.RemoveListener(IncreaseDmg);
            });
            skill?.RemoveAttachment(attachment);
            skill?.Player.RemoveEvent(EventType.OnHitReaction, ResetDmg);
        }

        void IncreaseDmg()
        {
            buff.AddSubBuff(skill.Player, new EventParameters(skill.Player));
        }

        void ResetDmg(EventParameters _)
        {
           skill.Player.RemoveType(SubBuffType.ExpansionBullet);
        }
    }
}