using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class MangPinkElementSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("스탭 거리")] public float distance;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("스탭 시간")] public float time;
        
        [TabGroup("스탯값/정령","분홍 정령")] [LabelText("유도탄 개수")] public int projCount;
        [TabGroup("스탯값/정령","분홍 정령")] [LabelText("소환 위치")][Tooltip("플레이어 중앙 기준 벡터")] public List<Vector2> pinkPos = new ();
        [TabGroup("스탯값/정령","분홍 정령")] [LabelText("투사체 설정")] public List<ProjectileInfo> pinkProjInfos = new();
        [TabGroup("스탯값/정령","분홍 정령")] [LabelText("그로기 계수")] public float pinkGroggy;

        private Tween tween;

        void Pink()
        {
            for (int i = 0; i < projCount; i++)
            {
                PinkElement proj = GameManager.Factory.Get<PinkElement>(
                    FactoryManager.FactoryType.AttackObject, "PinkProjectile",
                    user.Position + (Vector3)pinkPos[i]);
                proj.Init(attacker,new AtkBase(attacker,pinkProjInfos[i].dmg));
                proj.Init(pinkProjInfos[i]);
                proj.Init((int)pinkGroggy);
                proj.Fire();
            }
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
            Pink();
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
        }
    }
}