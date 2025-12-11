using Apis;
using chamwhy;
using UnityEngine;

public class ShotgunBullet : Projectile
{
    protected override void AttackInvoke(EventParameters parameters)
    {

        ParticleSystem effect = GameManager.Factory.Get<ParticleSystem>(FactoryManager.FactoryType.Effect,
            Define.PlayerEffect.LilpaBulletHit, transform.position);

        GameManager.Factory.Return(effect.gameObject, effect.main.duration);
        GameManager.Sound.Play(Define.SFXList.LilpaGunHit);
        
        base.AttackInvoke(parameters);
    }
}
