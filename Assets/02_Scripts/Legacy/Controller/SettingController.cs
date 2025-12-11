// using Save.Schema;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Apis
// {
//     public class SettingController : AbsController
//     {
//         [SerializeField] Slider masterVolume;
//         [SerializeField] Slider SFXVolume;
//         [SerializeField] Slider BGMVolume;
//         public override void Init(MenuController menu)
//         {
//             base.Init(menu);
//             ctrls.Add(new Setting_MenuCtrl(this));
//         }
//
//         private void OnEnable()
//         {
//             masterVolume.value = DataAccess.Settings.Data.Volumes[(int)Define.Sound.Master];
//             SFXVolume.value = DataAccess.Settings.Data.Volumes[(int)Define.Sound.SFX];
//             BGMVolume.value = DataAccess.Settings.Data.Volumes[(int)Define.Sound.BGM];
//         }
//         public void ChangeMasterVolume(float amount)
//         {
//             GameManager.Sound.ChangeVolume(amount, Define.Sound.Master);
//         }
//         public void ChangeSFXVolume(float amount)
//         {
//             GameManager.Sound.ChangeVolume(amount, Define.Sound.SFX);
//         }
//         public void ChangeBGMVolume(float amount)
//         {
//             GameManager.Sound.ChangeVolume(amount, Define.Sound.BGM);
//         }
//     }
//     public class Setting_MenuCtrl : ICtrl
//     {
//         readonly SettingController controller;
//         public Setting_MenuCtrl(SettingController controller)
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
//
// }
