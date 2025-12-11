using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GoseguLaserDrone : GoseguDrone
    {
        private ISearchStrategy _search;
        protected override ISearchStrategy SearchStrategy => _search ??= new ClosestTarget(this, _droneInfo.radius);

        [SerializeField] private LaserDroneInfo _droneInfo;
        protected override DroneInfo droneInfo => _droneInfo;

        protected override void SetAtkEvent()
        {
            player?.AddEvent(EventType.OnAttack,DroneAttack);
        }
        protected override void RemoveAtkEvent()
        {
            player?.RemoveEvent(EventType.OnAttack, DroneAttack);
        }
        
        [Serializable]
        [FoldoutGroup("기획쪽 수정 변수들")]
        [TabGroup("기획쪽 수정 변수들/group1","드론 설정")]
        [HideLabel]
        public class LaserDroneInfo : DroneInfo
        {
            [LabelText("탐색 반경")] public float radius;
            [LabelText("레이저 발사 설정")] public BeamEffect.BeamInfo beamInfo;
            [LabelText("레이저 공격 설정")] public ProjectileInfo atkInfo;
            [LabelText("레이저 그로기 수치")] public int groggy;
        }

        public override void ReturnToOriginalAtkStrategy()
        {
            ChangeAtkStrategy(new FireLaser(this,_droneInfo));
        }
    }
}