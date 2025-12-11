// using UnityEngine;
//
// namespace chamwhy
// {
//     public class SMPatrol : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//         private float _duration;
//         private float[] _patrolDurationRange;
//         private float _timer;
//         private float playerDist;
//         private EActorDirection goalDiraction;
//
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster = monster;
//             _timer = 0;
//             _duration = Random.Range(_commonMonster.MonsterData.patrolDuration[0],
//                 _commonMonster.MonsterData.patrolDuration[1]);
//             
//             _commonMonster.animator.SetBool("isPatrol", true);
//         }
//
//         public void Update()
//         {
//             if (_commonMonster._isInMonsterRecognizer)
//             {
//                 _commonMonster.CheckRecognition();
//             }
//             _commonMonster.CheckDisActivate();
//             
//             // checkRecognition으로 인식 상태가 되지 않았고, disActivate가 되지 않았을 때
//             if (!_commonMonster.IsRecognized && _commonMonster._isActivated)
//             {
//                 _timer += Time.deltaTime;
//                 if (!_commonMonster.MonsterMove(true))
//                 {
//                     _commonMonster.forcedPatrolRotation = -(int)_commonMonster.Direction;
//                     _commonMonster.SetMonsterState(MonsterState.Idle);
//                     return;
//                 }
//             
//                 if (_timer >= _duration)
//                 {
//                     _commonMonster.forcedPatrolRotation = 0;
//                     _commonMonster.SetMonsterState(MonsterState.Idle);
//                 }
//             }
//         }
//
//         public void FixedUpdate()
//         {
//             // physics logic for move state
//         }
//
//         public void OnExit()
//         {
//             
//         }
//     }
// }