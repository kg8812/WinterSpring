using System.Collections;
using System.Collections.Generic;
using chamwhy.UI.Focus;
using UnityEngine;

public class FocusParentNavTarget : MonoBehaviour ,ISelectableNavTarget
{
    [SerializeField] private FocusParent focusParent;
    public void OnNavigatedTo()
    {
        focusParent.OnNavigatedTo();
    }

    public void OnNavigatedFrom()
    {
        focusParent.OnNavigatedFrom();
    }

    public IUI_NavigationManager NavigationManager { get; set; }  // IUI_Navigatable 요구사항에 맞춰

    GameObject TargetObject => focusParent.gameObject;

    public void OnSelected(bool focus)
    {
        TargetObject.SetActive(true);
        if (focus) focusParent.MoveTo(0);
    }

    public void OnDeselected()
    {
        focusParent.FocusReset();
        TargetObject.SetActive(false);
    }

    public void KeyControl() => focusParent.KeyControl();
    public void GamePadControl() => focusParent.GamePadControl();
    public bool IsAtBoundary(NavigationDirection direction) => focusParent.IsAtBoundary(direction);
    public void InitCheck()
    {
        focusParent.InitCheck();
    }
}
