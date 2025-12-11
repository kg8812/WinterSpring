using UnityEngine;

namespace chamwhy
{
    public abstract class Trap: MonoBehaviour
    {
        protected bool Activated;

        public void ActiveCheck()
        {
            if (Activated) return;
            Activated = true;
            Active();
        }

        protected abstract void Active();
        public abstract void Deactive();
    }
}