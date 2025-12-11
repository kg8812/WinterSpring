using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class WhoSketch : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("필요 스킬 사용횟수")] public int needCount;
        [LabelText("쉴드 획득량 (최대체력 %)")] public float shield;
        [LabelText("쉴드 지속시간")] public float duration;

        private int count;
        private Buff _buff;
        public override void Init()
        {
            base.Init();
            count = 0;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Buff_Barrier)
            {
                buffPower = new[] { shield }, buffDuration = duration, buffMaxStack = 1, buffCategory = 1,
                buffDispellType = 1,
                showIcon = false, applyStrategy = 0,
            };
            _buff = new(data, user);
            user.AddEvent(EventType.OnWeaponSkillUse,AddCount);
            user.AddEvent(EventType.OnSkill,AddCount);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnWeaponSkillUse,AddCount);
            user.RemoveEvent(EventType.OnSkill,AddCount);
        }

        void AddCount(EventParameters _)
        {
            if (isCd) return;
            
            count++;
            if (count >= needCount)
            {
                GameManager.instance.StartCoroutine(CDCoroutine(cd));
                count = 0;
                _buff.AddSubBuff(user,null);
            }
        }
    }
}