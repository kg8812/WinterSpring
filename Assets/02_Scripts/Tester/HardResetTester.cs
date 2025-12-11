using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class HardResetTester: MonoBehaviour
    {
        [Button]
        public void HardReset()
        {
            InvenManager.instance.HardReset();
        }
    }
}