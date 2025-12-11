using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class OldPostcard : Accessory
    {
        [LabelText("결속력 증가량")] public float amount;
        
        public override void Init()
        {
            base.Init();
            BonusStat.SetValue(ActorStatType.CDReduction,amount);
        }
    }
}