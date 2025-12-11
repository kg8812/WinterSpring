using System.Collections;
using System.Collections.Generic;
using Apis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class BrushSet : Accessory
    {
        [LabelText("회복 증가량")] public float healRate;

        public override void Init()
        {
            base.Init();
            BonusStat.SetValue(ActorStatType.HealRate, healRate);
        }
    }
}