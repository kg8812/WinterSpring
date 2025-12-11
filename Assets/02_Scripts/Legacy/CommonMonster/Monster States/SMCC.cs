// using Default;
//
// namespace chamwhy
// {
//     public class SMCC : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//         public void OnEnter(CommonMonster monster)
//         {
//             this._commonMonster = monster;
//             _commonMonster.pGController.ForceCancel();
//             // _commonMonster.StopCheck();
//             _commonMonster.animator.SetTrigger("groggy");
//             Utils.ActionAfterFrame(() =>
//             {
//                 _commonMonster.IsGroggy = true;
//             });
//         }
//
//         public void Update()
//         {
//             // death state logic
//         }
//
//         public void FixedUpdate()
//         {
//             // if (!_commonMonster.ableMove)
//             // {
//             //     
//             // }
//         }
//
//         public void OnExit()
//         {
//             _commonMonster.animator.SetTrigger("GroggyEnd");
//             _commonMonster.IsGroggy = false;
//         }
//     }
// }