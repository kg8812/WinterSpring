using System;
using System.Collections.Generic;
using Apis;
using chamwhy;
using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

public partial class SeguMecha : Summon, IDashUser, IMovable , IPlayer , IPhysicsTransition
{
    class Idle : IState<SeguMecha>
    {
        private SeguMecha drone;
        public void OnEnter(SeguMecha t)
        {
            t.Rb.velocity = new Vector2(0, t.Rb.velocity.y);

            if (t.onAir)
            {
                t.Rb.velocity = new Vector2(0, t.Rb.velocity.y);
                t.Rb.gravityScale = t.ActorMovement.GravityScale;
            }
            else
            {
                t.Rb.velocity = Vector2.zero;   
            }

            drone = t;
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            if (!drone.ActorMovement.IsStick)
            {
                if (!drone.onAir)
                {
                    drone.onAir = true;
                }
                drone.Rb.gravityScale = drone.ActorMovement.GravityScale;
            }
            else
            {
                drone.dashCount = 0;
            }
            if (drone.onAir && drone.ActorMovement.IsStick)
            {
                drone.onAir = false;
            }
        }

        public void OnExit()
        {
        }
    }

    class Move : IState<SeguMecha>
    {
        private SeguMecha mecha;
        public void OnEnter(SeguMecha t)
        {
            mecha = t;
            mecha.animator.SetBool("IsMove",true);
            mecha.EffectSpawner.Spawn(Define.PlayerEffect.GoseguMechaMove, "center", false);
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            if (mecha.ActorMovement.IsStick)
            {
                mecha.dashCount = 0;
            }
            else
            {
                if (!mecha.onAir)
                {
                    mecha.onAir = true;
                }
                mecha.Rb.gravityScale = mecha.ActorMovement.GravityScale;
            }

            if (mecha.onAir && mecha.ActorMovement.IsStick)
            {
                mecha.onAir = false;
            }
        }

        public void OnExit()
        {
            mecha.animator.SetBool("IsMove",false);
            mecha.EffectSpawner.Remove(Define.PlayerEffect.GoseguMechaMove);
        }
    }

    class Jump : IState<SeguMecha>
    {
        private SeguMecha drone;
        public void OnEnter(SeguMecha t)
        {
            drone = t;
            t.ActorMovement.Jump(drone.player.playerStat.JumpPower * 1.5f);
            drone.Rb.gravityScale = drone.ActorMovement.GravityScale;
            drone.onAir = true;
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
        }
    }

    class Dash : IState<SeguMecha>
    {
        private Tween tween;
        private SeguMecha mecha;

        private Guid guid;
        public void OnEnter(SeguMecha t)
        {
            
            mecha = t;
            mecha.MoveComponent.MoveOff();
            mecha.BlockIdle = true;
            
            mecha.Rb.gravityScale = 0;
            mecha.Rb.velocity = new Vector2(mecha.Rb.velocity.x, 0);

            mecha.ExecuteEvent(EventType.OnDash, new EventParameters(mecha));

            mecha.dashCount++;
            mecha.skill.Gauge -= mecha.skill.gaugeReduce2;
            mecha.IsDash = true;
            
            guid = mecha.AddInvincibility();
            mecha.EffectSpawner.Spawn(Define.PlayerEffect.GoseguMechaDash, "center", false);
            Vector2 dir = Vector2.right;
            switch (t.controller.InputDir)
            {
                case DroneController.InputDirection.Up:
                    dir = Vector2.up;
                    break;
                case DroneController.InputDirection.Right:
                    dir = Vector2.right;
                    break;
                case DroneController.InputDirection.Down:
                    dir = Vector2.down;
                    break;
                case DroneController.InputDirection.Left:
                    dir = Vector2.left;
                    break;
                case DroneController.InputDirection.RightUp:
                    dir = new Vector2(1, 1);
                    break;
                case DroneController.InputDirection.LeftUp:
                    dir = new Vector2(-1, 1);
                    break;
                case DroneController.InputDirection.RightDown:
                    dir = new Vector2(1, -1);
                    break;
                case DroneController.InputDirection.LeftDown:
                    dir = new Vector2(-1, -1);
                    break;
            }

            tween = t.ActorMovement.DashInDirection(mecha.dashDuration, mecha.dashDistance, dir);
            tween.onKill += () =>
            {
                t.ChangeState(States.Idle);
            };
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
            mecha.BlockIdle = false;
            mecha.RemoveInvincibility(guid);
            mecha.EffectSpawner.Remove(Define.PlayerEffect.GoseguMechaDash);
        }
    }

    private Player player;
    public Transform firePos;
    public Transform ridePos;
    public bool BlockIdle;

    // 이동 방향, 바라보는 방향으로 움직이지 않는 경우도 있어서 기본 방향과 분리를 해뒀음.
    public EActorDirection moveDir; 
    
