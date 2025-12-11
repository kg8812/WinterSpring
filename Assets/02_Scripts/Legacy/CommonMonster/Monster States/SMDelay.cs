// using UnityEngine;
//
// namespace chamwhy
// {
//     public class SMDelay: IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//         private float startTime;
//         private float delayTime;
//         
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster = monster;
//             _commonMonster.pGController.ForceCancel();
//             _commonMonster.animator.SetTrigger("forceDelay");
//             startTime = Time.time;
//             delayTime = Random.Range(_commonMonster.MonsterData.delayDuration[0],
//                 _commonMonster.MonsterData.delayDuration[1]);
//         }
//
//         public void Update()
//         {
//             if (Time.time >= startTime + delayTime)
//             {
//                 _commonMonster.SetMonsterState(MonsterState.Move);
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
//             _commonMonster.animator.SetTrigger("exitDelay");
//         }
//     }
// }