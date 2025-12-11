// using System.Collections;
// using System.Collections.Generic;
// using UI;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Apis
// {
//     public class GoseguActive1 : PlayerActiveSkill
//     {
//         private StackCd cdActive;
//         public override ICdActive CDActive => cdActive ??= new StackCd(this);
//
//         protected override CDEnums _cdType => CDEnums.Stack;
//
//         protected override bool CdUse => false;
//
//         [Title("고세구 좌클릭 스킬")] [LabelText("포물선 높이")]
//         public float height;
//
//         [LabelText("투척거리")] public float distance;
//         [LabelText("웅덩이 크기")] public float radius;
//         [LabelText("웅덩이 지속시간")] public float duration2;
//
//         protected override ActiveEnums _activeType => ActiveEnums.Instant;
//
//         public override void Active()
//         {
//             base.Active();
//
//             GoseguRefinedFlask flask = GameManager.Factory
//                 .Get<GoseguRefinedFlask>(FactoryManager.FactoryType.AttackObject, "GoseguRefinedFlask",
//                     Player.Position);
//             flask.maxHeight = height;
//             flask.radius = radius;
//             flask.poolDuration = duration2;
//             flask.maxDistance = distance;
//             flask.Init(Player, new Actor.AtkBase(Player, Dmg));
//             flask.Fire();
//             Player.PlayerActiveSkill.Use();
//         }
//     }
// }