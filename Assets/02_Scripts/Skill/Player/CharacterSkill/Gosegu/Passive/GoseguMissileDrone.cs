using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GoseguMissileDrone : GoseguDrone
    {
        [SerializeField] private MissileDroneInfo _droneInfo;
        protected override DroneInfo droneInfo => _droneInfo;
        
        private ISearchStrategy _search;

        protected override ISearchStrategy SearchStrategy =>
            _search ??= new LockedOnTargets(this, _droneInfo.maxCount, _droneInfo.radius);

        [Serializable]
        [FoldoutGroup("기획쪽 수정 변수들")]
        [TabGroup("기획쪽 수정 변수들/group1","드론 설정")]
        [HideLabel]
        public class MissileDroneInfo : DroneInfo
        {
            [LabelText("탐색 반경")] public float radius;
            [LabelText("미사일 개수")] public int maxCount;
            [LabelText("미사일 투사체 설정")] public ProjectileInfo projInfo;
            [LabelText("미사일 그로기 수치")] public int groggy;
        }
        public override void ReturnToOriginalAtkStrategy()
        {
            ChangeAtkStrategy(new FireMissile(this, _droneInfo));
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