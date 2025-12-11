using UnityEngine;

public class jinhe : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform trans;

    private float posForce = 14f;
    
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rigid.gravityScale = 3;
            rigid.AddForceAtPosition((transform.position - trans.position).normalized * posForce, trans.position, ForceMode2D.Impulse);
        }
    }
}
