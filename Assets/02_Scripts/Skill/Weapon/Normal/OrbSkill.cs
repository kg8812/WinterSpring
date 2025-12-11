using System;
using System.Collections.Generic;
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class OrbSkill : MagicSkill
    {
        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;
        [Serializable]
        public struct WindBallInfo
        {
            [LabelText("투사체 설정")] public ProjectileInfo projectileInfo;
            [LabelText("소환 위치")][Tooltip("플레이어 중앙 기준 벡터")] public Vector2 pos;
            [LabelText("그로기 계수")] public float groggy;
            [LabelText("크기")] public float size;
        }

        [TitleGroup("스탯값")][LabelText("윈드볼 생성 텀")] public float term; 
        [TitleGroup("스탯값")][LabelText("스탭 거리")] public float distance;
        [TitleGroup("스탯값")][LabelText("스탭 시간")] public float time;
        
        [InfoBox("윈드볼 설정내 리스트의 개수가 발사 횟수 (연속발사), 크기가 각 횟수의 발사 개수가 됩니다.")]
        [TitleGroup("스탯값")][LabelText("윈드볼 설정")] 
        public List<List<WindBallInfo>> windBallInfos = new();

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        private Sequence seq;
        private Tween tween;
        
        public override void Active()
        {
            base.Active();

            if (mover != null)
            {
                mover.ActorMovement.SetGravityToZero();
                tween = mover.ActorMovement.DashTemp(time, distance, true).SetAutoKill(true);
                tween.onKill += EndMotion;
            }

            seq = DOTween.Sequence();
            for (int i = 0; i < windBallInfos.Count; i++)
            {
                int temp = i;
                seq.AppendCallback(() => SpawnWindBalls(temp));
                seq.AppendInterval(term);
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            tween?.Kill();
            seq?.Kill();
        }

        void SpawnWindBalls(int index)
        {
            windBallInfos[index].ForEach(x =>
            {
                Projectile atk = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                    "WindBall", (Vector2)user.Position + x.pos * (direction != null ? (int)direction.Direction : 1));
                atk.transform.localScale = new Vector3(x.size, x.size, 1);
                atk.Init(attacker,new AtkBase(attacker,x.projectileInfo.dmg));
                atk.Init(x.projectileInfo);
                atk.Init(_weapon.CalculateGroggy(x.groggy));
                atk.Fire();
            });
        }
    }
}