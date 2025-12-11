using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class UI_Tester: MonoBehaviour
    {
        public string uiname;

        public UIType type;

        private UI_Base baseUI;
        
        [Button("생성", 50)]
        public void Show()
        {
            baseUI = GameManager.UI.CreateUI(uiname,type);
        }
        
        [Button("삭제", 50)]
        public void Delete()
        {
            baseUI.CloseOwn();
        }
    }
}