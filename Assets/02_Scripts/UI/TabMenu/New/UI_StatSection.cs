using System;
using System.Collections;
using System.Collections.Generic;
using Default;
using TMPro;
using UnityEngine;

public class UI_StatSection : MonoBehaviour
{
    public ActorStatType statType;
    public TextMeshProUGUI title;
    public TextMeshProUGUI baseStat;
    public TextMeshProUGUI bonusStat;
    
    private void OnEnable()
    {
        Player player = GameManager.instance.Player;
        
        string titleText = Utils.GetStatText(statType);
        title.text = titleText;
    }
}
