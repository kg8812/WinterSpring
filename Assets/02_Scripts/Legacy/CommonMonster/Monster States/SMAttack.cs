// namespace chamwhy
// {
//     public class SMAttack : IState<CommonMonster>
//     {
//         private CommonMonster _commonMonster;
//
//         public void OnEnter(CommonMonster monster)
//         {
//             this._commonMonster = monster;
//             _commonMonster.ExecuteEvent(EventType.OnAttack, new EventParameters(_commonMonster));
//             int patternGroupId = _commonMonster.pGController.curPGId;
//             _commonMonster.TurnToPlayerWithoutDelay();
//             _commonMonster.animator.SetInteger("pGInd", patternGroupId);
//         }
//
//         public void Update()
//         {
//             // attack state logic
//         }
//
//         public void FixedUpdate()
//         {
//         }
//
//         public void OnExit()
//         {
//             _commonMonster.pGController.ResetPGController();
//             _commonMonster.animator.SetInteger("pGInd", -1);
//         }
//     }
// }