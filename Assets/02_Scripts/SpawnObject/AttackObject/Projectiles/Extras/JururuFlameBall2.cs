using Apis;
using chamwhy;

public class JururuFlameBall2 : JururuFlameBall
{
    private Buff buff;

    protected override string disEffect => Define.JururuBossEffect.Get(Define.JururuBossEffect.EffectType.JururuAttack3DisBlue);

    public override void Init(AttackObjectInfo atkObjectInfo)
    {
        base.Init(atkObjectInfo);
        
        hitEffect = Define.JururuBossEffect.Get(Define.JururuBossEffect.EffectType.JururuAttack3HitBlue);
    }

    protected override void AttackInvoke(EventParameters parameters)
    {
        base.AttackInvoke(parameters);

        if (parameters?.target is Actor t)
        {
            t.AddSubBuff(_eventUser,SubBuffType.Debuff_Burn);
        }
    }
}
