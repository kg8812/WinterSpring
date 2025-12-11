using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree3E : SkillTree
    {
        [System.Serializable]
        public struct Data
        {
            [LabelText("쿨감 속도 증가량 (%)")] public float cdReduction;
            [LabelText("필요 게이지")] public int needGauge;
            [LabelText("게이지 획득량")] public int gauge;
        }


        private GoseguPassive skill;
        private GoseguActiveSkill activeSkill;

        private BonusStat _stat;
        private BonusStat stat => _stat ??= new();

        public Data[] datas;

        BonusStat ReturnStat()
        {
            return stat;
        }

        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active, level);
            activeSkill = active as GoseguActiveSkill;

            if (activeSkill == null) return;
            activeSkill.OnGaugeChange.RemoveListener(SetStat);
            activeSkill.OnGaugeChange.AddListener(SetStat);
        }

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive, level);

            skill = passive as GoseguPassive;

            if (skill == null) return;

            GoseguDrone drone = skill.GetDrone(GoseguPassive.DroneType.Main);
            drone.BonusStatEvent -= ReturnStat;
            drone.BonusStatEvent += ReturnStat;
            drone.AddEvent(EventType.OnAttackSuccess, AddGauge);
        }

        public override void DeActivate()
        {
            base.DeActivate();

            GoseguDrone drone = skill?.GetDrone(GoseguPassive.DroneType.Main);
            if (drone != null)
            {
                drone.BonusStatEvent -= ReturnStat;
                drone.RemoveEvent(EventType.OnAttackSuccess, AddGauge);
            }

            activeSkill?.OnGaugeChange.RemoveListener(SetStat);
        }

        void SetStat(float gauge)
        {
            if (Mathf.Approximately(gauge, datas[level - 1].needGauge) || gauge >= datas[level - 1].needGauge)
            {
                stat.SetValue(ActorStatType.CDReduction, datas[level - 1].cdReduction);
            }
            else
            {
                stat.SetValue(ActorStatType.CDReduction, 0);
            }
        }

        void AddGauge(EventParameters parameters)
        {
            if (activeSkill != null)
            {
                activeSkill.Gauge += datas[level - 1].gauge;
            }
        }
    }
}