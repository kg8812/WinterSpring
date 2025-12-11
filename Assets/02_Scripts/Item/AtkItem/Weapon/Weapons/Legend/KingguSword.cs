using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class KingguSword : Weapon
    {
        [Title("검신신장 설정")] 
        [LabelText("최대스택")] public int stack;
        [LabelText("기본공격 증가량(%)")]public float basicAtk;
        [LabelText("지속시간")]public float duration;
        
        private Buff _buff;

        Buff buff
        {
            get
            {
                if (_buff == null)
                {
                    BuffDataType data = new(SubBuffType.Buff_BasicAtkEnhance)
                    {
                        buffPower = new[] { basicAtk }, buffCategory = 1, buffDuration = duration,
                        buffMaxStack = stack, stackDecrease = 0, valueType = ValueType.Ratio,
                        showIcon = true
                    };
                    _buff = new(data, Player);
                }

                return _buff;
            }
        }

        public override void SetGroundCollider(int idx, AttackObject[] attackObjs)
        {
            base.SetGroundCollider(idx, attackObjs);
            if (idx >= 3)
            {
                attackObjs.ForEach(x =>
                {
                    x.AddAtkEventOnce(param => { buff.AddSubBuff(Player, param); });
                });
            }
        }

        public override void SetAirCollider(int idx, AttackObject[] attackObjs)
        {
            base.SetAirCollider(idx, attackObjs);
            if (idx >= 3)
            {
                attackObjs.ForEach(x =>
                {
                    x.AddAtkEventOnce(param => { buff.AddSubBuff(Player, param); });
                });
            }
        }
    }
}