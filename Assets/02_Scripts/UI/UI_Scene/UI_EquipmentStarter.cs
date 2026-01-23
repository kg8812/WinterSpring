using System.Collections;
using System.Collections.Generic;
using Default;
using UnityEngine;

public class UI_EquipmentStarter : UI_Scene
{
    public UITab_Equipment equipment;

    protected override void Activated()
    {
        base.Activated();
        equipment.OnOpen();
    }

    protected override void Deactivated()
    {
        base.Deactivated();
        equipment.OnClose();
    }

    public override void Init()
    {
        base.Init();
        equipment.Init();
    }
}
