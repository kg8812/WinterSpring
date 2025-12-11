using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class PaintMine : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("대쉬 거리")] public float distance;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("대쉬 시간")] public float time;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("지뢰 개수")] public int count;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("생성 시간간격")] public float timePadding;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발시간")] public float expTime;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 반경")] public float radius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("기절 지속시간")] public float stunDuration;

        private Tween tween;
        
        public override void Active()
        {
            base.Active();
            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < count; i++)
            {
                seq.AppendCallback(() =>
                {
                    GameObject star = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "StarFragment",
                        user.transform.position);
                    GameManager.Factory.Return(star, expTime, () =>
                    {
                        GameManager.Factory.Return(star);
                        AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                            Define.DummyEffects.Explosion,
                            star.transform.position);
                        exp.AddEventUntilInitOrDestroy(AddStun);
                        exp.transform.localScale = Vector3.one * (radius * 0.6f);
                        exp.Init(attacker, new AtkBase(attacker, Atk), 1);
                        exp.Init(_weapon.CalculateGroggy(BaseGroggyPower));
                    });
                });
                seq.AppendInterval(timePadding);
            }

            if (mover != null)
            {
                mover.ActorMovement.SetGravityToZero();
                tween = mover.ActorMovement.DashTemp(time, distance, false).SetAutoKill(true);
                tween.onKill += EndMotion;
                tween.onKill += () =>
                {
                    mover.ActorMovement.ResetGravity();
                };
            }
        }

        void AddStun(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                target.StartStun(eventUser,stunDuration);
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}