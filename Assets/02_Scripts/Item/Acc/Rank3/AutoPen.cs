using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class AutoPen : Accessory
    {
        [LabelText("치명타 확률 증가량")] public float amount;
        
        public override void Init()
        {
            base.Init();
            
            BonusStat.SetValue(ActorStatType.CritProb,amount);
        }
    }
}