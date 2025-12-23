using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace chamwhy.UI.Focus
{
    public delegate void MoveEvent(int value);
    public enum NavigationMode
    {
        Horizontal, Vertical, Inventory
    }

    [System.Serializable]
    public struct TableNavigationData
    {
        [Tooltip("그리드의 가로, 세로 크기")]
        public int x, y;
        [InfoBox("각 방향의 끝에 도달했을 때 순환할지 여부")]
        public bool isLeftLoop, isRightLoop, isUpLoop, isDownLoop;
        [Tooltip("각 방향의 끝에 도달했을 때, 순환하는 대신 호출할 외부 함수를 지정합니다. (예: 장비창에서 아래로 누르면 인벤토리창으로 포커스를 넘기는 기능)")]
        public MoveEvent moveLeft;
        [Tooltip("각 방향의 끝에 도달했을 때, 순환하는 대신 호출할 외부 함수를 지정합니다. (예: 장비창에서 아래로 누르면 인벤토리창으로 포커스를 넘기는 기능)")]
        public MoveEvent moveRight;
        [Tooltip("각 방향의 끝에 도달했을 때, 순환하는 대신 호출할 외부 함수를 지정합니다. (예: 장비창에서 아래로 누르면 인벤토리창으로 포커스를 넘기는 기능)")]
        public MoveEvent moveUp;
        [Tooltip("각 방향의 끝에 도달했을 때, 순환하는 대신 호출할 외부 함수를 지정합니다. (예: 장비창에서 아래로 누르면 인벤토리창으로 포커스를 넘기는 기능)")]
        public MoveEvent moveDown;

        public bool fixedY;
    }
    public class FocusParent: MonoBehaviour, IUI_Navigatable
    {
        
        [Tooltip("true로 설정하면 클릭(Select)해야만 포커스를 받는 방식으로, false이면 마우스 호버만으로도 포커스를 받는 방식으로 동작을 변경할 수 있습니다.")]
        public bool isFocusSelect;
        [Tooltip("그룹 내에 아무것도 선택되지 않은 상태 허용여부")]
        public bool canNoneFocus; 
        [Tooltip("체크시 끝에서 이동시 순환됨")]
        public bool canLoop; 
        [Tooltip("숫자 키를 눌렀을 때 해당 인덱스의 요소로 포커스를 이동시킬지 여부.")]
        public bool useNumberKey; 
        [Tooltip("포커스 이동 방식을 결정합니다")]
        public NavigationMode navigation; 

        [Tooltip("Inventory일 때 사용되는 상세 설정 값입니다.")]
        [ShowIf("navigation", NavigationMode.Inventory)]
        public TableNavigationData tableData;

        
        [Tooltip("방향키 대신 LeftHeader, RightHeader로 포커스를 좌/우로 이동시킬지 결정합니다. 주로 탭(Tab) 메뉴 같은 UI에 사용됩니다.")]
        public bool isQE;
        
        private bool _isFocused;

        [HideInInspector] public IFocusGroup FocusGroup;
        [Tooltip("이 FocusParent가 관리하는 모든 UI 요소들의 리스트입니다.")]
        [SerializeField] public List<UIElement> focusList = new();

        [Tooltip("포커스된 요소가 변경될 때마다 호출되는 유니티 이벤트입니다. 현재 포커스된 요소의 인덱스(int)를 매개변수로 전달합니다.")]
        public UnityEvent<int> FocusChanged;
        // public UnityEvent<int> FocusSelected;

        private List<int> _labels = new();
        private int _lastLabel;
        public int curId { get; private set; } //현재 포커스를 받고 있는 요소의 focusList 내 인덱스(Index)입니다.

        private bool inited = false;

        public IUI_NavigationManager NavigationManager { get; set; }

        private UnityEvent _whenDisable;
        public UnityEvent WhenDisable => _whenDisable ??= new();
        
        public bool IsAtBoundary(NavigationDirection direction)
        {
            // focusList가 없거나 비어있으면 경계 판단 불가 (혹은 항상 경계라고 볼 수도 있음)
            if (focusList == null || focusList.Count == 0) return false; 

            int id = curId;
            // tableData.x가 0 이하일 경우를 대비해 최소 1로 설정 (0으로 나누기 방지)
            int xDim = tableData.x > 0 ? tableData.x : 1; 

            switch (direction)
            {
                case NavigationDirection.Up:    return id < xDim;
                case NavigationDirection.Down:  return id >= (focusList.Count - xDim);
                case NavigationDirection.Left:  return (id % xDim) == 0;
                case NavigationDirection.Right: 
                    // 마지막 열에 있거나, 전체 리스트의 마지막 항목인 경우
                    return ((id + 1) % xDim) == 0 || id == (focusList.Count - 1);
                default: 
                    return false;
            }
        }

        public void OnNavigatedTo()
        {
            // 이 그룹으로 포커스가 넘어오면, 기존에 선택중이던 요소에 포커스를 줌
            MoveTo(curId,true);
        }

        public void OnNavigatedFrom()
        {
            // 이 그룹에서 포커스가 떠나가면, 모든 포커스를 리셋
            FocusReset();
        }
        
        private void Start()
        {
            InitCheck();
        }

        public void InitCheck()
        {
            if (inited) return;
            Init();
        }


        protected virtual void Init()
        {
            if (inited) return;
            inited = true;
            
            FocusChanged = new();
            // FocusSelected = new();
            Reset();
            foreach (var focusItem in focusList)
            {
                focusItem.InitCheck();
                int id = RegisterElement(focusItem, isAlreadyList:true);
                if (UIElement.EqualSt(focusItem.ElState, isFocusSelect ? UIElementState.Select : UIElementState.Hover))
                {
                    MoveToLabel(id);
                    _isFocused = true;
                }
            }

            ChangeFocusType(isFocusSelect);
        }
        
        /// <summary>
        /// 그룹 내 모든 요소의 포커스를 해제합니다. canNoneFocus가 false라면, 포커스를 해제한 뒤 즉시 0번 인덱스 요소에 다시 포커스를 줍니다.
        /// </summary>
        public void FocusReset()
        {
            // if (!_isFocused) return;
            _isFocused = false;
            foreach (var focus in focusList)
            {
                focus.SelectOff(true);
                focus.HoverOff(true);
            }
        }

        public void Reset(bool isHard = false)
        {
            _lastLabel = 0;
            _isFocused = false;
            if (focusList == null)
                focusList = new();
            else if(isHard)
                focusList.Clear();
            
            if (_labels == null)
                _labels = new();
            else
                _labels.Clear();
        }

        /// <summary>
        /// 관리할 새로운 UI 요소를 focusList에 등록하고, 마우스 상호작용을 위한 이벤트를 연결합니다.
        /// </summary>
        /// <param name="el"></param>
        /// <param name="index"></param>
        /// <param name="isAlreadyList"></param>
        /// <returns></returns>
        public int RegisterElement(UIElement el, int index = -1, bool isAlreadyList = false)
        {
            el.focusOffByParent = !canNoneFocus;
            int label = CreateLabel();
            if (index == -1)
            {
                if(!isAlreadyList)
                    focusList.Add(el);
                _labels.Add(label);
            }
            else
            {
                if(!isAlreadyList)
                    focusList.Insert(index, el);
                _labels.Insert(index, label);
                // if (index <= _curId)
                //     _curId++;
            }

            RegisterMouseEvent(el, label);
            if (navigation == NavigationMode.Inventory)
            {
                if (focusList.Count > tableData.x * tableData.y)
                {
                    if (tableData.fixedY)
                    {
                        tableData.x += 1;
                    }
                    else
                    {
                        tableData.y += 1;
                    }
                }
            }
            return label;
        }
        /// <summary>
        /// 관리하던 UI 요소를 리스트에서 제거합니다.
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        public bool RemoveElement(UIElement el)
        {
            for (int i = 0; i < focusList.Count; i++)
            {
                if (ReferenceEquals(focusList[i], el))
                {
                    RemoveMouseEvent(el);
                    focusList.RemoveAt(i);
                    _labels.RemoveAt(i);
                    
                    if (navigation == NavigationMode.Inventory)
                    {
                        if (tableData.fixedY)
                        {
                            if (focusList.Count <= (tableData.x - 1) * (tableData.y))
                            {
                                tableData.x -= 1;
                            }
                        }
                        else
                        {
                            if (focusList.Count <= (tableData.x) * (tableData.y - 1))
                            {
                                tableData.y -= 1;
                            }
                        }
                    }
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 포커스 방식을 '클릭' 또는 '호버'로 동적으로 변경합니다.
        /// </summary>
        /// <param name="isSelect"></param>
        public void ChangeFocusType(bool isSelect)
        {
            if (isFocusSelect == isSelect) return;
            isFocusSelect = isSelect;
            foreach (var el in focusList)
            {
                el.isFocusSelect = isSelect;
                el.UpdateFocusType();
            }
        }

        public void ChangeCanNoneFocus(bool canNone)
        {
            if (canNoneFocus == canNone) return;
            canNoneFocus = canNone;
            foreach (var focusItem in focusList)
            {
                focusItem.focusOffByParent = !canNoneFocus;
            }
        }

        /// <summary>
        /// 관리하는 모든 UI 요소를 한 번에 비활성화하거나 활성화합니다.
        /// </summary>
        /// <param name="isDisable"></param>
        public void AllDisableToggle(bool isDisable)
        {
            foreach (var el in focusList)
            {
                if (isDisable)
                    el.DisableOn();
                else
                    el.DisableOff();
            }
        }
        
        public void AllFrozenToggle(bool isFrozen)
        {
            foreach (var el in focusList)
            {
                el.isFrozen = isFrozen;
            }
        }

        private int CreateLabel()
        {
            return _lastLabel++;
        }
        
        private Dictionary<UIElement, Action<UIElementState>> eventHandlers = new();

        private void RegisterMouseEvent(UIElement el, int label)
        {
            Action<UIElementState> callback = st => UIElementStateChanged(st, label);
            eventHandlers[el] = callback;
            el.StateChanged += callback;
        }
        private void RemoveMouseEvent(UIElement el)
        {
            if (eventHandlers.TryGetValue(el, out var callback))
            {
                el.StateChanged -= callback;
                eventHandlers.Remove(el);
            }
        }

        private void UIElementStateChanged(UIElementState elState, int label)
        {
            // Debug.LogError($"state changed to {elState} {label}");
            if (isFocusSelect)
            {
                if (UIElement.EqualSt(elState, UIElementState.Select))
                {
                    MoveToLabel(label);
                }
            }
            else
            {
                if (UIElement.EqualSt(elState, UIElementState.Hover))
                {
                    MoveToLabel(label);
                }
                // if (UIElement.EqualSt(elState, UIElementState.Select))
                // {
                //     SelectFocused(label);
                // }
            }
        }
        
        
        public void KeyControl()
        {
            // Debug.LogError("www");
            if (_isFocused)
            {
                // Debug.LogError($"cur id : {curId}");
                if(0 <= curId && curId < focusList.Count)
                    focusList[curId].KeyControl();
            }
            if (isQE)
            {
                if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.LeftHeader)))
                {
                    MoveFocus(true);
                }
                else if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.RightHeader)))
                {
                    MoveFocus(false);
                }
                else if(useNumberKey)
                {
                    for (int i = 0; i < focusList.Count; i++)
                    {
                        // keycode alpha 는 50부터 시작
                        if(InputManager.GetKeyDown((KeyCode)(i+50)))
                        {
                            MoveTo(i);
                        }
                    }
                }
            }
            else
            {
                if (navigation == NavigationMode.Horizontal)
                {
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Left)))
                    {
                        MoveFocus(true);
                    }
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Right)))
                    {
                        MoveFocus(false);
                    }
                }else if(navigation == NavigationMode.Vertical)
                {
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Up)))
                    {
                        MoveFocus(true);
                    }
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Down)))
                    {
                        MoveFocus(false);
                    }
                }else if (navigation == NavigationMode.Inventory)
                {
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Up)))
                    {
                        // 가장 상단에 위치
                        if (curId < tableData.x)
                        {
                            if (tableData.isUpLoop)
                            {
                                MoveTo(curId + ((focusList.Count - curId + 1) / tableData.x) * tableData.x );
                            }else if (tableData.moveUp != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveUp.Invoke(curId % tableData.x);
                                }
                            }
                        }
                        else
                        {
                            MoveTo(curId - tableData.x);
                        }
                    }
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Down)))
                    {
                        // 가장 하단에 위치
                        if (curId >= focusList.Count - tableData.x)
                        {
                            if (tableData.isDownLoop)
                            {
                                MoveTo(curId % tableData.x);
                            }else if (tableData.moveDown != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveDown.Invoke(curId % tableData.x);
                                }
                            }
                        }
                        else
                        {
                            MoveTo(curId + tableData.x);
                        }
                    }
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Left)))
                    {
                        // 테이블에 가장 왼쪽 열에 분포함.
                        if (curId % tableData.x == 0)
                        {
                            if (tableData.isLeftLoop)
                            {
                                MoveTo(Mathf.Max(curId + tableData.x - 1, focusList.Count - 1));
                            }else if (tableData.moveLeft != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveLeft.Invoke(curId / tableData.x);
                                }
                                // 만약에 can None Focus가 false라면 focus된 개체가 무조건 존재해야 한다는 의미이니 이벤트 발생 x.
                            }
                        }
                        else
                        {
                            // Debug.Log("왼쪽으로");
                            MoveTo(curId-1);
                        }
                    }
                    if (InputManager.GetKeyDown(KeySettingManager.GetUIKeyCode(Define.UIKey.Right)))
                    {
                        // 테이블에 가장 오른쪽 열에 분포함 (가장 하단은 마지막 요소도 포함)
                        if (curId == focusList.Count - 1 || (curId + 1) % tableData.x == 0)
                        {
                            if (tableData.isRightLoop)
                            {
                                MoveTo((curId / tableData.x) * tableData.x);
                            }else if (tableData.moveRight != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveRight.Invoke(curId / tableData.x);
                                }
                            }
                        }
                        else
                        {
                            MoveTo(curId+1);
                        }
                    }
                }
            }
        }

        public void GamePadControl()
        {
            if (_isFocused)
            {
                // Debug.LogError($"cur id : {curId}");
                if(0 <= curId && curId < focusList.Count)
                    focusList[curId].GamePadControl();
            }
            if (isQE)
            {
                if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.LeftHeader)))
                {
                    MoveFocus(true);
                }
                else if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.RightHeader)))
                {
                    MoveFocus(false);
                }
            }
            else
            {
                if (navigation == NavigationMode.Horizontal)
                {
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Left)))
                    {
                        MoveFocus(true);
                    }
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Right)))
                    {
                        MoveFocus(false);
                    }
                }else if(navigation == NavigationMode.Vertical)
                {
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Up)))
                    {
                        MoveFocus(true);
                    }
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Down)))
                    {
                        MoveFocus(false);
                    }
                }else if (navigation == NavigationMode.Inventory)
                {
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Up)))
                    {
                        // 가장 상단에 위치
                        if (curId < tableData.x)
                        {
                            if (tableData.isUpLoop)
                            {
                                MoveTo(curId + ((focusList.Count - curId + 1) / tableData.x) * tableData.x );
                            }else if (tableData.moveUp != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveUp.Invoke(curId % tableData.x);
                                }
                            }
                        }
                        else
                        {
                            MoveTo(curId - tableData.x);
                        }
                    }
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Down)))
                    {
                        // 가장 하단에 위치
                        if (curId >= focusList.Count - tableData.x)
                        {
                            if (tableData.isDownLoop)
                            {
                                MoveTo(curId % tableData.x);
                            }else if (tableData.moveDown != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveDown.Invoke(curId % tableData.x);
                                }
                            }
                        }
                        else
                        {
                            MoveTo(curId + tableData.x);
                        }
                    }
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Left)))
                    {
                        if (curId % tableData.x == 0)
                        {
                            if (tableData.isLeftLoop)
                            {
                                MoveTo(Mathf.Max(curId + tableData.x - 1, focusList.Count - 1));
                            }else if (tableData.moveLeft != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveLeft.Invoke(curId / tableData.x);
                                }
                                // 만약에 can None Focus가 false라면 focus된 개체가 무조건 존재해야 한다는 의미이니 이벤트 발생 x.
                            }
                        }
                        else
                        {
                            // Debug.Log("왼쪽으로");
                            MoveTo(curId-1);
                        }
                    }
                    if (InputManager.GetButtonDown(KeySettingManager.GetUIButton(Define.UIKey.Right)))
                    {
                        // 테이블에 가장 오른쪽 열에 분포함 (가장 하단은 마지막 요소도 포함)
                        if (curId == focusList.Count - 1 || (curId + 1) % tableData.x == 0)
                        {
                            if (tableData.isRightLoop)
                            {
                                MoveTo((curId / tableData.x) * tableData.x);
                            }else if (tableData.moveRight != null)
                            {
                                if (canNoneFocus)
                                {
                                    FocusReset();
                                    tableData.moveRight.Invoke(curId / tableData.x);
                                }
                            }
                        }
                        else
                        {
                            MoveTo(curId+1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 포커스를 이전(isBack=true) 또는 다음 요소로 한 칸 이동시킵니다.
        /// </summary>
        /// <param name="isBack"></param>
        public void MoveFocus(bool isBack = false){
            if (!_isFocused)
            {
                if (!canNoneFocus)
                {
                    Debug.LogError("불가능.");
                    return;
                }

                MoveTo(curId);
            }
            else
            {
                if (isBack)
                {
                    if (curId - 1 < 0)
                    {
                        if (canLoop)
                            MoveTo(focusList.Count - 1);
                    }
                    else
                        MoveTo(curId - 1);
                }
                else
                {
                    if (curId + 1 >= focusList.Count)
                    {
                        if (canLoop)
                            MoveTo(0);
                    }
                    else
                        MoveTo(curId + 1);
                }
            }
        }

        private int FindIndexByLabel(int label)
        {
            for (int i = 0; i < _labels.Count; i++)
            {
                if (_labels[i] == label)
                {
                    return i;
                }
            }
            return -1;
        }

        private void MoveToLabel(int label)
        {
            MoveTo(FindIndexByLabel(label));
        }

        /// <summary>
        /// 지정된 curId 인덱스에 해당하는 요소로 포커스를 즉시 이동시킵니다
        /// </summary>
        /// <param name="curId"></param>
        /// <param name="force"></param>
        public void MoveTo(int curId, bool force = false)
        {
            if (curId == this.curId && _isFocused && !force) return;
            if (curId < 0 || focusList.Count <= curId) return;
            FocusGroup?.ChangeFocusParent(this);
            NavigationManager?.SetCurrentNavigatable(this);
            if (_isFocused && this.curId < focusList.Count)
            {
                FocusOff(this.curId);
            }
            _isFocused = true;
            this.curId = curId;
            FocusChanged.Invoke(this.curId);
            FocusOn(this.curId);
        }

        /// <summary>
        /// 특정 요소의 포커스를 활성화합니다.
        /// </summary>
        /// <param name="id"></param>
        private void FocusOn(int id)
        {
            if (isFocusSelect)
            {
                focusList[id].SelectOn();
            }
            else
            {
                focusList[id].HoverOn();
            }
        }
        
        /// <summary>
        /// 특정 요소의 포커스를 비활성화합니다.
        /// </summary>
        /// <param name="id"></param>
        private void FocusOff(int id)
        {
            if (isFocusSelect)
            {
                focusList[id].SelectOff(true);
            }
            else
            {
                // Debug.Log("hover off");
                focusList[id].HoverOff(true);
            }
        }

        public void SetIndex(int targetIndex)
        {
            if (_isFocused)
            {
                MoveTo(targetIndex);
            }
            else
            {
                curId = targetIndex;
            }
        }
        private void OnDisable()
        {
            FocusOff(curId);
            curId = 0;
            WhenDisable.Invoke();
        }
    }
}