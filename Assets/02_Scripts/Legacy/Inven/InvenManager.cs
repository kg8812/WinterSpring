// using Managers;
// using Save.Schema;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace Apis
// {
//     public class InvenManager : Singleton<InvenManager>
//     {       
//         [Header("메뉴창")]
//         [SerializeField] GameObject menu;
//
//         [SerializeField] AccInvenManager acc = new();
//         [SerializeField] WpInvenManager wp = new();
//
//         [SerializeField] private MenuController menuController;
//
//         private Canvas canv;
//
//         private UnityEvent _onMenuOpen;
//         
//         public UnityEvent OnMenuOpen => _onMenuOpen ??= new();
//         public AccInvenManager Acc
//         {
//             get
//             {
//                 acc ??= new();
//                 return acc;
//             }
//         }
//         public WpInvenManager Wp
//         {
//             get
//             {
//                 wp ??= new();
//                 return wp;
//             }
//         }
//
//         protected override void Awake()
//         {
//             base.Awake();
//             DontDestroyOnLoad(gameObject);
//             
//             Acc.Init();
//             Wp.Init();
//         }
//
//         public void HardReset()
//         {
//             acc.Invens[InvenType.Inven].Clear();
//             acc.Invens[InvenType.Equipment].Clear();
//             wp.Invens[InvenType.Inven].Clear();
//             wp.Invens[InvenType.Equipment].Clear();
//         }
//         private void Start()
//         {
//             var descriptions = GetComponentsInChildren<ItemDescription>(true);
//             foreach (var x in descriptions )
//             {
//                 // x.Inven.OnSelect.AddListener(x.OnSelect);
//             }
//             
//             canv = GetComponent<Canvas>();
//             canv.renderMode = RenderMode.ScreenSpaceCamera;
//             canv.worldCamera = CameraManager.instance.UICam;
//             canv.sortingLayerName = "UI";
//             
//             // ui manager의 sceneOrder와 같은 값
//             canv.sortingOrder = 10;
//         }
//
//         public void OnOffMenu()
//         {
//             menu.SetActive(!menu.activeSelf);
//
//             if (menu.activeSelf)
//             {
//                 OnMenuOpen.Invoke();
//                 GameManager.UiController = menuController;
//                 GameManager.instance.Pause();
//                 GameManager.instance.InteractionStateClass.CheckState();
//             }
//             else
//             {
//                 GameManager.UiController = null;
//                 GameManager.instance.InteractionStateClass.ToNonBattleState();
//             }
//         }
//
//         public bool IsMenuActive => menu.activeSelf;
//     }
// }