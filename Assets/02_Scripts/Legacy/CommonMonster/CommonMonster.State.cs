// using System.Collections.Generic;
// using Apis;
// using UnityEngine;
//
// namespace chamwhy
// {
//     // 몬스터 상태
//     public enum MonsterState
//     {
//         Idle,
//         Patrol,
//         Move,
//         Delay,
//         Jump,
//         Attack,
//         CC,
//        // Groggy,
//         Death,
//         Turn
//     }
//
//     public enum MonsterCCType
//     {
//         Charm, // 매혹
//         Restraint, // 속박
//         Freezing
//     }
//
//     // monster state partial class
//     public partial class CommonMonster
//     {
//         [Space(10f)] [Header("STATE")]
//         // 테스트용 public
//         private StateMachine<CommonMonster> _mState;
//
//         [HideInInspector] public MonsterState _curMState;
//
//
//         public string MState
//         {
//             get { return _mState.CurrentState.ToString(); }
//         }
//
//
//         private Dictionary<MonsterState, IState<CommonMonster>> _mStates;
//
//
//         // Have to fix
//         private void AwakeState()
//         {
//             _mStates = new Dictionary<MonsterState, IState<CommonMonster>>();
//
//             IState<CommonMonster> newSMIdle = new SMIdle();
//             _curMState = MonsterState.Idle;
//
//             _mStates.Add(MonsterState.Idle, newSMIdle);
//             _mStates.Add(MonsterState.Patrol, new SMPatrol());
//             _mStates.Add(MonsterState.Move, new SMMove());
//             _mStates.Add(MonsterState.Delay, new SMDelay());
//             _mStates.Add(MonsterState.Jump, new SMJump());
//             _mStates.Add(MonsterState.Attack, new SMAttack());
//             _mStates.Add(MonsterState.CC, new SMCC());
//             //_mStates.Add(MonsterState.Groggy, new SMGroggy());
//             _mStates.Add(MonsterState.Death, new SMDeath());
//             _mStates.Add(MonsterState.Turn, new SMTurn());
//         }
//
//         private void InitState()
//         {
//             _mState = new StateMachine<CommonMonster>(this, _mStates[MonsterState.Idle]);
//         }
//
//         protected override void Update()
//         {
//             base.Update();
//             if (ReferenceEquals(_playerTrans, null)) return;
//             CheckActivation();
//             if (_isActivated)
//             {
//                 _mState?.Update();
//             }
//             // hpBarUi?.SetText($"ma: {_isInMonsterActivator}\n mr: {_isInMonsterRecognizer}\n a: {_isActivated}\n r: {IsRecognized}");
//             
//         }
//
//         protected override void FixedUpdate()
//         {
//             base.FixedUpdate();
//
//             // base.FixedUpdate();
//             if (_isActivated)
//             {
//                 _mState?.FixedUpdate();
//             }
//         }
//
//         public void SetMonsterState(MonsterState newStateType)
//         {
//             if (_curMState == MonsterState.CC && SubBuffCount(SubBuffType.Debuff_Stun) > 0)
//             {
//                 return;
//             }
//             
//             if (_mStates.TryGetValue(newStateType, out IState<CommonMonster> newState))
//             {
//                 if (_curMState != newStateType)
//                 {
//                     _curMState = newStateType;
//                     _mState.SetState(newState);
//                     ExecuteEvent(EventType.OnStateChanged, new EventParameters(this){monsterState = _curMState});
//                 }
//             }
//         }
//
//         public override void IdleOn()
//         {
//             // Debug.Log("강제 idle 진입");
//             SetMonsterState(MonsterState.Idle);
//         }
//
//         public override void EndStun()
//         {
//             base.EndStun();
//             IdleOn();
//         }
//
//         public void TurnComplete()
//         {
//             isTurning = false;
//         }
//         
//         public override void StartStun(IEventUser actor, float duration)
//         {
//             base.StartStun(actor,duration);
//
//             if (_curMState != MonsterState.CC)
//             {
//                 SetMonsterState(MonsterState.CC);
//             }
//             else
//             {
//                 SubBuffManager.AddCC(actor, SubBuffType.Debuff_Stun, duration);
//             }
//         }
//     }
// }