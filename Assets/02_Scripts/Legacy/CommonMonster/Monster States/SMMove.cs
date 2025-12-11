// using UnityEngine;
//
// namespace chamwhy
// {
//     public class SMMove : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//         
//         private const float rayMaxDist = 30f;
//         private readonly int _layerMask = LayerMask.GetMask("Player", "Map");
//
//         private float playerDist;
//
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster = monster;
//             _commonMonster.animator.SetBool("recognized", true);
//             _commonMonster.animator.SetBool("isPatrol", false);
//             _commonMonster.pGController.EndPlayingPG();
//             lastMoveStop = _commonMonster.animator.GetBool("moveStop");
//         }
//
//         public void Update()
//         {
//             _commonMonster.CheckDisRecognition();
//
//             // check disRecognition으로 먼저 체크 후 판단.
//             if (!_commonMonster.IsRecognized) return;
//             playerDist = _commonMonster.CheckRecognition();
//
//             if (_commonMonster.IsRecognized && _commonMonster.ableAttack)
//             {
//                 int availablePG = _commonMonster.pGController.SelectRandomPg(playerDist);
//                 if (availablePG != -1)
//                 {
//                     if (_commonMonster.pGController.PlayPatternGroup(availablePG))
//                     {
//                         _commonMonster.SetMonsterState(MonsterState.Attack);
//                         return;
//                     }
//                 }
//             }
//         }
//
//         private bool lastMoveStop = false;
//
//         public void FixedUpdate()
//         {
//             if (!_commonMonster.IsRecognized) return;
//             if (_commonMonster.ableMove && !_commonMonster.CheckPlayerRL())
//             {
//                 _commonMonster.preState = MonsterState.Move;
//                 _commonMonster.SetMonsterState(MonsterState.Turn);
//             }
//             else
//             {
//                 Debug.LogError($"{_commonMonster.MonsterData.isMove}");
//                 if (_commonMonster.MonsterData.isMove)
//                 {
//                     // TODO: isFlying일때 MonsterMove어떻게 할지
//
//                     if (_commonMonster.MonsterMove(false))
//                     {
//                         if (lastMoveStop)
//                         {
//                             lastMoveStop = false;
//                             _commonMonster.animator.SetBool("moveStop", false);
//                         }
//                     }
//                     else
//                     {
//                         if (!lastMoveStop && _commonMonster.canMoveStop)
//                         {
//                             lastMoveStop = true;
//                             _commonMonster.animator.SetBool("moveStop", true);
//                         }
//
//                         // _commonMonster.StopCheck();
//                     }
//                 }
//             }
//         }
//
//         public void OnExit()
//         {
//             if (_commonMonster._curMState != MonsterState.CC)
//             {
//                 _commonMonster.animator.SetBool("moveStop", false);
//             }
//             if (_commonMonster.Rb.velocity.x != 0)
//             {
//                 if (_commonMonster.ActorMovement.dirVec.x == 0) return;
//                 _commonMonster.Rb.velocity -= _commonMonster.ActorMovement.dirVec *
//                                               (_commonMonster.Rb.velocity.x / _commonMonster.ActorMovement.dirVec.x);
//             }
//         }
//     }
// }