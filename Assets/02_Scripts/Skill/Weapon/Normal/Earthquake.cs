using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class Earthquake : MagicSkill
    {
        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;

        [TabGroup("스탯값/스킬","지상 스킬")] [LabelText("지상 공격설정")] public ProjectileInfo groundInfo;
        [TabGroup("스탯값/스킬","지상 스킬")][LabelText("그로기 계수")] public float groundGroggy;
        [TabGroup("스탯값/스킬","지상 스킬")][LabelText("기절 지속시간")] public float groundStun;
        
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("낙하 속도")] public float speed;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("낙하 Ease")] public Ease ease;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("최대 지속시간")] public float maxDuration;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("공중 공격설정")] public ProjectileInfo airInfo;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("기절 지속시간")] public float airStun;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("최소 체공시간")] public float minAirTime;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("그로기 계수")] public float airGroggy;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override bool TryUse()
        {
            return base.TryUse() && mover != null && (mover.ActorMovement.IsStick || mover.ActorMovement.AirHoldingTime >= minAirTime);
        }
        
        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(SpawnGroundImpact);
        }

        private Tween tween;
        
        void SpawnGroundImpact()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                "LandEffect2", user.transform.position + Vector3.right * (direction != null ?(int)direction.Direction * 1.5f : 1));
            obj.Init(attacker,new AtkBase(attacker,groundInfo.dmg),1);
            obj.Init(groundInfo);
            obj.Init((int)(groundGroggy));
            obj.AddEventUntilInitOrDestroy(x =>
            {
                if (x?.target is Actor target)
                {
                    target.StartStun(eventUser,groundStun);
                }
            });
        }
        public override void Active()
        {
            base.Active();
            if (mover == null) return;
            
            if (!mover.ActorMovement.IsStick)
            {
                RaycastHit2D hit = Physics2D.Raycast( user.Position, Vector2.down, Mathf.Infinity, LayerMasks.GroundAndPlatform);
                if (hit.collider != null)
                {
                    float curTime = Time.time;

                    tween = mover.Rb.DOMoveY(hit.point.y, speed).SetUpdate(UpdateType.Fixed).SetSpeedBased()
                        .SetEase(ease).SetAutoKill(true);

                    tween.onUpdate += () =>
                    {
                        if (curTime + maxDuration <= Time.time)
                        {
                            tween.Kill();
                        }
                    };

                    tween.onKill += EndMotion;
                    tween.onKill += GameManager.instance.Player.StopJumping;
                    tween.onComplete += () =>
                    {
                        AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                            "LandEffect2", user.transform.position);
                        obj.Init(attacker,new AtkBase(attacker,airInfo.dmg),1);
                        obj.Init(airInfo);
                        obj.Init((int)(airGroggy));
                        obj.AddEventUntilInitOrDestroy(x =>
                        {
                            if (x?.target is Actor target)
                            {
                                target.StartStun(eventUser,airStun);
                            }
                        });
                    };
                }
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}