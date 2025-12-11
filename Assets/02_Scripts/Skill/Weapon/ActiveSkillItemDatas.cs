using System.Collections.Generic;
using System.Linq;
using chamwhy.DataType;
using Default;
using Sirenix.Utilities;

namespace Apis
{
    public class ActiveSkillItemDatas: Database
    {
        public static Dictionary<int, ActiveSkillItemDataType> activeSkillItems;
        public override void ProcessDataLoad()
        {
            var dict = GameManager.Data.GetDataTable<ActiveSkillItemDataType>(chamwhy.DataTableType.ActiveSkillItem);
            activeSkillItems = dict.ToDictionary(x => int.Parse(x.Key), x => x.Value);
        }
    }
}