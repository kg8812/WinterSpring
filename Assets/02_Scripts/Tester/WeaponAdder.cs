using System.Collections.Generic;
using Apis;
using chamwhy;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponAdder : MonoBehaviour
{
    public int[] wpIds;
    
    [Button(ButtonSizes.Large)]
    public void AddWeapon()
    {
        List<Weapon> wps = new();
        foreach (var x in wpIds)
        {
            wps.Add(GameManager.Item.GetWeapon(x));
        }
            
        foreach (var weapon in wps)
        {
            int eqInd = InvenManager.instance.AttackItem.Invens[InvenType.Equipment].GetEmptySlot();
            InvenManager.instance.AttackItem.Add(eqInd, weapon, InvenType.Storage);
        }

        InvenManager.instance.AttackItem.MoveInvenType(0, InvenType.Storage, InvenType.Equipment);
    }
}
