using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class Saver: MonoBehaviour
    {
        [Button(50)]
        public void SaveSlot()
        {
            GameManager.instance.SaveSlot();
        }
    }
}