    public override float MaxHp => base.MaxHp * skill.Hp / 100;

    public override float Atk => skill.CalculateDmg();

    public override StatManager StatManager => player.StatManager;

    [FoldoutGroup("기획쪽 수정 변수들")]
    [TabGroup("기획쪽 수정 변수들/group1","메카 스탯")] 
    [LabelText("기본 공격속도 (100 = 초당 1발)")] public float baseAtkSpeed;
    [FoldoutGroup("기획쪽 수정 변수들")]
    [TabGroup("기획쪽 수정 변수들/group1","메카 스탯")]
    [LabelText("홀딩시 초당 공속 증가치 (%)")] public float atkSpeedIncrement;
    [FoldoutGroup("기획쪽 수정 변수들")]
    [TabGroup("기획쪽 수정 변수들/group1","메카 스탯")]
    [LabelText("최대 공속 증가치 (%)")] public float maxIncrement;
    [FoldoutGroup("기획쪽 수정 변수들")]
    [TabGroup("기획쪽 수정 변수들/group1","메카 스탯")]
    [LabelText("투사체 설정")] public ProjectileInfo bulletInfo;
    [FoldoutGroup("기획쪽 수정 변수들")]
    [TabGroup("기획쪽 수정 변수들/group1","메카 스탯")]
    [LabelText("그로기 수치")] public int groggy;

    [FoldoutGroup("기획쪽 수정 변수들")] [TabGroup("기획쪽 수정 변수들/group1", "메카 스탯")]
    [LabelText("대쉬 거리")] public float dashDistance;

    [FoldoutGroup("기획쪽 수정 변수들")] [TabGroup("기획쪽 수정 변수들/group1", "메카 스탯")] [LabelText("대쉬 시간")]
    public float dashDuration;
    
    private GoseguActiveSkill skill;

    public Transform gaugePos;

    [HideInInspector] public float curChargeTime;
    [HideInInspector] public float maxChargeTime;

    [HideInInspector] public int dashCount;
    public int maxDashCount = 1;
    
    [Serializable]
    public struct LaserInfo
    {
        [LabelText("레이저 공격 설정")] public ProjectileInfo atkInfo;
        [LabelText("레이저 발사 설정")] public BeamEffect.BeamInfo beamInfo;
        [LabelText("레이저 그로기 수치")] public int groggy;
        [LabelText("레이저 지속시간")] public float duration;
        [LabelText("최소 차징 시간")] public float minChargeTime;
        [LabelText("최대 차징 시간")] public float maxChargeTime;
        [LabelText("초당 데미지 증가량 (%)")] public float chargeDmg;
        [LabelText("초당 사정거리 증가 수치")] public float chargeDistance;
    }
    public override void IdleOn()
    {
    }

    public override void AttackOn()
    {
    }

    public override void AttackOff()
    {
    }

    private UI_SeguMechaGauge gaugeUI;

    public override float OnHit(EventParameters parameters)
    {
        float dmg = base.OnHit(parameters);

        float playerDmg = dmg;
        if (playerDmg > (player.MaxHp / 10f))
        {
            playerDmg = player.MaxHp / 10f;
        }
        player.CurHp -= playerDmg;

        return dmg;
    }

    private Guid playerGuid;
    
    public override void SetMaster(Actor master)
    {
        base.SetMaster(master);
        player = master as Player;
        SetDirection(master.Direction);
        playerGuid = player.AddInvincibility();
        IsDead = false;
        hpBarUi = GameManager.UI.CreateUI("UI_SemiHpBar", UIType.Ingame, withoutActivation:true) as UI_SemiHpBar;
        if (hpBarUi != null)
        {
            hpBarUi.InitActor(this);
            hpBarUi.SetTrans(transform);
            hpBarUi.TryActivated();
        }

        controller.ReturnToBase();
        ResetCurHp();
        ChangeAtkStrategy(new FireBullets(this));
        UpdateInfos();
        GameManager.instance.ChangeControllingEntity(this);
    }

    public void Init(GoseguActiveSkill active)
    {
        skill = active;
        // ItemId - 4503: 메카 공격
        attackSkill = (InvenManager.instance.PresetManager.GetOverrideItem(4503) as ActiveSkillItem)?.ActiveSkill as SeguMechaAttackSkill;
        attackSkill?.Init(this);
        active.cannonSkill?.Init(this);
    }
    
    private StateMachine<SeguMecha> _stateMachine;
    private Dictionary<States, IState<SeguMecha>> stateDict;
    private UI_SemiHpBar hpBarUi;
    
    public enum States
    {
        Idle,Jump,Move,Dash
    }

    DroneController controller;
    public DroneController Controller => controller;

    private IAtkStrategy _atkStrategy;
    IAtkStrategy AtkStrategy => _atkStrategy;

    public bool IsDash { get => isDash; set => isDash = value; }
    private bool isDash = false;

