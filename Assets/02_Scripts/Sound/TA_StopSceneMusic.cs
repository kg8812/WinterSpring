using System.Collections;
using System.Collections.Generic;
using chamwhy;
using UnityEngine;

public class TA_StopSceneMusic : ITriggerActivate
{
    public void Activate()
    {
        GameManager.Sound.StopSceneBGM();
    }
}
