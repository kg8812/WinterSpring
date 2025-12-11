using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SmallMemoryPotion : Accessory
    {
        [LabelText("중독 데미지 증가량 (%)")] public float dmg;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            if (BuffDatabase.DataLoad.TryGetSubBuffOption(SubBuffType.Debuff_Poison, out var option))
            {
                option.amount[0] *= 1 + dmg / 100f;
            }
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            if (BuffDatabase.DataLoad.TryGetSubBuffOption(SubBuffType.Debuff_Poison, out var option))
            {
                option.amount[0] /= 1 + dmg / 100f;
            }
        }
    }
}