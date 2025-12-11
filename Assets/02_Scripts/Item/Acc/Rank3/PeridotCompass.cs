using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class PeridotCompass : Accessory
    {
        [LabelText("필요 원념량")] public int gold;
        [LabelText("수리 충전량")] public int count;
        [LabelText("최대치일시 회복량 (%)")] public float healAmount;


        private int currGold;

        public override void Init()
        {
            base.Init();
            currGold = 0;
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            GameManager.instance.OnSoulChange.AddListener(SetCurrentGold);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            GameManager.instance.OnSoulChange.RemoveListener(SetCurrentGold);
        }

        void SetCurrentGold(int change)
        {
            if (change > 0)
            {
                currGold += change;
                while (user is Player player && currGold >= gold)
                {
                    currGold -= gold;

                    if (player.CurrentPotionCapacity == player.MaxPotionCapacity)
                    {
                        player.CurHp += player.MaxHp / 100f * healAmount;
                    }
                    else
                    {
                        player.increasePotionCapacity(count);
                    }
                }
            }
        }
    }
}