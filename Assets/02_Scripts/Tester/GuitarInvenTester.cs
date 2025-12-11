using Apis;
using chamwhy;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class GuitarInvenTester: MonoBehaviour
    {
        public int id;
        public GuitarInvenType type;

        [Button]
        public void Set()
        {
            InvenManager.instance.GuitarInven.Add(GameManager.Item.RandAcc, type);
        }
    }
}