using chamwhy;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class InvenCounterTester: MonoBehaviour
    {
        public int count;
        public InvenType type;
        public bool isAtk;

        [Button(ButtonSizes.Large)]
        public void SetCount()
        {
            if(isAtk)
                InvenManager.instance.AttackItem.Invens[type].Count = count;
            else
                InvenManager.instance.Acc.Invens[type].Count = count;
        }

        [Button(ButtonSizes.Large)]
        public void Debugging()
        {
            if(isAtk)
                Debug.Log($"count {InvenManager.instance.AttackItem.Invens[type].Count}");
            else
                Debug.Log($"count {InvenManager.instance.Acc.Invens[type].Count}");
        }
    }
}