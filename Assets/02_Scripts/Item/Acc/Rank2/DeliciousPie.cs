using System.Collections;
using System.Collections.Generic;
using chamwhy.DataType;
using GameStateSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DeliciousPie : Accessory
    {
        [LabelText("체력 증가량")] public float amount;
        public override void Init()
        {
            base.Init();
            BonusStat.SetValue(ActorStatType.MaxHp,amount);
        }

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
        }
    }
}