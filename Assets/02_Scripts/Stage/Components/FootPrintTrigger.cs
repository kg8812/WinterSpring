using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintTrigger : MonoBehaviour
{
    public enum GroundType
    {
        Water,Grass,Snow,Rock
    }

    public GroundType groundType;
    public int priority;
}
