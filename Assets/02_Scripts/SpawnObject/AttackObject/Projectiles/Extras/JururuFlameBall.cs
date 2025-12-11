using Apis;
using chamwhy;
using UnityEngine;

public class JururuFlameBall : Projectile
{
    protected BossMonster boss;

    protected virtual string disEffect => Define.JururuBossEffect.Get(Define.JururuBossEffect.EffectType.JururuAttack3Dis);

    public override void Init(AttackObjectInfo atkObjectInfo)
    {
        base.Init(atkObjectInfo);
        boss = _attacker as BossMonster;
        hitEffect = Define.JururuBossEffect.Get(Define.JururuBossEffect.EffectType.JururuAttack3Hit);
    }

    public override void Destroy()
    {
        var effect = GameManager.Factory.Get<ParticleSystem>(FactoryManager.FactoryType.Effect,
            disEffect, transform.position);
        
        GameManager.Factory.Return(effect.gameObject, effect.main.duration);
        base.Destroy();
    }
}
