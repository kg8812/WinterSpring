using System.Collections;
using System.Collections.Generic;
using chamwhy.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class KnightCanteen : Accessory
    {
        [LabelText("영혼 증가량")] public float spirit;
        [LabelText("기량 증가량")] public float finesse;
        [LabelText("신체 증가량")] public float body;

        public override void Init()
        {
            base.Init();
            BonusStat.SetValue(ActorStatType.Spirit, spirit);
            BonusStat.SetValue(ActorStatType.Finesse, finesse);
            BonusStat.SetValue(ActorStatType.Body, body);
        }
    }
}