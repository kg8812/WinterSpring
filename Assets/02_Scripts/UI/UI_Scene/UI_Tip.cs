using _02_Scripts.UI.UI_SubItem;
using chamwhy.DataType;
using chamwhy.UI;
using chamwhy.UI.Focus;
using Default;
using Save.Schema;
using UnityEngine;

namespace chamwhy
{
    public class UI_Tip : UI_Scene
    {
        [SerializeField] private UI_TipContent content;
        private FocusParent _focusParent;
        
        #region 바인딩

        private enum Btns
        {
            okBtn
        }

        #endregion
        
        public override void Init()
        {
            base.Init();
            Bind<UIAsset_Button>(typeof(Btns));
            
            Get<UIAsset_Button>((int)Btns.okBtn).OnClick.AddListener(CloseOwn);
            subItems.Add(content);
            content.Init();
            _focusParent = GetComponent<FocusParent>();
            _focusParent?.InitCheck();
        }

        public void InitData(TipDataType tipData)
        {
            content.SetInfo(tipData);
            DataAccess.Codex.UnLock(CodexData.CodexType.Tip, tipData.index);
        }

        protected override void Activated()
        {
            base.Activated();
            _focusParent?.FocusReset();
        }

        public override void KeyControl()
        {
            base.KeyControl();
            _focusParent?.KeyControl();
        }

        public override void GamePadControl()
        {
            base.GamePadControl();
            _focusParent?.GamePadControl();
        }
    }
}