using System.Collections.Generic;
using System.Linq;
using chamwhy.DataType;
using UnityEngine;

namespace chamwhy
{
    public class NpcModel : Database
    {
        public static Dictionary<int, List<ScriptDataType>> scriptDict { get; private set; }
        public override void ProcessDataLoad()
        {
            {
                scriptDict = new();
                foreach (var value in GameManager.Data.GetDataTable<ScriptDataType>(DataTableType.Script))
                {
                    if(value.Value.scriptsKor.Length != value.Value.duration.Length && value.Value.isAuto)
                        Debug.LogError("기획 오류!!!!");
                    
                    if (scriptDict.ContainsKey(value.Value.speaker))
                    {
                        scriptDict[value.Value.speaker].Add(value.Value);
                    }
                    else
                    {
                        List<ScriptDataType> newList = new();
                        newList.Add(value.Value);
                        scriptDict.Add(value.Value.speaker, newList);
                    }
                }
                foreach (var key in scriptDict.Keys.ToList())
                {
                    scriptDict[key] = scriptDict[key].OrderByDescending(x => x.priority).ToList();
                }
            }
        }
    }
}