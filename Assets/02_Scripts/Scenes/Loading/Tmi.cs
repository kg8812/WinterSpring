using System.Collections.Generic;
using UnityEngine;
using TMPro;
using chamwhy.DataType;

namespace chamwhy
{
    public class Tmi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI tmiText;
        [SerializeField] private string[] tmis;

        void Start()
        {
            Dictionary<string, TmiDataType> tmiDict = GameManager.Data.GetDataTable<TmiDataType>(DataTableType.Tmi);
            tmiText.text = "TMI : " + tmiDict[Random.Range(0, tmiDict.Count).ToString()].tmi;
        }

    
    }

}
