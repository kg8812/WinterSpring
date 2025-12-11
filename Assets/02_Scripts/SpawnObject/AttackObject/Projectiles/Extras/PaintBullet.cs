using Apis;
using chamwhy;
using UnityEngine;

public class PaintBullet : Projectile
{
    [HideInInspector] public float size;

    public override void Destroy()
    {
        AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
            "paintExplosion", transform.position);

        exp.transform.localScale = Vector3.one * size;
        exp.Init(_attacker,new AtkBase(_attacker,DmgRatio),1);
        exp.AddEventUntilInitOrDestroy(AddDebuff);
        base.Destroy();
    }

    void AddDebuff(EventParameters parameters)
    {
        if (parameters?.target is Actor t)
        {
            t.AddSubBuff(_eventUser,SubBuffType.Debuff_Poison);
        }
    }
}
