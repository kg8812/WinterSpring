

// namespace chamwhy
// {
//     public class SMGroggy : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//         public SMGroggy(CommonMonster commonMonster)
//         {
//             _commonMonster = commonMonster;
//             
//         }
//
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster.pGController.ResetWhenGroggyStart();
//             _commonMonster.animator.SetTrigger("groggy");
//         }
//
//         public void Update()
//         {
//         }
//
//         public void FixedUpdate()
//         {
//             _commonMonster.StopCheck();
//         }
//
//         public void OnExit()
//         {
//             _commonMonster.animator.SetTrigger("GroggyEnd");
//         }
//     }
// }
