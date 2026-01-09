using System.Collections;
using System.Collections.Generic;
using chamwhy;
using chamwhy.UI;
using chamwhy.UI.Focus;
using UnityEngine;

public class UITab_Equipment : UI_FocusContent
{
    UI_NavigationController navigation;

    public UIAsset_Toggle wpCategory;
    public UIAsset_Toggle accCategory;
    public UI_FocusSelector categorySelect;
        
    public List<UI_FocusContent> childs = new List<UI_FocusContent>();
    
    public override void Init()
    {
        base.Init();
        navigation ??= GetComponent<UI_NavigationController>();
        wpCategory.OnValueChanged.AddListener(x =>
        {
            if (x)
            {
                categorySelect.SelectFocusByIndex(0,false);
            }
        }); 
        
        accCategory.OnValueChanged.AddListener(x =>
        {
            if (x)
            {
                categorySelect.SelectFocusByIndex(1,false);
            }
        });
        foreach (var uiBase in childs)
        {
            uiBase.InitCheck();
        }
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
        base.OnOpen();
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
