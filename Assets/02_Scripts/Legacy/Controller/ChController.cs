// namespace Apis
// {
//     public class ChController : AbsController
//     {
//         
//         public override void Init(MenuController menu)
//         {
//             base.Init(menu);
//             ctrls.Add(new Ch_MenuCtrl(this));
//         }
//     }
//
//     public class Ch_MenuCtrl : ICtrl
//     {
//         readonly ChController controller;
//         public Ch_MenuCtrl(ChController controller)
//         {
//             this.controller = controller;
//         }
//         public void OnEnter()
//         {
//             
//         }
//
//         public void OnExit()
//         {
//             
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
