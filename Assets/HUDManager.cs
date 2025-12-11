using UnityEngine;

public class HUDManager : MonoBehaviour
{
    void Start()
    {
        GameManager.UI.CreateUI("UI_MainHud", chamwhy.UIType.Main);
    }

    
}
