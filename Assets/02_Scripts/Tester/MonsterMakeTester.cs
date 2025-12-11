using Apis;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class MonsterMakeTester: MonoBehaviour
    {
        public string monsterAddress;
        public void Start()
        {
            
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameObject obj = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, monsterAddress);


                obj.transform.position = transform.position;
            }
        }
    }
}