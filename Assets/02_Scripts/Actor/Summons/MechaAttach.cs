using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaAttach : SkeletonAttach
{
    private SeguMecha mecha;

    protected override void Awake()
    {
        base.Awake();
        
        mecha = transform.parent.GetComponent<SeguMecha>();
    }

    public void DoPulse()
    {
        Debug.Log("DoPulse");
        mecha.DoPulse();
    }

    public void DoCannon()
    {
        Debug.Log("DoCannon");
        mecha.DoCannon();
    }
    
}
