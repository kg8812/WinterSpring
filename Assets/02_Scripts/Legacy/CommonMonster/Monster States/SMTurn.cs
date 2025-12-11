// using UnityEngine;
//
// namespace chamwhy
// {
//     public class SMTurn : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//         private float startTime;
//         private float turnTime;
//
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster = monster;
//             turnTime = _commonMonster.turnTime;
//             startTime = Time.time;
//             _commonMonster.isTurning = true;
//             _commonMonster.animator.SetBool("turn", false);
//         }
//
//         public void Update()
//         {
//             if (startTime + turnTime <= Time.time && _commonMonster.isTurning)
//             {
//                 ExitTurn();
//             }
//         }
//
//         public void FixedUpdate()
//         {
//             // _commonMonster.StopCheck();
//         }
//
//         public void OnExit()
//         {
//             
//         }
//
//         private void ExitTurn()
//         {
//             _commonMonster.TurnComplete();
//             _commonMonster.TurnWithoutDelay();
//             _commonMonster.animator.SetBool("turn", false);
//             _commonMonster.SetMonsterState(_commonMonster.preState);
//         }
//     }
// }