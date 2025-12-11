using System.Collections.Generic;
using System.Linq;
using chamwhy;
using chamwhy.DataType;
using Save.Schema;

public class TipDatabase : Database
{
    private static Dictionary<int, TipDataType> dict;

    public override void ProcessDataLoad()
    {
        dict = GameManager.Data.GetDataTable<TipDataType>(DataTableType.Tip)
            ?.ToDictionary(x => int.Parse(x.Key), x => x.Value);
    }

    public static bool TryGetTipData(int index, out TipDataType data)
    {
        return dict.TryGetValue(index, out data);
    }
}
