using Sirenix.OdinInspector;
using UnityEngine;

public class DebuffTester : MonoBehaviour
{
    public Actor target;

    private Player player => GameManager.instance.Player;

    [Button("중독")]
    public void AddPoison()
    {
        target?.AddSubBuff(player,SubBuffType.Debuff_Poison);
    }
    
    [Button("화상")]
    public void AddBurn()
    {
        target?.AddSubBuff(player,SubBuffType.Debuff_Burn);
    }

    [Button("출혈")]
    public void AddBleed()
    {
        target?.AddSubBuff(player,SubBuffType.Debuff_Bleed);
    }

    [Button("냉기")]
    public void AddChill()
    {
        target?.AddSubBuff(player,SubBuffType.Debuff_Chill);
    }

    [Button("기절")]
    public void AddStun()
    {
        target?.StartStun(player,3);
    }
}
