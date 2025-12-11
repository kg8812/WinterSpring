using System;
using System.Collections;
using System.Collections.Generic;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class UnfinishVow : Accessory
    {
        [LabelText("스탯 기본 증가량")] public int baseAmount;
        [LabelText("적당 스탯 증가량")] public int extraAmount;
        [LabelText("탐색 반경")] public float radius;
        
        private BonusStat _extraStat;

        public override BonusStat BonusStat
        {
            get
            {
                return base.BonusStat + (_extraStat ??= new());
            }
        }

        public override void Init()
        {
            base.Init();
            _extraStat ??= new();
            base.BonusStat.SetValue(ActorStatType.Body,baseAmount);
            base.BonusStat.SetValue(ActorStatType.Finesse,baseAmount);
            base.BonusStat.SetValue(ActorStatType.Spirit,baseAmount);
        }

        protected override void UpdateFunc(EventParameters _)
        {
            base.UpdateFunc(_);

            var targets = user.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy);
            SetBonusValue(targets.Count);
        }

        void SetBonusValue(int count)
        {
            _extraStat ??= new();
            
            _extraStat.SetValue(ActorStatType.Body,extraAmount * count);
            _extraStat.SetValue(ActorStatType.Finesse,extraAmount * count);
            _extraStat.SetValue(ActorStatType.Spirit,extraAmount * count);
        }
    }
}