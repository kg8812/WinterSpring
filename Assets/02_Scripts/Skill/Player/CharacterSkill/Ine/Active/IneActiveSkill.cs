using System;
using System.Collections.Generic;
using chamwhy;
using DG.Tweening;
using NewNewInvenSpace;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneActive", menuName = "Scriptable/Skill/IneActive")]
    public class IneActiveSkill : PlayerActiveSkill
    {
        protected override bool UseGroggyRatio => false;

        protected override ActiveEnums _activeType => ActiveEnums.Toggle;

        private float _mana;

        public float mana
        {
            get => _mana;
            set
            {
                if (isCircle4 && value > _mana) return;

                _mana = Mathf.Clamp(value, 0, MaxMana);
                OnManaChange.Invoke(mana);
            }
        }

        UnityEvent<float> _OnManaChange;
        UnityEvent _OnMaxManaChange;
        UnityEvent<int> _OnCircleUse;
        private UnityEvent<int> _afterCircleUse;
        private UnityEvent<bool> _onToggle;
        public UnityEvent<float> OnManaChange => _OnManaChange ??= new();
        public UnityEvent OnMaxManaChange => _OnMaxManaChange ??= new();
        public UnityEvent<int> OnCircleUse => _OnCircleUse ??= new();
        public UnityEvent<int> AfterCircleUse => _afterCircleUse ??= new();
        public UnityEvent<bool> OnToggle => _onToggle ??= new();
        
        [HideInInspector] public int maxCircle = 3;
        
        #region 인스펙터

        [TitleGroup("스탯값")] [LabelText("마나 최대치")] [SerializeField]
        private int maxMana;

        [TitleGroup("스탯값")] [LabelText("마법진 반경")] [SerializeField]
        public float magicCircleRadius;
        [TitleGroup("스탯값")] [LabelText("초당 마나 회복량")] [SerializeField]
        private float manaGainInSecond;

        [PropertyOrder(0)] [TabGroup("스탯값/아이네스킬", "1서클")] [LabelText("마나 획득량")] [SerializeField]
        float manaGain;

        [PropertyOrder(0)] [TabGroup("스탯값/아이네스킬", "1서클")] [LabelText("조각 발사 개수")] [SerializeField]
        int circle1Count;

        [PropertyOrder(0)] [TabGroup("스탯값/아이네스킬", "1서클")] [LabelText("조각 투사체 설정")] [SerializeField]
        public ProjectileInfo circle1Info;

        [PropertyOrder(0)] [TabGroup("스탯값/아이네스킬", "1서클")] [LabelText("조각 투사체 반경")] [SerializeField]
        public float circle1Radius;

        [PropertyOrder(0)] [TabGroup("스탯값/아이네스킬", "1서클")] [LabelText("그로기 수치")] [SerializeField]
        int circle1groggy;

        [PropertyOrder(0)] [TabGroup("스탯값/아이네스킬", "1서클")] [LabelText("소환위치")] [InfoBox("플레이어 중앙위치 기준")] public
        Vector2 circle1Pos;

        
        [PropertyOrder(2)] [TabGroup("스탯값/아이네스킬", "2서클")] [LabelText("마나 필요량")] [SerializeField]
        float circle2Mana;

        [PropertyOrder(2)] [TabGroup("스탯값/아이네스킬", "2서클")] [LabelText("영역 반경")] [SerializeField]
        int circle2Radius;

        [PropertyOrder(2)] [TabGroup("스탯값/아이네스킬", "2서클")] [LabelText("영역 공격 설정")] [SerializeField]
        public ProjectileInfo circle2Info;

        [FormerlySerializedAs("circle2groggy")]
        [PropertyOrder(2)]
        [TabGroup("스탯값/아이네스킬", "2서클")]
        [LabelText("그로기 수치")]
        [SerializeField]
        int circle2Groggy;

        [PropertyOrder(2)] [TabGroup("스탯값/아이네스킬", "2서클")] [LabelText("영역 공격 주기")] [SerializeField]
        float circle2AtkFrequency;

        [FormerlySerializedAs("circle2duration")]
        [PropertyOrder(2)]
        [TabGroup("스탯값/아이네스킬", "2서클")]
        [LabelText("지속시간")]
        [SerializeField]
        int circle2Duration;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("마나 필요량")] [SerializeField]
        float circle3Mana;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달조각 투사체 설정")] [SerializeField]
        public ProjectileInfo circle3Info;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달조각 투사체 반경")] [SerializeField]
        public float circle3Radius;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달조각 그로기 수치")] [SerializeField]
        int circle3groggy;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달 반경")] [SerializeField]
        float meteorRadius;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달 소환 각도 (우측 기준)")] [SerializeField]
        public float meteorAngle;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달 떨어지는 속도")] [SerializeField]
        public float meteorSpeed;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("달 떨어지는 Ease")] [SerializeField]
        public Ease meteorEase;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("폭발 공격설정")] [SerializeField]
        public ProjectileInfo meteorAtkInfo;
        
        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("폭발 반경")] [SerializeField]
        float circle3ExpRadius;

        [PropertyOrder(3)] [TabGroup("스탯값/아이네스킬", "3서클")] [LabelText("폭발 그로기")] [SerializeField]
        int circle3ExpGroggy;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("마나 필요량")]
        public float circle4Mana;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("초당 마나 소모량")]
        public float circle4ManaUse;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("결계 데미지")]
        public float circle4Dmg;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("결계 그로기 수치")]
        public float circle4Groggy;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빔 투사체 설정")]
        public ProjectileInfo circle4BeamInfo1;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빔 그로기 수치")]
        public int beam1Groggy;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("추가 빔 발사 반경")]
        public float circle4BeamRadius;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("추가 빔 투사체 설정")]
        public ProjectileInfo circle4BeamInfo2;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("추가빔 그로기 수치")]
        public int beam2Groggy;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빛의 창 생성 주기")]
        public float spearFrequency;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빛의 창 생성 개수")]
        public int spearCount;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빛의 창 생성 반경")]
        public float spearRadius;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빛의 창 투사체 설정")]
        public ProjectileInfo spearInfo;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("빛의 창 그로기 수치")]
        public int spearGroggy;

        [PropertyOrder(4)] [TabGroup("스탯값/아이네스킬", "4서클")] [LabelText("방전 시간")]
        
        #endregion
        
        #region 스탯

        public float AbilityPower => Atk;

        public int MaxMana
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return Mathf.RoundToInt((maxMana + IneStat.IneStat.maxMana) *
                                            (1 + IneStat.IneStat.maxManaRatio / 100));
                }

                return maxMana;
            }
        }

        float ManaGainInSecond
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return manaGainInSecond + IneStat.IneStat.manaGainInSecond;
                }

                return manaGainInSecond;
            }
        }
        public float ManaGain
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (manaGain + IneStat.IneStat.manaGain) * (1 + IneStat.IneStat.manaGainRatio / 100);
                }

                return manaGain;
            }
        }

        public int Circle1Count
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle1Count + IneStat.IneStat.circle1Count) *
                           (1 + IneStat.IneStat.circle1CountRatio / 100);
                }

                return circle1Count;
            }
        }

        public float Circle1Radius
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle1Radius + IneStat.IneStat.circle1Radius) *
                           (1 + IneStat.IneStat.circle1RadiusRatio / 100);
                }

                return circle1Radius;
            }
        }

        public int Circle1groggy
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle1groggy + IneStat.IneStat.circle1groggy) *
                           (1 + IneStat.IneStat.circle1groggyRatio / 100);
                }

                return circle1groggy;
            }
        }

        public float Circle2Mana
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle2Mana + IneStat.IneStat.circle2Mana) * (1 + IneStat.IneStat.circle2ManaRatio / 100);
                }

                return circle2Mana;
            }
        }

        public float Circle2Radius
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle2Radius + IneStat.IneStat.circle2Radius) *
                           (1 + IneStat.IneStat.circle2RadiusRatio / 100);
                }

                return circle2Radius;
            }
        }

        public int Circle2Groggy
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle2Groggy + IneStat.IneStat.circle2groggy) *
                           (1 + IneStat.IneStat.circle2groggyRatio / 100);
                }

                return circle2Groggy;
            }
        }

        public float Circle2Duration
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle2Duration + IneStat.IneStat.circle2Duration) *
                           (1 + IneStat.IneStat.circle2DurationRatio / 100);
                }

                return circle2Duration;
            }
        }

        public float Circle2AtkFrequency
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle2AtkFrequency + IneStat.IneStat.circle2AtkFrequency) *
                           (1 + IneStat.IneStat.circle2AtkFrequencyRatio / 100);
                }

                return circle2AtkFrequency;
            }
        }

        public float Circle3Mana
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle3Mana + IneStat.IneStat.circle3Mana) * (1 + IneStat.IneStat.circle3ManaRatio / 100);
                }

                return circle3Mana;
            }
        }

        public float Circle3Radius
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle3Radius + IneStat.IneStat.circle3Radius) *
                           (1 + IneStat.IneStat.circle3RadiusRatio / 100);
                }

                return circle3Radius;
            }
        }

        public int Circle3groggy
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle3groggy + IneStat.IneStat.circle3groggy) *
                           (1 + IneStat.IneStat.circle3groggyRatio / 100);
                }

                return circle3groggy;
            }
        }

        public float MeteorRadius
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (meteorRadius + IneStat.IneStat.meteorRadius) *
                           (1 + IneStat.IneStat.meteorRadiusRatio / 100);
                }

                return meteorRadius;
            }
        }

        public float Circle3ExpRadius
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle3ExpRadius + IneStat.IneStat.circle3ExpRadius) *
                           (1 + IneStat.IneStat.circle3ExpRadiusRatio / 100);
                }

                return circle3ExpRadius;
            }
        }

        public int Circle3ExpGroggy
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (circle3ExpGroggy + IneStat.IneStat.circle3ExpGroggy) *
                           (1 + IneStat.IneStat.circle3ExpGroggyRatio / 100);
                }

                return circle3ExpGroggy;
            }
        }

        public float HighCircleDmgIncrement
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (highCircleDmgIncrement + IneStat.IneStat.highCircleDmgIncrement) *
                           (1 + IneStat.IneStat.highCircleDmgIncrementRatio / 100);
                }

                return highCircleDmgIncrement;
            }
        }

        #endregion

        private float highCircleDmgIncrement;
        IneActiveSkillOff activeOff => Skill2 as IneActiveSkillOff;

        [HideInInspector]public PetFollower ineBook;

        public InePassiveSkill passive => GameManager.instance.Player.PassiveSkill as InePassiveSkill;
        protected override void SetConfig()
        {
            baseConfig = new IneActiveConfig(new IneActiveStat());
        }

        public override void Decorate()
        {
            stats = baseConfig;
            attachments.ForEach(x => { stats = new IneActiveDecorator(stats, x); });
            OnMaxManaChange.Invoke();
        }

        IIneActive IneStat => stats as IIneActive;

        public override bool TryUse()
        {
            return base.TryUse() && !isCircle4;
        }

        public float AdditionalCircleDmg(int currentCircle)
        {
            return currentCircle >= 2 ? HighCircleDmgIncrement : 0;
        }
        public override void Init()
        {
            base.Init();
            chargeEffects ??= new[]
            {
                Define.PlayerEffect.Ine_Magic_Charge,
                Define.PlayerEffect.Ine_Magic_1Circle,
                Define.PlayerEffect.Ine_Magic_2Circle,
                Define.PlayerEffect.Ine_Magic_3Circle,
                Define.PlayerEffect.Ine_Magic_4Circle,
                Define.PlayerEffect.Ine_Magic_CircleEnter,
            };

            maxCircle = 3;
            float amount = MaxMana / ChargeTime;

            WhenCharging.AddListener(() =>
            {
                if (CurChargeTime / ChargeTime > mana / MaxMana)
                {
                    DeActive();
                }
            });
            OnMaxManaChange.AddListener(() => { ChargeTime = MaxMana / amount; });
            if (activeOff != null)
            {
                activeOff.active = this;
            }
        }

        public void WhenChanged()
        {
            if (IsToggleOn)
            {
                Use();
            }
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Feather;

        protected override float TagIncrement => GameManager.Tag.Data.SpellIncrement;

        private float curTime;
        protected override void UpdateEvent(EventParameters parameters)
        {
            
            base.UpdateEvent(parameters);
            if (curTime < 1)
            {
                curTime += Time.deltaTime;
            }
            else
            {
                mana += ManaGainInSecond;
                curTime = 0;
            }
        }

        public override void Active()
        {
            base.Active();
            if (activeOff != null)
            {
                activeOff.active = this;
            }
            passive?.Disable();
            AttackItemManager.ApplyPreset(10);
            OnToggle.Invoke(true);
            ineBook = GameManager.Factory.Get<PetFollower>(FactoryManager.FactoryType.Normal,
                Define.PlayerSkillObjects.IneBook, Player.ineBookPos.position);
            ineBook.Init(Player.ineBookPos,Player);
            SpawnEffect(Define.PlayerEffect.Ine_Book_Appear, 0.5f, ineBook.transform.position, false);
            RemoveEffect(Define.PlayerEffect.Ine_WingCircle02);
            SpawnEffect(Define.PlayerEffect.Ine_MoonCircle01_Change, Define.PlayerEffect.Ine_MoonCircle02,
                magicCircleRadius,false,null,"ctrl");
        }

        [HideInInspector] public Action<AttackObject> OnCircle2Spawn;

        public UnityEvent<Vector2> OnMeteorCollide => _onMeteorCollide ??= new();
        UnityEvent<Vector2> _onMeteorCollide;
        
        [HideInInspector]public bool isCircle4;

        #region 이펙트 변수들

        private string[] chargeEffects =
        {
            Define.PlayerEffect.Ine_Magic_Charge,
            Define.PlayerEffect.Ine_Magic_1Circle,
            Define.PlayerEffect.Ine_Magic_2Circle,
            Define.PlayerEffect.Ine_Magic_3Circle,
            Define.PlayerEffect.Ine_Magic_4Circle,
            Define.PlayerEffect.Ine_Magic_CircleEnter,
        };

        #endregion
        
        public override void StartCharge()
        {
            base.StartCharge();

            //SpawnEffect(Define.PlayerEffect.Ine_Magic_Charge, 0.5f,true);
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);

            curTime = 0;
            SpawnEffect(Define.PlayerEffect.Ine_WingCircle02, magicCircleRadius,true,"ctrl");
            skeleton?.Mecanim.skeleton.SetSkin("player_ine");
            highCircleDmgIncrement = 0;

            for (int i = 0; i < 4; i++)
            {
                // ItemId - 4103~4106 : 서클1~서클4
                ActiveSkillItem item = InvenManager.instance.PresetManager.GetOverrideItem(4103+i) as ActiveSkillItem;
                if (item?.ActiveSkill is IneCircleMagic magic)
                {
                    magic.Init(this);
                }
            }
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        void AfterCharge()
        {
            chargeEffects.ForEach(RemoveEffect);
            SpawnEffect(Define.PlayerEffect.Ine_Magic_Circle_dissolution, 0.5f, user.Position,false);
        }

        protected override void ChargeInvoke(int idx)
        {
            base.ChargeInvoke(idx);

            // if (idx >= maxCircle || idx >= 3 && isDischarge) return;
            //
            // circle = Mathf.Clamp(idx + 1, 1, isDischarge ? 3 : maxCircle);
            // switch (circle)
            // {
            //     case 1:
            //         SpawnEffect(Define.PlayerEffect.Ine_Magic_CircleEnter, Define.PlayerEffect.Ine_Magic_1Circle, 0.5f);
            //         break;
            //     case 2:
            //         SpawnEffect(Define.PlayerEffect.Ine_Magic_CircleEnter, Define.PlayerEffect.Ine_Magic_2Circle, 0.5f);
            //         break;
            //     case 3:
            //         SpawnEffect(Define.PlayerEffect.Ine_Magic_CircleEnter, Define.PlayerEffect.Ine_Magic_3Circle, 0.5f);
            //         break;
            //     case 4:
            //         SpawnEffect(Define.PlayerEffect.Ine_Magic_CircleEnter, Define.PlayerEffect.Ine_Magic_4Circle, 0.5f);
            //         break;
            // }
        }
    }
}