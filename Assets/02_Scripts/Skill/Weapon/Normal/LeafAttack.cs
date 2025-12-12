using System.Collections.Generic;
using chamwhy;
using DG.Tweening;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class LeafAttack : MagicSkill
    {
        
        [TabGroup("스탯값/스킬","지상 스킬")] [LabelText("도약 거리")] public float distance;
        [TabGroup("스탯값/스킬","지상 스킬")][LabelText("도약 높이")] public float height;
        [TabGroup("스탯값/스킬","지상 스킬")][LabelText("도약 시간")] public float jumpTime;
        [TabGroup("스탯값/스킬","지상 스킬")][LabelText("데미지 계수")] public float groundDmg;
        [TabGroup("스탯값/스킬","지상 스킬")][LabelText("그로기 계수")] public float groundGroggy;
        
        
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("거리")] public float distance2;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("낙하 속도")] public float speed;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("낙하 Ease")] public Ease ease;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("데미지 계수")] public float landDmg;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("그로기 계수")] public float landgroggy;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("최대 지속시간")] public float maxDuration;
        [TabGroup("스탯값/스킬","공중 스킬")][LabelText("최소 체공시간")] public float minAirTime;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected Tween tween;

        private List<Tween> _allTweens;
        private List<Tween> allTweens => _allTweens ??= new();

        public override bool TryUse()
        {
            return base.TryUse() && mover!= null &&(mover.ActorMovement.IsStick || mover.ActorMovement.AirHoldingTime >= minAirTime);
        }
        protected virtual void AirSkill()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                "LandEffect", user.transform.position);
            obj.Init(attacker,new AtkBase(attacker,landDmg),1);
            obj.Init((int)(landgroggy));
        }

        protected virtual void GroundSkill()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                "LandEffect", user.transform.position);
            obj.Init(attacker, new AtkBase(attacker, groundDmg), 1);
            obj.Init((int)(groundGroggy));
        }
        public override void Active()
        {
            base.Active();

            allTweens.Clear();
            if (mover == null) return;
            
            if (mover.ActorMovement.IsStick)
            {
                mover.Rb.DOKill();
                (Tween x, Tween y) tweens = mover.ActorMovement.DoJumpTween(jumpTime, height,
                    (Vector2)mover.transform.position + Vector2.right * (direction?.DirectionScale ?? 1 * distance),
                    LayerMasks.Wall | LayerMasks.Enemy);
                
                tweens.y.onComplete += GroundSkill;
                
                tween = tweens.y;
                tweens.y.onKill += EndMotion;
                allTweens.Add(tweens.x);
                allTweens.Add(tweens.y);
            }
            else
            {
                float curTime = Time.time;
                Vector3 dir =
                    new Vector3(user.transform.position.x + (direction != null ?(int)direction.Direction : 1) * distance2,
                        user.transform.position.y - 1) - user.transform.position;
                dir.z = 0;
                dir.Normalize();
                
                mover.Rb.DOKill();
                var tempTween = mover.Rb.DOMove(user.transform.position + dir * (speed * (maxDuration + 0.5f)), speed);
                tempTween.SetUpdate(UpdateType.Fixed).SetSpeedBased().SetEase(ease).SetAutoKill(true);
                tempTween.KillWhenBoxCast(mover.Rb, new Vector2(0.5f, 0.1f), LayerMasks.GroundAndPlatform);
                tween.onKill += EndMotion;
                tween.onKill += GameManager.instance.Player.StopJumping;
                tween.onKill += AirSkill;
                
                tween.onUpdate += () =>
                {
                    if (curTime + maxDuration <= Time.time)
                    {
                        tween.Kill();
                    }
                };
                allTweens.Add(tween);
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            allTweens.ForEach(x =>
            {
                x.Kill();
            });
            allTweens.Clear();
        }
    }
}