using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class BarrierTester: MonoBehaviour
    {
        [SerializeField] private float value;
        
        [Button(ButtonSizes.Large)]
        public void AddBarrierToPlayer()
        {
            Resolution[] re = Screen.resolutions;
            
            for (int i = 0; i < re.Length; i++)
            {
                Debug.LogError($"{re[i].width}, {re[i].height}");
            }
            
        }
    }
}