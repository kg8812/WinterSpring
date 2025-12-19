using System.Collections.Generic;
using chamwhy.UI;
using chamwhy.UI.Focus;
using Default;
using UnityEngine;
using UnityEngine.UI;

namespace chamwhy
{
    public class UITab_Character: UI_FocusContent
    {
        UI_NavigationController navigation;

        public UIAsset_Toggle wpCategory;
        public UIAsset_Toggle accCategory;
        public UI_FocusSelector equipCategorySelect;
        
        public List<UI_FocusContent> childs = new List<UI_FocusContent>();
        public UIAsset_Toggle playerIconButton;
        
        public override void Init()
        {
            base.Init();
            navigation ??= GetComponent<UI_NavigationController>();
            wpCategory.OnValueChanged.AddListener(x =>
            {
                if (x)
                {
                    equipCategorySelect.SelectFocusByIndex(0,false);
                }
            });
            accCategory.OnValueChanged.AddListener(x =>
            {
                if (x)
                {
                    equipCategorySelect.SelectFocusByIndex(1,false);
                }
            });
            foreach (var uiBase in childs)
            {
                uiBase.InitCheck();
            }
            playerIconButton.OnClicked.AddListener(OpenSkillInfo);
        }

        void OpenSkillInfo()
        {
            GameManager.UI.CreateUI("UI_SkillInfo", UIType.Popup);
        }
        public override void KeyControl()
        {
            base.KeyControl();
            // 캐릭터 탭은 예외적으로 아무런 상호작용을 하지 않음.
            navigation?.KeyControl();
        }

        public override void GamePadControl()
        {
            base.GamePadControl();
            navigation?.GamePadControl();
        }

        public override void OnOpen()
        {
            navigation.Activate();
            childs.ForEach(x => x.OnOpen());
        }

        public override void OnClose()
        {
            base.OnClose();
            navigation.Deactivate();
            childs.ForEach(x => x.OnClose());
        }
    }
}