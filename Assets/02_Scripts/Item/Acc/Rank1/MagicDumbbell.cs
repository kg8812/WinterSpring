using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using GameStateSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class MagicDumbbell : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("지속시간")] public float duration;
        [LabelText("보호막 양")] public float amount;

        private Buff _buff;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Buff_Barrier)
            {
                buffPower = new[] { amount }, buffCategory = 1, buffDuration = duration, buffDispellType = 1,
                buffMaxStack = 1, showIcon = false
            };
            _buff = new(data, user);
        }
        
        protected override void UpdateFunc(EventParameters _)
        {
            base.UpdateFunc(_);

            if (GameManager.instance.CurGameStateType == GameStateType.BattleState)
            {
                if (!isCd)
                {
                    _buff.AddSubBuff(user,null);
                    GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
                }
            }
        }
    }
}