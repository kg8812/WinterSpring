// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace Apis
// {
//     
//     public class MenuController : MonoBehaviour, IController
//     {
//         // 메뉴 컨트롤러
//
//         int index; // 현재 컨트롤러 인덱스
//
//         public UnityEvent _inputQ = new();
//         public UnityEvent _inputE = new();
//         public UnityEvent _inputTab = new();
//         public UnityEvent InputQ => _inputQ??= new();
//         public UnityEvent InputE => _inputE ??= new();
//         public UnityEvent InputTab => _inputTab ??= new();
//         
//         [SerializeField] List<AbsController> list = new(); // 탭 컨트롤러 목록
//         public UnityEvent StartEvent = new();
//         public UnityEvent EnableEvent = new();
//         private void Awake()
//         {            
//             InputQ.AddListener(MoveLeft);
//             InputE.AddListener(MoveRight);
//             InputTab.AddListener(() => InvenManager.instance.OnOffMenu());
//             index = 0;
//             foreach (var c in list)
//             {
//                 c.gameObject.SetActive(false);
//                 c.Init(this);
//             }
//         }
//
//         private void Start() {
//             list[index].OnEnter();
//             StartEvent.Invoke();
//         }
//
//         private void OnEnable()
//         {
//             foreach (var c in list)
//             {
//                 c.OnExit();
//             }
//             
//             list[index].OnEnter();         
//             EnableEvent.Invoke();
//         }
//
//         private void OnDisable()
//         {
//             list[index].OnExit();
//         }
//
//         public void Control()
//         {
//             if (InputManager.GetKeyDown(KeyCode.Q))
//             {
//                 InputQ.Invoke();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.E))
//             {
//                 InputE.Invoke();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.Tab))
//             {
//                 InputTab.Invoke();
//             }
//             else if (InputManager.GetKeyDown(KeyCode.Escape))
//             {
//                 InvenManager.instance.OnOffMenu();
//             }
//             list[index].Control();
//         }
//
//         public void MoveLeft() // 왼쪽탭으로 이동
//         {
//             list[index].OnExit();
//             index--;
//             if (index < 0)
//             {
//                 index = list.Count - 1;
//             }
//             list[index].OnEnter();
//         }
//         public void MoveRight() // 오른쪽탭으로 이동
//         {
//             list[index].OnExit();
//             index++;
//             if (index >= list.Count)
//             {
//                 index = 0;
//             }
//             list[index].OnEnter();
//         }      
//         
//         public void Select(int x) // 탭 선택 (버튼용)
//         {
//             list[index].OnExit();
//
//             index = x;
//
//             list[index].OnEnter();
//         }
//     }   
// }
