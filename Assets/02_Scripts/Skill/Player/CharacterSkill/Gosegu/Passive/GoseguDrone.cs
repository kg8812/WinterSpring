using System;
using System.Collections;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public abstract partial class GoseguDrone : Summon
    {
        protected Player player;
        private ParticleSystem freezeEffect;
        
        public override void IdleOn()
        {
        }

        public override void AttackOn()
        {
        }

        public override void AttackOff()
        {
        }

        public override float OnHit(EventParameters parameters)
        {
            return 0;
        }

        private IAtkStrategy _atkStrategy;
        protected IAtkStrategy AtkStrategy => _atkStrategy;

        protected abstract ISearchStrategy SearchStrategy { get; }

        public Action<AttackObject> OnAttack;
        public void ChangeAtkStrategy(IAtkStrategy atkStrategy)
        {
            _atkStrategy = atkStrategy;
        }

        public abstract void ReturnToOriginalAtkStrategy();
        
        [Serializable]
        [FoldoutGroup("기획쪽 수정 변수들")]
        [TabGroup("기획쪽 수정 변수들/group1","드론 설정")]
        [HideLabel]
        public class DroneInfo
        {
            [LabelText("쿨타임")] public float cd;

            public static DroneInfo operator +(DroneInfo a,DroneInfo b)
            {
                DroneInfo c = new()
                {
                    cd = a.cd + b.cd,
                };

                return c;
            }

            public static DroneInfo operator -(DroneInfo a, DroneInfo b)
            {
                DroneInfo c = new()
                {
                    cd = a.cd - b.cd,
                };

                return c;
            }
        }
        
        private float curCd;

        public float CurCd
        {
            get => curCd;
            set => curCd = Mathf.Clamp(value,0,_DroneInfo.cd);
        }
        private PetFollower petFollower;
        public PetFollower PetFollower => petFollower;
        
        private bool isCd;
        
        protected abstract DroneInfo droneInfo { get; }

        public virtual DroneInfo _DroneInfo
        {
            get
            {
                DroneInfo temp = new();
                temp += droneInfo;
                
                passive?._droneInfos.ForEach(x =>
                {
                    temp += x;
                });

                return temp;
            }
        }
        
        private GoseguPassive passive;

        public override float Atk => passive?.CalculateDmg() ?? 0;

        protected override void Awake()
        {
            base.Awake();
            freezeEffect = GetComponentInChildren<ParticleSystem>();
            petFollower = GetComponent<PetFollower>();
            animator = GetComponent<Animator>();
            isCd = false;
            ReturnToOriginalAtkStrategy();
            AddEvent(EventType.OnKill, x =>
            {
                passive?.OnDroneKill.Invoke();
            });
            OnAttack += x =>
            {
                passive?.OnDroneAttack?.Invoke(x);
            };
        }
        
        public override void SetMaster(Actor master)
        {
            base.SetMaster(master);
            player = master as Player;
            if (player == null) return;
            freezeEffect.Play();
            SetAtkEvent();
            isCd = false;
            CurCd = 0;
        }

        public void Init(GoseguPassive skill,Vector2 offset)
        {
            passive = skill;
            petFollower.Init(player.transform,player,offset);
        }

        protected override void Update()
        {
            base.Update();
            if (!isCd)
            {
                SearchStrategy?.Update();
            }
        }

        protected abstract void SetAtkEvent();
        protected abstract void RemoveAtkEvent();

        protected void DroneAttack(EventParameters parameters)
        {
            if (!gameObject.activeSelf || isCd) return;

            param = parameters;
            animator.SetTrigger("Attack");
            
        }

        private EventParameters param;
        public void DoAttack()
        {
            if (!gameObject.activeSelf || isCd) return;
            
            if (AtkStrategy.Attack(SearchStrategy.GetTargets(param)))
            {
                GameManager.instance.StartCoroutine(CdCoroutine());
            }
        }

        public void ReleaseMaster()
        {
            RemoveAtkEvent();
        }
        IEnumerator CdCoroutine()
        {
            if (isCd) yield break;
            isCd = true;
            CurCd = _DroneInfo.cd;
            while (CurCd > 0)
            {
                CurCd -= Time.deltaTime * (1 + CDReduction / 100);
                yield return null;
            }

            isCd = false;
        }

        private void OnDisable()
        {
            SearchStrategy?.OnDisable();
        }
    }
}
