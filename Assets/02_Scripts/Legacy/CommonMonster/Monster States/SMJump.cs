// namespace chamwhy
// {
//     public class SMJump : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//         public void OnEnter(CommonMonster monster)
//         {
//             _commonMonster = monster;
//             _commonMonster.ExecuteEvent(EventType.OnJump, new EventParameters(_commonMonster));
//         }
//
//         public void Update()
//         {
//             // jump state logic
//         }
//
//         public void FixedUpdate()
//         {
//             // physics logic for jump state
//         }
//
//         public void OnExit()
//         {
//             // exit jump state logic
//         }
//     }
// }
