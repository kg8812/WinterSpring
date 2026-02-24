using System;
using System.Collections;
using System.Collections.Generic;
using chamwhy.UI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UIAsset_Toggle))]
public class UIAsset_ToggleEventTool : MonoBehaviour
{
    private UIAsset_Toggle _assetToggle;

    UIAsset_Toggle AssetToggle => _assetToggle ??= GetComponent<UIAsset_Toggle>();

    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    private void Awake()
    {
        AssetToggle.OnValueChanged.AddListener(x =>
        {
            if(x) OnSelected.Invoke();
            else
            {
                OnDeselected.Invoke();
            }
        });
    }
}
