using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy.DataType;
using UnityEngine;

public class NoteModel : Database
{
    public static Dictionary<int, NoteDataType> notes;
    
    public override void ProcessDataLoad()
    {
        notes = GameManager.Data.GetDataTable<NoteDataType>(chamwhy.DataTableType.Note).ToDictionary(
            x => int.Parse(x.Key), x =>
            {
                x.Value.index = int.Parse(x.Key);
                return x.Value;
            });
    }
}
