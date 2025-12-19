using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace chamwhy.UI
{
    public class UIAsset_Toggle: UIEffector
    {
        // 자기 자신을 클릭해서 off 할 수 있는 지 여부.
        // focus parent의 can none focus가 false라면 꺼지면 안됨.
        public bool canOffOwn;
        [Tooltip("선택 혹은 해제될 때 호출 (true or false)")]
        public UnityEvent<bool> OnValueChanged;

        public UnityEvent OnClicked;
        
        public override void Init()
        {
            base.Init();
            OnValueChanged ??= new UnityEvent<bool>();
        }

        protected override void Start()
        {
            base.Start();
            if (EqualSt(ElState, UIElementState.Select))
            {
                ToggleOn();
            }
        }

        public override void KeyControl()
        {
            if (canOffOwn || !IsSelected)
            {
                if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Select)))
                {
                    OnPointerClick(null);
                }
            }
            else if (IsSelected)
            {
                if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Select)))
                {
                    OnClicked?.Invoke();
                }
            }
        }
        
        public override void GamePadControl()
        {
            base.GamePadControl();
            if (canOffOwn || !IsSelected)
            {
                if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Select)))
                {
                    OnPointerClick(null);
                }
            }
            else if (IsSelected)
            {
                if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Select)))
                {
                    OnClicked?.Invoke();
                }
            }
        }

        public override void SelectOn()
        {
            base.SelectOn();
            if (IsSelected)
            {
                ToggleOn();
            }
        }

        public override void SelectOff(bool force = false)
        {
            base.SelectOff(force);
            if (!IsSelected)
            {
                ToggleOff();
            }
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
            ChangeSelected();
        }

        protected virtual void ChangeSelected()
        {
            if (!IsDisable)
            {
                if (IsSelected)
                {
                    if(canOffOwn)
                        SelectOff();
                }
                else
                {
                    SelectOn();
                }
            }
        }

        protected virtual void ToggleOn() //선택될 때 호출
        {
            OnValueChanged.Invoke(true);
        }

        protected virtual void ToggleOff() // 선택해제될 때 호출
        {
            OnValueChanged.Invoke(false);
        }
    }
}