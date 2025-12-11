using UnityEngine;

namespace chamwhy
{
    public abstract class MonsterMovementStrategy: ScriptableObject
    {
        public abstract void Movement(Monster monster);
    }
}