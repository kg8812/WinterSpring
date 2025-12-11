using System;
using System.Collections;
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Apis
{
    [CreateAssetMenu(fileName = "ViichanPassive", menuName = "Scriptable/Skill/ViichanPassive")]
    public class ViichanPassiveSkill : PlayerPassiveSkill
    {

        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")] [LabelText("야수 그로기 수치")] [SerializeField]
        public float groggy;

        [TitleGroup("스탯값")] [LabelText("최대 게이지")]
        [SerializeField] public int maxGauge;

        [TitleGroup("스탯값")] [LabelText("공격당 게이지 회복량")] [SerializeField]
        public int atkGauge;

        [TitleGroup("스탯값")] [LabelText("추가 회복 최소 그로기")] [SerializeField]
        public int minGroggy;

        [TitleGroup("스탯값")] [LabelText("추가 게이지 회복량")] [SerializeField]
        public int extraGauge;
        
        [TitleGroup("스탯값")] [LabelText("상호작용 반경")] [SerializeField]
        public float interactionRadius;
        
        [TitleGroup("스탯값")] [LabelText("야수 공격모션 최소 지속시간")] [SerializeField]
        public float atkTime;

        [TitleGroup("스탯값")] [LabelText("야수 지속시간")] [SerializeField]
        public float beastDuration;

        [TitleGroup("스탯값")] [LabelText("이동속도 증가량")] [SerializeField]
        public float moveSpeedBuff;

        enum State
        {
            Demand, // 수급상태
            Activated, // 발동상태
        }

        private State curState;

        bool CheckInteractable(Monster _)
        {
            return Mathf.Approximately(CurGauge,maxGauge) || CurGauge >= maxGauge;
        }
        private float curGauge;
        public float CurGauge
        {
            get => curGauge;
            set
            {
                if (curState != State.Demand && value > curGauge)
                {
                    return;
                }
                
                curGauge = Mathf.Clamp(value, 0, maxGauge);
                OnGaugeChange?.Invoke(curGauge);
                // if (Mathf.Approximately(curGauge, maxGauge) && curState == State.Demand)
                // {
                //     GameManager.instance.StartCoroutine(StartWaiting());
                // }
            }
        }

        [HideInInspector]public bool isDemandEnhanced = false;
        
        [HideInInspector] public Action<float> OnGaugeChange;
        [HideInInspector] public Action OnBeastStart;
        [HideInInspector] public Action OnBeastEnd;
        [HideInInspector] public Action<int> OnBeastAtk;

        [HideInInspector] public ActiveSkill shieldSkill;
        [HideInInspector] public ActiveSkill grabSkill;

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Beast;

        protected override float TagIncrement => GameManager.Tag.Data.BeastIncrement;
        
        private int beastIndex;
        public override void Init()
        {
            base.Init();
            isDemandEnhanced = false;
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            beastIndex = animator?.animator.GetLayerIndex("BeastLayer") ?? 0;
            eventUser?.EventManager.AddEvent(EventType.OnBasicAttack,AddAtkGauge);
            Monster.MakeInteractable(interactionRadius);
            curState = State.Demand;
            Monster.InteractEvent.AddListener(StartBeastMode);
            Monster.CheckInteractable -= CheckInteractable;
            Monster.CheckInteractable += CheckInteractable;
            animator?.animator.SetLayerWeight(beastIndex,0);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            eventUser?.EventManager.RemoveEvent(EventType.OnBasicAttack,AddAtkGauge);
            if (curState == State.Activated)
            {
                EndBeastMode();
            }
            Monster.RemoveInteractable();
            Monster.InteractEvent.RemoveListener(StartBeastMode);
            Monster.CheckInteractable -= CheckInteractable;
        }

        public override void Active()
        {
            base.Active();
            GameManager.instance.Player.BlockSkillChange = true;
            animator?.animator.SetLayerWeight(beastIndex,1);
            animator?.animator.SetBool("IsBeast",true);
            
            GameManager.instance.Player.SetAttack(new Player.ViichanBeastAttack(GameManager.instance.Player,this));
            AttackItemManager.ApplyPreset(9);
            Player.ChangeActiveSkill(shieldSkill);

            if (statUser != null)
            {
                statUser.StatManager.BonusStatEvent += BeastStat;
            }

            guid = hit.AddHitImmunity();
            dashUser.SetDash(new Player.BeastDash(user as Player));
            isBeastPause = false;
            GameManager.instance.StartCoroutine(BeastMode());
            OnBeastStart?.Invoke();
        }

        void AddAtkGauge(EventParameters parameters)
        {
            if (parameters == null) return;

            CurGauge += atkGauge;
            if (parameters.atkData.groggyAmount >= minGroggy)
            {
                CurGauge += extraGauge;
            }
        }

        //IEnumerator StartWaiting()
        //{
            // curState = State.Wait;
            // animator?.animator.SetLayerWeight(beastIndex,0);
            // while (!Mathf.Approximately(CurGauge,0))
            // {
            //     if (curState != State.Wait)
            //         break;
            //     
            //     CurGauge -= maxGauge / waitingDuration * Time.deltaTime;
            //     yield return null;
            // }
            //
            // if (Mathf.Approximately(CurGauge, 0))
            // {
            //     curState = State.Demand;
            // }
        //}

        private BonusStat _bonusStat;
        
        BonusStat BeastStat()
        {
            if (_bonusStat == null)
            {
                _bonusStat = new();
                _bonusStat.AddRatio(ActorStatType.MoveSpeed, moveSpeedBuff);
            }
            return _bonusStat;
        }

        private Guid guid;
        
        void StartBeastMode(Monster _)
        {
           Use();
        }

        public bool IsBeast => curState == State.Activated;
        
        IEnumerator BeastMode()
        {
            if (IsBeast) yield break;

            curState = State.Activated;

            curGauge = maxGauge;
            while (!Mathf.Approximately(CurGauge, 0) && curState == State.Activated)
            {
                if (!isBeastPause)
                {
                    CurGauge -= maxGauge / beastDuration * Time.deltaTime;
                }
                yield return null;
            }

            var state = GameManager.instance.Player.GetState();
            while (state is EPlayerState.Attack or EPlayerState.Dash or EPlayerState.Skill)
            {
                state = GameManager.instance.Player.GetState();
                yield return null;
            }
            EndBeastMode();
        }
        void EndBeastMode()
        {
            OnBeastEnd?.Invoke();
            curState = State.Demand;
            animator?.animator.SetLayerWeight(beastIndex,0);
            animator?.animator.SetBool("IsBeast",false);
            GameManager.instance.Player.SetAttackToNormal();
            dashUser?.SetDashToNormal();
           
            Player.ApplyPlayerPreset();
            GameManager.instance.Player.BlockSkillChange = false;
            Player.ResetActiveSkill();
            if (statUser != null)
            {
                statUser.StatManager.BonusStatEvent -= BeastStat;
            }

            hit.RemoveHitImmunity(guid);
        }

        public void IncreaseBeastDuration(float time)
        {
            if (IsBeast)
            {
                float value = curGauge + maxGauge / beastDuration * time;
                curGauge = Mathf.Clamp(value, 0, maxGauge);
            }
        }

        private bool isBeastPause;
        
        public void PauseBeastDuration(float time)
        {
            isBeastPause = true;
            Sequence seq = DOTween.Sequence();
            seq.SetDelay(time);
            seq.AppendCallback(() =>
            {
                isBeastPause = false;
            });
        }

        public void ApplyChange()
        {
            if (IsBeast)
            {
                AttackItemManager.ApplyPreset(9);
                Player.ChangeActiveSkill(shieldSkill);
            }
        }
    }
}