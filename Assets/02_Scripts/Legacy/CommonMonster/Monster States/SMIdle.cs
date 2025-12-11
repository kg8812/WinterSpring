// using UnityEngine;
//
// namespace chamwhy
// {
//     public class SMIdle : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//
//         // 상태 value
//         private float _duration;
//         private float _timer;
//
//         private bool _isPatrol;
//         private float[] _durationRange;
//         private EActorDirection goalDiraction;
//
//
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster = monster;
//             // 생성자에서 초기화 해줄 수 있지만 혹시 Patrol 여부나 delayDuration
//             bool isNull = _commonMonster.MonsterData != null;
//             _isPatrol = isNull && _commonMonster.MonsterData.isPatrol;
//             if (_isPatrol && isNull)
//             {
//                 _durationRange = _commonMonster.MonsterData.patrolDuration;
//                 _duration = Random.Range(_durationRange[0], _durationRange[1]);
//             }
//
//             _timer = 0;
//
//             _commonMonster.animator.SetInteger("pGInd", -1);
//             _commonMonster.animator.SetBool("isPatrol", false);
//             _commonMonster.animator.SetBool("recognized", false);
//         }
//
//         public void Update()
//         {
//             // Debug.Log("idle update");
//             if (_commonMonster._isInMonsterRecognizer)
//             {
//                 _commonMonster.CheckRecognition();
//             }
//             _commonMonster.CheckDisActivate();
//             // Debug.Log($"idle time update2 {_isPatrol}");
//             // checkRecognition으로 인식 상태가 되지 않았고, disActivate가 되지 않았을 때
//             if (_isPatrol && !_commonMonster.IsRecognized && _commonMonster._isActivated)
//             {
//                 // Debug.Log("idle time update");
//                 _timer += Time.deltaTime;
//                 if (_timer >= _duration)
//                 {
//                     goalDiraction = (EActorDirection)(_commonMonster.forcedPatrolRotation != 0
//                         ? _commonMonster.forcedPatrolRotation
//                         : Random.Range(0, 2));
//                     if (goalDiraction != _commonMonster.Direction)
//                     {
//                         _commonMonster.preState = MonsterState.Patrol;
//                         _commonMonster.SetMonsterState(MonsterState.Turn);
//                     }
//                     else
//                     {
//                         _commonMonster.SetMonsterState(MonsterState.Patrol);
//                     }
//                     
//                 }
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
//             // exit idle state logic
//         }
//     }
// }