using System.Collections.Generic;
using System.Linq;
using chamwhy;
using chamwhy.DataType;

namespace Directing
{
    public class InGameCutSceneModel : Database
    {
        public static Dictionary<int, List<InGameCutSceneDataType>> igDict;

        public override void ProcessDataLoad()
        {
            igDict = new();
            foreach (var value in GameManager.Data.GetDataTable<InGameCutSceneDataType>(DataTableType.InGameCutScene))
            {
                if (igDict.ContainsKey(value.Value.cutSceneId))
                {
                    igDict[value.Value.cutSceneId].Add(value.Value);
                }
                else
                {
                    List<InGameCutSceneDataType> newList = new();
                    newList.Add(value.Value);
                    igDict.Add(value.Value.cutSceneId, newList);
                }
            }
            foreach (var key in igDict.Keys.ToList())
            {
                igDict[key] = igDict[key].OrderBy(x => x.index).ToList();
            }
        }
    }
}