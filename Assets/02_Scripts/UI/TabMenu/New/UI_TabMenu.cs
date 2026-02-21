using System;
using System.Collections.Generic;
using chamwhy.Managers;
using chamwhy.UI;
using Default;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace chamwhy
{
    public delegate void OnUIOpen();
    public class UI_TabMenu: UI_Scene
    {
        public static Action<bool> OnUiToggle;
        [SerializeField] private UI_DragItem dragImg;
        public UI_HeaderMenu_nton tabHeaderMenuNtoN;

        [SerializeField] Sprite[] icons;
        [SerializeField] List<int> menuTextIndexes;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI menuText;

        protected virtual bool useTabButton => true;
        
        public override void Init()
        {
            base.Init();
            tabHeaderMenuNtoN.Init();
            dragImg.Init();
            foreach (var value in tabHeaderMenuNtoN.contentControllers)
            {
                AddSubItem(value);
            }
            tabHeaderMenuNtoN.FocusChanged.RemoveListener(ChangeMenuIcon);
            tabHeaderMenuNtoN.FocusChanged.AddListener(ChangeMenuIcon);
        }

        public void ChangeMenuIcon(int index)
        {
            iconImage.sprite = icons[index];
            menuText.text = LanguageManager.Str(menuTextIndexes[index]);
        }
        public void MoveHeader(int index)
        {
            tabHeaderMenuNtoN.headerController.MoveTo(index);
        }

        public override void TryActivated(bool force = false)
        {
            UITab_Inventory.DragUI = dragImg;
            // ItemSlot.DragImg = dragImg;
            if (!tabHeaderMenuNtoN.gameObject.activeSelf)
            {
                tabHeaderMenuNtoN.gameObject.SetActive(true);
            }

            tabHeaderMenuNtoN.Reset();
            OnUiToggle?.Invoke(true);
            base.TryActivated(force);
        }

        public override void TryDeactivated(bool force = false)
        {
            OnUiToggle?.Invoke(false);
            base.TryDeactivated(force);
        }

        public override void KeyControl()
        {
            if (useTabButton && InputManager.GetKeyDown(KeySettingManager.GetGameKeyCode(Define.GameKey.Tab)))
            {
                CloseOwn();
                return;
            }
            tabHeaderMenuNtoN.KeyControl();
            base.KeyControl();
        }

        public override void GamePadControl()
        {
            if (useTabButton && InputManager.GetButtonDown(KeySettingManager.GetGameButton(Define.GameKey.Tab)))
            {
                CloseOwn();
                return;
            }
            tabHeaderMenuNtoN.GamePadControl();
            base.GamePadControl();
        }
    }
}