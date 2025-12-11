// namespace Apis
// {
//     public class SkillController : AbsController
//     {
//         // 고유트리 탭 컨트롤러
//
//         public override void Init(MenuController menu)
//         {
//             base.Init(menu);
//             ctrls.Add(new Skill_MenuCtrl(this));
//         }
//     }
//
//     public class Skill_MenuCtrl : ICtrl
//     {
//         // 고유트리용 첫번째 컨트롤러(a,d 입력시 다음 탭으로 이동)
//
//         readonly SkillController controller;
//         public Skill_MenuCtrl(SkillController controller)
//         {
//             this.controller = controller;
//         }
//         public void OnEnter()
//         {
//         }
//
//         public void OnExit()
//         {
//         }
//
//         public void OnInputA()
//         {
//             controller.Menu.MoveLeft();
//         }
//
//         public void OnInputD()
//         {
//             controller.Menu.MoveRight();
//         }
//
//         public void OnInputEnter()
//         {
//         }
//
//         public void OnInputS()
//         {
//         }
//
//         public void OnInputSpace()
//         {
//         }
//
//         public void OnInputW()
//         {
//         }
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//         }
//     }
// }
