using UnityEngine;

public class TweenTester : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collide");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
    }
}
