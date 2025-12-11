using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GoseguHealingDrone : GoseguDrone
    {
        private ISearchStrategy _search;

        protected override ISearchStrategy SearchStrategy => _search ??= new AttackedTarget();

        [Serializable]
        [FoldoutGroup("기획쪽 수정 변수들")]
        [TabGroup("기획쪽 수정 변수들/group1","드론 설정")]
        [HideLabel]
        public class HealingDroneInfo : DroneInfo
        {
            [LabelText("즉시 회복량(%)")] public float instantHeal;
            [LabelText("회복 주기")] public float frequency;
            [LabelText("회복 횟수")] public int count;
            [LabelText("주기당 회복량(고정)")] public float heal;
        }
        public override void ReturnToOriginalAtkStrategy()
        {
            ChangeAtkStrategy(new Heal(this,_droneInfo));
        }

        [SerializeField] private HealingDroneInfo _droneInfo;
        protected override DroneInfo droneInfo => _droneInfo;

        protected override void SetAtkEvent()
        {
            player.AddEvent(EventType.OnAfterHit,DroneAttack);
        }

        protected override void RemoveAtkEvent()
        {
            player.RemoveEvent(EventType.OnAfterHit,DroneAttack);
        }
    }
}