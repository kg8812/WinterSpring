using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "MangGreenElementalSkill", menuName = "Scriptable/Skill/Weapon/MangGreenElementalSkill")]
    public class MangGreenElementSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("스탭 거리")]
        public float distance;

        [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("스탭 시간")]
        public float time;

        [TabGroup("스탯값/정령", "초록 정령")] [LabelText("독가스 발사시간")]
        public float greenTime;

        [TabGroup("스탯값/정령", "초록 정령")] [LabelText("독가스 데미지")]
        public float greenDmg;

        [TabGroup("스탯값/정령", "초록 정령")] [LabelText("그로기 계수")]
        public float greenGroggy;

        [TabGroup("스탯값/정령", "초록 정령")] [LabelText("독가스 지속시간")]
        public float greenDuration;

        [TabGroup("스탯값/정령", "초록 정령")] [LabelText("독가스 크기")]
        public Vector2 greenSize;

        private Tween tween;

        void Green()
        {
            GreenElement green = GameManager.Factory.Get<GreenElement>(FactoryManager.FactoryType.AttackObject,
                "GreenProjectile", user.Position);
            green.fireTime = greenTime;
            green.distance = greenSize.x;
            green.size = greenSize.y;
            green.fireDir = BeamEffect.FireDir.Horizontal;
            green.Init(attacker, new AtkBase(attacker, greenDmg), greenDuration);
            green.Init((int)greenGroggy);
            green.Fire();
            green.AddEventUntilInitOrDestroy(param =>
            {
                if (param?.target is Actor target)
                {
                    target.AddSubBuff(eventUser,SubBuffType.Debuff_Poison);
                }
            });
        }
        public override void Active()
        {
            base.Active();
            if (mover != null)
            {
                mover.ActorMovement.SetGravityToZero();
                tween = mover.ActorMovement.DashTemp(time, distance, true).SetAutoKill(true);
                tween.onKill += EndMotion;
            }

            Green();
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}