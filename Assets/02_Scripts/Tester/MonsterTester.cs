// using Apis;
// using chamwhy;
// using chamwhy.DataType;
// using UI;
// using UnityEngine;
//
// namespace Tester
// {
//     public class MonsterTester: MonoBehaviour
//     {
//         public CommonMonster monsterPrefab;
//         public bool isMonsterDataWithInspector;
//         public MonsterDataType MonsterDataType;
//         
//         private CommonMonster _monster;
//         private UI_ActorInfo _actorInfo;
//
//         public JururuBoss jururu;
//         private void Start()
//         {
//             jururu.OnRecognized();
//         }
//         public void SpawnMonster()
//         {
//             _monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity, transform);
//             if (isMonsterDataWithInspector)
//             {
//                 _monster.Init(MonsterDataType);
//             }
//             _monster.CurHp = _monster.MaxHp;           
//
//         }
//         
//         private void Update()
//         {
//             if(Input.GetKeyDown(KeyCode.K))
//             {
//                 SpawnMonster();
//             }
//             
//         }
//     }
// }