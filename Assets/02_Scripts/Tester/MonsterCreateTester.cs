using Apis;
using chamwhy;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class MonsterCreateTester: MonoBehaviour
    {
        public string id;
        public Monster monster;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                GameObject obj = GameManager.Factory.Get(FactoryManager.FactoryType.Monster, $"Prefabs/Components/MonsterComponent/{id}");
                obj.transform.position = transform.position;
                monster = obj.GetComponent<Monster>();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                monster?.ReturnToPool();
            }
        }
    }
}