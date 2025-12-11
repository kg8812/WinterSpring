using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GoseguMainDrone : GoseguDrone
    {
        [System.Serializable]
        [FoldoutGroup("기획쪽 수정 변수들")]
        [TabGroup("기획쪽 수정 변수들/group1", "드론 설정")]
        [HideLabel]
        public class MainDroneInfo : DroneInfo
        {
            [LabelText("투사체 반경")] public float radius;
            [LabelText("투사체 설정")] public ProjectileInfo projInfo;
            [LabelText("그로기 수치")] public int groggy;
            [LabelText("발사 개수")] public int atkCount;
        }

        [SerializeField] private MainDroneInfo _droneInfo;

        protected override DroneInfo droneInfo => _droneInfo;

        public override DroneInfo _DroneInfo => droneInfo;

        protected override void SetAtkEvent()
        {
            player?.AddEvent(EventType.OnBasicAttack, DroneAttack);
        }

        protected override void RemoveAtkEvent()
        {
            player?.RemoveEvent(EventType.OnBasicAttack, DroneAttack);
        }

        private ISearchStrategy _search;
        protected override ISearchStrategy SearchStrategy => _search ??= new AttackedTarget();

        public override void ReturnToOriginalAtkStrategy()
        {
            ChangeAtkStrategy(new FireProjectile(this, _droneInfo, _droneInfo.atkCount));
        }
    }
}