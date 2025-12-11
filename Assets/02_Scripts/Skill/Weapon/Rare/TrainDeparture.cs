
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apis
{
    public class TrainDeparture : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)][TabGroup("스탯값/스킬","지상 스킬")][LabelText("기차 소환횟수")] public int count;
        [PropertyOrder(2)][TabGroup("스탯값/스킬","지상 스킬")][LabelText("기차 투사체 설정")] public ProjectileInfo info;
        [PropertyOrder(2)][TabGroup("스탯값/스킬","지상 스킬")][LabelText("기차 그로기계수")] public float trainGroggy;
        [PropertyOrder(2)][TabGroup("스탯값/스킬","지상 스킬")][LabelText("기차 최소 지속시간")] public float time1;
        [PropertyOrder(2)][TabGroup("스탯값/스킬","지상 스킬")][LabelText("기차 최대 지속시간")] public float time2;
        [PropertyOrder(2)][TabGroup("스탯값/스킬","지상 스킬")][LabelText("기차 발사 텀")] public float term;

        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("낙하 속도")] public float speed;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("낙하 Ease")] public Ease ease;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("최대 지속시간")] public float maxDuration;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("최소 체공시간")] public float minAirTime;
        
        private Tween tween;

        public override bool TryUse()
        {
            return base.TryUse() && mover != null && (mover.ActorMovement.IsStick || mover.ActorMovement.AirHoldingTime >= minAirTime);
        }
        
        public override void Active()
        {
            base.Active();
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
                    tween.onComplete += SpawnTrains;
                }
            }
        }

        void SpawnTrains()
        {
            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < count; i++)
            {
                var temp = i;
                seq.AppendCallback(() =>
                {
                    Projectile proj =
                        GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "TrainDummy",
                            user.transform.position);
                    
                    float d = Random.Range(time1, time2);
                    proj.Init(attacker,new AtkBase(attacker,Atk),d);
                    proj.Init(info);
                    proj.Init((int)trainGroggy);
                    proj.firstVelocity.x = Mathf.Abs(proj.firstVelocity.x) * Mathf.Pow(-1, temp);

                    proj.Fire(false);
                });
                seq.AppendInterval(term);
            }
        }

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(SpawnTrains);
        }
        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}