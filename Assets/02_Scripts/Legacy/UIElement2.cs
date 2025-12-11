// using Default;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace chamwhy.UI
// {
//     [RequireComponent(typeof(UI_EventHandler))]
//     public class UIElement2: MonoBehaviour
//     {
//         public UIElementState initState;
//         protected UI_EventHandler EventHandler;
//         
//         [HideInInspector] public bool focusOffByParent;
//         
//         [HideInInspector] public UnityEvent<UIElementState> willStateChange;
//         [HideInInspector] public UnityEvent<UIElementState> stateChanged;
//
//         private UIElementState _elState = UIElementState.Default;
//
//
//
//         protected bool isPointerOn;
//         protected bool isSelected;
//         protected bool isDisabled;
//
//         public UIElementState ElState
//         {
//             get => _elState;
//             set
//             {
//                 willStateChange.Invoke(_elState);
//                 _elState = value;
//                 stateChanged.Invoke(_elState);
//             }
//         }
//
//         private bool _isInit = false;
//
//         public void InitCheck()
//         {
//             if (_isInit) return;
//             _isInit = true;
//             Init();
//         }
//
//         protected virtual void Init()
//         {
//             EventHandler = GetComponent<UI_EventHandler>();
//             stateChanged = new UnityEvent<UIElementState>();
//             willStateChange = new UnityEvent<UIElementState>();
//             ElState = initState;
//         }
//
//         protected void Start()
//         {
//             InitCheck();
//         }
//     }
// }