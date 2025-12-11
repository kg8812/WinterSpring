// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace Apis
// {
//     public abstract class AbsController : MonoBehaviour, IController
//     {
//         // 컨트롤러 추상 클래스 (메뉴용)
//         private List<ICtrl> _ctrls;
//         public List<ICtrl> ctrls => _ctrls ??= new(); // 컨트롤러 목록, 필요에 따라 컨트롤러 변경함
//         public ICtrl Current; // 현재 컨트롤러
//         [HideInInspector] public int index = 0; // 현재 컨트롤러 인덱스
//
//         private UnityEvent _onTabEnter;
//         private UnityEvent _onTabExit;
//         public UnityEvent OnTabEnter => _onTabEnter ??= new();
//         public UnityEvent OnTabExit => _onTabExit ??= new();
//         public void Control()
//         {
//             // InputManager.ClearPushedKeycode();
//             // 키 입력 받기
//             if (InputManager.GetKeyDown(KeyCode.W) || InputManager.GetKeyDown(KeyCode.UpArrow))
//             {
//                 Current.OnInputW();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.A) || InputManager.GetKeyDown(KeyCode.LeftArrow))
//             {
//                 Current.OnInputA();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.S) || InputManager.GetKeyDown(KeyCode.DownArrow))
//             {
//                 Current.OnInputS();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.D) || InputManager.GetKeyDown(KeyCode.RightArrow))
//             {
//                 Current.OnInputD();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.Space))
//             {
//                 Current.OnInputSpace();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.Return))
//             {
//                 Current.OnInputEnter();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.Escape))
//             {
//                 Current.OnInputEsc();
//             }
//         }
//         public virtual void OnEnter() // 현재 탭에 들어왔을때 함수
//         {
//             // 현재 탭, 선택 이미지 활성화
//             gameObject.SetActive(true);
//
//             // 첫번째 컨트롤러로 교체
//             if (ctrls.Count > 0)
//             {
//                 index = 0;
//                 Current = ctrls[0];
//                 Current.OnEnter();
//             }
//             
//             OnTabEnter.Invoke();
//         }
//
//         public virtual void OnExit() // 현재 탭에서 벗어날 때 함수
//         {
//             // 탭, 선택 이미지 비활성화
//             gameObject.SetActive(false);
//             
//             OnTabExit.Invoke();
//
//         }
//         public MenuController Menu { get; private set; } // 전체 메뉴 컨트롤러 (이 클래스는 탭 컨트롤러)
//
//         public virtual void Init(MenuController menu) // 메뉴 컨트롤러 초기화
//         {
//             Menu = menu;
//         }
//         
//     }
// }
