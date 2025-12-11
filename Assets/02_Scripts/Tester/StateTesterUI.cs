using UnityEngine;
using TMPro;

public class StateTesterUI : MonoBehaviour
{
    public Player _p;
    public TextMeshProUGUI test;
    public TextMeshProUGUI anim;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        test.text = _p.GetState().ToString();
        AnimatorClipInfo[] info;
        info = _p.animator.GetCurrentAnimatorClipInfo(0);
        if(info == null || info.Length == 0) return;
        anim.text = info[0].clip.name;
    }
}
