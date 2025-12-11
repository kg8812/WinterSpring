using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class PresetTester: MonoBehaviour
    {
        public int slotInd;
        public int itemId;
        
        [Button]
        public void ApplyPreset()
        {
            InvenManager.instance.PresetManager.ModifyPresetItem(
                (int)GameManager.instance.Player.playerType,
                1,
                slotInd,
                itemId);
            GameManager.instance.Player.ApplyPlayerPreset();
        }

        [Button]
        public void Print()
        {
            InvenManager.instance.PresetManager.PrintPreset();
        }
    }
}