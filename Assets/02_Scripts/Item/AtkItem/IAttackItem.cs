using System;
using System.Collections;
using System.Collections.Generic;
using Apis;
using chamwhy;
using UnityEngine;

public enum AttackCategory
{
    Sword,GreatSword,Orb,Fist,Staff,Spear,Gun,Magic
}

public interface IAttackItem
{
    // public string Name { get; }
    public void BeforeAttack();
    public void UseAttack();    
    public bool TryAttack();
    public AttackCategory Category { get; }
    public void Equip(IMonoBehaviour user);
    public void UnEquip();
    void WhenIconIsSet(UI_AtkItemIcon icon);
    public void EndAttack();
    public void OnAttackItemChange();
    
    public UI_AtkItemIcon Icon { get; set; }
    public int AtkSlotIndex { get; set; }
    public int InvenSlotIndex { get; }
    public void SetIcon(UI_AtkItemIcon icon)
    {
        Icon = icon;
        WhenIconIsSet(icon);
    }
}

public interface IAttackItemStat
{
    public float Atk { get; }
    public float BaseGroggyPower { get; }
    public float BodyFactor {get;}
    public float SpiritFactor {get;}
    public float FinesseFactor {get;}

}