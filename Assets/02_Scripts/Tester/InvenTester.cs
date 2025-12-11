using Apis;
using chamwhy;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class InvenTester: MonoBehaviour
    {
        [Button(ButtonSizes.Gigantic)]
        public void Wp()
        {
            for (int i = 0; i < InvenManager.instance.AttackItem.Invens[InvenType.Equipment].Count; i++)
            {
                Debug.Log($"무기 eq {i} {InvenManager.instance.AttackItem.Invens[InvenType.Equipment][i]?.ItemId}");
            }
            for (int i = 0; i < InvenManager.instance.AttackItem.Invens[InvenType.Storage].Count; i++)
            {
                Debug.Log($"무기 iv {i} {InvenManager.instance.AttackItem.Invens[InvenType.Storage][i]?.ItemId}");
            }
        }
        
        [Button(ButtonSizes.Gigantic)]
        public void Acc()
        {
            Debug.Log(InvenManager.instance.Acc.Invens[InvenType.Storage][0]);
            for (int i = 0; i < InvenManager.instance.Acc.Invens[InvenType.Equipment].Count; i++)
            {
                Debug.Log($"악세 eq {i} {InvenManager.instance.Acc.Invens[InvenType.Equipment][i]?.ItemId}");
            }
            for (int i = 0; i < InvenManager.instance.Acc.Invens[InvenType.Storage].Count; i++)
            {
                Debug.Log($"악세 iv {i} {InvenManager.instance.Acc.Invens[InvenType.Storage][i]?.ItemId}");
            }
        }

        [Button(ButtonSizes.Medium)]
        public void Spe()
        {
            Debug.Log(InvenManager.instance.Acc.Invens[InvenType.Storage][0]);
        }
    }
}