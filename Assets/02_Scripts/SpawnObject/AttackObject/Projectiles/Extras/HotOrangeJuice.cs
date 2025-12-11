using Apis;
using chamwhy;
using UnityEngine;

public class HotOrangeJuice : Projectile
{
    [HideInInspector] public float explosionDmg;
    [HideInInspector] public float explosionSize;
    [HideInInspector] public float explosionDuration;
    [HideInInspector] public float explosionGroggy;
    
    public override void Destroy()
    {
        AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
            "JuiceExplosion", transform.position);
        exp.transform.localScale = Vector3.one * explosionSize;
        exp.Init(_attacker,new AtkBase(_attacker,explosionDmg),explosionDuration);
        //exp.Init(GameManager.instance.Player.AttackItemManager.Weapon.CalculateGroggy(explosionGroggy));
        base.Destroy();
    }
}
