// using UI;
// using UnityEngine;
//
// namespace Apis
// {
//     public class MapController : AbsController
//     {
//         [SerializeField] private UI_Map _uiMap; 
//
//         public override void Init(MenuController menu)
//         {
//             base.Init(menu);
//             ctrls.Add(new Map_MenuCtrl(this));
//             _uiMap.Init();
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//     }
//
//     public class Map_MenuCtrl : ICtrl
//     {
//         readonly MapController controller;
//         public Map_MenuCtrl(MapController controller)
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
