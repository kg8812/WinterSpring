using System.Collections.Generic;
using chamwhy;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace Apis
{
    public class FirePerforming : MagicSkill
    {
        protected override bool UseAtkRatio => false;

        protected override bool UseGroggyRatio => false;

        protected override ActiveEnums _activeType => ActiveEnums.Casting;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 반경")] public float radius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 데미지")] public float dmg2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 그로기")] public float expGroggy;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("외곽 불 개수")] public int outsideCount;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("외곽 반경")] public float outRadius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("외곽 회전속도")] public float speed1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("내부 불 개수")] public int insideCount;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("내부 반경")] public float inRadius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("내부 회전속도")] public float speed2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("불덩이 데미지")] public float dmg3;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("불덩이 그로기")] public float flameGroggy;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("영역 데미지 설정")] public ProjectileInfo atkInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("영역 그로기")] public float areaGroggy;
        
        private List<CircleAroundProjectile> _outsides = new();
        private List<CircleAroundProjectile> _insides = new();
        private List<CircleAroundProjectile> outsides => _outsides ??= new();
        private List<CircleAroundProjectile> insides  => _insides ??= new();

        private AttackObject c;

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(Start);
        }

        void Start()
        {
            void Remove(EventParameters info)
            {
                if (info?.user == null) return;
                
                Destroy(info.user.gameObject.GetComponent<BoneFollower>());
                info.user.EventManager.RemoveEvent(EventType.OnDestroy,Remove);
            }
            AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "InstrumentExplosion", user.Position);

            exp.transform.localScale = Vector3.one * (radius * 2 * 0.3f);
            exp.AddEvent(EventType.OnAttackSuccess,AddBurn);
            exp.Init(attacker,new AtkBase(attacker,dmg2),1);
            exp.Init((int)expGroggy);

            c = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "CircleAttackObject", user.Position);
            SpineUtils.AddBoneFollower(skeleton?.Mecanim,"center",c.gameObject);
            c.AddEvent(EventType.OnDestroy,Remove);
            c.transform.localScale = Vector3.one * (outRadius * 2);
            c.Init(attacker,new AtkBase(attacker,atkInfo.dmg));
            c.Init(atkInfo);
            c.Init((int)areaGroggy);
            for (int i = 0; i < outsideCount; i++)
            {
                CircleAroundProjectile circle =
                    GameManager.Factory.Get<CircleAroundProjectile>(FactoryManager.FactoryType.AttackObject,
                        "InstrumentFlameBall");
                circle.Init(attacker,new AtkBase(attacker,dmg3));
                circle.Init(speed1,outRadius);
                circle.Init((int)flameGroggy);
                circle.move.Degree = 360f / outsideCount * i;

                circle.AddEvent(EventType.OnAttackSuccess,AddBurn);
                outsides.Add(circle);
            }

            for (int i = 0; i < insideCount; i++)
            {
                CircleAroundProjectile circle =
                    GameManager.Factory.Get<CircleAroundProjectile>(FactoryManager.FactoryType.AttackObject,
                        "InstrumentFlameBall");
                circle.Init(attacker,new AtkBase(attacker,dmg3));
                circle.Init(speed2,inRadius,CircleAround.Direction.AntiClockWise);
                circle.move.Degree = 360f / insideCount * i;
                circle.Init((int)flameGroggy);

                circle.AddEvent(EventType.OnAttackSuccess,AddBurn);
                insides.Add(circle);
            }
        }
        
        public override void AfterDuration()
        {
            base.AfterDuration();
            outsides.ForEach(x => x.Destroy());
            insides.ForEach(x => x.Destroy());
            outsides.Clear();
            insides.Clear();
            if (c != null)
            {
                c.Destroy();
                c = null;
            }
        }

        void AddBurn(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                target.AddSubBuff(eventUser,SubBuffType.Debuff_Burn);
            }
        }
    }
}