using System;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class GoseguTree3B : SkillTree
    {
        [Serializable]
        public struct Data
        {
            [LabelText("드론당 쿨타임 감소량")] public float cdReduce;
        }
        private GoseguPassive skill;

        private GoseguDrone.DroneInfo droneInfo;

        public Data[] datas;
        
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            droneInfo ??= new();
            
            skill = passive as GoseguPassive;
            skill?._droneInfos.Remove(droneInfo);
            skill?._droneInfos.Add(droneInfo);
            ChangeStat();
            if (skill != null)
            {
                skill.OnDroneChange -= ChangeStat;
                skill.OnDroneChange += ChangeStat;
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?._droneInfos.Remove(droneInfo);
            if (skill != null)
            {
                skill.OnDroneChange -= ChangeStat;
            }
        }

        void ChangeStat()
        {
            droneInfo.cd = -skill?.DroneCount * datas[level-1].cdReduce ?? 0;
        }
        
    }
}