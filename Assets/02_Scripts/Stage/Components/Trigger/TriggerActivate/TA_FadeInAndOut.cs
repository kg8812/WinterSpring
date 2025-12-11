using System.Collections;
using System.Collections.Generic;
using chamwhy;
using UI;
using UnityEngine;

public class TA_FadeInAndOut : ITriggerActivate
{
    
    public TA_FadeInAndOut()
    {
    }
    public void Activate()
    {
        FadeManager.instance.Fading(null,null,0.3f);
    }
}
