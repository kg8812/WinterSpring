using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class IceThorn : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Charge;

        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;

        [BoxGroup("스탯값/가시설정")]
        [TabGroup("스탯값/가시설정/설정","1단계")] [LabelText("가시 개수")] public int count1;
        [TabGroup("스탯값/가시설정/설정","1단계")] [LabelText("가시 크기")] public Vector2 size1;

        [TabGroup("스탯값/가시설정/설정","1단계")] [LabelText("가시 데미지")] public float dmg1;
        [TabGroup("스탯값/가시설정/설정","1단계")] [LabelText("가시 그로기계수")] public float groggy1;

        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("가시 개수")] public int count2;
        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("가시 크기")] public Vector2 size2;

        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("가시 데미지")] public float dmg2;
        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("가시 그로기계수")] public float groggy2;

        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("폭발 크기")] public Vector2 size3;

        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("폭발 데미지")]
        public float dmg3;
        [TabGroup("스탯값/가시설정/설정","2단계")] [LabelText("폭발 그로기계수")] public float groggy3;
        
        [BoxGroup("스탯값/공중설정")][LabelText("낙하 속도")] public float speed;
        [BoxGroup("스탯값/공중설정")][LabelText("낙하 Ease")] public Ease ease;
        [BoxGroup("스탯값/공중설정")][LabelText("최대 지속시간")] public float maxDuration;
        [BoxGroup("스탯값/공중설정")][LabelText("최소 체공시간")] public float minAirTime;

        private int chargeStep;

        public override void StartCharge()
        {
            base.StartCharge();
            chargeStep = 0;
        }

        protected override void ChargeInvoke(int idx)
        {
            base.ChargeInvoke(idx);
            chargeStep = idx;
        }

        public override bool TryUse()
        {
            return base.TryUse() && (mover == null || mover.ActorMovement.IsStick || mover.ActorMovement.AirHoldingTime >= minAirTime);
        }
        
        void SpawnThorns()
        {
            switch (chargeStep)
            {
                case 0:
                    for (int i = 0; i < count1; i++)
                    {
                        BeamEffect thorn = GameManager.Factory.Get<BeamEffect>(FactoryManager.FactoryType.AttackObject,
                            "DesireThorn",
                            user.transform.position + Vector3.right * ((i - count1 / 2f) * 0.5f));
                        thorn.size = size1.x;
                        thorn.distance = size1.y;
                        thorn.fireTime = 0.5f;
                        thorn.ease = Ease.Linear;
                        thorn.fireDir = BeamEffect.FireDir.Vertical;
                        thorn.Init(attacker, new AtkBase(attacker, dmg1),
                            1);
                        thorn.Init((int)groggy1);
                        thorn.Fire();
                    }

                    break;
                case 1:
                    for (int i = 0; i < count2; i++)
                    {
                        BeamEffect thorn = GameManager.Factory.Get<BeamEffect>(FactoryManager.FactoryType.AttackObject,
                            "DesireThorn",
                            user.transform.position + Vector3.right * ((i - count2 / 2f) * 0.5f));
                        thorn.size = size2.x;
                        thorn.distance = size2.y;
                        thorn.fireTime = 0.5f;
                        thorn.ease = Ease.Linear;
                        thorn.fireDir = BeamEffect.FireDir.Vertical;
                        thorn.Init(attacker, new AtkBase(attacker, dmg2),
                            1);
                        thorn.Init((int)groggy2);
                        thorn.AddEventUntilInitOrDestroy(Explode,EventType.OnDestroy);

                        thorn.Fire();
                    }
                    break;
            }
        }

        void Action()
        {
            if (GameManager.instance.Player.onAir)
            {
                RaycastHit2D rayHit = Physics2D.Raycast(user.Position, Vector2.down, Mathf.Infinity,
                    LayerMasks.GroundAndPlatform);
                if (rayHit.collider != null)
                {
                    float curTime = Time.time;

                    Tween tween = mover.Rb.DOMoveY(rayHit.point.y, speed).SetUpdate(UpdateType.Fixed).SetSpeedBased()
                        .SetEase(ease).SetAutoKill(true);

                    tween.onUpdate += () =>
                    {
                        if (curTime + maxDuration <= Time.time)
                        {
                            tween.Kill();
                        }
                    };
                    tween.onComplete += SpawnThorns;
                    tween.onKill += GameManager.instance.Player.StopJumping;
                }
            }
            else
            {
                SpawnThorns();
            }
        }
        public override void Active()
        {
            base.Active();

            actionList.Clear();
            actionList.Add(Action);
        }

        void Explode(EventParameters parameters)
        {
            AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                "DesireExplosion", parameters.user.transform.position + Vector3.up * size2.y / 2);
            exp.transform.localScale = size3;
            exp.Init(attacker, new AtkBase(attacker, dmg3), 1);
            exp.Init((int)groggy3);
        }
    }
}