using System;
using System.Collections;
using System.Collections.Generic;
using Apis;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class FoxSoldier : Summon
{
    AttackObject pokdoAttackCollider;

    public enum EffectType
    {
        GunnerBullet,
        GunnerShoot,
        MagicianReady,
        MagicianShoot,
        Hit,
        Release,
        Summon,
        SummonLoop,
    }

    string GetEffectAddress(EffectType type)
    {
        return type switch
        {
            EffectType.GunnerBullet => Define.PlayerEffect.JururuFoxGunnerBullet,
            EffectType.GunnerShoot => Define.PlayerEffect.JururuFoxGunnerShoot,
            EffectType.MagicianReady => Define.PlayerEffect.JururuFoxMagicianReady,
            EffectType.MagicianShoot => Define.PlayerEffect.JururuFoxMagicianShoot,
            EffectType.Hit => Define.PlayerEffect.JururuFoxSoldierHit,
            EffectType.Release => Define.PlayerEffect.JururuFoxSoldierRelease,
            EffectType.Summon => Define.PlayerEffect.JururuFoxSoldierSummon,
            EffectType.SummonLoop => Define.PlayerEffect.JururuFoxSoldierSummonLoop,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    public void SpawnVFX(EffectType type)
    {
        string address = GetEffectAddress(type);
        EffectSpawner.Spawn(address, effectParent.position, true);
    }

    public void SpawnVFXInSpine(EffectType type)
    {
        string address = GetEffectAddress(type);
        EffectSpawner.Spawn(address, "center",Vector2.zero,true);
    }
    [Serializable]
    [HideLabel]
    public class SoldierInfo
    {
        [LabelText("공격 사거리")] public float atkRange;
        [LabelText("공격 설정")] public ProjectileInfo atkInfo;
        [LabelText("그로기 수치")] public int groggy;
        [LabelText("최소 소환 반경 (적 기준)")] public float minSummonRadius;
        [LabelText("최대 소환 반경 (적 기준")] public float maxSummonRadius;
        [LabelText("공격 쿨타임")] public float atkCd;
        [LabelText("탐색 거리")] public float findRange;
    }
    
    public override float Atk => skill.CalculateDmg();

    [FoldoutGroup("기획쪽 수정 변수들")] [TabGroup("기획쪽 수정 변수들/group1", "병사 스탯")]
    public SoldierInfo info;

    [LabelText("투사체 발사위치")]
    public Transform firePos;

    protected abstract IAtkStrategy AtkStrategy { get; } // 공격 전략 (방패, 칼, 마법 등)
    protected abstract IAtkCondition AtkCondition { get; } // 공격 조건 (수직거리, 사정거리 내 등)

    public enum States
    {
        Idle,Attack,Death
    }
    private StateMachine<FoxSoldier> stateMachine;
    private Dictionary<States, IState<FoxSoldier>> stateDicts;
    
    
    public override void IdleOn()
    {
    }

    public override void AttackOn()
    {
    }

    public override void AttackOff()
    {
    }

    private bool isAttackable;
    private Actor target;
    private JururuPassiveSkill skill;
    
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        pokdoAttackCollider = GetComponentInChildren<AttackObject>(true);
        stateDicts = new();
        stateDicts.Add(States.Idle,new IdleState());
        stateDicts.Add(States.Attack,new AttackState());
        stateDicts.Add(States.Death,new DeadState());

        stateMachine = new(this, stateDicts[States.Idle]);
        AddEvent(EventType.OnAttackSuccess, param =>
        {
            if (param?.target != null)
            {
                EffectSpawner.Spawn(GetEffectAddress(EffectType.Hit), param.target.Position, false);
            }
        });
    }
    
    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
        if (isAttackable)
        {
            AtkCondition?.Update();
        }
    }

    public void ChangeState(States state)
    {
        stateMachine.SetState(stateDicts[state]);
    }
    private Coroutine destroyCoroutine;
    public void Init(float duration,PatternType patternType,JururuPassiveSkill _skill)
    {
        isAttackable = true;
        destroyCoroutine = StartCoroutine(StartDuration(duration));
        pokdoAttackCollider?.gameObject.SetActive(false);
        IsDead = false;
        currentPattern = patternType;
        target = null;
        ChangeState(States.Idle);
        skill = _skill;
        EffectSpawner.Remove(GetEffectAddress(EffectType.Summon));
        EffectSpawner.Remove(GetEffectAddress(EffectType.SummonLoop));
        SpawnVFX(EffectType.Summon);
        SpawnVFX(EffectType.SummonLoop);
    }

    private IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(info.atkCd);
        isAttackable = true;
    }

    public void Attack() // 애니메이션에서 사용
    {
        AtkStrategy?.Attack();
    }

    public void AttackEnd() // 애니메이션에서 사용
    {
        atkPattern?.AfterAtk();
    }
    public void DoAttack(Transform target)
    {
        ChangeState(States.Attack);
        atkMethod?.Attack(target);
        isAttackable = false;
    }

    public void Attack(Transform target)
    {
        EActorDirection dir = target.position.x > transform.position.x ? EActorDirection.Right : EActorDirection.Left;
        SetDirection(dir);
        animator.SetTrigger("Attack");
    }
    private void Attack(EActorDirection dir)
    {
        SetDirection(dir);
        animator.SetTrigger("Attack");
    }
    IEnumerator StartDuration(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Death");
    }
    public override void Die()
    {
        base.Die();
        EffectSpawner.Remove(GetEffectAddress(EffectType.SummonLoop));
        GameManager.Factory.Return(gameObject);
        StopCoroutine(destroyCoroutine);
        pokdoAttackCollider?.gameObject.SetActive(false);
        EffectSpawner.Spawn(GetEffectAddress(EffectType.Release), Position, false);
    }

    public override float OnHit(EventParameters parameters)
    {
        return 0;
    }

    public enum PatternType
    {
        Normal, Following
    }

    protected PatternType currentPattern;

    protected abstract IIdlePattern idlePattern { get; }
    protected abstract IAttackMethod atkMethod { get; }
    protected abstract IAtkPattern atkPattern { get; }
}