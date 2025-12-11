// using System.Collections.Generic;
// using Apis;
// using UnityEngine;
// using Sirenix.OdinInspector;
// using UI;
// using UnityEngine.Events;
//
// namespace chamwhy
// {
//     public partial class CommonMonster
//     {
//         public Transform _thisTrans { get; private set; }
//         private Transform _playerTrans;
//         [HideInInspector] public MonsterState preState;
//         private float playerDist;
//
//         [TabGroup("기획쪽 수정 변수들/group1", "몬스터 설정")][LabelText("회전 시간")] public float turnTime;
//         [HideInInspector] public List<Projectile> Projectiles;
//         
//         // events
//         [HideInInspector] public UnityEvent<bool> OnRecognitionChanged;
//
//
//         // 0: non-forced, 1: right forced, -1: left forced
//         [HideInInspector] public int forcedPatrolRotation = 0;
//
//         private void AwakeUtil()
//         {
//             // _playerTrans = GameManager.instance.Player.transform;
//             _thisTrans = gameObject.GetComponent<Transform>();
//             _playerTrans = GameManager.instance.PlayerTrans;
//             GameManager.instance.playerRegistered.AddListener(RegisterPlayer);
//             OnRecognitionChanged = new UnityEvent<bool>();
//         }
//
//         private void RegisterPlayer(Player p)
//         {
//             _playerTrans = ReferenceEquals(p, null) ? null : p.transform;
//         }
//
//         private void InitUtil()
//         {
//             AddEvent(EventType.OnRecognitionEnter, (ai) =>
//             {
//                 GameManager.instance.BattleStateClass.AddRecogMonster(this);
//             });
//             AddEvent(EventType.OnRecognitionExit, (ai) =>
//             {
//                 GameManager.instance.BattleStateClass.RemoveRecogMonster(this);
//             });
//         }
//
//         /// <returns>몬스터가 플레이어를 향해 있으면 true</returns>
//         public bool CheckPlayerRL()
//         {
//             EActorDirection playerRL = IsPlayerRight() ? EActorDirection.Right : EActorDirection.Left;
//             return Direction != playerRL;
//         }
//
//
//         public bool CheckWallAndCliff()
//         {
//             if (ActorMovement.CheckClimb())
//             {
//                 return true;
//             }
//             else
//             {
//                 if (ActorMovement.CheckCliff())
//                 {
//                     return true;
//                 }
//                 else
//                 {
//                     return false;
//                 }
//             }
//         }
//
//        
//         public virtual List<int> GetCheckedPatternGroupsWithCondition(float playerDist)
//         {
//             return pGChecker.GetCheckedPatternGroups(playerDist);
//         }
//         
//
//         public void TurnToPlayerWithoutDelay()
//         {
//             if(CheckPlayerRL())
//                 TurnWithoutDelay();
//         }
//         
//         public void TurnWithoutDelay()
//         {
//             Debug.Log("monster - turn without delay");
//             Direction = (EActorDirection)((int)Direction * -1);
//             Vector3 localScale = _thisTrans.localScale;
//             transform.localScale =
//                 new Vector3(
//                     (Direction == EActorDirection.Right ? 1 : -1) * Mathf.Abs(localScale.x),
//                     localScale.y, 
//                     localScale.z
//                 );
//         }
//
//
//      
//         
//         public float ShotRayToPlayer()
//         {
//             Vector3 myRayPoint = Position;
//             Vector2 dir = GameManager.instance.Player.Position - myRayPoint;
//             RaycastHit2D hit = Physics2D.Raycast(myRayPoint, dir, Mathf.Infinity, LayerMasks.Player | LayerMasks.Map);
// #if UNITY_EDITOR
//             Debug.DrawRay(myRayPoint, dir,Color.yellow);
// #endif
//             if (hit)
//             {
//                 if (hit.collider.gameObject.CompareTag("Player"))
//                 {
//                     return hit.distance;
//                 }
//                 else
//                 {
//                     return -1;
//                 }
//             }
//             return -1;
//         }
//
//         public bool IsPlayerRight()
//         {
//             // return true;
//             if (!ReferenceEquals(_playerTrans, null))
//             {
//                 return _playerTrans.position.x > _thisTrans.position.x;
//             }
//             else
//             {
//                 // 플레이어가 없음. 그냥 왼쪽이라고 대충 알려주기
//                 return false;
//             }
//         }
//
//
//         public void IdleAnimationStated()
//         {
//             // delay 혹은 idle, movestop 상태 진입 때 호출됨.
//             if (_curMState == MonsterState.Attack)
//             {
//                 SetMonsterState(MonsterState.Delay);
//             }
//         }
//         
//         private void SetActorInfoUI()
//         {
//             hpBarUi = GameManager.UI.CreateUI("UI_SemiHpBar", UIType.Ingame, withoutActivation:true) as UI_SemiHpBar;
//             hpBarUi.InitActor(this);
//             hpBarUi.SetTrans(_thisTrans);
//             hpBarUi.TryActivated();
//         }
//     }
// }