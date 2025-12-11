// using EventData;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace chamwhy
// {
//     public partial class CommonMonster
//     {
//         protected override void OnHitReaction(EventParameters eventParameters)
//         {
//             base.OnHitReaction(eventParameters);
//         }
//
//         // public override void KnockBack(Vector2 src, float knockBackForce, float knockBackTime, float knockBackAngle,
//         //     UnityAction OnBegin, UnityAction OnEnd)
//         // {
//         //     ExecuteEvent(EventType.OnHitReaction, null);
//         //     base.KnockBack(src, knockBackForce, knockBackTime, knockBackAngle, (() =>
//         //     {
//         //         // 넉백 CC 상태로 변경하는거 제거함
//         //         OnBegin?.Invoke();
//         //     }), () =>
//         //     {
//         //         OnEnd?.Invoke();
//         //     });
//         // }
//     }
// }