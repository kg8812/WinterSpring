using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class FrozenDiary : Accessory
    {
        [LabelText("데미지 증가량(%)")] public float dmgAmount;
        [LabelText("냉기 적용 횟수")] public int count;
        [LabelText("냉기 적용 반경")] public float radius;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            user.AddEvent(EventType.OnAttackSuccess,IncreaseDmg);
            user.AddEvent(EventType.OnAfterAtk,ApplyFreeze);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnAttackSuccess,IncreaseDmg);
            user.RemoveEvent(EventType.OnAfterAtk,ApplyFreeze);
        }

        void IncreaseDmg(EventParameters param)
        {
            if (param?.target is IBuffUser target)
            {
                if (target.SubBuffManager.Contains(SubBuffType.Debuff_Frozen))
                {
                    param.statData.stat.AddValue(ActorStatType.ExtraDmg,dmgAmount);
                }
            }
        }
        void ApplyFreeze(EventParameters param)
        {
            if (param?.target == null) return;

            var targets = param.target.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy).OfType<IBuffUser>();
            targets.ForEach(x =>
            {
                for (int i = 0; i < count; i++)
                {
                    x.SubBuffManager.AddSubBuff(SubBuffType.Debuff_Chill, user);
                }
            });
        }
    }
}