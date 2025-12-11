using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ExtendVodka : Accessory
    {
        [LabelText("적용 반경")] public float radius;
        [LabelText("데미지 증가량(%)")] public float dmgAdd;
        [LabelText("출혈 부여 시 공격력 증가량(%)")] public float atkAdd;
        [LabelText("공격력 증가 최대 스택")] public int atkMax;
        [LabelText("공격력 증가 지속시간")] public float duration;

        private Buff _buff;

        Buff buff
        {
            get
            {
                if (_buff == null)
                {
                    BuffDatabase.DataLoad.TryGetSubBuffOption(SubBuffType.Buff_Atk, out var option);

                    BuffDataType data = new(SubBuffType.Buff_Atk)
                    {
                        buffPower = new[] { atkAdd }, buffCategory = 1, buffDuration = duration, buffDispellType = 1,
                        applyType = 0, buffMaxStack = atkMax, stackDecrease = 1, valueType = ValueType.Ratio,
                        showIcon = true,
                        buffIconPath = option.iconPath, buffName = option.name, buffDesc = option.description
                    };

                    _buff = new(data, user);
                }

                return _buff;
            }
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnAttackSuccess,AddDmg);
            user.AddEvent(EventType.OnSubBuffApply,AddAtk);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            
            user.RemoveEvent(EventType.OnAttackSuccess,AddDmg);
            user.RemoveEvent(EventType.OnAttackSuccess,AddAtk);
        }
        
        void AddDmg(EventParameters param)
        {
            if (param?.target is IBuffUser target)
            {
                float distance = Vector2.Distance(user.Position, target.Position);
                if (distance <= radius && target.SubBuffManager.Contains(SubBuffType.Debuff_Bleed))
                {
                    param.statData.stat.AddValue(ActorStatType.ExtraDmg,dmgAdd);
                }
            }
        }

        void AddAtk(EventParameters param)
        {
            if (param.buffData.activatedSubBuff.Type == SubBuffType.Debuff_Bleed)
            {
                buff.AddSubBuff(user, new(user));
            }
        }
    }
}