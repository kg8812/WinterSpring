// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Apis
// {
//     public interface ICtrl // 메뉴내 컨트롤러 인터페이스
//     {
//         // 메뉴 조종은 ICtrl을 상속받은 컨트롤러를 갈아끼우는 형식으로 진행됨.
//         public void OnEnter(); // 이 컨트롤러 사용에 들어올 때
//         public void OnExit(); // 이 컨트롤러 사용을 그만둘 때
//         public void OnInputSpace(); // 스페이스 입력
//         public void OnInputEnter(); // 엔터 입력
//         public void OnInputW(); // w 입력
//         public void OnInputA(); // a 입력
//         public void OnInputS(); // s 입력
//         public void OnInputD(); // d 입력
//         public void OnInputEsc(); // esc 입력
//     }
//
//     public interface IInvenCtrl : ICtrl // 칸 선택 인덱스 값 받기 위한 인벤토리용 인터페이스
//     {
//         public void OnEnter(int x, int y);
//         public int[] GetIndex();
//     }
//     public class AccInvenController : AbsController 
//     {
//         public Button button;
//         [HideInInspector] public TextMeshProUGUI[] texts;
//         [HideInInspector] public Image[] images;
//
//         public AccDescription description;
//         private void Awake()
//         {
//             texts = GetComponentsInChildren<TextMeshProUGUI>();
//             images = GetComponentsInChildren<Image>();
//         }
//
//         //인벤 탭 컨트롤러
//         public override void Init(MenuController menu)
//         {
//             // ICtrl 컨트롤러 추가 (탭 메뉴, 장비창, 인벤 컨트롤러)
//
//             base.Init(menu);
//             ctrls.Add(new Inven_MenuCtrl(this));
//             ctrls.Add(new EquipCtrl(this));
//             ctrls.Add(new InvenCtrl(this));
//             index = 0;
//             Current = ctrls[index];
//             InvenManager.instance.Acc.OnMouseSelect.AddListener(SelectController);
//             InvenManager.instance.Acc.BeforeSelect.AddListener(() =>
//             {
//                 foreach (var x in ctrls)
//                 {
//                     x.OnExit();
//                 }
//             }
//             );
//             InvenManager.instance.Acc.OnMenuOpen.AddListener(ChangeToWindow);
//             InvenManager.instance.Acc.OnMenuClose.AddListener(Revert);
//             button.onClick.AddListener(InvenManager.instance.Acc.CloseMenuWindow);
//         }
//         
//         public void MoveUp()
//         {
//             Current.OnExit();
//
//             if (index > 0)
//             {
//                 index--;
//             }
//             
//             int[] idx = { -1, -1 };
//
//             if (Current is IInvenCtrl invenctrl)
//             {
//                 idx = invenctrl.GetIndex();
//             }
//             Current = ctrls[index];
//
//             if (Current is IInvenCtrl invenctrl2 && idx[0] != -1 && idx[1] != -1)
//             {
//                 invenctrl2.OnEnter(idx[0], 0);
//             }
//             else
//             {
//                 Current.OnEnter();
//             }
//         }
//
//         public void MoveDown()
//         {
//             Current.OnExit();
//             index++;
//             if (index > ctrls.Count - 1)
//             {
//                 index = 0;
//             }
//             int[] idx = { -1, -1 };
//
//             if (Current is IInvenCtrl invenctrl)
//             {
//                 idx = invenctrl.GetIndex();
//             }
//             Current = ctrls[index];
//
//             if (Current is IInvenCtrl invenctrl2 && idx[0] != -1 && idx[1] != -1)
//             {
//                 invenctrl2.OnEnter(idx[0], 0);
//             }
//             else
//             {
//                 Current.OnEnter();
//             }
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//             foreach (var x in ctrls)
//             {
//                 x.OnExit();
//             }
//         }
//
//         public void ChangeToWindow()
//         {
//             Current = new WindowCtrl(this);
//         }
//
//         public void Revert()
//         {
//             if (Current is Acc_ChangeCtrl) { return; }
//
//             Current = ctrls[index];
//         }
//
//         void SelectController()
//         {
//             switch (InvenManager.instance.Acc.selected.Type)
//             {
//                 case InvenType.Inven:
//                     if (Current is Acc_ChangeCtrl) { break; }
//
//                     index = 0;
//                     foreach (var ctrl in ctrls)
//                     {
//                         if (ctrl is InvenCtrl)
//                         {
//                             Current = ctrl;
//                             break;
//                         }
//                         index++;
//                     }
//                     break;
//                 case InvenType.Equipment:
//                     index = 0;
//                     foreach (var ctrl in ctrls)
//                     {
//                         if (ctrl is EquipCtrl)
//                         {
//                             Current = ctrl;
//                             break;
//                         }
//                         index++;
//                     }
//                     break;
//             }
//         }
//
//         public void ToChangeCtrl()
//         {
//             Current.OnExit();
//             InvenType tp = InvenManager.instance.Acc.changeSlot.Type == InvenType.Inven ? InvenType.Equipment : InvenType.Inven;
//             Current = new Acc_ChangeCtrl(this,tp);
//
//             Current.OnEnter();
//         }
//     }
//
//     class InvenCtrl : IInvenCtrl
//     {
//         readonly AccInvenController controller;
//         readonly Inventory inven;
//         public InvenCtrl(AccInvenController controller)
//         {
//             this.controller = controller;
//             inven = InvenManager.instance.Acc.Invens[InvenType.Inven];
//         }
//         public void OnEnter()
//         {
//             inven.OnEnter();
//         }
//
//         public void OnEnter(int x, int y)
//         {
//             inven.OnEnter();
//             inven.Select(x, y);
//         }
//         public void OnExit()
//         {
//             inven.OnExit();
//         }
//         public void OnInputA()
//         {
//             inven.MoveLeft();
//         }
//
//         public void OnInputD()
//         {
//             inven.MoveRight();
//         }
//
//         public void OnInputEnter()
//         {
//             if (InvenManager.instance.Acc.selected.Item != null)
//             {
//                 InvenManager.instance.Acc.OpenMenuWindow(InvenType.Inven);
//             }
//         }
//         public void OnInputW()
//         {
//             int x = inven.MoveUp();
//             if (x != -1)
//             {
//                 controller.MoveUp();
//             }
//         }
//         public void OnInputS()
//         {
//             inven.MoveDown();
//         }
//
//         public void OnInputSpace()
//         {
//             if (InvenManager.instance.Acc.selected.Item != null)
//             {
//                 InvenManager.instance.Acc.OpenMenuWindow(InvenType.Inven);
//                 controller.ChangeToWindow();
//             }
//         }
//
//         public int[] GetIndex()
//         {
//             int[] idx = new int[2];
//             idx[0] = inven.X;
//             idx[1] = inven.Y;
//
//             return idx;
//         }
//
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//         }
//     }
//
//     class EquipCtrl : IInvenCtrl
//     {
//         readonly AccInvenController controller;
//         readonly Inventory inven;
//         public EquipCtrl(AccInvenController controller)
//         {
//             this.controller = controller;
//             inven = InvenManager.instance.Acc.Invens[InvenType.Equipment];
//         }
//         public void OnEnter()
//         {
//             inven.OnEnter();
//         }
//         public void OnEnter(int x, int y)
//         {
//             inven.OnEnter();
//             inven.Select(x, y);
//         }
//         public int[] GetIndex()
//         {
//             int[] idx = new int[2];
//             idx[0] = inven.X;
//             idx[1] = inven.Y;
//
//             return idx;
//         }
//         public void OnExit()
//         {
//             inven.OnExit();
//         }
//
//         public void OnInputA()
//         {
//             inven.MoveLeft();
//         }
//         public void OnInputD()
//         {
//             inven.MoveRight();
//         }
//         public void OnInputEnter()
//         {
//             if (InvenManager.instance.Acc.selected.Item != null)
//             {
//                 InvenManager.instance.Acc.OpenMenuWindow(InvenType.Equipment);
//                 controller.ChangeToWindow();
//             }
//         }
//         public void OnInputW()
//         {
//             int x = inven.MoveUp();
//             if (x != -1)
//             {
//                 controller.MoveUp();
//             }
//         }
//         public void OnInputS()
//         {
//             int x = inven.MoveDown();
//             if (x != -1)
//             {
//                 controller.MoveDown();
//             }
//         }
//         public void OnInputSpace()
//         {
//             if (InvenManager.instance.Acc.selected.Item != null)
//             {
//                 InvenManager.instance.Acc.OpenMenuWindow(InvenType.Equipment);
//                 controller.ChangeToWindow();
//             }
//         }
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//         }
//     }
//     class Inven_MenuCtrl : ICtrl
//     {
//         readonly AccInvenController controller;
//         public Inven_MenuCtrl(AccInvenController controller)
//         {
//             this.controller = controller;
//         }
//         public void OnEnter()
//         {
//             foreach (var x in controller.images)
//             {
//                 if (Mathf.Approximately(x.color.a ,1))
//                 {
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 0.4f);
//                 }
//             }
//
//             foreach (var x in controller.texts)
//             {
//                 if (Mathf.Approximately(x.color.a , 1))
//                 {
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 0.4f);
//                 }
//             }
//             controller.description.InfoOff();
//         }
//
//         public void OnExit()
//         {
//             foreach (var x in controller.images)
//             {
//                 if (Mathf.Approximately(x.color.a , 0.4f))
//
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 1);
//             }
//             foreach (var x in controller.texts)
//             {
//                 if (Mathf.Approximately(x.color.a , 0.4f))
//
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 1);
//             }
//
//         }
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
//             controller.MoveDown();
//         }
//
//         public void OnInputS()
//         {
//             controller.MoveDown();
//         }
//
//         public void OnInputSpace()
//         {
//             controller.MoveDown();
//
//         }
//
//         public void OnInputW()
//         {
//             
//         }
//
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//         }
//     }
//
//     class WindowCtrl : ICtrl
//     {
//         readonly InvenMenuWindow menu;
//         readonly AccInvenController controller;
//
//         public WindowCtrl(AccInvenController controller)
//         {
//             this.controller = controller;
//             menu = InvenManager.instance.Acc.MenuWindow;
//         }
//
//         public void OnEnter()
//         {
//
//         }
//
//         public void OnExit()
//         {
//         }
//
//         public void OnInputA()
//         {
//         }
//
//         public void OnInputD()
//         {
//         }
//
//         public void OnInputEnter()
//         {
//             menu.InvokeButton();
//             controller.Revert();
//         }
//
//         public void OnInputS()
//         {
//             menu.MoveToBottomButton();
//         }
//
//         public void OnInputSpace()
//         {
//             menu.InvokeButton();
//             controller.Revert();
//         }
//
//         public void OnInputW()
//         {
//             menu.MoveToTopButton();
//         }
//
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//
//             InvenManager.instance.Acc.OnMenuClose.Invoke();
//         }
//     }
//     public class Acc_ChangeCtrl : ICtrl
//     {
//         readonly AccInvenController controller;
//         readonly Inventory inven;
//         public Acc_ChangeCtrl(AccInvenController controller,InvenType type)
//         {
//             this.controller = controller;
//             inven = InvenManager.instance.Acc.Invens[type];
//         }
//         public static void Change(Acc_ChangeCtrl ct)
//         {
//             InvenType type = InvenManager.instance.Acc.changeSlot.Type;
//             AccSlot sl = InvenManager.instance.Acc.selected as AccSlot;
//
//             if (sl != null) sl.Change();
//             ct.RevertToNormal(type);
//         }
//         public void OnEnter()
//         {
//             inven.OnEnter();
//             foreach(var x in inven.Slots)
//             {
//                 if(x.Item == null)
//                 {
//                     inven.Select(x.Idx2, x.Idx1);
//                     break;
//                 }
//             }
//         }
//
//         public void OnExit()
//         {
//             inven.OnExit();
//             InvenManager.instance.Acc.changeSlot = null;
//         }
//
//         public void OnInputA()
//         {
//             inven.MoveLeft();
//         }
//
//         public void OnInputD()
//         {
//             inven.MoveRight();
//         }
//
//         public void OnInputEnter()
//         {
//             Change(this);
//         }
//
//         public void OnInputEsc()
//         {
//             RevertToNormal(InvenManager.instance.Acc.changeSlot.Type);
//         }
//
//         public void OnInputS()
//         {
//             int x = inven.MoveDown();
//             if (x != -1)
//             {
//                 RevertToNormal(InvenManager.instance.Acc.changeSlot.Type);
//             }
//         }
//
//         public void OnInputSpace()
//         {
//             Change(this);
//         }
//
//         public void OnInputW()
//         {
//             int x = inven.MoveUp();
//             if (x != -1)
//             {
//                 RevertToNormal(InvenManager.instance.Acc.changeSlot.Type);
//             }
//         }
//
//         void RevertToNormal(InvenType type)
//         {
//             OnExit();
//             switch(type)
//             {
//                 case InvenType.Inven:
//
//                     controller.index = 0;
//                     foreach (var ctrl in controller.ctrls)
//                     {
//                         if (ctrl is InvenCtrl)
//                         {
//                             controller.Current = ctrl;
//                             break;
//                         }
//                         controller.index++;
//                     }
//                     break;
//                 case InvenType.Equipment:
//                     controller.index = 0;
//                     foreach (var ctrl in controller.ctrls)
//                     {
//                         if (ctrl is EquipCtrl)
//                         {
//                             controller.Current = ctrl;
//                             break;
//                         }
//                         controller.index++;
//                     }
//                     break;
//             }
//             int[] idx = { -1, -1 };
//
//             if (controller.Current is IInvenCtrl invenctrl)
//             {
//                 idx = invenctrl.GetIndex();
//             }
//             controller.Current.OnEnter();
//
//             if (controller.Current is IInvenCtrl invenctrl2 && idx[0] != -1 && idx[1] != -1)
//             {
//                 invenctrl2.OnEnter(idx[0], 0);
//             }
//         }
//     }
// }