    private float atkSpeed;

    private UnitMoveComponent _moveComponent;

    public UnitMoveComponent MoveComponent
    {
        get
        {
            if (_moveComponent == null)
            {
                _moveComponent = gameObject.GetOrAddComponent<UnitMoveComponent>();
                _moveComponent.Init(this,Collider);
            }

            return _moveComponent;
        }
    }

    public ActorMovement ActorMovement => MoveComponent.ActorMovement;
    private SeguMechaAttackSkill attackSkill;
    
    protected override void Awake()
    {
        base.Awake();
        stateDict = new()
        {
            { States.Idle, new Idle() },
            { States.Move, new Move() },
            { States.Jump, new Jump() },
            { States.Dash, new Dash() },
        };

        _stateMachine = new StateMachine<SeguMecha>(this, stateDict[States.Idle]);
        curState = States.Idle;
        controller = GetComponent<DroneController>();
        animator = GetComponentInChildren<Animator>();
        AddEvent(EventType.OnAttackSuccess, x =>
        {
            if (x.target != null)
            {
                EffectSpawner.Spawn(Define.PlayerEffect.GoseguMechaAtkEffect, x.target.Position, false);
            }
        });
    }
    
    public virtual void CorrectingPlayerPosture(bool isLanding = true /* 바닥에 맞게 할지 여부 */)
    {
        Rb.velocity = Vector2.zero;

        if (isLanding)
        {
            MoveToFloor();
        }

        MoveComponent.ActorMovement.Tweener.Kill();
        IdleOn();
        // TODO: 플레이이 애니메이션 초기화..? 대쉬중 포탈들어갈때도 있어서 그거 idle로 바꿔줘야 함
    }

    private States curState;
    
    public void ChangeState(States state)
    {
        if (state == curState) return;
        
        _stateMachine.SetState(stateDict[state]);
        curState = state;
    }

    public States GetState()
    {
        return curState;
    }
    public void ChangeAtkStrategy(IAtkStrategy atkStrategy)
    {
        _atkStrategy = atkStrategy;
        if (AtkStrategy is FireLaser)
        {
            gaugeUI = GameManager.UI.CreateUI("UI_SeguMechaGauge", UIType.Ingame) as UI_SeguMechaGauge;
            gaugeUI?.Init(this);
        }
    }

    public void AttackReset()
    {
        animator.ResetTrigger("AttackEnd");
    }
    public void FireOn()
    {
        if (AtkStrategy is { IsAtk: true }) return;
        AtkStrategy?.FireOn();
    }

    public void FireEnd()
    {
        AtkStrategy?.FireOff();
    }

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update();
        player.transform.position = ridePos.position;
        
        AtkStrategy?.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _stateMachine.FixedUpdate();
        player.transform.position = ridePos.position;
    }
    
    public void DoMove()
    {
        ActorMovement.Move(moveDir,1);
    }
    public override void SetDirection(EActorDirection dir)
    {
        if (AtkStrategy is { IsAtk: true, IsAbleToTurn: false }) return;
        
        base.SetDirection(dir);
        player.SetDirection(dir);
    }

    public override void Die()
    {
        base.Die();
        EffectSpawner.Spawn(Define.PlayerEffect.GoseguMechaRide, transform.position, false);
        skill.OnMechaDie.Invoke(this);
        skill?.Use();
        WhenDisable();

        animator.SetTrigger("Death");
    }

    public void WhenDisable()
    {
        FireEnd();
        player.RemoveInvincibility(playerGuid);
        player.ApplyPlayerPreset();
        GameManager.instance.ChangeControllingEntity(player);
        player.PhysicsTransitionHandler.StartColliderTransition(Collider,player.Collider);
        GameManager.UI.CloseUI(hpBarUi);
        if (gaugeUI != null)
        {
            GameManager.UI.CloseUI(gaugeUI);
            gaugeUI = null;
        }
        GameManager.instance.Player.BlockSkillChange = false;
        
        RemoveAllBuff();
    }

    public void UpdateInfos()
    {
        if (IsDead || !gameObject.activeSelf) return;

        skill.cannonSkill?.Init(this);
        AttackItemManager.ApplyPreset(8);
    }

    public void DoCannon()
    {
        skill.cannonSkill.actionList[0].Invoke();
    }

    public void DoPulse()
    {
        skill.pulseGunSkill.actionList[0].Invoke();
    }
    public void SetDash(Player.IPlayerDash dash)
    {
    }

    public void SetDashToNormal()
    {
    }

    public void DashOn()
    {
    }

    public void DashOff()
    {
    }

    public ActorPhysicsTransitionHandler PhysicsTransitionHandler => player.PhysicsTransitionHandler;

    public void CannonAction()
    {
        
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        player.OnTriggerEnter2D(other);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        player.OnTriggerExit2D(other);
    }
}
