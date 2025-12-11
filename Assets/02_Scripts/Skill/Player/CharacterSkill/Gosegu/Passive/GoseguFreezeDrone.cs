using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GoseguFreezeDrone : GoseguDrone
    {
        private ISearchStrategy _search;

        protected override ISearchStrategy SearchStrategy => _search ??= new ClosestTarget(this,_droneInfo.radius);

        [Serializable]
        [FoldoutGroup("기획쪽 수정 변수들")]
        [TabGroup("기획쪽 수정 변수들/group1", "드론 설정")]
        [HideLabel]
        public class FreezeDroneInfo : DroneInfo
        {
            [LabelText("탐색 범위")] public float radius;
            [LabelText("빙결탄 투사체 설정")] public ProjectileInfo projInfo;
            [LabelText("빙결 폭발 반경")] public float expRadius;
            [LabelText("빙결 폭발 데미지")] public float dmg;
            [LabelText("빙결 폭발 그로기 수치")] public int groggy;
            [LabelText("빙결 지속시간")] public float duration;
        }

        [SerializeField] private FreezeDroneInfo _droneInfo;

        protected override DroneInfo droneInfo => _droneInfo;

        public override void ReturnToOriginalAtkStrategy()
        {
            ChangeAtkStrategy(new FireFreezeBullet(this,_droneInfo));
        }
        protected override void SetAtkEvent()
        {
            player?.AddEvent(EventType.OnAttack,DroneAttack);
        }

        protected override void RemoveAtkEvent()
        {
            player?.RemoveEvent(EventType.OnAttack,DroneAttack);
        }
    }
}