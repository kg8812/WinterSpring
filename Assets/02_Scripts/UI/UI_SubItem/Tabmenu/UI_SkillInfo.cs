using System.Collections;
using System.Collections.Generic;
using Apis;
using chamwhy.UI.Focus;
using Default;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class UI_SkillInfo : UI_Popup
{
    public UI_SkillList activeList;
    public UI_SkillList passiveList;
    UI_NavigationController navigation;

    public override void Init()
    {
        base.Init();
        navigation = GetComponent<UI_NavigationController>();
    }

    public override void TryActivated(bool force = false)
    {
        base.TryActivated(force);
        ActiveSkill activeSkill = GameManager.instance.Player.ActiveSkill;
        PassiveSkill passiveSkill = GameManager.instance.Player.PassiveSkill;
        
        activeList.Set(activeSkill);
        passiveList.Set(passiveSkill);
        navigation.Activate();
    }
    
    public override void TryDeactivated(bool force = false)
    {
        base.TryDeactivated(force);
        navigation.Deactivate();
    }


    public override void KeyControl()
    {
        base.KeyControl();
        

        KeyCode leftKey = KeySettingManager.GetUIKeyCode(Define.UIKey.Left);
        KeyCode rightKey = KeySettingManager.GetUIKeyCode(Define.UIKey.Right);
        if (InputManager.GetKeyDown(leftKey))
        {
            if ((UI_SkillList)navigation.CurrentNavigatable == activeList)
            {
                activeList.equipSkillInfo.PlayVideo();
            }
        }
        else if (InputManager.GetKeyDown(rightKey))
        {
            if ((UI_SkillList)navigation.CurrentNavigatable == passiveList)
            {
                passiveList.equipSkillInfo.PlayVideo();
            }
        }
        navigation.KeyControl();

    }

    public override void GamePadControl()
    {
        base.GamePadControl();
        ButtonControl leftKey = KeySettingManager.GetUIButton(Define.UIKey.Left);
        ButtonControl rightKey = KeySettingManager.GetUIButton(Define.UIKey.Right);
        
        if (InputManager.GetButtonDown(leftKey))
        {
            if ((UI_SkillList)navigation.CurrentNavigatable == activeList)
            {
                activeList.equipSkillInfo.PlayVideo();
            }
        }
        else if (InputManager.GetButtonDown(rightKey))
        {
            if ((UI_SkillList)navigation.CurrentNavigatable == passiveList)
            {
                passiveList.equipSkillInfo.PlayVideo();
            }
        }
        navigation.GamePadControl();

    }
}
