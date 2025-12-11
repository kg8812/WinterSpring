using System;
using System.Collections;
using chamwhy;
using GameStateSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Apis
{
    [CreateAssetMenu(fileName = "GoseguActive", menuName = "Scriptable/Skill/GoseguActive")]
    public class GoseguActiveSkill : PlayerActiveSkill
    {
        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")] [LabelText("최소 게이지")] [SerializeField] private float minGauge;
        [TitleGroup("스탯값")] [LabelText("체력계수 (%)")] [SerializeField] private float hp;
        [TitleGroup("스탯값")] [LabelText("초당 게이지 감소량")] [SerializeField] public float gaugeReduce1;
        [TitleGroup("스탯값")] [LabelText("회피시 게이지 감소량")] [SerializeField]public float gaugeReduce2;
        [TitleGroup("스탯값")] [LabelText("공격시 게이지 회복량")] [SerializeField] float gaugeRatio1;
        [TitleGroup("스탯값")] [LabelText("처치시 게이지 회복량")] [SerializeField] float gaugeRatio2;
        [TitleGroup("스탯값")] [LabelText("최대 게이지")] [SerializeField] private float maxGauge;
        protected override ActiveEnums _activeType => ActiveEnums.Toggle;

        private SeguMecha mecha;

        public SeguMecha Mecha => mecha;
        float gauge;


        [HideInInspector] public GoseguPassive passive;
        IGoseguActive goseguStat => stats as IGoseguActive;
        
        #region 스탯값

        public float Hp => (hp + goseguStat.GoseguStat.hp) * (1 + goseguStat.GoseguStat.hpRatio / 100 +
                                                              GameManager.Tag.GetTagCount(TagManager.SkillTreeTag.Mecha)
                                                              * GameManager.Tag.Data.MechaHpIncrement / 100);
        public float MaxGauge => (maxGauge + goseguStat.GoseguStat.maxGauge) * (1 + goseguStat.GoseguStat.maxGaugeRatio / 100);

        public float FinalDmgIncrement => goseguStat.GoseguStat.finalDmgIncrement;
        
        #endregion
        public override bool TryUse()
        {
            return base.TryUse() && (IsToggleOn || Mathf.Approximately(Gauge, minGauge) || Gauge >= minGauge);
        }

        GoseguActiveOff activeOff => Skill2 as GoseguActiveOff;
        public float Gauge
        {
            get => gauge;
            set
            {
                if (gauge > value && GameManager.instance.CurGameStateType == GameStateType.BattleState)
                {
                    return;
                }
                float dif = value - gauge;
                dif = (dif + goseguStat.GoseguStat.gaugeGain) * (1 + goseguStat.GoseguStat.gaugeGainRatio / 100);

                gauge = Mathf.Clamp(gauge + dif, 0, MaxGauge);
                OnGaugeChange.Invoke(gauge);
                if (gauge <= 0 && Mecha != null && IsToggleOn)
                {
                    Mecha.Die();
                }
            }
        }
        
        void AddGauge1(EventParameters _)
        {
            Gauge += gaugeRatio1;
        }

        void AddGauge2(EventParameters _)
        {
            Gauge += gaugeRatio2;
        }

        protected override void SetConfig()
        {
            baseConfig = new GoseguActiveConfig(new GoseguActiveStat());
        }

        public override void Decorate()
        {
            stats = baseConfig;
            attachments.ForEach(x =>
            {
                stats = new GoseguActiveDecorator(stats, x);
            });
            OnGaugeChange.Invoke(gauge);
        }

        public void MountDown(EventParameters _)
        {
            if (IsToggleOn) Use();
        }
        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            GameManager.instance.OnRest += Recover;
            Gauge = 0;
            eventUser?.EventManager.AddEvent(EventType.OnBasicAttack, AddGauge1);
            eventUser?.EventManager.AddEvent(EventType.OnKill, AddGauge2);
        }

        private UnityEvent<float> _onGaugeChange;
        public UnityEvent<float> OnGaugeChange => _onGaugeChange ??= new();

        private UnityEvent<SeguMecha> _onMechaSpawn;
        public UnityEvent<SeguMecha> OnMechaSpawn => _onMechaSpawn ??= new();

        private UnityEvent<SeguMecha> _onMechaDie;
        public UnityEvent<SeguMecha> OnMechaDie => _onMechaDie ??= new();

        [HideInInspector] public SeguMechaCannonSkill cannonSkill;
        [HideInInspector] public SeguMechaKnockBackSkill pulseGunSkill;
        public void SetPassive(GoseguPassive _passive)
        {
            passive = _passive;
            activeOff.passive = _passive;
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Mecha;

        protected override float TagIncrement => GameManager.Tag.Data.MechaDmgIncrement;

        void Recover()
        {
            MountDown(null);
            Gauge = MaxGauge;
        }

        public override void Active()
        {
            base.Active();
            activeOff?.Init(this);
            Player.StateEvent.AddEvent(EventType.OnCutScene,MountDown);
            GameManager.instance.Player.IdleFixOn();
            mecha = GameManager.Factory.Get<SeguMecha>(FactoryManager.FactoryType.Normal,
                Define.PlayerSkillObjects.SeguMecha, user.transform.position);
            mecha.Collider.enabled = false;
            Player.PhysicsTransitionHandler.StartColliderTransition(Player.Collider,Mecha.Collider);
            user.transform.position = mecha.ridePos.position;
            GameManager.PlayerController = mecha.Controller;
            mecha.Init(this);
            mecha.SetMaster(GameManager.instance.Player);
            OnMechaSpawn.Invoke(mecha);
            mecha.EffectSpawner.Spawn(Define.PlayerEffect.GoseguMechaRide, mecha.transform.position, false);
            
            if (Skill2 is GoseguActiveOff off)
            {
                off.mecha = mecha;
            }
            
            passive.TurnOffDrones();
            if (AttackItemManager.CurrentItem is ActiveSkillItem skillItem)
            {
                skillItem.ActiveSkill.Cancel();
            }

            mecha.SetHpWithoutEvent(mecha.MaxHp / 100f * (Gauge / MaxGauge * 100f));
            GameManager.instance.Player.BlockSkillChange = true;

        }
        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            GameManager.instance.OnRest -= Recover;
            Player.RemoveEvent(EventType.OnCutScene,MountDown);

            if (Player != null)
            {
                Player.IdleFixOff();
                GameManager.PlayerController = Player.Controller;
            }

            if (mecha != null && !mecha.IsDead)
            {
                mecha.Die();
            }

            eventUser?.EventManager.RemoveEvent(EventType.OnBasicAttack, AddGauge1);
            eventUser?.EventManager.RemoveEvent(EventType.OnKill, AddGauge2);
        }

        protected override void UpdateEvent(EventParameters parameters)
        {
            base.UpdateEvent(parameters);
            if (IsToggleOn)
            {
                Gauge -= gaugeReduce1 * Time.deltaTime;
            }
        }

        public float GetGaugeDifference()
        {
            return gauge - maxGauge;
        }
    }
}