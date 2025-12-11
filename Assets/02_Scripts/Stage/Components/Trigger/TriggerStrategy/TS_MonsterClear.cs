using System.Collections.Generic;
using System.Linq;
using Default;
using UnityEngine;

namespace chamwhy.Components
{
    public class TS_MonsterClear: ITriggerStrategy
    {
        private Trigger _triggerComponent;
        private List<Monster> monsters;
        private int mCnt;

        public TS_MonsterClear(Trigger triggerComponent)
        {
            _triggerComponent = triggerComponent;
            monsters = new List<Monster>();
        }

        public void Update()
        {
            return;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            Monster monster = col.transform.GetComponentInParentAndChild<Monster>();
            if (monster)
            {
                if (!monsters.Contains(monster))
                {
                    monsters.Add(monster);

                    void OnDeath(EventParameters e)
                    {
                        monsters.Remove(monster);
                        monster.RemoveEvent(EventType.OnDeath, OnDeath);
                        if (monsters.Count == 0)
                        {
                            _triggerComponent.ActivateTrigger();
                        }
                    }

                    monster.AddEvent(EventType.OnDeath, OnDeath);
                }
            }
        }

        

        public void OnTriggerExit2D(Collider2D col)
        {
            // if (col.gameObject.CompareTag("Enemy") && col.isTrigger)
            // {
            //     Monster monster = col.transform.GetComponentInParentAndChild<Monster>();
            //     if (monster)
            //     {
            //         if (monsters.Contains(monster))
            //         {
            //             monsters.Remove(monster);
            //             if (monsters.Count == 0)
            //             {
            //                 _triggerComponent.ActivateTrigger();
            //                 
            //             }
            //         }
            //     }
            // }
        }

        public bool CheckAvailable(Collider2D col)
        {
            return col.gameObject.CompareTag("Enemy") && col.isTrigger;
        }
    }
}