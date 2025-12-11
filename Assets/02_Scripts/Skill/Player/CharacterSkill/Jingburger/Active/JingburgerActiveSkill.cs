using System.Collections.Generic;
using Apis;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "JingburgerActive", menuName = "Scriptable/Skill/JingburgerActive")]
public partial class JingburgerActiveSkill : PlayerActiveSkill
{
    protected override ActiveEnums _activeType => ActiveEnums.Instant;

    private StackCd stackCd;
    public override ICdActive CDActive => stackCd ??= new StackCd(this);

    protected override CDEnums _cdType => CDEnums.Stack;

    protected override bool fixedActiveType => false;

    [Title("징버거 액티브")] [LabelText("투사체 반경")]
    public float radius1;
    [LabelText("투사체 설정")] public ProjectileInfo info1;
    [LabelText("폭발 반경")] public float radius2;
    [LabelText("폭발 공격설정")] public ProjectileInfo info2;
    
    [HideInInspector] public List<float> chargeRadius;

    private float Radius2;

    private UnityEvent<AttackObject> _onExplosionSpawn;
    public UnityEvent<AttackObject> OnExplosionSpawn => _onExplosionSpawn ??= new();

    private UnityEvent<EventParameters> _onAttack;
    public UnityEvent<EventParameters> OnAttack => _onAttack ??= new();

    
    private ISpawnStrategy spawnStrategy;
    private IUseStrategy useStrategy;

    protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Coloring;

    protected override float TagIncrement => GameManager.Tag.Data.ColoringIncrement;

    public override void Init()
    {
        base.Init();
        Radius2 = radius2;
        spawnStrategy = new SpawnRasengan(this);
        useStrategy = new FireProjectile();
        actionList.Clear();
        actionList.Add(Spawn);
    }

    protected override void TurnOnActiveMotion()
    {
        base.TurnOnActiveMotion();
        animator?.animator.SetInteger("ActiveSkillType",useStrategy.MotionType);

    }

    protected override void OnEquip(IMonoBehaviour owner)
    {
        base.OnEquip(owner);
        spawnStrategy = new SpawnRasengan(this);
        useStrategy = new FireProjectile();
    }
    public void ChangeSpawnStrategy(ISpawnStrategy strategy)
    {
        spawnStrategy = strategy;
    }

    public void ChangeUseStrategy(IUseStrategy strategy)
    {
        useStrategy = strategy;
    }

    void Spawn()
    {
        var rasengan = spawnStrategy.Spawn();
            
        rasengan.AddEventUntilInitOrDestroy(x =>
        {
            OnExplosionSpawn.Invoke(SpawnExplosion(x.user.Position,info2.dmg));
        },EventType.OnDestroy);
        useStrategy.Use(rasengan);
    }
    public AttackObject SpawnExplosion(Vector2 pos , float dmg)
    {
        AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
            Define.PlayerEffect.JingRasenganExplosion, pos);
        exp.transform.localScale = Vector3.one * (Radius2 * 2);
        exp.Init(attacker, new AtkItemCalculation(attacker as Actor, this, dmg), 1);
        exp.Init(info2);
        exp.Init(Mathf.RoundToInt(BaseGroggyPower));
        exp.AddEventUntilInitOrDestroy(param => OnAttack.Invoke(param));
        return exp;
    }

    public AttackObject SpawnExtraExplosion(Vector2 pos, float dmg)
    {
        AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
            Define.PlayerEffect.JingRasenganExplosion, pos);
        exp.transform.localScale = Vector3.one * (radius2 * 2);
        exp.Init(attacker, new FixedAmount(baseDmg * dmg / 100), 1);
        exp.Init(info2);
        exp.Init(Mathf.RoundToInt(BaseGroggyPower));
        return exp;
    }
    protected override void ChargeInvoke(int idx)
    {
        base.ChargeInvoke(idx);

        Radius2 = radius2 * chargeRadius[idx] / 100;
    }

    public override void Cancel()
    {
        base.Cancel();
        useStrategy.Cancel();
    }
}
