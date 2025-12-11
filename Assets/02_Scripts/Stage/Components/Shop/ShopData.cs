using System.Collections.Generic;
using System.Linq;
using chamwhy;
using chamwhy.DataType;
using chamwhy.Managers;
using Save.Schema;

namespace Apis
{
    public class ShopData : Database
    {
        private static Dictionary<int, ShopDataType> dataDict;
        private static Dictionary<int, List<ShopListDataType>> groupDict;
        public override void ProcessDataLoad()
        {
            dataDict = GameManager.Data.GetDataTable<ShopDataType>(DataTableType.Shop)
                .ToDictionary(x => int.Parse(x.Key), x => x.Value);

            {
                var dict = GameManager.Data.GetDataTable<ShopListDataType>(DataTableType.ShopList)
                    .ToDictionary(x => int.Parse(x.Key), x => x.Value);
                
                groupDict = new();
                foreach (var x in dict.Values)
                {
                    if (!groupDict.ContainsKey(x.groupIndex))
                    {
                        groupDict.Add(x.groupIndex, new());
                    }

                    groupDict[x.groupIndex] ??= new();

                    groupDict[x.groupIndex].Add(x);
                }
            }
        }

        public static bool TryGetShopData(int id, out ShopDataType data)
        {
            return dataDict.TryGetValue(id, out data);
        }

        public static bool TryGetShopListData(int groupId, out List<ShopListDataType> data)
        {
            return groupDict.TryGetValue(groupId, out data);
        }

        /*
        public static Dictionary<Item, int> GetShopItemList(int groupId)
        {
            Dictionary<Item, int> dict = new();

            if (TryGetShopListData(groupId, out var data))
            {
                foreach (var x in data )
                {
                    switch ((DropItemType)x.itemType)
                    {
                        case DropItemType.Accessory:
                            if (!DataAccess.Codex.IsOpen(CodexData.CodexType.Item, x.itemId))
                                continue;
                            
                            Accessory acc = GameManager.Item.GetAcc(x.itemId);
                            dict.TryAdd(acc, x.chance);
                            continue;
                        case DropItemType.Weapon:
                            if (!DataAccess.Codex.IsOpen(CodexData.CodexType.Item, x.itemId))
                                continue;
                            
                            Weapon wp = GameManager.Item.GetWeapon(x.itemId);
                            dict.TryAdd(wp, x.chance);
                            continue;
                    }
                }
            }
            return dict;

        }
        */
    }
}