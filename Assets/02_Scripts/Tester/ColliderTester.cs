using UnityEngine;

namespace _02_Scripts.Tester
{
    public class ColliderTester: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
        }
    }
}