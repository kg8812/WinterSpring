// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Apis
// {
//     public class WeaponController : AbsController
//     {
//         public Button button;
//
//         [HideInInspector] public TextMeshProUGUI[] texts;
//         [HideInInspector] public Image[] images;
//
//         public WeaponDescription desc;
//         private void Awake()
//         {
//             texts = GetComponentsInChildren<TextMeshProUGUI>();
//             images = GetComponentsInChildren<Image>();
//         }
//         public override void Init(MenuController menu)
//         {
//             base.Init(menu);
//             ctrls.Add(new Weapon_MenuCtrl(this));
//             ctrls.Add(new Weapon_EquipCtrl(this));
//             ctrls.Add(new Weapon_InvenCtrl(this));
//             InvenManager.instance.Wp.OnMouseSelect.AddListener(SelectController);
//             InvenManager.instance.Wp.BeforeSelect.AddListener(() =>
//             {
//                 foreach (var x in ctrls)
//                 {
//                     x.OnExit();
//                 }
//             }
//             );
//             InvenManager.instance.Wp.OnMenuOpen.AddListener(ToWindowCtrl);
//             InvenManager.instance.Wp.OnMenuClose.AddListener(Revert);
//             button.onClick.AddListener(InvenManager.instance.Wp.CloseMenuWindow);
//
//         }
//
//         public void MoveUp()
//         {
//             Current.OnExit();
//             if (index > 0)
//             {
//                 index--;
//             }
//             Current = ctrls[index];
//             Current.OnEnter();
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
//             Current = ctrls[index];
//
//             Current.OnEnter();
//         }
//
//         public void ToWindowCtrl()
//         {
//             Current = new Weapon_WindowCtrl(this);
//         }
//         public void ToChangeCtrl()
//         {
//             Current.OnExit();
//             Current = new Weapon_ChangeCtrl(this);
//
//             Current.OnEnter();
//         }
//
//         public void Revert()
//         {
//             if(Current is Weapon_ChangeCtrl) { return; }
//             Current = ctrls[index];
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
//         void SelectController()
//         {
//             index = 0;
//
//             switch (InvenManager.instance.Wp.selected.Type)
//             {
//                 case InvenType.Inven:
//                     if (Current is Weapon_ChangeCtrl) break;
//                     index = 0;
//                     foreach (var ctrl in ctrls)
//                     {
//                         if (ctrl is Weapon_InvenCtrl)
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
//                         if (ctrl is Weapon_EquipCtrl)
//                         {
//                             Current = ctrl;
//
//                             break;
//                         }
//                         index++;
//                     }
//                     break;
//             }
//
//         }
//     }
//
//
//
//     public class Weapon_MenuCtrl : ICtrl
//     {
//         readonly WeaponController controller;
//
//         public Weapon_MenuCtrl(WeaponController controller)
//         {
//             this.controller = controller;
//         }
//         public void OnEnter()
//         {
//             foreach (var x in controller.images)
//             {
//                 if (Mathf.Approximately( x.color.a , 1))
//                 {
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 0.4f);
//                 }
//             }
//
//             foreach (var x in controller.texts)
//             {
//                 if (Mathf.Approximately( x.color.a , 1))
//                 {
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 0.4f);
//                 }
//             }
//             controller.desc.selectText.SetActive(false);
//             controller.desc.tab.SetActive(false);
//         }
//
//         public void OnExit()
//         {
//             foreach (var x in controller.images)
//             {
//                 if (Mathf.Approximately( x.color.a , 0.4f))
//
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 1);
//             }
//             foreach (var x in controller.texts)
//             {
//                 if (Mathf.Approximately( x.color.a , 0.4f))
//
//                     x.color = new Color(x.color.r, x.color.g, x.color.b, 1);
//             }   
//             controller.desc.tab.SetActive(true);
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
//             controller.MoveDown();
//
//         }
//
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
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
//     }
//
//     public class Weapon_EquipCtrl : ICtrl
//     {
//
//         WeaponController controller;
//         Inventory inven;
//         public Weapon_EquipCtrl(WeaponController controller)
//         {
//             this.controller = controller;
//             inven = InvenManager.instance.Wp.Invens[InvenType.Equipment];
//         }
//         public void OnEnter()
//         {
//             inven.OnEnter();
//         }
//
//         public void OnExit()
//         {
//             inven.OnExit();
//         }
//
//         public void OnInputA()
//         {
//         }
//
//         public void OnInputD()
//         {
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
//         public void OnInputEnter()
//         {
//             if (InvenManager.instance.Wp.selected.Item != null)
//             {
//                 InvenManager.instance.Wp.OpenMenuWindow(InvenType.Equipment);
//             }
//         }
//
//         public void OnInputEsc()
//         {
//         }
//         public void OnInputSpace()
//         {
//             if (InvenManager.instance.Wp.selected.Item != null)
//             {
//                 InvenManager.instance.Wp.OpenMenuWindow(InvenType.Equipment);
//             }
//         }
//     }
//     public class Weapon_InvenCtrl : ICtrl
//     {
//         readonly WeaponController controller;
//         readonly Inventory inven;
//         public Weapon_InvenCtrl(WeaponController controller)
//         {
//             this.controller = controller;
//             inven = InvenManager.instance.Wp.Invens[InvenType.Inven];
//         }
//         public void OnEnter()
//         {
//             inven.OnEnter();
//         }
//
//         public void OnExit()
//         {
//             inven.OnExit();
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
//             if (InvenManager.instance.Wp.selected.Item != null)
//             {
//                 InvenManager.instance.Wp.OpenMenuWindow(InvenType.Inven);
//             }
//         }
//
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//         }
//
//         public void OnInputS()
//         {
//             inven.MoveDown();
//         }
//
//         public void OnInputSpace()
//         {
//             if (InvenManager.instance.Wp.selected.Item != null)
//             {
//                 InvenManager.instance.Wp.OpenMenuWindow(InvenType.Inven);
//                 controller.ToWindowCtrl();
//             }
//         }
//
//         public void OnInputW()
//         {
//             int x = inven.MoveUp();
//             if (x != -1)
//             {
//                 controller.MoveUp();
//             }
//         }
//     }
//
//     public class Weapon_WindowCtrl : ICtrl
//     {
//         readonly InvenMenuWindow menu;
//
//         public Weapon_WindowCtrl(WeaponController _)
//         {
//             menu = InvenManager.instance.Wp.MenuWindow;
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
//         }
//
//         public void OnInputW()
//         {
//             menu.MoveToTopButton();
//         }
//         public void OnInputEsc()
//         {
//             InvenManager.instance.OnOffMenu();
//             InvenManager.instance.Wp.OnMenuClose.Invoke();
//         }
//     }
//
//     public class Weapon_ChangeCtrl : ICtrl
//     {
//         readonly WeaponController controller;
//         readonly Inventory inven;
//         public Weapon_ChangeCtrl(WeaponController controller)
//         {
//             this.controller = controller;
//             inven = InvenManager.instance.Wp.Invens[InvenType.Inven];
//         }
//
//         public static void Equip(Weapon_ChangeCtrl ct)
//         {
//             if (InvenManager.instance.Wp.selected.Item == null) return;
//
//             InvenManager.instance.Wp.selected.Equip();
//             ct.RevertToEquip();
//         }
//         public void OnEnter()
//         {
//             inven.OnEnter();
//         }
//
//         public void OnExit()
//         {
//             inven.OnExit();
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
//             if (InvenManager.instance.Wp.selected.Item == null) return;
//             InvenManager.instance.Wp.selected.Equip();
//             inven.Refresh();
//             RevertToEquip();
//         }
//
//         public void OnInputEsc()
//         {
//             RevertToEquip();
//         }
//
//         public void OnInputS()
//         {
//             inven.MoveDown();
//         }
//
//         public void OnInputSpace()
//         {
//             if (InvenManager.instance.Wp.selected.Item == null) return;
//             InvenManager.instance.Wp.selected.Equip();
//             inven.Refresh();
//
//             RevertToEquip();
//         }
//
//         public void OnInputW()
//         {
//             int x = inven.MoveUp();
//             if (x != -1)
//             {
//                 RevertToEquip();
//             }
//         }
//
//         void RevertToEquip()
//         {
//             OnExit();
//             controller.index = 0;
//             foreach (var ctrl in controller.ctrls)
//             {
//                 if (ctrl is Weapon_EquipCtrl)
//                 {
//                     controller.Current = ctrl;
//                     controller.Current.OnEnter();
//                     break;
//                 }
//                 controller.index++;
//             }
//         }
//     }
// }
