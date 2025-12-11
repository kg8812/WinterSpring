// using System.Collections.Generic;
//
// namespace chamwhy.Inven
// {
//     public interface IInvenGroupManager
//     {
//         public Dictionary<InvenType, InventoryGroup> Invens { get; }
//         public bool Add(Item item, InvenType type);
//         public bool Add(int index, Item item, InvenType type);
//         public bool Equip(int index);
//         public bool UnEquip(int index);
//         public bool UnEquipAll(bool unEquipWithAvailable=false);
//         public bool Change(int index1, InvenType type1, int index2, InvenType type2);
//         public Item Remove(int index, InvenType type);
//         public Item Abandon(int index, InvenType type);
//     }
// }