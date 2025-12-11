// using System.Collections.Generic;
// using Apis;
// using chamwhy.CommonMonster2;
// using chamwhy.DataType;
// using NewMonster;
// using UI;
// using UnityEngine;
//
// namespace chamwhy
// {
//     [RequireComponent(typeof(PatternGroupController), typeof(PatternGroupChecker))]
//     public partial class CommonMonster : Monster
//     {
//         
//         [HideInInspector] public PatternGroupChecker pGChecker;
//         [HideInInspector] public PatternGroupController pGController;
//         [HideInInspector] public UI_SemiHpBar hpBarUi;
//
//         [HideInInspector] public bool isTurning = false;
//         [HideInInspector] public bool pGCanceled = false;
//         [HideInInspector] public bool canMoveStop = true;
//
//         private float _recognizedTime;
//         
//
//         public List<AttackObject> colliderAttack;
//
//         // When: 몬스터 생성
//         protected override void Awake()
//         {
//             base.Awake();
//             pGCanceled = false;
//             animator = SkeletonTrans.GetComponent<Animator>();
//
//             pGChecker = GetComponent<PatternGroupChecker>();
//             pGController = GetComponent<PatternGroupController>();
//             pGController.InitCheck();
//             Direction = Mathf.Approximately(transform.localScale.x, 1f)? EActorDirection.Right : EActorDirection.Left; 
//             AwakeState();
//             AwakeUtil();
//
//             // whenMonsterDead = new();
//             // whenMonsterDead.AddListener(() => { _mState.CurrentState.OnExit(); });
//         }
//
//         // When: 몬스터 초기화
//         public override void Init(MonsterDataType monsterDataType)
//         {
//             base.Init(monsterDataType);
//             Rb.bodyType = RigidbodyType2D.Dynamic;
//
//             _isActivated = false;
//             IsRecognized = false;
//             _isInMonsterActivator = false;
//             _isInMonsterRecognizer = false;
//             
//             isTurning = false;
//             forcedPatrolRotation = 0;
//             // Debug.Log($"eeewfwfwfw {hitCollider.enabled}");
//             HitCollider.enabled = true;
//             animator.enabled = false;
//             // OnDisActivated();
//
//             InitState();
//             InitUtil();
//
//             MoveComponent?.MoveOn();
//             AttackOn();
//
//             if (Direction != EActorDirection.Right)
//             {
//                 TurnWithoutDelay();
//             }
//
//             foreach (var ca in colliderAttack)
//             {
//                 ca.Init(this, new AtkBase(this));
//             }
//
//             //hitCollider.enabled = false;
//         }
//
//
//         [HideInInspector] public bool _isInMonsterActivator;
//         [HideInInspector] public bool _isInMonsterRecognizer;
//         [HideInInspector] public bool _isActivated;
//         
//         #region MonsterStateRegion
//
//         private void CheckActivation()
//         {
//             // Debug.Log($"check {gameObject.name} {_isInMonsterRecognizer} {_isInMonsterActivator}");
//             if (_isInMonsterRecognizer)
//             {
//                 if (!_isActivated)
//                 {
//                     OnActivated();
//                 }
//
//                 // CheckRecognition2(); 여기가 아닌, idle과 patrol, move에서 실행. 해당 로직 이후에 mState.Update가 실행되기 때문에 순서대로라면 문제 없음.
//             }
//             else if (_isInMonsterActivator && !_isActivated)
//             {
//                 OnActivated();
//             }
//
//             // 해당 로직은 idle, patrol에서만 실행.
//             /*
//             CheckDisActivate();
//             */
//
//             // 해당 로직은 SMMove에서만 실행.
//             /*
//             CheckDisRecognition();
//             */
//         }
//         public float CheckRecognition()
//         {
//             playerDist = ShotRayToPlayer();
//             if (playerDist >= 0)
//             {
//                 if (!IsRecognized || _curMState == MonsterState.Idle)
//                 {
//                     OnRecognized();
//                 }
//             }
//             else
//             {
//                 if (IsRecognized)
//                 {
//                     OnDisRecognized();
//                 }
//             }
//             return playerDist;
//         }
//         
//
//         public void CheckDisRecognition()
//         {
//             // Debug.Log($"check dis recognition {_isInMonsterRecognizer}");
//             if (IsRecognized && !_isInMonsterRecognizer)
//             {
//                 if (MonsterData.monsterType != MonsterType.Common) return;
//                 if (FormulaConfig.minMonsterRecognitionTime + _recognizedTime <= Time.time)
//                 {
//                     // Debug.Log($"탈출띠");
//                     OnDisRecognized();
//                     if (!_isInMonsterActivator)
//                     {
//                         OnDisActivated();
//                     }
//                 }
//                 else
//                 {
//                     // Debug.Log($"시간 안지남. {FormulaConfig.minMonsterRecognitionTime} {_recognizedTime}, {Time.time}");
//                 }
//             }
//         }
//
//         public void CheckDisActivate()
//         {
//             if (!_isInMonsterActivator && !IsRecognized && _isActivated)
//             {
//                 OnDisActivated();
//             }
//         }
//
//         
//         private void OnActivated()
//         {
//             _isActivated = true;
//             animator.enabled = true;
//             // hitCollider.enabled = true;
//             SetActorInfoUI();
//             CurGroggyGauge = CurGroggyGauge;
//         }
//
//         private void OnDisActivated()
//         {
//             _isActivated = false;
//             animator.enabled = false;
//             // hitCollider.enabled = false;
//             // Debug.Log("monster disactivated");
//             GameManager.UI.CloseUI(hpBarUi);
//         }
//
//         // 강제로 인식하게 만들수도 있음.
//         public override void OnRecognized()
//         {
//             base.OnRecognized();
//             // Debug.Log("Reset recogtime");
//             _recognizedTime = Time.time;
//             SetMonsterState(MonsterState.Move);
//         }
//
//         public override void OnDisRecognized()
//         {
//             if (isDead) return;
//             if (MonsterData.monsterType != MonsterType.Common) return;
//             base.OnDisRecognized();
//             SetMonsterState(MonsterState.Idle);
//         }
//
//         #endregion
//
//         public void MoveCCOn()
//         {
//             MoveComponent.JumpOff();
//             MoveComponent.MoveOff();
//         }
//
//         public void MoveCCOff()
//         {
//             MoveComponent.JumpOn();
//             MoveComponent.MoveOn();
//         }
//
//         public override void AnimPauseOn()
//         {
//             canMoveStop = false;
//             base.AnimPauseOn();
//         }
//
//         public override void AnimPauseOff()
//         {
//             canMoveStop = true;
//             base.AnimPauseOff();
//         }
//
//         public override float OnHit(EventParameters parameters)
//         {
//             if (!IsRecognized)
//             {
//                 IsRecognized = !(ShotRayToPlayer() < 0);
//             }
//             return base.OnHit(parameters);
//         }
//
//         // protected override void Groggyed()
//         // {
//         //     base.Groggyed();
//         //     SetMonsterState(MonsterState.Groggy);
//         // }
//
//         // protected override void GroggyEnded()
//         // {
//         //     base.GroggyEnded();
//         //     if (!isDead)
//         //     {
//         //         SetMonsterState(MonsterState.Move);
//         //     }
//         // }
//         
//         public override void Die()
//         {
//             base.Die();
//
//             SetMonsterState(MonsterState.Death);
//         }
//
//
//         // animator가 아니라 코드에서도 키고 끌수 있도록 핸들링
//         public void AttackColliderOn(int index)
//         {
//             if (colliderAttack[index] != null)
//             {
//                 colliderAttack[index].gameObject.SetActive(true);
//             }
//         }
//
//         public void AttackColliderOff(int index)
//         {
//             if (colliderAttack[index] != null)
//             {
//                 colliderAttack[index].gameObject.SetActive(false);
//             }
//         }
//
//         protected override void OnDestroy()
//         {
//             base.OnDestroy();
//             hpBarUi?.SetTrans(null);
//         }
//     }
// }