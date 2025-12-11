using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialHpDown : TaskActivation
{
    [LabelText("체력조절 %")] public float hpRatio;
    protected override void Task()
    {
        base.Task();
        HpDown();
    }

    public void HpDown()
    {
        Player player = GameManager.instance.Player;
        if (player == null || player.CurHp < player.MaxHp / 100 * hpRatio) return;

        player.CurHp = player.MaxHp / 100 * hpRatio;
    }
}
