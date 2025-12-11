// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace chamwhy
// {
//     public enum MovementResult
//     {
//         None,
//         Cliff,
//         Wall
//     }
//
//     public partial class CommonMonster
//     {
//         [TabGroup("기획쪽 수정 변수들/group1", "몬스터 설정")][LabelText("패트롤 이동속도 비율")][SerializeField][Tooltip("1 = 기본 이동속도")]
//         private float patrolCoef = 0.3f;
//
//         
//
//         // 나중에 몬스터마다 달라질수 있지만 일단 통일
//         public bool MonsterMove(bool isPatrol, float ratio = 1)
//         {
//             if (!ableMove || CheckWallAndCliff())
//             {
//                 Debug.LogError("monster move false");
//                 return false;
//             }
//             else
//             {
//                 Debug.LogError("monster move true");
//                 ActorMovement.Move(Direction, (isPatrol ? patrolCoef : 1)*ratio, MonsterData.isFlying);
//                 return true;
//             }
//         }
//     }
// }