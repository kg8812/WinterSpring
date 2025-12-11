using System;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Apis
{
    public class RushSlash : MagicSkill
    {
        [TitleGroup("스탯값")] [LabelText("돌진 거리")]
        public float distance;
        [TitleGroup("스탯값")][LabelText("돌진 시간")] public float time;
        [TitleGroup("돌진 공격설정")] public ProjectileInfo rushInfo;
        private ICdActive _cd;
        public override ICdActive CDActive => _cd ??= new StackCd(this);
        protected override CDEnums _cdType => CDEnums.Stack;

        public override bool TryUse()
        {
            return base.TryUse() && !tweener.IsActive();
        }

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected Tween tweener;
        
        public override void Active()
        {
            base.Active();
            foreach (var x in GameManager.instance.Player.attackColliders)
            {
                SetAttackObject(x,rushInfo);
            }

            mover?.ActorMovement.SetGravityToZero();
            tweener = mover?.ActorMovement.DashTemp(time, distance, false).SetAutoKill(true);

            if (tweener != null && hit != null)
            {
                Guid guid = hit.AddInvincibility();

                tweener.onKill += () =>
                {
                    hit?.RemoveInvincibility(guid);
                    EndMotion();
                    mover.ActorMovement.ResetGravity();
                };
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            tweener?.Kill();
        }
    }
}