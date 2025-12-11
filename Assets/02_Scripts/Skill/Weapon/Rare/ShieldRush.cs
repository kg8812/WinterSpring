using System;
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Spine.Unity;
using UnityEngine;

namespace Apis
{
    public class ShieldRush : MagicSkill
    {
        private Guid guid;
        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;
        
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("피해 감소량")] [SerializeField] private float dmgReduce;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("돌진속도")][Tooltip("이동속도 비례 백분율")] public float speed;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("돌진 공격설정")] public ProjectileInfo atkInfo1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("돌진 그로기 계수")] public float groggy1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("최대 충돌 횟수")] public int maxCount;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("충돌주기")] public float collideTime;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("마지막 공격설정")] public ProjectileInfo atkInfo2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("마지막 그로기 계수")] public float groggy2;
        private int count;

        public override void Init()
        {
            base.Init();
            count = 0;
        }

        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        private bool stop;

        private Sequence seq;

        private AttackObject atk;
        
        public override void Active()
        {
            base.Active();
            guid = hit.AddHitImmunity();
            count = 0;
            statUser.StatManager.AddStat(ActorStatType.DmgReduce,dmgReduce,ValueType.Ratio);
            atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject, "SquareAttackObject",
                user.Position);
            SpineUtils.AddBoneFollower(skeleton?.Mecanim, "center", atk.gameObject);
            atk.Init(attacker, new AtkBase(attacker, atkInfo1.dmg), Duration);
            atk.Init(atkInfo1);
            atk.Init((int)(groggy1));
            atk.AddEvent(EventType.OnAttackSuccess,Collide);
            
            seq = DOTween.Sequence();
            seq.AppendInterval(Duration);
            seq.SetUpdate(UpdateType.Fixed);
            seq.onUpdate += () =>
            {
                if (!stop)
                {
                    mover.ActorMovement.Move(direction?.Direction ?? EActorDirection.Right,speed / 100);
                }
                
                var ray = Physics2D.Raycast(user.Position, Vector2.right * (direction != null ? (int)direction.Direction : 1), 0.5f,
                    LayerMasks.Wall);

                if (ray.collider != null)
                {
                    Cancel();
                }
            };
        }

        void Collide(EventParameters parameters)
        {
            count++;
            if (count >= maxCount)
            {
                Cancel();
            }
            stop = true;
            
            animator.animator.enabled = false;
            mover?.Stop();

            atk.Collider.enabled = false;
            Sequence s = DOTween.Sequence();
            s.AppendInterval(collideTime);
            s.onComplete += () =>
            {
                stop = false;
                atk.Collider.enabled = true;
                animator.animator.enabled = true;
            };
        }

        public override void AfterDuration()
        {
            base.AfterDuration();

            statUser?.StatManager.AddStat(ActorStatType.DmgReduce,-dmgReduce,ValueType.Ratio);
            if (hit != null)
            {
                hit.RemoveHitImmunity(guid);
            }

            seq?.Kill();
            GameManager.instance.Player?.attackColliders.ForEach(x =>
            {
                x.Init(attacker,new AtkBase(attacker,atkInfo2.dmg));
                x.Init(atkInfo2);
            });
            mover?.Stop();
            atk.RemoveEvent(EventType.OnAttack, Collide);
            atk.Destroy();
            Destroy(atk.GetComponent<BoneFollower>());
            EndMotion();

        }
    }
}