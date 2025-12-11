using UnityEngine;

namespace Tester
{
    public class ColliderTest: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"trigger enter {other}");
        }
        
        // private void OnTriggerStay2D(Collider2D other)
        // {
        //     Debug.Log($"trigger stay {other}");
        // }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"trigger exit {other}");
        }
    }
}