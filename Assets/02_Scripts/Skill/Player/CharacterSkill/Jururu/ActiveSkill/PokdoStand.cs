using System;
using System.Collections.Generic;
using Apis;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

public class PokdoStand : Summon
{
    public override Actor Master => masterPlayer;

    [HideInInspector] public Player masterPlayer;

    public AttackObject pokdoCollider;

    private PetFollower _petFollower;
    private JururuActiveSkill skill;
    public PetFollower PetFollower
    {
        get
        {
            if (_petFollower == null)
            {
                _petFollower = GetComponent<PetFollower>();
            }

            return _petFollower;
        }
    }

    private StateMachine<PokdoStand> StateMachine;
    private Dictionary<PokdoState, IState<PokdoStand>> states;
    
    public enum PokdoState
    {
        Idle,Attack,Dead,Skill,Dash
    }

    public override float OnHit(EventParameters parameters)
    {
        return 0;
    }

    class Dead : IState<PokdoStand>
    {
        public void OnEnter(PokdoStand t)
        {
            t.PetFollower.moveOn = false;
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
    class Idle : IState<PokdoStand>
    {
        private PokdoStand p;
        public void OnEnter(PokdoStand t)
        {
            p = t;

            p.PetFollower.moveOn = true;
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
            p.PetFollower.moveOn = false;
        }
    }

    class Dash : IState<PokdoStand>
    {
        private PokdoStand p;
        public void OnEnter(PokdoStand t)
        {
            p = t;

            p.PetFollower.moveOn = true;
            p.animator.SetTrigger("DashStart");
        }
        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
            p.animator.SetTrigger("DashEnd");
        }
    }
    class AttackState : IState<PokdoStand>
    {

        private PokdoStand p;
        public void OnEnter(PokdoStand t)
        {
            p = t;
            p.PetFollower.moveOn = false;
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
            p.AttackEnd();
            p.transform.position = p.masterPlayer.transform.position + Vector3.left * (int)p.masterPlayer.Direction;
            
        }
    }
    
    class SkillState : IState<PokdoStand>
    {
        public void OnEnter(PokdoStand t)
        {
            t.PetFollower.moveOn = false;
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

    [Serializable]
    public struct AttackInfo
    {
        public float dmg;
        public int groggy;
    }

    [LabelText("공격 정보")] public List<AttackInfo> attackInfos;
    
    protected override void Awake()
    {
        base.Awake();

        states = new()
        {
            { PokdoState.Idle, new Idle() },
            { PokdoState.Dash ,new Dash()},
            { PokdoState.Attack, new AttackState() },
            { PokdoState.Dead ,new Dead()},
            { PokdoState.Skill ,new SkillState()}
        };
        StateMachine = new(this, states[PokdoState.Idle]);
        pokdoCollider.Init(this,new AtkBase(this,0));
    }

    public void Init(Player master,JururuActiveSkill _skill)
    {
        masterPlayer = master;

        animator = Utils.GetComponentInParentAndChild<Animator>(gameObject);
        animator.keepAnimatorStateOnDisable = true;
        animator.Rebind();
        animator.SetBool("IsSkill",false);
        skill = _skill;
        masterPlayer.animator.ResetTrigger("AttackEnd");
        if (skill.grabSkill != null)
        {
            skill.grabSkill.pokdo = this;
        }
        if (skill.crySkill != null)
        {
            skill.crySkill.pokdo = this;
        }
        skill.atkSkill?.Init(this);
        
        IsDead = false;
        PetFollower.Init(masterPlayer.transform,masterPlayer,new Vector2(1,0));
        PetFollower.SetPosition();
        if (effectParent == null)
        {
            effectParent = new GameObject("PokdoEffectParent").transform;
            DontDestroyOnLoad(effectParent);
        }
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        StateMachine.FixedUpdate();
    }

    public override void AttackOff()
    {
    }

    public override void AttackOn()
    {
    }

    public override void IdleOn()
    {
    }
    
    public override float Atk => skill.CalculateDmg();

    public void AttackCommand()
    {
        animator.SetTrigger("Attack");
    }
    public void AttackInit(int index) // 애니메이션에서 사용, 공격 시작할 때 호출
    {
        pokdoCollider.Init(this,new AtkBase(this,attackInfos[index].dmg));
        pokdoCollider.Init(attackInfos[index].groggy);
        transform.position = Master.transform.position + Vector3.right * (int)Master.Direction;
        SetDirection(Master.Direction);
    }

    public void Attack(int index) // 애니메이션에서 공격 타이밍에 호출
    {
        skill.OnPokdoAttack?.Invoke(this,index);
    }
    public void FinalAttack() // 애니메이션에서 사용
    {
        masterPlayer.OnFinalAttack = true;
    }

    public void AttackEnd()
    {
        animator.ResetTrigger("Attack");
        masterPlayer.animator.SetTrigger("AttackEnd");
    }

    public override void Die()
    {
        IsDead = true;

        SetState("Dead");
        animator.SetTrigger("Dead");
        ReturnPlayerControl();
    }

    public void ReturnPlayerControl()
    {
        masterPlayer.animator.SetTrigger("AttackEnd");
        masterPlayer.SetAttackToNormal();
    }
    private PokdoState curState;

    public PokdoState GetState()
    {
        return curState;
    }
    public void SetState(string state)
    {
        if (Enum.TryParse(state, out PokdoState st))
        {
            StateMachine.SetState(states[st]);
            curState = st;
        }
    }

    public void UseESkill()
    {
        pokdoCollider.Init(skill.attacker,new AtkBase(this,skill.crySkill.Atk));
        SetState("Skill");
        animator.SetTrigger("SkillStart");
        skill.Stop();
        ReturnPlayerControl();
        animator.SetBool("IsSkill",true);
    }
    public void Cancel() // 애니메이터에서 사용
    {
        skill.Cancel();
    }

    private Grab grab;
    private Vector2 startPos;

    public AttackObject SpawnGrab(PokdoMouseRSkill grabSkill)
    {
        masterPlayer.IdleOn();
        masterPlayer.ControlOff();
        grab = GameManager.Factory.Get<Grab>(FactoryManager.FactoryType.AttackObject,
            Define.PlayerSkillObjects.JururuPokdoGrab,
            transform.position + Vector3.right * (grabSkill.distance * (int)Direction));
        
        Vector3 scale = grab.transform.localScale;
        grab.transform.SetParent(effectParent);
        grab.transform.localScale = scale;
        grab.Init(Master,new AtkBase(this,grabSkill.atkInfo.dmg));
        grab.Init(grabSkill.atkInfo);
        grab.Init(grabSkill.groggy);
        
        grab.transform.localScale = new Vector3(Mathf.Abs(scale.x) * (int)Direction, scale.y, scale.z);
        startPos = masterPlayer.transform.position + Vector3.right * ((int)Direction * grabSkill.endDistance);

        return grab;
    }

    public void Pull(float time)
    {
        grab?.MoveToPos(startPos,time, x =>
        {
            x.Destroy();
            GameManager.instance.Player.ControlOn();
        });
    }
}
