using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class OvItemStoragePrinter: MonoBehaviour
    {
        [Button]
        public void ItemPrint()
        {
            InvenManager.instance.Storage.Print();
        }

        [Button]
        public void OvItemPrint()
        {
            InvenManager.instance.PresetManager.OverrideItems.OverrideItemStorage.Print();
        }
    }
}