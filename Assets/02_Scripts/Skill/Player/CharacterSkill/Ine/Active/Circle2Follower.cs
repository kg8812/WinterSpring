using chamwhy;
using Default;
using UnityEngine;

public class Circle2Follower : MonoBehaviour
{
    private float speed;
    private float radius;

    public void Init(float speed, float radius)
    {
        this.speed = speed;
        this.radius = radius;
    }
    private void FixedUpdate()
    {
        var targets = this.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy);
        targets = targets.OrderByDistance(transform.position);
        if (targets.Count > 0 && targets[0] != null && !targets[0].IsDead)
        {
            float distance = Mathf.Abs(transform.position.x - targets[0].Position.x);
            if (distance > 0.2f)
            {
                transform.Translate(Vector2.right *
                                    (speed * Time.fixedDeltaTime * (targets[0].Position.x > transform.position.x
                                        ? 1 : -1)));
            }
        }

    }
}
